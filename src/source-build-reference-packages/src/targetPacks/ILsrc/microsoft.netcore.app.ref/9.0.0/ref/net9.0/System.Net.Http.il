
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
// Time-date stamp:            0xd621b321
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000ba00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000d8da
// Base of code:                   0x00002000
// Base of data:                   0x0000e000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00012000
// Size of headers:                0x00000200
// Checksum:                       0x00014167
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000d888 [0x0000004f] address [size] of Import Directory:          
// 0x0000e000 [0x00000454] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000c400 [0x00002938] address [size] of Security Directory:        
// 0x00010000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000d86c [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000b8e0 Virtual Size
//              0x00002000 Virtual Address
//              0x0000ba00 Size of Raw Data
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
//              0x0000e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000bc00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00010000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000c200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000d000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000008dc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000d8ca Import Name Table
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
// 0x00002ae0 [0x0000ad0c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000d7ec [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00006400 Size
//              '#~' Name
//    Stream 2:
//              0x0000646c Offset
//              0x0000373c Size
//              '#Strings' Name
//    Stream 3:
//              0x00009ba8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00009bac Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00009bbc Offset
//              0x00001150 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000006090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 60728
// PE header size       : 512 (496 used)    ( 0.84%)
// PE additional info   : 11859             (19.53%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.12%)
// CLR meta-data size  : 44300              (72.95%)
// CLR additional info : 128                ( 0.21%)
// CLR method headers  : 792                ( 1.30%)
// Managed code         : 1909              ( 3.14%)
// Data                 : 2048              ( 3.37%)
// Unaccounted          : -892              (-1.47%)

// Num.of PE sections   : 3
//   .text    - 47616
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 44300
//   Module        -    1 (10 bytes)
//   TypeDef       -   62 (868 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   81 (486 bytes)
//   MethodDef     -  801 (11214 bytes)   5 abstract, 0 native, 792 bodies
//   FieldDef      -   36 (216 bytes)     0 constant
//   MemberRef     -   65 (390 bytes)
//   ParamDef      -  635 (3810 bytes)
//   MethodImpl    -   58 (348 bytes)
//   Constant      -   26 (156 bytes)
//   CustomAttribute-  585 (3510 bytes)
//   InterfaceImpl -   52 (208 bytes)
//   PropertyMap   -   46 (184 bytes)
//   Property      -  260 (1560 bytes)
//   MethodSemantic-  389 (2334 bytes)
//   TypeSpec      -   17 (34 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   NestedClass   -    2 (8 bytes)
//   GenericParam  -    5 (40 bytes)
//   Strings       - 14137 bytes
//   Blobs         -  4432 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   213 bytes

// CLR additional info : 128

// CLR method headers : 792
//   Num.of method bodies  - 792
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 792

// Managed code : 1909
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class ByteArrayContent               (public) (auto) (ansi) 
// Class ClientCertificateOption        (public) (auto) (ansi) (sealed) 
// Class DelegatingHandler              (public) (abstract) (auto) (ansi) 
// Class FormUrlEncodedContent          (public) (auto) (ansi) 
// Class HeaderEncodingSelector`1       (public) (auto) (ansi) (sealed) 
// Class HttpClient                     (public) (auto) (ansi) 
// Class HttpClientHandler              (public) (auto) (ansi) 
// Class HttpCompletionOption           (public) (auto) (ansi) (sealed) 
// Class HttpContent                    (public) (abstract) (auto) (ansi) 
// Class HttpIOException                (public) (auto) (ansi) 
// Class HttpKeepAlivePingPolicy        (public) (auto) (ansi) (sealed) 
// Class HttpMessageHandler             (public) (abstract) (auto) (ansi) 
// Class HttpMessageInvoker             (public) (auto) (ansi) 
// Class HttpMethod                     (public) (auto) (ansi) 
// Class HttpProtocolException          (public) (auto) (ansi) (sealed) 
// Class HttpRequestError               (public) (auto) (ansi) (sealed) 
// Class HttpRequestException           (public) (auto) (ansi) 
// Class HttpRequestMessage             (public) (auto) (ansi) 
// Class HttpRequestOptions             (public) (auto) (ansi) (sealed) 
// Class HttpRequestOptionsKey`1        (public) (sequential) (ansi) (sealed) 
// Class HttpResponseMessage            (public) (auto) (ansi) 
// Class HttpVersionPolicy              (public) (auto) (ansi) (sealed) 
// Class MessageProcessingHandler       (public) (abstract) (auto) (ansi) 
// Class MultipartContent               (public) (auto) (ansi) 
// Class MultipartFormDataContent       (public) (auto) (ansi) 
// Class ReadOnlyMemoryContent          (public) (auto) (ansi) (sealed) 
// Class SocketsHttpConnectionContext   (public) (auto) (ansi) (sealed) 
// Class SocketsHttpHandler             (public) (auto) (ansi) (sealed) 
// Class SocketsHttpPlaintextStreamFilterContext (public) (auto) (ansi) (sealed) 
// Class StreamContent                  (public) (auto) (ansi) 
// Class StringContent                  (public) (auto) (ansi) 
// Class HttpMetricsEnrichmentContext   (public) (auto) (ansi) (sealed) 
// Class AuthenticationHeaderValue      (public) (auto) (ansi) 
// Class CacheControlHeaderValue        (public) (auto) (ansi) 
// Class ContentDispositionHeaderValue  (public) (auto) (ansi) 
// Class ContentRangeHeaderValue        (public) (auto) (ansi) 
// Class EntityTagHeaderValue           (public) (auto) (ansi) 
// Class HeaderStringValues             (public) (sequential) (ansi) (sealed) 
// Class HttpContentHeaders             (public) (auto) (ansi) (sealed) 
// Class HttpHeaders                    (public) (abstract) (auto) (ansi) 
// Class HttpHeadersNonValidated        (public) (sequential) (ansi) (sealed) 
// Class HttpHeaderValueCollection`1    (public) (auto) (ansi) (sealed) 
// Class HttpRequestHeaders             (public) (auto) (ansi) (sealed) 
// Class HttpResponseHeaders            (public) (auto) (ansi) (sealed) 
// Class MediaTypeHeaderValue           (public) (auto) (ansi) 
// Class MediaTypeWithQualityHeaderValue (public) (auto) (ansi) (sealed) 
// Class NameValueHeaderValue           (public) (auto) (ansi) 
// Class NameValueWithParametersHeaderValue (public) (auto) (ansi) 
// Class ProductHeaderValue             (public) (auto) (ansi) 
// Class ProductInfoHeaderValue         (public) (auto) (ansi) 
// Class RangeConditionHeaderValue      (public) (auto) (ansi) 
// Class RangeHeaderValue               (public) (auto) (ansi) 
// Class RangeItemHeaderValue           (public) (auto) (ansi) 
// Class RetryConditionHeaderValue      (public) (auto) (ansi) 
// Class StringWithQualityHeaderValue   (public) (auto) (ansi) 
// Class TransferCodingHeaderValue      (public) (auto) (ansi) 
// Class TransferCodingWithQualityHeaderValue (public) (auto) (ansi) (sealed) 
// Class ViaHeaderValue                 (public) (auto) (ansi) 
// Class WarningHeaderValue             (public) (auto) (ansi) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000002*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000003*/ System.Security.Cryptography
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000004*/ System.Diagnostics.DiagnosticSource
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 9:0:0:0
}
.assembly extern /*23000005*/ System.Net.Security
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.Http
{
  .custom /*0C000005:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 0F 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                        74 70 00 00 )                                     // tp..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0F 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                       74 70 00 00 )                                     // tp..
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 0F 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                 74 70 00 00 )                                     // tp..
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
  .ver 9:0:0:0
}
.module System.Net.Http.dll
// MVID: {8ed2c701-3009-4e15-8887-ccf482a6fc26}
.custom /*0C000002:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F16F4657000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Net.Http.ByteArrayContent
       extends System.Net.Http.HttpContent/*0200000A*/
{
  .custom /*0C000017:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] content) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ByteArrayContent::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] content,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ByteArrayContent::.ctor

  .method /*06000003*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          CreateContentReadStream(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 55 11 59
  {
    // Method begins at RVA 0x2060
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayContent::CreateContentReadStream

  .method /*06000004*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync() cil managed
  // SIG: 20 00 15 12 5D 01 12 55
  {
    // Method begins at RVA 0x2063
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayContent::CreateContentReadStreamAsync

  .method /*06000005*/ family hidebysig virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                           class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 55 12 61 11 59
  {
    .param [2]/*08000007*/ 
    .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2066
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ByteArrayContent::SerializeToStream

  .method /*06000006*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context) cil managed
  // SIG: 20 02 12 65 12 55 12 61
  {
    .param [2]/*0800000A*/ 
    .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayContent::SerializeToStreamAsync

  .method /*06000007*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*0800000C*/ 
    .custom /*0C000036:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayContent::SerializeToStreamAsync

  .method /*06000008*/ famorassem hidebysig virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayContent::TryComputeLength

} // end of class System.Net.Http.ByteArrayContent

.class /*02000003*/ public auto ansi sealed System.Net.Http.ClientCertificateOption
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Net.Http.ClientCertificateOption/*02000003*/ Manual = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Net.Http.ClientCertificateOption/*02000003*/ Automatic = int32(0x00000001)
} // end of class System.Net.Http.ClientCertificateOption

.class /*02000004*/ public abstract auto ansi beforefieldinit System.Net.Http.DelegatingHandler
       extends System.Net.Http.HttpMessageHandler/*0200000D*/
{
  .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000009*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2071
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000A3       */ call       instance void System.Net.Http.HttpMessageHandler/*0200000D*/::.ctor() /* 060000A3 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DelegatingHandler::.ctor

  .method /*0600000A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/ innerHandler) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x2079
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000A3       */ call       instance void System.Net.Http.HttpMessageHandler/*0200000D*/::.ctor() /* 060000A3 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DelegatingHandler::.ctor

  .method /*0600000B*/ public hidebysig specialname 
          instance class System.Net.Http.HttpMessageHandler/*0200000D*/ 
          get_InnerHandler() cil managed
  // SIG: 20 00 12 34
  {
    .custom /*0C000030:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2081
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DelegatingHandler::get_InnerHandler

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_InnerHandler(class System.Net.Http.HttpMessageHandler/*0200000D*/ 'value') cil managed
  // SIG: 20 01 01 12 34
  {
    .custom /*0C000035:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000010*/ 
    .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2084
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DelegatingHandler::set_InnerHandler

  .method /*0600000D*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2086
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DelegatingHandler::Dispose

  .method /*0600000E*/ famorassem hidebysig virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    // Method begins at RVA 0x2088
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DelegatingHandler::Send

  .method /*0600000F*/ famorassem hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    // Method begins at RVA 0x208b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DelegatingHandler::SendAsync

  .property /*17000001*/ instance callconv(8) class System.Net.Http.HttpMessageHandler/*0200000D*/
          InnerHandler()
  {
    .custom /*0C000004:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.HttpMessageHandler/*0200000D*/ System.Net.Http.DelegatingHandler/*02000004*/::get_InnerHandler() /* 0600000B */
    .set instance void System.Net.Http.DelegatingHandler/*02000004*/::set_InnerHandler(class System.Net.Http.HttpMessageHandler/*0200000D*/) /* 0600000C */
  } // end of property DelegatingHandler::InnerHandler
} // end of class System.Net.Http.DelegatingHandler

.class /*02000005*/ public auto ansi beforefieldinit System.Net.Http.FormUrlEncodedContent
       extends System.Net.Http.ByteArrayContent/*02000002*/
{
  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,string>> nameValueCollection) cil managed
  // SIG: 20 01 01 15 12 71 01 15 11 75 02 0E 0E
  {
    .param [1]/*08000016*/ 
    .custom /*0C00005D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x208e
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Net.Http.ByteArrayContent/*02000002*/::.ctor(uint8[]) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method FormUrlEncodedContent::.ctor

  .method /*06000011*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000018*/ 
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2097
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormUrlEncodedContent::SerializeToStreamAsync

} // end of class System.Net.Http.FormUrlEncodedContent

.class /*02000006*/ public auto ansi sealed System.Net.Http.HeaderEncodingSelector`1<TContext>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001E*/
{
  .param type TContext /*2A000001*/ 
    .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method HeaderEncodingSelector`1::.ctor

  .method /*06000013*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*01000020*/ 
          Invoke(string headerName,
                 !TContext context) runtime managed
  // SIG: 20 02 12 80 81 0E 13 00
  {
    .custom /*0C00004F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800001C*/ 
    .custom /*0C00007A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method HeaderEncodingSelector`1::Invoke

  .method /*06000014*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginInvoke(string headerName,
                      !TContext context,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 85 0E 13 00 12 80 89 1C
  {
    .param [1]/*0800001F*/ 
    .custom /*0C000085:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000020*/ 
    .custom /*0C00008A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method HeaderEncodingSelector`1::BeginInvoke

  .method /*06000015*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*01000020*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ result) runtime managed
  // SIG: 20 01 12 80 81 12 80 85
  {
    .param [0]/*08000023*/ 
    .custom /*0C000096:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method HeaderEncodingSelector`1::EndInvoke

} // end of class System.Net.Http.HeaderEncodingSelector`1

.class /*02000007*/ public auto ansi beforefieldinit System.Net.Http.HttpClient
       extends System.Net.Http.HttpMessageInvoker/*0200000E*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209a
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)0000A8       */ call       instance void System.Net.Http.HttpMessageInvoker/*0200000E*/::.ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/) /* 060000A8 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method HttpClient::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/ 'handler') cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x20a3
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)0000A8       */ call       instance void System.Net.Http.HttpMessageInvoker/*0200000E*/::.ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/) /* 060000A8 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method HttpClient::.ctor

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/ 'handler',
                               bool disposeHandler) cil managed
  // SIG: 20 02 01 12 34 02
  {
    // Method begins at RVA 0x20ac
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)0000A8       */ call       instance void System.Net.Http.HttpMessageInvoker/*0200000E*/::.ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/) /* 060000A8 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method HttpClient::.ctor

  .method /*06000019*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 
          get_BaseAddress() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C000067:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_BaseAddress

  .method /*0600001A*/ public hidebysig specialname 
          instance void  set_BaseAddress(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::set_BaseAddress

  .method /*0600001B*/ public hidebysig specialname static 
          class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ 
          get_DefaultProxy() cil managed
  // SIG: 00 00 12 80 91
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_DefaultProxy

  .method /*0600001C*/ public hidebysig specialname static 
          void  set_DefaultProxy(class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ 'value') cil managed
  // SIG: 00 01 01 12 80 91
  {
    // Method begins at RVA 0x20bd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::set_DefaultProxy

  .method /*0600001D*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/ 
          get_DefaultRequestHeaders() cil managed
  // SIG: 20 00 12 80 B0
  {
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_DefaultRequestHeaders

  .method /*0600001E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ 
          get_DefaultRequestVersion() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_DefaultRequestVersion

  .method /*0600001F*/ public hidebysig specialname 
          instance void  set_DefaultRequestVersion(class [System.Runtime/*23000001*/]System.Version/*01000025*/ 'value') cil managed
  // SIG: 20 01 01 12 80 95
  {
    // Method begins at RVA 0x20c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::set_DefaultRequestVersion

  .method /*06000020*/ public hidebysig specialname 
          instance valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ 
          get_DefaultVersionPolicy() cil managed
  // SIG: 20 00 11 5C
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_DefaultVersionPolicy

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_DefaultVersionPolicy(valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 11 5C
  {
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::set_DefaultVersionPolicy

  .method /*06000022*/ public hidebysig specialname 
          instance int64  get_MaxResponseContentBufferSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_MaxResponseContentBufferSize

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_MaxResponseContentBufferSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::set_MaxResponseContentBufferSize

  .method /*06000024*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_Timeout() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::get_Timeout

  .method /*06000025*/ public hidebysig specialname 
          instance void  set_Timeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x20d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::set_Timeout

  .method /*06000026*/ public hidebysig instance void 
          CancelPendingRequests() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::CancelPendingRequests

  .method /*06000027*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          DeleteAsync(string requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 12 58 0E
  {
    .param [1]/*0800002E*/ 
    .custom /*0C0000B7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::DeleteAsync

  .method /*06000028*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          DeleteAsync(string requestUri,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 0E 11 59
  {
    .param [1]/*0800002F*/ 
    .custom /*0C0000BC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::DeleteAsync

  .method /*06000029*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          DeleteAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 12 58 12 80 8D
  {
    .param [1]/*08000031*/ 
    .custom /*0C0000C3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::DeleteAsync

  .method /*0600002A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          DeleteAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 80 8D 11 59
  {
    .param [1]/*08000032*/ 
    .custom /*0C0000C6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::DeleteAsync

  .method /*0600002B*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClient::Dispose

  .method /*0600002C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(string requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 12 58 0E
  {
    .param [1]/*08000035*/ 
    .custom /*0C0000CF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*0600002D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(string requestUri,
                   valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 0E 11 24
  {
    .param [1]/*08000036*/ 
    .custom /*0C0000D1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*0600002E*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(string requestUri,
                   valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption,
                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 0E 11 24 11 59
  {
    .param [1]/*08000038*/ 
    .custom /*0C0000D8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*0600002F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(string requestUri,
                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 0E 11 59
  {
    .param [1]/*0800003B*/ 
    .custom /*0C0000E0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*06000030*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 12 58 12 80 8D
  {
    .param [1]/*0800003D*/ 
    .custom /*0C0000E5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*06000031*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                   valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 80 8D 11 24
  {
    .param [1]/*0800003E*/ 
    .custom /*0C0000E7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*06000032*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                   valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption,
                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 12 80 8D 11 24 11 59
  {
    .param [1]/*08000040*/ 
    .custom /*0C0000EA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*06000033*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          GetAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 80 8D 11 59
  {
    .param [1]/*08000043*/ 
    .custom /*0C0000F0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetAsync

  .method /*06000034*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<uint8[]> 
          GetByteArrayAsync(string requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 1D 05 0E
  {
    .param [1]/*08000045*/ 
    .custom /*0C0000F3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetByteArrayAsync

  .method /*06000035*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<uint8[]> 
          GetByteArrayAsync(string requestUri,
                            valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 1D 05 0E 11 59
  {
    .param [1]/*08000046*/ 
    .custom /*0C0000F6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetByteArrayAsync

  .method /*06000036*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<uint8[]> 
          GetByteArrayAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 1D 05 12 80 8D
  {
    .param [1]/*08000048*/ 
    .custom /*0C0000FB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetByteArrayAsync

  .method /*06000037*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<uint8[]> 
          GetByteArrayAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                            valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 1D 05 12 80 8D 11 59
  {
    .param [1]/*08000049*/ 
    .custom /*0C0000FE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetByteArrayAsync

  .method /*06000038*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          GetStreamAsync(string requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 12 55 0E
  {
    .param [1]/*0800004B*/ 
    .custom /*0C000101:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000102:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStreamAsync

  .method /*06000039*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          GetStreamAsync(string requestUri,
                         valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 55 0E 11 59
  {
    .param [1]/*0800004C*/ 
    .custom /*0C000104:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000105:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStreamAsync

  .method /*0600003A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          GetStreamAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 12 55 12 80 8D
  {
    .param [1]/*0800004E*/ 
    .custom /*0C000109:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStreamAsync

  .method /*0600003B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          GetStreamAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                         valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 55 12 80 8D 11 59
  {
    .param [1]/*0800004F*/ 
    .custom /*0C00010B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStreamAsync

  .method /*0600003C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<string> 
          GetStringAsync(string requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 0E 0E
  {
    .param [1]/*08000051*/ 
    .custom /*0C00010D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00010E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStringAsync

  .method /*0600003D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<string> 
          GetStringAsync(string requestUri,
                         valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 0E 0E 11 59
  {
    .param [1]/*08000052*/ 
    .custom /*0C00010F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000110:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStringAsync

  .method /*0600003E*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<string> 
          GetStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri) cil managed
  // SIG: 20 01 15 12 5D 01 0E 12 80 8D
  {
    .param [1]/*08000054*/ 
    .custom /*0C000112:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStringAsync

  .method /*0600003F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<string> 
          GetStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                         valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 0E 12 80 8D 11 59
  {
    .param [1]/*08000055*/ 
    .custom /*0C000114:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::GetStringAsync

  .method /*06000040*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PatchAsync(string requestUri,
                     class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 0E 12 28
  {
    .custom /*0C0000E9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000057*/ 
    .custom /*0C000115:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000058*/ 
    .custom /*0C000116:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PatchAsync

  .method /*06000041*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PatchAsync(string requestUri,
                     class System.Net.Http.HttpContent/*0200000A*/ content,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 0E 12 28 11 59
  {
    .custom /*0C0000EC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800005A*/ 
    .custom /*0C000117:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800005B*/ 
    .custom /*0C000118:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PatchAsync

  .method /*06000042*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PatchAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                     class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 80 8D 12 28
  {
    .custom /*0C0000ED:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800005E*/ 
    .custom /*0C00011C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PatchAsync

  .method /*06000043*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PatchAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                     class System.Net.Http.HttpContent/*0200000A*/ content,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 12 80 8D 12 28 11 59
  {
    .custom /*0C0000EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000061*/ 
    .custom /*0C00011E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PatchAsync

  .method /*06000044*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PostAsync(string requestUri,
                    class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 0E 12 28
  {
    .custom /*0C0000F1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000065*/ 
    .custom /*0C000121:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000066*/ 
    .custom /*0C000122:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PostAsync

  .method /*06000045*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PostAsync(string requestUri,
                    class System.Net.Http.HttpContent/*0200000A*/ content,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 0E 12 28 11 59
  {
    .custom /*0C0000F2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000068*/ 
    .custom /*0C000123:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000069*/ 
    .custom /*0C000125:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PostAsync

  .method /*06000046*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PostAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                    class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 80 8D 12 28
  {
    .custom /*0C0000F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800006C*/ 
    .custom /*0C000127:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PostAsync

  .method /*06000047*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PostAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                    class System.Net.Http.HttpContent/*0200000A*/ content,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 12 80 8D 12 28 11 59
  {
    .custom /*0C0000F8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800006F*/ 
    .custom /*0C00012A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PostAsync

  .method /*06000048*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PutAsync(string requestUri,
                   class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 0E 12 28
  {
    .custom /*0C0000FA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000073*/ 
    .custom /*0C00012B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000074*/ 
    .custom /*0C00012D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PutAsync

  .method /*06000049*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PutAsync(string requestUri,
                   class System.Net.Http.HttpContent/*0200000A*/ content,
                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 0E 12 28 11 59
  {
    .custom /*0C0000FD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000076*/ 
    .custom /*0C00012F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000077*/ 
    .custom /*0C000131:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PutAsync

  .method /*0600004A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PutAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                   class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 80 8D 12 28
  {
    .custom /*0C0000FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800007A*/ 
    .custom /*0C000132:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PutAsync

  .method /*0600004B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          PutAsync(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri,
                   class System.Net.Http.HttpContent/*0200000A*/ content,
                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 12 80 8D 12 28 11 59
  {
    .custom /*0C000100:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800007D*/ 
    .custom /*0C000133:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::PutAsync

  .method /*0600004C*/ public hidebysig instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request') cil managed
  // SIG: 20 01 12 58 12 4C
  {
    .custom /*0C000103:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::Send

  .method /*0600004D*/ public hidebysig instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption) cil managed
  // SIG: 20 02 12 58 12 4C 11 24
  {
    .custom /*0C000107:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::Send

  .method /*0600004E*/ public hidebysig instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 58 12 4C 11 24 11 59
  {
    .custom /*0C000108:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::Send

  .method /*0600004F*/ public hidebysig virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    .custom /*0C00010A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::Send

  .method /*06000050*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request') cil managed
  // SIG: 20 01 15 12 5D 01 12 58 12 4C
  {
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::SendAsync

  .method /*06000051*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 24
  {
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::SendAsync

  .method /*06000052*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype System.Net.Http.HttpCompletionOption/*02000009*/ completionOption,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 5D 01 12 58 12 4C 11 24 11 59
  {
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::SendAsync

  .method /*06000053*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClient::SendAsync

  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000023*/
          BaseAddress()
  {
    .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ System.Net.Http.HttpClient/*02000007*/::get_BaseAddress() /* 06000019 */
    .set instance void System.Net.Http.HttpClient/*02000007*/::set_BaseAddress(class [System.Runtime/*23000001*/]System.Uri/*01000023*/) /* 0600001A */
  } // end of property HttpClient::BaseAddress
  .property /*17000003*/ callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/
          DefaultProxy()
  {
    .get class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ System.Net.Http.HttpClient/*02000007*/::get_DefaultProxy() /* 0600001B */
    .set void System.Net.Http.HttpClient/*02000007*/::set_DefaultProxy(class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/) /* 0600001C */
  } // end of property HttpClient::DefaultProxy
  .property /*17000004*/ instance callconv(8) class System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/
          DefaultRequestHeaders()
  {
    .get instance class System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/ System.Net.Http.HttpClient/*02000007*/::get_DefaultRequestHeaders() /* 0600001D */
  } // end of property HttpClient::DefaultRequestHeaders
  .property /*17000005*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000025*/
          DefaultRequestVersion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ System.Net.Http.HttpClient/*02000007*/::get_DefaultRequestVersion() /* 0600001E */
    .set instance void System.Net.Http.HttpClient/*02000007*/::set_DefaultRequestVersion(class [System.Runtime/*23000001*/]System.Version/*01000025*/) /* 0600001F */
  } // end of property HttpClient::DefaultRequestVersion
  .property /*17000006*/ instance callconv(8) valuetype System.Net.Http.HttpVersionPolicy/*02000017*/
          DefaultVersionPolicy()
  {
    .get instance valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ System.Net.Http.HttpClient/*02000007*/::get_DefaultVersionPolicy() /* 06000020 */
    .set instance void System.Net.Http.HttpClient/*02000007*/::set_DefaultVersionPolicy(valuetype System.Net.Http.HttpVersionPolicy/*02000017*/) /* 06000021 */
  } // end of property HttpClient::DefaultVersionPolicy
  .property /*17000007*/ instance callconv(8) int64
          MaxResponseContentBufferSize()
  {
    .get instance int64 System.Net.Http.HttpClient/*02000007*/::get_MaxResponseContentBufferSize() /* 06000022 */
    .set instance void System.Net.Http.HttpClient/*02000007*/::set_MaxResponseContentBufferSize(int64) /* 06000023 */
  } // end of property HttpClient::MaxResponseContentBufferSize
  .property /*17000008*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          Timeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.HttpClient/*02000007*/::get_Timeout() /* 06000024 */
    .set instance void System.Net.Http.HttpClient/*02000007*/::set_Timeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 06000025 */
  } // end of property HttpClient::Timeout
} // end of class System.Net.Http.HttpClient

.class /*02000008*/ public auto ansi beforefieldinit System.Net.Http.HttpClientHandler
       extends System.Net.Http.HttpMessageHandler/*0200000D*/
{
  .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000028:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000A3       */ call       instance void System.Net.Http.HttpMessageHandler/*0200000D*/::.ctor() /* 060000A3 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::.ctor

  .method /*06000055*/ public hidebysig specialname 
          instance bool  get_AllowAutoRedirect() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_AllowAutoRedirect

  .method /*06000056*/ public hidebysig specialname 
          instance void  set_AllowAutoRedirect(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2169
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_AllowAutoRedirect

  .method /*06000057*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/ 
          get_AutomaticDecompression() cil managed
  // SIG: 20 00 11 80 A5
  {
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_AutomaticDecompression

  .method /*06000058*/ public hidebysig specialname 
          instance void  set_AutomaticDecompression(valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A5
  {
    // Method begins at RVA 0x216e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_AutomaticDecompression

  .method /*06000059*/ public hidebysig specialname 
          instance bool  get_CheckCertificateRevocationList() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_CheckCertificateRevocationList

  .method /*0600005A*/ public hidebysig specialname 
          instance void  set_CheckCertificateRevocationList(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2173
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_CheckCertificateRevocationList

  .method /*0600005B*/ public hidebysig specialname 
          instance valuetype System.Net.Http.ClientCertificateOption/*02000003*/ 
          get_ClientCertificateOptions() cil managed
  // SIG: 20 00 11 0C
  {
    // Method begins at RVA 0x2175
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_ClientCertificateOptions

  .method /*0600005C*/ public hidebysig specialname 
          instance void  set_ClientCertificateOptions(valuetype System.Net.Http.ClientCertificateOption/*02000003*/ 'value') cil managed
  // SIG: 20 01 01 11 0C
  {
    // Method begins at RVA 0x2178
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_ClientCertificateOptions

  .method /*0600005D*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/ 
          get_ClientCertificates() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C00011A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x217a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_ClientCertificates

  .method /*0600005E*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/ 
          get_CookieContainer() cil managed
  // SIG: 20 00 12 80 AD
  {
    .custom /*0C00011B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_CookieContainer

  .method /*0600005F*/ public hidebysig specialname 
          instance void  set_CookieContainer(class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    .custom /*0C00011D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2180
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_CookieContainer

  .method /*06000060*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_Credentials

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_Credentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x2185
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_Credentials

  .method /*06000062*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool> 
          get_DangerousAcceptAnyServerCertificateValidator() cil managed
  // SIG: 00 00 15 12 80 B5 05 12 4C 12 80 B9 12 80 BD 11 80 C1 02
  {
    .param [0]/*08000097*/ 
    .custom /*0C000139:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 02 02 00 00 ) 
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_DangerousAcceptAnyServerCertificateValidator

  .method /*06000063*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 
          get_DefaultProxyCredentials() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_DefaultProxyCredentials

  .method /*06000064*/ public hidebysig specialname 
          instance void  set_DefaultProxyCredentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x218d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_DefaultProxyCredentials

  .method /*06000065*/ public hidebysig specialname 
          instance int32  get_MaxAutomaticRedirections() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_MaxAutomaticRedirections

  .method /*06000066*/ public hidebysig specialname 
          instance void  set_MaxAutomaticRedirections(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2192
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_MaxAutomaticRedirections

  .method /*06000067*/ public hidebysig specialname 
          instance int32  get_MaxConnectionsPerServer() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_MaxConnectionsPerServer

  .method /*06000068*/ public hidebysig specialname 
          instance void  set_MaxConnectionsPerServer(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2197
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_MaxConnectionsPerServer

  .method /*06000069*/ public hidebysig specialname 
          instance int64  get_MaxRequestContentBufferSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_MaxRequestContentBufferSize

  .method /*0600006A*/ public hidebysig specialname 
          instance void  set_MaxRequestContentBufferSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x219c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_MaxRequestContentBufferSize

  .method /*0600006B*/ public hidebysig specialname 
          instance int32  get_MaxResponseHeadersLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x219e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_MaxResponseHeadersLength

  .method /*0600006C*/ public hidebysig specialname 
          instance void  set_MaxResponseHeadersLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21a1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_MaxResponseHeadersLength

  .method /*0600006D*/ public hidebysig specialname 
          instance class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/ 
          get_MeterFactory() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_MeterFactory

  .method /*0600006E*/ public hidebysig specialname 
          instance void  set_MeterFactory(class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x21a6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_MeterFactory

  .method /*0600006F*/ public hidebysig specialname 
          instance bool  get_PreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_PreAuthenticate

  .method /*06000070*/ public hidebysig specialname 
          instance void  set_PreAuthenticate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21ab
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_PreAuthenticate

  .method /*06000071*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object> 
          get_Properties() cil managed
  // SIG: 20 00 15 12 80 C9 02 0E 1C
  {
    .param [0]/*0800009F*/ 
    .custom /*0C00013D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_Properties

  .method /*06000072*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_Proxy

  .method /*06000073*/ public hidebysig specialname 
          instance void  set_Proxy(class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x21b3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_Proxy

  .method /*06000074*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool> 
          get_ServerCertificateCustomValidationCallback() cil managed
  // SIG: 20 00 15 12 80 B5 05 12 4C 12 80 B9 12 80 BD 11 80 C1 02
  {
    .param [0]/*080000A1*/ 
    .custom /*0C00013F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_ServerCertificateCustomValidationCallback

  .method /*06000075*/ public hidebysig specialname 
          instance void  set_ServerCertificateCustomValidationCallback(class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool> 'value') cil managed
  // SIG: 20 01 01 15 12 80 B5 05 12 4C 12 80 B9 12 80 BD 11 80 C1 02
  {
    .param [1]/*080000A2*/ 
    .custom /*0C000140:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    // Method begins at RVA 0x21b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_ServerCertificateCustomValidationCallback

  .method /*06000076*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000033*/ 
          get_SslProtocols() cil managed
  // SIG: 20 00 11 80 CD
  {
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_SslProtocols

  .method /*06000077*/ public hidebysig specialname 
          instance void  set_SslProtocols(valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000033*/ 'value') cil managed
  // SIG: 20 01 01 11 80 CD
  {
    // Method begins at RVA 0x21bd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_SslProtocols

  .method /*06000078*/ public hidebysig newslot specialname virtual 
          instance bool  get_SupportsAutomaticDecompression() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_SupportsAutomaticDecompression

  .method /*06000079*/ public hidebysig newslot specialname virtual 
          instance bool  get_SupportsProxy() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_SupportsProxy

  .method /*0600007A*/ public hidebysig newslot specialname virtual 
          instance bool  get_SupportsRedirectConfiguration() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_SupportsRedirectConfiguration

  .method /*0600007B*/ public hidebysig specialname 
          instance bool  get_UseCookies() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_UseCookies

  .method /*0600007C*/ public hidebysig specialname 
          instance void  set_UseCookies(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_UseCookies

  .method /*0600007D*/ public hidebysig specialname 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_UseDefaultCredentials

  .method /*0600007E*/ public hidebysig specialname 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_UseDefaultCredentials

  .method /*0600007F*/ public hidebysig specialname 
          instance bool  get_UseProxy() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::get_UseProxy

  .method /*06000080*/ public hidebysig specialname 
          instance void  set_UseProxy(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::set_UseProxy

  .method /*06000081*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpClientHandler::Dispose

  .method /*06000082*/ famorassem hidebysig virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    .custom /*0C000134:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000135:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::Send

  .method /*06000083*/ famorassem hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    .custom /*0C000136:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientHandler::SendAsync

  .property /*17000009*/ instance callconv(8) bool
          AllowAutoRedirect()
  {
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_AllowAutoRedirect() /* 06000055 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_AllowAutoRedirect(bool) /* 06000056 */
  } // end of property HttpClientHandler::AllowAutoRedirect
  .property /*1700000A*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/
          AutomaticDecompression()
  {
    .custom /*0C00002F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/ System.Net.Http.HttpClientHandler/*02000008*/::get_AutomaticDecompression() /* 06000057 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_AutomaticDecompression(valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/) /* 06000058 */
  } // end of property HttpClientHandler::AutomaticDecompression
  .property /*1700000B*/ instance callconv(8) bool
          CheckCertificateRevocationList()
  {
    .custom /*0C000034:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_CheckCertificateRevocationList() /* 06000059 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_CheckCertificateRevocationList(bool) /* 0600005A */
  } // end of property HttpClientHandler::CheckCertificateRevocationList
  .property /*1700000C*/ instance callconv(8) valuetype System.Net.Http.ClientCertificateOption/*02000003*/
          ClientCertificateOptions()
  {
    .get instance valuetype System.Net.Http.ClientCertificateOption/*02000003*/ System.Net.Http.HttpClientHandler/*02000008*/::get_ClientCertificateOptions() /* 0600005B */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_ClientCertificateOptions(valuetype System.Net.Http.ClientCertificateOption/*02000003*/) /* 0600005C */
  } // end of property HttpClientHandler::ClientCertificateOptions
  .property /*1700000D*/ instance callconv(8) class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/
          ClientCertificates()
  {
    .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00003A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/ System.Net.Http.HttpClientHandler/*02000008*/::get_ClientCertificates() /* 0600005D */
  } // end of property HttpClientHandler::ClientCertificates
  .property /*1700000E*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/
          CookieContainer()
  {
    .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00003F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/ System.Net.Http.HttpClientHandler/*02000008*/::get_CookieContainer() /* 0600005E */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_CookieContainer(class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/) /* 0600005F */
  } // end of property HttpClientHandler::CookieContainer
  .property /*1700000F*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/
          Credentials()
  {
    .custom /*0C000043:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ System.Net.Http.HttpClientHandler/*02000008*/::get_Credentials() /* 06000060 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_Credentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/) /* 06000061 */
  } // end of property HttpClientHandler::Credentials
  .property /*17000010*/ callconv(8) class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool>
          DangerousAcceptAnyServerCertificateValidator()
  {
    .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 02 02 00 00 ) 
    .custom /*0C000047:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool> System.Net.Http.HttpClientHandler/*02000008*/::get_DangerousAcceptAnyServerCertificateValidator() /* 06000062 */
  } // end of property HttpClientHandler::DangerousAcceptAnyServerCertificateValidator
  .property /*17000011*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/
          DefaultProxyCredentials()
  {
    .custom /*0C00004A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ System.Net.Http.HttpClientHandler/*02000008*/::get_DefaultProxyCredentials() /* 06000063 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_DefaultProxyCredentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/) /* 06000064 */
  } // end of property HttpClientHandler::DefaultProxyCredentials
  .property /*17000012*/ instance callconv(8) int32
          MaxAutomaticRedirections()
  {
    .custom /*0C00004E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance int32 System.Net.Http.HttpClientHandler/*02000008*/::get_MaxAutomaticRedirections() /* 06000065 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_MaxAutomaticRedirections(int32) /* 06000066 */
  } // end of property HttpClientHandler::MaxAutomaticRedirections
  .property /*17000013*/ instance callconv(8) int32
          MaxConnectionsPerServer()
  {
    .custom /*0C000053:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance int32 System.Net.Http.HttpClientHandler/*02000008*/::get_MaxConnectionsPerServer() /* 06000067 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_MaxConnectionsPerServer(int32) /* 06000068 */
  } // end of property HttpClientHandler::MaxConnectionsPerServer
  .property /*17000014*/ instance callconv(8) int64
          MaxRequestContentBufferSize()
  {
    .get instance int64 System.Net.Http.HttpClientHandler/*02000008*/::get_MaxRequestContentBufferSize() /* 06000069 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_MaxRequestContentBufferSize(int64) /* 0600006A */
  } // end of property HttpClientHandler::MaxRequestContentBufferSize
  .property /*17000015*/ instance callconv(8) int32
          MaxResponseHeadersLength()
  {
    .custom /*0C00005A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance int32 System.Net.Http.HttpClientHandler/*02000008*/::get_MaxResponseHeadersLength() /* 0600006B */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_MaxResponseHeadersLength(int32) /* 0600006C */
  } // end of property HttpClientHandler::MaxResponseHeadersLength
  .property /*17000016*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/
          MeterFactory()
  {
    .custom /*0C00005F:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/ System.Net.Http.HttpClientHandler/*02000008*/::get_MeterFactory() /* 0600006D */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_MeterFactory(class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/) /* 0600006E */
  } // end of property HttpClientHandler::MeterFactory
  .property /*17000017*/ instance callconv(8) bool
          PreAuthenticate()
  {
    .custom /*0C000061:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_PreAuthenticate() /* 0600006F */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_PreAuthenticate(bool) /* 06000070 */
  } // end of property HttpClientHandler::PreAuthenticate
  .property /*17000018*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>
          Properties()
  {
    .custom /*0C000066:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object> System.Net.Http.HttpClientHandler/*02000008*/::get_Properties() /* 06000071 */
  } // end of property HttpClientHandler::Properties
  .property /*17000019*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/
          Proxy()
  {
    .custom /*0C00006B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .custom /*0C00006C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C00006D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ System.Net.Http.HttpClientHandler/*02000008*/::get_Proxy() /* 06000072 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_Proxy(class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/) /* 06000073 */
  } // end of property HttpClientHandler::Proxy
  .property /*1700001A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool>
          ServerCertificateCustomValidationCallback()
  {
    .custom /*0C000072:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    .custom /*0C000073:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool> System.Net.Http.HttpClientHandler/*02000008*/::get_ServerCertificateCustomValidationCallback() /* 06000074 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_ServerCertificateCustomValidationCallback(class [System.Runtime/*23000001*/]System.Func`5/*0100002D*/<class System.Net.Http.HttpRequestMessage/*02000013*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100002E*/,class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100002F*/,valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*01000030*/,bool>) /* 06000075 */
  } // end of property HttpClientHandler::ServerCertificateCustomValidationCallback
  .property /*1700001B*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000033*/
          SslProtocols()
  {
    .custom /*0C000077:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000033*/ System.Net.Http.HttpClientHandler/*02000008*/::get_SslProtocols() /* 06000076 */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_SslProtocols(valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000033*/) /* 06000077 */
  } // end of property HttpClientHandler::SslProtocols
  .property /*1700001C*/ instance callconv(8) bool
          SupportsAutomaticDecompression()
  {
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_SupportsAutomaticDecompression() /* 06000078 */
  } // end of property HttpClientHandler::SupportsAutomaticDecompression
  .property /*1700001D*/ instance callconv(8) bool
          SupportsProxy()
  {
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_SupportsProxy() /* 06000079 */
  } // end of property HttpClientHandler::SupportsProxy
  .property /*1700001E*/ instance callconv(8) bool
          SupportsRedirectConfiguration()
  {
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_SupportsRedirectConfiguration() /* 0600007A */
  } // end of property HttpClientHandler::SupportsRedirectConfiguration
  .property /*1700001F*/ instance callconv(8) bool
          UseCookies()
  {
    .custom /*0C000087:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_UseCookies() /* 0600007B */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_UseCookies(bool) /* 0600007C */
  } // end of property HttpClientHandler::UseCookies
  .property /*17000020*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .custom /*0C00008C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_UseDefaultCredentials() /* 0600007D */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_UseDefaultCredentials(bool) /* 0600007E */
  } // end of property HttpClientHandler::UseDefaultCredentials
  .property /*17000021*/ instance callconv(8) bool
          UseProxy()
  {
    .custom /*0C000090:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.Http.HttpClientHandler/*02000008*/::get_UseProxy() /* 0600007F */
    .set instance void System.Net.Http.HttpClientHandler/*02000008*/::set_UseProxy(bool) /* 06000080 */
  } // end of property HttpClientHandler::UseProxy
} // end of class System.Net.Http.HttpClientHandler

.class /*02000009*/ public auto ansi sealed System.Net.Http.HttpCompletionOption
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Net.Http.HttpCompletionOption/*02000009*/ ResponseContentRead = int32(0x00000000)
  .field /*04000006*/ public static literal valuetype System.Net.Http.HttpCompletionOption/*02000009*/ ResponseHeadersRead = int32(0x00000001)
} // end of class System.Net.Http.HttpCompletionOption

.class /*0200000A*/ public abstract auto ansi beforefieldinit System.Net.Http.HttpContent
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
{
  .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
  .custom /*0C000001:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000084*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21df
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpContent::.ctor

  .method /*06000085*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpContentHeaders/*02000028*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 80 A0
  {
    // Method begins at RVA 0x21e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::get_Headers

  .method /*06000086*/ public hidebysig instance void 
          CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 55 12 61 11 59
  {
    .param [2]/*080000AD*/ 
    .custom /*0C000144:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContent::CopyTo

  .method /*06000087*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 01 12 65 12 55
  {
    // Method begins at RVA 0x21ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CopyToAsync

  .method /*06000088*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                      class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context) cil managed
  // SIG: 20 02 12 65 12 55 12 61
  {
    .param [2]/*080000B1*/ 
    .custom /*0C000145:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CopyToAsync

  .method /*06000089*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                      class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*080000B3*/ 
    .custom /*0C000148:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CopyToAsync

  .method /*0600008A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 65 12 55 11 59
  {
    // Method begins at RVA 0x21f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CopyToAsync

  .method /*0600008B*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          CreateContentReadStream(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 55 11 59
  {
    // Method begins at RVA 0x21f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CreateContentReadStream

  .method /*0600008C*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync() cil managed
  // SIG: 20 00 15 12 5D 01 12 55
  {
    // Method begins at RVA 0x21fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CreateContentReadStreamAsync

  .method /*0600008D*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 5D 01 12 55 11 59
  {
    // Method begins at RVA 0x21fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::CreateContentReadStreamAsync

  .method /*0600008E*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2201
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContent::Dispose

  .method /*0600008F*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2203
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContent::Dispose

  .method /*06000090*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          LoadIntoBufferAsync() cil managed
  // SIG: 20 00 12 65
  {
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::LoadIntoBufferAsync

  .method /*06000091*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          LoadIntoBufferAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 65 11 59
  {
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::LoadIntoBufferAsync

  .method /*06000092*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          LoadIntoBufferAsync(int64 maxBufferSize) cil managed
  // SIG: 20 01 12 65 0A
  {
    // Method begins at RVA 0x220b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::LoadIntoBufferAsync

  .method /*06000093*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          LoadIntoBufferAsync(int64 maxBufferSize,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 65 0A 11 59
  {
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::LoadIntoBufferAsync

  .method /*06000094*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<uint8[]> 
          ReadAsByteArrayAsync() cil managed
  // SIG: 20 00 15 12 5D 01 1D 05
  {
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsByteArrayAsync

  .method /*06000095*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<uint8[]> 
          ReadAsByteArrayAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 5D 01 1D 05 11 59
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsByteArrayAsync

  .method /*06000096*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          ReadAsStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsStream

  .method /*06000097*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          ReadAsStream(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 55 11 59
  {
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsStream

  .method /*06000098*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          ReadAsStreamAsync() cil managed
  // SIG: 20 00 15 12 5D 01 12 55
  {
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsStreamAsync

  .method /*06000099*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          ReadAsStreamAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 5D 01 12 55 11 59
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsStreamAsync

  .method /*0600009A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<string> 
          ReadAsStringAsync() cil managed
  // SIG: 20 00 15 12 5D 01 0E
  {
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsStringAsync

  .method /*0600009B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<string> 
          ReadAsStringAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 5D 01 0E 11 59
  {
    // Method begins at RVA 0x2226
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::ReadAsStringAsync

  .method /*0600009C*/ family hidebysig newslot virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                           class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 55 12 61 11 59
  {
    .param [2]/*080000C3*/ 
    .custom /*0C000154:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2229
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContent::SerializeToStream

  .method /*0600009D*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context) cil managed
  // SIG: 20 02 12 65 12 55 12 61
  {
    .param [2]/*080000C6*/ 
    .custom /*0C000157:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method HttpContent::SerializeToStreamAsync

  .method /*0600009E*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*080000C8*/ 
    .custom /*0C000158:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContent::SerializeToStreamAsync

  .method /*0600009F*/ famorassem hidebysig newslot abstract virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x0
  } // end of method HttpContent::TryComputeLength

  .property /*17000022*/ instance callconv(8) class System.Net.Http.Headers.HttpContentHeaders/*02000028*/
          Headers()
  {
    .get instance class System.Net.Http.Headers.HttpContentHeaders/*02000028*/ System.Net.Http.HttpContent/*0200000A*/::get_Headers() /* 06000085 */
  } // end of property HttpContent::Headers
} // end of class System.Net.Http.HttpContent

.class /*0200000B*/ public auto ansi beforefieldinit System.Net.Http.HttpIOException
       extends [System.Runtime/*23000001*/]System.IO.IOException/*01000035*/
{
  .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Http.HttpRequestError/*02000011*/ httpRequestError,
                               [opt] string message,
                               [opt] class [System.Runtime/*23000001*/]System.Exception/*01000036*/ innerException) cil managed
  // SIG: 20 03 01 11 44 0E 12 80 D9
  {
    .custom /*0C00013E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000CC*/  = nullref
    .param [3]/*080000CD*/  = nullref
    // Method begins at RVA 0x222e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000040       */ call       instance void [System.Runtime/*23000001*/]System.IO.IOException/*01000035*/::.ctor() /* 0A000040 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpIOException::.ctor

  .method /*060000A1*/ public hidebysig specialname 
          instance valuetype System.Net.Http.HttpRequestError/*02000011*/ 
          get_HttpRequestError() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpIOException::get_HttpRequestError

  .method /*060000A2*/ public hidebysig specialname virtual 
          instance string  get_Message() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpIOException::get_Message

  .property /*17000023*/ instance callconv(8) valuetype System.Net.Http.HttpRequestError/*02000011*/
          HttpRequestError()
  {
    .get instance valuetype System.Net.Http.HttpRequestError/*02000011*/ System.Net.Http.HttpIOException/*0200000B*/::get_HttpRequestError() /* 060000A1 */
  } // end of property HttpIOException::HttpRequestError
  .property /*17000024*/ instance callconv(8) string
          Message()
  {
    .get instance string System.Net.Http.HttpIOException/*0200000B*/::get_Message() /* 060000A2 */
  } // end of property HttpIOException::Message
} // end of class System.Net.Http.HttpIOException

.class /*0200000C*/ public auto ansi sealed System.Net.Http.HttpKeepAlivePingPolicy
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000007*/ public specialname rtspecialname int32 value__
  .field /*04000008*/ public static literal valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/ WithActiveRequests = int32(0x00000000)
  .field /*04000009*/ public static literal valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/ Always = int32(0x00000001)
} // end of class System.Net.Http.HttpKeepAlivePingPolicy

.class /*0200000D*/ public abstract auto ansi beforefieldinit System.Net.Http.HttpMessageHandler
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
{
  .method /*060000A3*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x223c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpMessageHandler::.ctor

  .method /*060000A4*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2244
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpMessageHandler::Dispose

  .method /*060000A5*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2246
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpMessageHandler::Dispose

  .method /*060000A6*/ famorassem hidebysig newslot virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    .custom /*0C000141:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMessageHandler::Send

  .method /*060000A7*/ famorassem hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    .custom /*0C000142:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method HttpMessageHandler::SendAsync

} // end of class System.Net.Http.HttpMessageHandler

.class /*0200000E*/ public auto ansi beforefieldinit System.Net.Http.HttpMessageInvoker
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
{
  .custom /*0C00003B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/ 'handler') cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x224b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpMessageInvoker::.ctor

  .method /*060000A9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/ 'handler',
                               bool disposeHandler) cil managed
  // SIG: 20 02 01 12 34 02
  {
    // Method begins at RVA 0x2253
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpMessageInvoker::.ctor

  .method /*060000AA*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpMessageInvoker::Dispose

  .method /*060000AB*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x225d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpMessageInvoker::Dispose

  .method /*060000AC*/ public hidebysig newslot virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    .custom /*0C000143:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x225f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMessageInvoker::Send

  .method /*060000AD*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    // Method begins at RVA 0x2262
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMessageInvoker::SendAsync

} // end of class System.Net.Http.HttpMessageInvoker

.class /*0200000F*/ public auto ansi beforefieldinit System.Net.Http.HttpMethod
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000037*/<class System.Net.Http.HttpMethod/*0200000F*/>/*1B000001*/
{
  .custom /*0C000040:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*01000037*/<class System.Net.Http.HttpMethod/*0200000F*/>/*1B000001*/
  .custom /*0C00001E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*060000AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'method') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2265
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpMethod::.ctor

  .method /*060000AF*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Connect() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x226d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Connect

  .method /*060000B0*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Delete() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2270
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Delete

  .method /*060000B1*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Get() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2273
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Get

  .method /*060000B2*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Head() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2276
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Head

  .method /*060000B3*/ public hidebysig specialname 
          instance string  get_Method() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2279
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Method

  .method /*060000B4*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Options() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x227c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Options

  .method /*060000B5*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Patch() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Patch

  .method /*060000B6*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Post() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Post

  .method /*060000B7*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Put() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Put

  .method /*060000B8*/ public hidebysig specialname static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Trace() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::get_Trace

  .method /*060000B9*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Net.Http.HttpMethod/*0200000F*/ other) cil managed
  // SIG: 20 01 02 12 3C
  {
    .custom /*0C00014A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000DC*/ 
    .custom /*0C000161:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::Equals

  .method /*060000BA*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00014C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000DD*/ 
    .custom /*0C000162:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::Equals

  .method /*060000BB*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::GetHashCode

  .method /*060000BC*/ public hidebysig specialname static 
          bool  op_Equality(class System.Net.Http.HttpMethod/*0200000F*/ left,
                            class System.Net.Http.HttpMethod/*0200000F*/ right) cil managed
  // SIG: 00 02 02 12 3C 12 3C
  {
    .custom /*0C00014D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::op_Equality

  .method /*060000BD*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Net.Http.HttpMethod/*0200000F*/ left,
                              class System.Net.Http.HttpMethod/*0200000F*/ right) cil managed
  // SIG: 00 02 02 12 3C 12 3C
  {
    .custom /*0C00014E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2297
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::op_Inequality

  .method /*060000BE*/ public hidebysig static 
          class System.Net.Http.HttpMethod/*0200000F*/ 
          Parse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000039*/<char> 'method') cil managed
  // SIG: 00 01 12 3C 15 11 80 E5 01 03
  {
    .custom /*0C000150:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000E2*/ 
    .custom /*0C000163:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x229a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::Parse

  .method /*060000BF*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMethod::ToString

  .property /*17000025*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Connect()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Connect() /* 060000AF */
  } // end of property HttpMethod::Connect
  .property /*17000026*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Delete()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Delete() /* 060000B0 */
  } // end of property HttpMethod::Delete
  .property /*17000027*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Get()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Get() /* 060000B1 */
  } // end of property HttpMethod::Get
  .property /*17000028*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Head()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Head() /* 060000B2 */
  } // end of property HttpMethod::Head
  .property /*17000029*/ instance callconv(8) string
          Method()
  {
    .get instance string System.Net.Http.HttpMethod/*0200000F*/::get_Method() /* 060000B3 */
  } // end of property HttpMethod::Method
  .property /*1700002A*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Options()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Options() /* 060000B4 */
  } // end of property HttpMethod::Options
  .property /*1700002B*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Patch()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Patch() /* 060000B5 */
  } // end of property HttpMethod::Patch
  .property /*1700002C*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Post()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Post() /* 060000B6 */
  } // end of property HttpMethod::Post
  .property /*1700002D*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Put()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Put() /* 060000B7 */
  } // end of property HttpMethod::Put
  .property /*1700002E*/ callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Trace()
  {
    .get class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpMethod/*0200000F*/::get_Trace() /* 060000B8 */
  } // end of property HttpMethod::Trace
} // end of class System.Net.Http.HttpMethod

.class /*02000010*/ public auto ansi sealed beforefieldinit System.Net.Http.HttpProtocolException
       extends System.Net.Http.HttpIOException/*0200000B*/
{
  .method /*060000C0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int64 errorCode,
                               string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000036*/ innerException) cil managed
  // SIG: 20 03 01 0A 0E 12 80 D9
  {
    .custom /*0C000151:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000E6*/ 
    .custom /*0C000165:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a0
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (06)0000A0       */ call       instance void System.Net.Http.HttpIOException/*0200000B*/::.ctor(valuetype System.Net.Http.HttpRequestError/*02000011*/,
                                                                                                                        string,
                                                                                                                        class [System.Runtime/*23000001*/]System.Exception/*01000036*/) /* 060000A0 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method HttpProtocolException::.ctor

  .method /*060000C1*/ public hidebysig specialname 
          instance int64  get_ErrorCode() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpProtocolException::get_ErrorCode

  .property /*1700002F*/ instance callconv(8) int64
          ErrorCode()
  {
    .get instance int64 System.Net.Http.HttpProtocolException/*02000010*/::get_ErrorCode() /* 060000C1 */
  } // end of property HttpProtocolException::ErrorCode
} // end of class System.Net.Http.HttpProtocolException

.class /*02000011*/ public auto ansi sealed System.Net.Http.HttpRequestError
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*0400000A*/ public specialname rtspecialname int32 value__
  .field /*0400000B*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ Unknown = int32(0x00000000)
  .field /*0400000C*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ NameResolutionError = int32(0x00000001)
  .field /*0400000D*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ ConnectionError = int32(0x00000002)
  .field /*0400000E*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ SecureConnectionError = int32(0x00000003)
  .field /*0400000F*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ HttpProtocolError = int32(0x00000004)
  .field /*04000010*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ ExtendedConnectNotSupported = int32(0x00000005)
  .field /*04000011*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ VersionNegotiationError = int32(0x00000006)
  .field /*04000012*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ UserAuthenticationError = int32(0x00000007)
  .field /*04000013*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ ProxyTunnelError = int32(0x00000008)
  .field /*04000014*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ InvalidResponse = int32(0x00000009)
  .field /*04000015*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ ResponseEnded = int32(0x0000000A)
  .field /*04000016*/ public static literal valuetype System.Net.Http.HttpRequestError/*02000011*/ ConfigurationLimitExceeded = int32(0x0000000B)
} // end of class System.Net.Http.HttpRequestError

.class /*02000012*/ public auto ansi beforefieldinit System.Net.Http.HttpRequestException
       extends [System.Runtime/*23000001*/]System.Exception/*01000036*/
{
  .custom /*0C00004B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000C2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22ae
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000041       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000036*/::.ctor() /* 0A000041 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestException::.ctor

  .method /*060000C3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Http.HttpRequestError/*02000011*/ httpRequestError,
                               [opt] string message,
                               [opt] class [System.Runtime/*23000001*/]System.Exception/*01000036*/ inner,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/> statusCode) cil managed
  // SIG: 20 04 01 11 44 0E 12 80 D9 15 11 80 E9 01 11 80 ED
  {
    .param [2]/*080000E8*/  = nullref
    .param [3]/*080000E9*/  = nullref
    .param [4]/*080000EA*/  = nullref
    // Method begins at RVA 0x22b6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000041       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000036*/::.ctor() /* 0A000041 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestException::.ctor

  .method /*060000C4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22be
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000041       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000036*/::.ctor() /* 0A000041 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestException::.ctor

  .method /*060000C5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000036*/ inner) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    // Method begins at RVA 0x22c6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000041       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000036*/::.ctor() /* 0A000041 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestException::.ctor

  .method /*060000C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000036*/ inner,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/> statusCode) cil managed
  // SIG: 20 03 01 0E 12 80 D9 15 11 80 E9 01 11 80 ED
  {
    // Method begins at RVA 0x22ce
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000041       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000036*/::.ctor() /* 0A000041 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestException::.ctor

  .method /*060000C7*/ public hidebysig specialname 
          instance valuetype System.Net.Http.HttpRequestError/*02000011*/ 
          get_HttpRequestError() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x22d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestException::get_HttpRequestError

  .method /*060000C8*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/> 
          get_StatusCode() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 ED
  {
    // Method begins at RVA 0x22d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestException::get_StatusCode

  .property /*17000030*/ instance callconv(8) valuetype System.Net.Http.HttpRequestError/*02000011*/
          HttpRequestError()
  {
    .get instance valuetype System.Net.Http.HttpRequestError/*02000011*/ System.Net.Http.HttpRequestException/*02000012*/::get_HttpRequestError() /* 060000C7 */
  } // end of property HttpRequestException::HttpRequestError
  .property /*17000031*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/>
          StatusCode()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/> System.Net.Http.HttpRequestException/*02000012*/::get_StatusCode() /* 060000C8 */
  } // end of property HttpRequestException::StatusCode
} // end of class System.Net.Http.HttpRequestException

.class /*02000013*/ public auto ansi beforefieldinit System.Net.Http.HttpRequestMessage
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
{
  .custom /*0C000050:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000051:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22dc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::.ctor

  .method /*060000CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMethod/*0200000F*/ 'method',
                               string requestUri) cil managed
  // SIG: 20 02 01 12 3C 0E
  {
    .param [2]/*080000F2*/ 
    .custom /*0C00016B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00016C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000027*/::.ctor(string) /* 0A000016 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x22e4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::.ctor

  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMethod/*0200000F*/ 'method',
                               class [System.Runtime/*23000001*/]System.Uri/*01000023*/ requestUri) cil managed
  // SIG: 20 02 01 12 3C 12 80 8D
  {
    .param [2]/*080000F4*/ 
    .custom /*0C00016D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ec
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::.ctor

  .method /*060000CC*/ public hidebysig specialname 
          instance class System.Net.Http.HttpContent/*0200000A*/ 
          get_Content() cil managed
  // SIG: 20 00 12 28
  {
    .custom /*0C000159:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_Content

  .method /*060000CD*/ public hidebysig specialname 
          instance void  set_Content(class System.Net.Http.HttpContent/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 12 28
  {
    .custom /*0C00015A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::set_Content

  .method /*060000CE*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 80 B0
  {
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_Headers

  .method /*060000CF*/ public hidebysig specialname 
          instance class System.Net.Http.HttpMethod/*0200000F*/ 
          get_Method() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_Method

  .method /*060000D0*/ public hidebysig specialname 
          instance void  set_Method(class System.Net.Http.HttpMethod/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x22ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::set_Method

  .method /*060000D1*/ public hidebysig specialname 
          instance class System.Net.Http.HttpRequestOptions/*02000014*/ 
          get_Options() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x2301
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_Options

  .method /*060000D2*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object> 
          get_Properties() cil managed
  // SIG: 20 00 15 12 80 C9 02 0E 1C
  {
    .param [0]/*080000F7*/ 
    .custom /*0C00016E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x2304
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_Properties

  .method /*060000D3*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 
          get_RequestUri() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C00015C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2307
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_RequestUri

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_RequestUri(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C00015E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x230a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::set_RequestUri

  .method /*060000D5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ 
          get_Version() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_Version

  .method /*060000D6*/ public hidebysig specialname 
          instance void  set_Version(class [System.Runtime/*23000001*/]System.Version/*01000025*/ 'value') cil managed
  // SIG: 20 01 01 12 80 95
  {
    // Method begins at RVA 0x230f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::set_Version

  .method /*060000D7*/ public hidebysig specialname 
          instance valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ 
          get_VersionPolicy() cil managed
  // SIG: 20 00 11 5C
  {
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::get_VersionPolicy

  .method /*060000D8*/ public hidebysig specialname 
          instance void  set_VersionPolicy(valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 11 5C
  {
    // Method begins at RVA 0x2314
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::set_VersionPolicy

  .method /*060000D9*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2316
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::Dispose

  .method /*060000DA*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2318
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestMessage::Dispose

  .method /*060000DB*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x231a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestMessage::ToString

  .property /*17000032*/ instance callconv(8) class System.Net.Http.HttpContent/*0200000A*/
          Content()
  {
    .custom /*0C0000C8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.HttpContent/*0200000A*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_Content() /* 060000CC */
    .set instance void System.Net.Http.HttpRequestMessage/*02000013*/::set_Content(class System.Net.Http.HttpContent/*0200000A*/) /* 060000CD */
  } // end of property HttpRequestMessage::Content
  .property /*17000033*/ instance callconv(8) class System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/
          Headers()
  {
    .get instance class System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_Headers() /* 060000CE */
  } // end of property HttpRequestMessage::Headers
  .property /*17000034*/ instance callconv(8) class System.Net.Http.HttpMethod/*0200000F*/
          Method()
  {
    .get instance class System.Net.Http.HttpMethod/*0200000F*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_Method() /* 060000CF */
    .set instance void System.Net.Http.HttpRequestMessage/*02000013*/::set_Method(class System.Net.Http.HttpMethod/*0200000F*/) /* 060000D0 */
  } // end of property HttpRequestMessage::Method
  .property /*17000035*/ instance callconv(8) class System.Net.Http.HttpRequestOptions/*02000014*/
          Options()
  {
    .get instance class System.Net.Http.HttpRequestOptions/*02000014*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_Options() /* 060000D1 */
  } // end of property HttpRequestMessage::Options
  .property /*17000036*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>
          Properties()
  {
    .custom /*0C0000D3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .custom /*0C0000D4:0A000019*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100003C*/::.ctor(string) /* 0A000019 */ = ( 01 00 47 48 74 74 70 52 65 71 75 65 73 74 4D 65   // ..GHttpRequestMe
                                                                                                                                                   73 73 61 67 65 2E 50 72 6F 70 65 72 74 69 65 73   // ssage.Properties
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 2E 20 55 73 65 20 4F 70 74 69 6F 6E   // ated. Use Option
                                                                                                                                                   73 20 69 6E 73 74 65 61 64 2E 00 00 )             // s instead...
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object> System.Net.Http.HttpRequestMessage/*02000013*/::get_Properties() /* 060000D2 */
  } // end of property HttpRequestMessage::Properties
  .property /*17000037*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000023*/
          RequestUri()
  {
    .custom /*0C0000D5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_RequestUri() /* 060000D3 */
    .set instance void System.Net.Http.HttpRequestMessage/*02000013*/::set_RequestUri(class [System.Runtime/*23000001*/]System.Uri/*01000023*/) /* 060000D4 */
  } // end of property HttpRequestMessage::RequestUri
  .property /*17000038*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000025*/
          Version()
  {
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_Version() /* 060000D5 */
    .set instance void System.Net.Http.HttpRequestMessage/*02000013*/::set_Version(class [System.Runtime/*23000001*/]System.Version/*01000025*/) /* 060000D6 */
  } // end of property HttpRequestMessage::Version
  .property /*17000039*/ instance callconv(8) valuetype System.Net.Http.HttpVersionPolicy/*02000017*/
          VersionPolicy()
  {
    .get instance valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ System.Net.Http.HttpRequestMessage/*02000013*/::get_VersionPolicy() /* 060000D7 */
    .set instance void System.Net.Http.HttpRequestMessage/*02000013*/::set_VersionPolicy(valuetype System.Net.Http.HttpVersionPolicy/*02000017*/) /* 060000D8 */
  } // end of property HttpRequestMessage::VersionPolicy
} // end of class System.Net.Http.HttpRequestMessage

.class /*02000014*/ public auto ansi sealed beforefieldinit System.Net.Http.HttpRequestOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000006*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000004*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/
{
  .custom /*0C000054:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000055:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/
  .custom /*0C000022:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000006*/
  .custom /*0C000026:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/
  .custom /*0C00002A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000004*/
  .custom /*0C00002E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/
  .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .method /*060000DC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x231d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::.ctor

  .method /*060000DD*/ private hidebysig newslot specialname virtual final 
          instance int32  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'() cil managed
  // SIG: 20 00 08
  {
    .override  method instance int32 class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::get_Count() /* 0A00001A */
    // Method begins at RVA 0x2325
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'

  .method /*060000DE*/ private hidebysig newslot specialname virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_IsReadOnly'() cil managed
  // SIG: 20 00 02
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::get_IsReadOnly() /* 0A00001B */
    // Method begins at RVA 0x2328
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_IsReadOnly'

  .method /*060000DF*/ private hidebysig newslot specialname virtual final 
          instance object  'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Item'(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .override  method instance !1 class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::get_Item(!0) /* 0A00001C */
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Item'

  .method /*060000E0*/ private hidebysig newslot specialname virtual final 
          instance void  'System.Collections.Generic.IDictionary<System.String,System.Object>.set_Item'(string key,
                                                                                                        object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::set_Item(!0,
                                                                                                                                                                !1) /* 0A00001D */
    // Method begins at RVA 0x232e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.set_Item'

  .method /*060000E1*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> 
          'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Keys'() cil managed
  // SIG: 20 00 15 12 80 F5 01 0E
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::get_Keys() /* 0A00001E */
    // Method begins at RVA 0x2330
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Keys'

  .method /*060000E2*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<object> 
          'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Values'() cil managed
  // SIG: 20 00 15 12 80 F5 01 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<!1> class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::get_Values() /* 0A00001F */
    // Method begins at RVA 0x2333
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Values'

  .method /*060000E3*/ private hidebysig newslot specialname virtual final 
          instance int32  'System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'() cil managed
  // SIG: 20 00 08
  {
    .override  method instance int32 class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000004*/::get_Count() /* 0A000020 */
    // Method begins at RVA 0x2336
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'

  .method /*060000E4*/ private hidebysig newslot specialname virtual final 
          instance object  'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Item'(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .override  method instance !1 class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/::get_Item(!0) /* 0A000021 */
    // Method begins at RVA 0x2339
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Item'

  .method /*060000E5*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> 
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Keys'() cil managed
  // SIG: 20 00 15 12 71 01 0E
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/::get_Keys() /* 0A000022 */
    // Method begins at RVA 0x233c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Keys'

  .method /*060000E6*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<object> 
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Values'() cil managed
  // SIG: 20 00 15 12 71 01 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<!1> class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/::get_Values() /* 0A000023 */
    // Method begins at RVA 0x233f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Values'

  .method /*060000E7*/ public hidebysig instance void 
          Set<TValue>(valuetype System.Net.Http.HttpRequestOptionsKey`1/*02000015*/<!!TValue> key,
                      !!TValue 'value') cil managed
  // SIG: 30 01 02 01 15 11 54 01 1E 00 1E 00
  {
    .param type TValue /*2A000004*/ 
      .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000100*/ 
    .custom /*0C000174:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2342
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::Set

  .method /*060000E8*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object> item) cil managed
  // SIG: 20 01 01 15 11 75 02 0E 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::Add(!0) /* 0A000024 */
    // Method begins at RVA 0x2344
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add'

  .method /*060000E9*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Clear'() cil managed
  // SIG: 20 00 01
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::Clear() /* 0A000025 */
    // Method begins at RVA 0x2346
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Clear'

  .method /*060000EA*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 75 02 0E 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::Contains(!0) /* 0A000026 */
    // Method begins at RVA 0x2348
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains'

  .method /*060000EB*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>[] 'array',
                                                                                                                                               int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 75 02 0E 1C 08
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::CopyTo(!0[],
                                                                                                                                                                                                                                                           int32) /* 0A000027 */
    // Method begins at RVA 0x234b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo'

  .method /*060000EC*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 75 02 0E 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000002*/::Remove(!0) /* 0A000028 */
    // Method begins at RVA 0x234d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove'

  .method /*060000ED*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.IDictionary<System.String,System.Object>.Add'(string key,
                                                                                                   object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::Add(!0,
                                                                                                                                                           !1) /* 0A000029 */
    // Method begins at RVA 0x2350
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.Add'

  .method /*060000EE*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.String,System.Object>.ContainsKey'(string key) cil managed
  // SIG: 20 01 02 0E
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::ContainsKey(!0) /* 0A00002A */
    // Method begins at RVA 0x2352
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.ContainsKey'

  .method /*060000EF*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.String,System.Object>.Remove'(string key) cil managed
  // SIG: 20 01 02 0E
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::Remove(!0) /* 0A00002B */
    // Method begins at RVA 0x2355
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.Remove'

  .method /*060000F0*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue'(string key,
                                                                                                           [out] object& 'value') cil managed
  // SIG: 20 02 02 0E 10 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>/*1B000003*/::TryGetValue(!0,
                                                                                                                                                                   !1&) /* 0A00002C */
    // Method begins at RVA 0x2358
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue'

  .method /*060000F1*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 81 01 01 15 11 75 02 0E 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,object>>/*1B000006*/::GetEnumerator() /* 0A00002D */
    // Method begins at RVA 0x235b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'

  .method /*060000F2*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.ContainsKey'(string key) cil managed
  // SIG: 20 01 02 0E
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/::ContainsKey(!0) /* 0A00002E */
    // Method begins at RVA 0x235e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.ContainsKey'

  .method /*060000F3*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.TryGetValue'(string key,
                                                                                                                   [out] object& 'value') cil managed
  // SIG: 20 02 02 0E 10 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,object>/*1B000005*/::TryGetValue(!0,
                                                                                                                                                                           !1&) /* 0A00002F */
    // Method begins at RVA 0x2361
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.TryGetValue'

  .method /*060000F4*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 09
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/::GetEnumerator /*01000041::0A000030*/ 
    // Method begins at RVA 0x2364
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::System.Collections.IEnumerable.GetEnumerator

  .method /*060000F5*/ public hidebysig instance bool 
          TryGetValue<TValue>(valuetype System.Net.Http.HttpRequestOptionsKey`1/*02000015*/<!!TValue> key,
                              [out] !!TValue& 'value') cil managed
  // SIG: 30 01 02 02 15 11 54 01 1E 00 10 1E 00
  {
    .param type TValue /*2A000005*/ 
      .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000110*/ 
    .custom /*0C00017A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000111*/ 
    .custom /*0C00017B:0A000031*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000043*/::.ctor(bool) /* 0A000031 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2367
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptions::TryGetValue

  .property /*1700003A*/ instance callconv(8) int32
          'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Count'()
  {
    .get instance int32 System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'() /* 060000DD */
  } // end of property HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Count'
  .property /*1700003B*/ instance callconv(8) bool
          'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.IsReadOnly'()
  {
    .get instance bool System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_IsReadOnly'() /* 060000DE */
  } // end of property HttpRequestOptions::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.IsReadOnly'
  .property /*1700003C*/ instance callconv(8) object
          'System.Collections.Generic.IDictionary<System.String,System.Object>.Item'(string)
  {
    .custom /*0C0000E4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Item'(string) /* 060000DF */
    .set instance void System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.set_Item'(string,
                                                                                                                                                      object) /* 060000E0 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.Item'
  .property /*1700003D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string>
          'System.Collections.Generic.IDictionary<System.String,System.Object>.Keys'()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Keys'() /* 060000E1 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.Keys'
  .property /*1700003E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<object>
          'System.Collections.Generic.IDictionary<System.String,System.Object>.Values'()
  {
    .custom /*0C0000E8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<object> System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Values'() /* 060000E2 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IDictionary<System.String,System.Object>.Values'
  .property /*1700003F*/ instance callconv(8) int32
          'System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Count'()
  {
    .get instance int32 System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'() /* 060000E3 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Count'
  .property /*17000040*/ instance callconv(8) object
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.Item'(string)
  {
    .custom /*0C0000EB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Item'(string) /* 060000E4 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.Item'
  .property /*17000041*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.Keys'()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Keys'() /* 060000E5 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.Keys'
  .property /*17000042*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<object>
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.Values'()
  {
    .custom /*0C0000EE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<object> System.Net.Http.HttpRequestOptions/*02000014*/::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.get_Values'() /* 060000E6 */
  } // end of property HttpRequestOptions::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Object>.Values'
} // end of class System.Net.Http.HttpRequestOptions

.class /*02000015*/ public sequential ansi sealed beforefieldinit System.Net.Http.HttpRequestOptionsKey`1<TValue>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000045*/
{
  .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000058:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000059:0A000032*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000044*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
  .param type TValue /*2A000002*/ 
    .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .field /*04000017*/ private initonly object _dummy
  .field /*04000018*/ private initonly int32 _dummyPrimitive
  .method /*060000F6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string key) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x236a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptionsKey`1::.ctor

  .method /*060000F7*/ public hidebysig specialname 
          instance string  get_Key() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x236d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestOptionsKey`1::get_Key

  .property /*17000043*/ instance callconv(8) string
          Key()
  {
    .get instance string System.Net.Http.HttpRequestOptionsKey`1/*02000015*/::get_Key() /* 060000F7 */
  } // end of property HttpRequestOptionsKey`1::Key
} // end of class System.Net.Http.HttpRequestOptionsKey`1

.class /*02000016*/ public auto ansi beforefieldinit System.Net.Http.HttpResponseMessage
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
{
  .custom /*0C00005B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
  .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000F8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2370
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::.ctor

  .method /*060000F9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/ statusCode) cil managed
  // SIG: 20 01 01 11 80 ED
  {
    // Method begins at RVA 0x2378
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::.ctor

  .method /*060000FA*/ public hidebysig specialname 
          instance class System.Net.Http.HttpContent/*0200000A*/ 
          get_Content() cil managed
  // SIG: 20 00 12 28
  {
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_Content

  .method /*060000FB*/ public hidebysig specialname 
          instance void  set_Content(class System.Net.Http.HttpContent/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 12 28
  {
    .param [1]/*08000114*/ 
    .custom /*0C00017C:0A000033*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000046*/::.ctor() /* 0A000033 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2383
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::set_Content

  .method /*060000FC*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 80 B4
  {
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_Headers

  .method /*060000FD*/ public hidebysig specialname 
          instance bool  get_IsSuccessStatusCode() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_IsSuccessStatusCode

  .method /*060000FE*/ public hidebysig specialname 
          instance string  get_ReasonPhrase() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000171:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_ReasonPhrase

  .method /*060000FF*/ public hidebysig specialname 
          instance void  set_ReasonPhrase(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000172:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::set_ReasonPhrase

  .method /*06000100*/ public hidebysig specialname 
          instance class System.Net.Http.HttpRequestMessage/*02000013*/ 
          get_RequestMessage() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C000173:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2390
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_RequestMessage

  .method /*06000101*/ public hidebysig specialname 
          instance void  set_RequestMessage(class System.Net.Http.HttpRequestMessage/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C000175:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2393
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::set_RequestMessage

  .method /*06000102*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/ 
          get_StatusCode() cil managed
  // SIG: 20 00 11 80 ED
  {
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_StatusCode

  .method /*06000103*/ public hidebysig specialname 
          instance void  set_StatusCode(valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/ 'value') cil managed
  // SIG: 20 01 01 11 80 ED
  {
    // Method begins at RVA 0x2398
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::set_StatusCode

  .method /*06000104*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/ 
          get_TrailingHeaders() cil managed
  // SIG: 20 00 12 80 B4
  {
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_TrailingHeaders

  .method /*06000105*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ 
          get_Version() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x239d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::get_Version

  .method /*06000106*/ public hidebysig specialname 
          instance void  set_Version(class [System.Runtime/*23000001*/]System.Version/*01000025*/ 'value') cil managed
  // SIG: 20 01 01 12 80 95
  {
    // Method begins at RVA 0x23a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::set_Version

  .method /*06000107*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::Dispose

  .method /*06000108*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23a4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseMessage::Dispose

  .method /*06000109*/ public hidebysig instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          EnsureSuccessStatusCode() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::EnsureSuccessStatusCode

  .method /*0600010A*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseMessage::ToString

  .property /*17000044*/ instance callconv(8) class System.Net.Http.HttpContent/*0200000A*/
          Content()
  {
    .get instance class System.Net.Http.HttpContent/*0200000A*/ System.Net.Http.HttpResponseMessage/*02000016*/::get_Content() /* 060000FA */
    .set instance void System.Net.Http.HttpResponseMessage/*02000016*/::set_Content(class System.Net.Http.HttpContent/*0200000A*/) /* 060000FB */
  } // end of property HttpResponseMessage::Content
  .property /*17000045*/ instance callconv(8) class System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/
          Headers()
  {
    .get instance class System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/ System.Net.Http.HttpResponseMessage/*02000016*/::get_Headers() /* 060000FC */
  } // end of property HttpResponseMessage::Headers
  .property /*17000046*/ instance callconv(8) bool
          IsSuccessStatusCode()
  {
    .get instance bool System.Net.Http.HttpResponseMessage/*02000016*/::get_IsSuccessStatusCode() /* 060000FD */
  } // end of property HttpResponseMessage::IsSuccessStatusCode
  .property /*17000047*/ instance callconv(8) string
          ReasonPhrase()
  {
    .custom /*0C0000F9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.HttpResponseMessage/*02000016*/::get_ReasonPhrase() /* 060000FE */
    .set instance void System.Net.Http.HttpResponseMessage/*02000016*/::set_ReasonPhrase(string) /* 060000FF */
  } // end of property HttpResponseMessage::ReasonPhrase
  .property /*17000048*/ instance callconv(8) class System.Net.Http.HttpRequestMessage/*02000013*/
          RequestMessage()
  {
    .custom /*0C0000FC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.HttpRequestMessage/*02000013*/ System.Net.Http.HttpResponseMessage/*02000016*/::get_RequestMessage() /* 06000100 */
    .set instance void System.Net.Http.HttpResponseMessage/*02000016*/::set_RequestMessage(class System.Net.Http.HttpRequestMessage/*02000013*/) /* 06000101 */
  } // end of property HttpResponseMessage::RequestMessage
  .property /*17000049*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/
          StatusCode()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/ System.Net.Http.HttpResponseMessage/*02000016*/::get_StatusCode() /* 06000102 */
    .set instance void System.Net.Http.HttpResponseMessage/*02000016*/::set_StatusCode(valuetype [System.Net.Primitives/*23000002*/]System.Net.HttpStatusCode/*0100003B*/) /* 06000103 */
  } // end of property HttpResponseMessage::StatusCode
  .property /*1700004A*/ instance callconv(8) class System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/
          TrailingHeaders()
  {
    .get instance class System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/ System.Net.Http.HttpResponseMessage/*02000016*/::get_TrailingHeaders() /* 06000104 */
  } // end of property HttpResponseMessage::TrailingHeaders
  .property /*1700004B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000025*/
          Version()
  {
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ System.Net.Http.HttpResponseMessage/*02000016*/::get_Version() /* 06000105 */
    .set instance void System.Net.Http.HttpResponseMessage/*02000016*/::set_Version(class [System.Runtime/*23000001*/]System.Version/*01000025*/) /* 06000106 */
  } // end of property HttpResponseMessage::Version
} // end of class System.Net.Http.HttpResponseMessage

.class /*02000017*/ public auto ansi sealed System.Net.Http.HttpVersionPolicy
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000019*/ public specialname rtspecialname int32 value__
  .field /*0400001A*/ public static literal valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ RequestVersionOrLower = int32(0x00000000)
  .field /*0400001B*/ public static literal valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ RequestVersionOrHigher = int32(0x00000001)
  .field /*0400001C*/ public static literal valuetype System.Net.Http.HttpVersionPolicy/*02000017*/ RequestVersionExact = int32(0x00000002)
} // end of class System.Net.Http.HttpVersionPolicy

.class /*02000018*/ public abstract auto ansi beforefieldinit System.Net.Http.MessageProcessingHandler
       extends System.Net.Http.DelegatingHandler/*02000004*/
{
  .custom /*0C000062:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000063:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600010B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000009       */ call       instance void System.Net.Http.DelegatingHandler/*02000004*/::.ctor() /* 06000009 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MessageProcessingHandler::.ctor

  .method /*0600010C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.HttpMessageHandler/*0200000D*/ innerHandler) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x23b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000009       */ call       instance void System.Net.Http.DelegatingHandler/*02000004*/::.ctor() /* 06000009 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MessageProcessingHandler::.ctor

  .method /*0600010D*/ family hidebysig newslot abstract virtual 
          instance class System.Net.Http.HttpRequestMessage/*02000013*/ 
          ProcessRequest(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                         valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 4C 12 4C 11 59
  {
    // Method begins at RVA 0x0
  } // end of method MessageProcessingHandler::ProcessRequest

  .method /*0600010E*/ family hidebysig newslot abstract virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          ProcessResponse(class System.Net.Http.HttpResponseMessage/*02000016*/ response,
                          valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 58 11 59
  {
    // Method begins at RVA 0x0
  } // end of method MessageProcessingHandler::ProcessResponse

  .method /*0600010F*/ famorassem hidebysig virtual final 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    // Method begins at RVA 0x23bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MessageProcessingHandler::Send

  .method /*06000110*/ famorassem hidebysig virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    // Method begins at RVA 0x23bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MessageProcessingHandler::SendAsync

} // end of class System.Net.Http.MessageProcessingHandler

.class /*02000019*/ public auto ansi beforefieldinit System.Net.Http.MultipartContent
       extends System.Net.Http.HttpContent/*0200000A*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<class System.Net.Http.HttpContent/*0200000A*/>/*1B000007*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
{
  .custom /*0C000068:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000069:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<class System.Net.Http.HttpContent/*0200000A*/>/*1B000007*/
  .custom /*0C000038:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
  .custom /*0C00003D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000111*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23c2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MultipartContent::.ctor

  .method /*06000112*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string subtype) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23ca
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MultipartContent::.ctor

  .method /*06000113*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string subtype,
                               string boundary) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x23d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MultipartContent::.ctor

  .method /*06000114*/ public hidebysig specialname 
          instance class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpContent/*0200000A*/> 
          get_HeaderEncodingSelector() cil managed
  // SIG: 20 00 15 12 18 01 12 28
  {
    .param [0]/*08000126*/ 
    .custom /*0C00017D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::get_HeaderEncodingSelector

  .method /*06000115*/ public hidebysig specialname 
          instance void  set_HeaderEncodingSelector(class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpContent/*0200000A*/> 'value') cil managed
  // SIG: 20 01 01 15 12 18 01 12 28
  {
    .param [1]/*08000127*/ 
    .custom /*0C00017F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartContent::set_HeaderEncodingSelector

  .method /*06000116*/ public hidebysig newslot virtual 
          instance void  Add(class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x23df
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartContent::Add

  .method /*06000117*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          CreateContentReadStream(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 55 11 59
  {
    // Method begins at RVA 0x23e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::CreateContentReadStream

  .method /*06000118*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync() cil managed
  // SIG: 20 00 15 12 5D 01 12 55
  {
    // Method begins at RVA 0x23e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::CreateContentReadStreamAsync

  .method /*06000119*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 5D 01 12 55 11 59
  {
    // Method begins at RVA 0x23e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::CreateContentReadStreamAsync

  .method /*0600011A*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartContent::Dispose

  .method /*0600011B*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<class System.Net.Http.HttpContent/*0200000A*/> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 81 01 01 12 28
  {
    // Method begins at RVA 0x23ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::GetEnumerator

  .method /*0600011C*/ family hidebysig virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                           class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 55 12 61 11 59
  {
    .param [2]/*0800012D*/ 
    .custom /*0C000180:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartContent::SerializeToStream

  .method /*0600011D*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context) cil managed
  // SIG: 20 02 12 65 12 55 12 61
  {
    .param [2]/*08000130*/ 
    .custom /*0C000181:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::SerializeToStreamAsync

  .method /*0600011E*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*08000132*/ 
    .custom /*0C000182:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::SerializeToStreamAsync

  .method /*0600011F*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 09
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/::GetEnumerator /*01000041::0A000030*/ 
    // Method begins at RVA 0x23f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::System.Collections.IEnumerable.GetEnumerator

  .method /*06000120*/ famorassem hidebysig virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x23fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartContent::TryComputeLength

  .property /*1700004C*/ instance callconv(8) class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpContent/*0200000A*/>
          HeaderEncodingSelector()
  {
    .custom /*0C000106:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpContent/*0200000A*/> System.Net.Http.MultipartContent/*02000019*/::get_HeaderEncodingSelector() /* 06000114 */
    .set instance void System.Net.Http.MultipartContent/*02000019*/::set_HeaderEncodingSelector(class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpContent/*0200000A*/>) /* 06000115 */
  } // end of property MultipartContent::HeaderEncodingSelector
} // end of class System.Net.Http.MultipartContent

.class /*0200001A*/ public auto ansi beforefieldinit System.Net.Http.MultipartFormDataContent
       extends System.Net.Http.MultipartContent/*02000019*/
{
  .custom /*0C00006F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000070:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000121*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23fd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000111       */ call       instance void System.Net.Http.MultipartContent/*02000019*/::.ctor() /* 06000111 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MultipartFormDataContent::.ctor

  .method /*06000122*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string boundary) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2405
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000111       */ call       instance void System.Net.Http.MultipartContent/*02000019*/::.ctor() /* 06000111 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MultipartFormDataContent::.ctor

  .method /*06000123*/ public hidebysig virtual 
          instance void  Add(class System.Net.Http.HttpContent/*0200000A*/ content) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x240d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartFormDataContent::Add

  .method /*06000124*/ public hidebysig instance void 
          Add(class System.Net.Http.HttpContent/*0200000A*/ content,
              string name) cil managed
  // SIG: 20 02 01 12 28 0E
  {
    // Method begins at RVA 0x240f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartFormDataContent::Add

  .method /*06000125*/ public hidebysig instance void 
          Add(class System.Net.Http.HttpContent/*0200000A*/ content,
              string name,
              string fileName) cil managed
  // SIG: 20 03 01 12 28 0E 0E
  {
    // Method begins at RVA 0x2411
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MultipartFormDataContent::Add

  .method /*06000126*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*0800013D*/ 
    .custom /*0C000185:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2413
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartFormDataContent::SerializeToStreamAsync

} // end of class System.Net.Http.MultipartFormDataContent

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Net.Http.ReadOnlyMemoryContent
       extends System.Net.Http.HttpContent/*0200000A*/
{
  .custom /*0C000074:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000075:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000127*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000047*/<uint8> content) cil managed
  // SIG: 20 01 01 15 11 81 1D 01 05
  {
    .custom /*0C00017E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2416
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlyMemoryContent::.ctor

  .method /*06000128*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          CreateContentReadStream(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 55 11 59
  {
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyMemoryContent::CreateContentReadStream

  .method /*06000129*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync() cil managed
  // SIG: 20 00 15 12 5D 01 12 55
  {
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyMemoryContent::CreateContentReadStreamAsync

  .method /*0600012A*/ family hidebysig virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                           class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 55 12 61 11 59
  {
    .param [2]/*08000142*/ 
    .custom /*0C000186:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2424
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyMemoryContent::SerializeToStream

  .method /*0600012B*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context) cil managed
  // SIG: 20 02 12 65 12 55 12 61
  {
    .param [2]/*08000145*/ 
    .custom /*0C000187:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2426
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyMemoryContent::SerializeToStreamAsync

  .method /*0600012C*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*08000147*/ 
    .custom /*0C000188:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyMemoryContent::SerializeToStreamAsync

  .method /*0600012D*/ famorassem hidebysig virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyMemoryContent::TryComputeLength

} // end of class System.Net.Http.ReadOnlyMemoryContent

.class /*0200001C*/ public auto ansi sealed beforefieldinit System.Net.Http.SocketsHttpConnectionContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
{
  .custom /*0C000078:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000079:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600012E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x242f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SocketsHttpConnectionContext::.ctor

  .method /*0600012F*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.DnsEndPoint/*01000048*/ 
          get_DnsEndPoint() cil managed
  // SIG: 20 00 12 81 21
  {
    // Method begins at RVA 0x2437
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpConnectionContext::get_DnsEndPoint

  .method /*06000130*/ public hidebysig specialname 
          instance class System.Net.Http.HttpRequestMessage/*02000013*/ 
          get_InitialRequestMessage() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x243a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpConnectionContext::get_InitialRequestMessage

  .property /*1700004D*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.DnsEndPoint/*01000048*/
          DnsEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.DnsEndPoint/*01000048*/ System.Net.Http.SocketsHttpConnectionContext/*0200001C*/::get_DnsEndPoint() /* 0600012F */
  } // end of property SocketsHttpConnectionContext::DnsEndPoint
  .property /*1700004E*/ instance callconv(8) class System.Net.Http.HttpRequestMessage/*02000013*/
          InitialRequestMessage()
  {
    .get instance class System.Net.Http.HttpRequestMessage/*02000013*/ System.Net.Http.SocketsHttpConnectionContext/*0200001C*/::get_InitialRequestMessage() /* 06000130 */
  } // end of property SocketsHttpConnectionContext::InitialRequestMessage
} // end of class System.Net.Http.SocketsHttpConnectionContext

.class /*0200001D*/ public auto ansi sealed beforefieldinit System.Net.Http.SocketsHttpHandler
       extends System.Net.Http.HttpMessageHandler/*0200000D*/
{
  .custom /*0C00007C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00007D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00007E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x243d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000A3       */ call       instance void System.Net.Http.HttpMessageHandler/*0200000D*/::.ctor() /* 060000A3 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::.ctor

  .method /*06000132*/ public hidebysig specialname 
          instance class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.DistributedContextPropagator/*01000049*/ 
          get_ActivityHeadersPropagator() cil managed
  // SIG: 20 00 12 81 25
  {
    // Method begins at RVA 0x2445
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_ActivityHeadersPropagator

  .method /*06000133*/ public hidebysig specialname 
          instance void  set_ActivityHeadersPropagator(class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.DistributedContextPropagator/*01000049*/ 'value') cil managed
  // SIG: 20 01 01 12 81 25
  {
    // Method begins at RVA 0x2448
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_ActivityHeadersPropagator

  .method /*06000134*/ public hidebysig specialname 
          instance bool  get_AllowAutoRedirect() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_AllowAutoRedirect

  .method /*06000135*/ public hidebysig specialname 
          instance void  set_AllowAutoRedirect(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x244d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_AllowAutoRedirect

  .method /*06000136*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/ 
          get_AutomaticDecompression() cil managed
  // SIG: 20 00 11 80 A5
  {
    // Method begins at RVA 0x244f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_AutomaticDecompression

  .method /*06000137*/ public hidebysig specialname 
          instance void  set_AutomaticDecompression(valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A5
  {
    // Method begins at RVA 0x2452
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_AutomaticDecompression

  .method /*06000138*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpConnectionContext/*0200001C*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>> 
          get_ConnectCallback() cil managed
  // SIG: 20 00 15 12 81 29 03 12 70 11 59 15 11 81 2D 01 12 55
  {
    .param [0]/*0800014D*/ 
    .custom /*0C000189:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 00 01 00 00 ) 
    // Method begins at RVA 0x2454
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_ConnectCallback

  .method /*06000139*/ public hidebysig specialname 
          instance void  set_ConnectCallback(class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpConnectionContext/*0200001C*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>> 'value') cil managed
  // SIG: 20 01 01 15 12 81 29 03 12 70 11 59 15 11 81 2D 01 12 55
  {
    .param [1]/*0800014E*/ 
    .custom /*0C00018A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 00 01 00 00 ) 
    // Method begins at RVA 0x2457
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_ConnectCallback

  .method /*0600013A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_ConnectTimeout() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x2459
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_ConnectTimeout

  .method /*0600013B*/ public hidebysig specialname 
          instance void  set_ConnectTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x245c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_ConnectTimeout

  .method /*0600013C*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/ 
          get_CookieContainer() cil managed
  // SIG: 20 00 12 80 AD
  {
    .custom /*0C000183:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x245e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_CookieContainer

  .method /*0600013D*/ public hidebysig specialname 
          instance void  set_CookieContainer(class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    .custom /*0C000184:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000150*/ 
    .custom /*0C00018B:0A000033*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000046*/::.ctor() /* 0A000033 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2461
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_CookieContainer

  .method /*0600013E*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x2463
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_Credentials

  .method /*0600013F*/ public hidebysig specialname 
          instance void  set_Credentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x2466
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_Credentials

  .method /*06000140*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 
          get_DefaultProxyCredentials() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x2468
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_DefaultProxyCredentials

  .method /*06000141*/ public hidebysig specialname 
          instance void  set_DefaultProxyCredentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x246b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_DefaultProxyCredentials

  .method /*06000142*/ public hidebysig specialname 
          instance bool  get_EnableMultipleHttp2Connections() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x246d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_EnableMultipleHttp2Connections

  .method /*06000143*/ public hidebysig specialname 
          instance void  set_EnableMultipleHttp2Connections(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2470
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_EnableMultipleHttp2Connections

  .method /*06000144*/ public hidebysig specialname 
          instance bool  get_EnableMultipleHttp3Connections() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_EnableMultipleHttp3Connections

  .method /*06000145*/ public hidebysig specialname 
          instance void  set_EnableMultipleHttp3Connections(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2475
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_EnableMultipleHttp3Connections

  .method /*06000146*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_Expect100ContinueTimeout() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x2477
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_Expect100ContinueTimeout

  .method /*06000147*/ public hidebysig specialname 
          instance void  set_Expect100ContinueTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x247a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_Expect100ContinueTimeout

  .method /*06000148*/ public hidebysig specialname 
          instance int32  get_InitialHttp2StreamWindowSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x247c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_InitialHttp2StreamWindowSize

  .method /*06000149*/ public hidebysig specialname 
          instance void  set_InitialHttp2StreamWindowSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x247f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_InitialHttp2StreamWindowSize

  .method /*0600014A*/ public hidebysig specialname static 
          bool  get_IsSupported() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2481
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_IsSupported

  .method /*0600014B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_KeepAlivePingDelay() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x2484
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_KeepAlivePingDelay

  .method /*0600014C*/ public hidebysig specialname 
          instance void  set_KeepAlivePingDelay(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x2487
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_KeepAlivePingDelay

  .method /*0600014D*/ public hidebysig specialname 
          instance valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/ 
          get_KeepAlivePingPolicy() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x2489
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_KeepAlivePingPolicy

  .method /*0600014E*/ public hidebysig specialname 
          instance void  set_KeepAlivePingPolicy(valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/ 'value') cil managed
  // SIG: 20 01 01 11 30
  {
    // Method begins at RVA 0x248c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_KeepAlivePingPolicy

  .method /*0600014F*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_KeepAlivePingTimeout() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x248e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_KeepAlivePingTimeout

  .method /*06000150*/ public hidebysig specialname 
          instance void  set_KeepAlivePingTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x2491
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_KeepAlivePingTimeout

  .method /*06000151*/ public hidebysig specialname 
          instance int32  get_MaxAutomaticRedirections() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2493
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_MaxAutomaticRedirections

  .method /*06000152*/ public hidebysig specialname 
          instance void  set_MaxAutomaticRedirections(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2496
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_MaxAutomaticRedirections

  .method /*06000153*/ public hidebysig specialname 
          instance int32  get_MaxConnectionsPerServer() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2498
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_MaxConnectionsPerServer

  .method /*06000154*/ public hidebysig specialname 
          instance void  set_MaxConnectionsPerServer(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x249b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_MaxConnectionsPerServer

  .method /*06000155*/ public hidebysig specialname 
          instance int32  get_MaxResponseDrainSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x249d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_MaxResponseDrainSize

  .method /*06000156*/ public hidebysig specialname 
          instance void  set_MaxResponseDrainSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x24a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_MaxResponseDrainSize

  .method /*06000157*/ public hidebysig specialname 
          instance int32  get_MaxResponseHeadersLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_MaxResponseHeadersLength

  .method /*06000158*/ public hidebysig specialname 
          instance void  set_MaxResponseHeadersLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x24a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_MaxResponseHeadersLength

  .method /*06000159*/ public hidebysig specialname 
          instance class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/ 
          get_MeterFactory() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x24a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_MeterFactory

  .method /*0600015A*/ public hidebysig specialname 
          instance void  set_MeterFactory(class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x24aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_MeterFactory

  .method /*0600015B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>> 
          get_PlaintextStreamFilter() cil managed
  // SIG: 20 00 15 12 81 29 03 12 78 11 59 15 11 81 2D 01 12 55
  {
    .param [0]/*0800015F*/ 
    .custom /*0C00018C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 00 01 00 00 ) 
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_PlaintextStreamFilter

  .method /*0600015C*/ public hidebysig specialname 
          instance void  set_PlaintextStreamFilter(class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>> 'value') cil managed
  // SIG: 20 01 01 15 12 81 29 03 12 78 11 59 15 11 81 2D 01 12 55
  {
    .param [1]/*08000160*/ 
    .custom /*0C00018D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 00 01 00 00 ) 
    // Method begins at RVA 0x24af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_PlaintextStreamFilter

  .method /*0600015D*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_PooledConnectionIdleTimeout() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x24b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_PooledConnectionIdleTimeout

  .method /*0600015E*/ public hidebysig specialname 
          instance void  set_PooledConnectionIdleTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x24b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_PooledConnectionIdleTimeout

  .method /*0600015F*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_PooledConnectionLifetime() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x24b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_PooledConnectionLifetime

  .method /*06000160*/ public hidebysig specialname 
          instance void  set_PooledConnectionLifetime(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x24b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_PooledConnectionLifetime

  .method /*06000161*/ public hidebysig specialname 
          instance bool  get_PreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_PreAuthenticate

  .method /*06000162*/ public hidebysig specialname 
          instance void  set_PreAuthenticate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_PreAuthenticate

  .method /*06000163*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object> 
          get_Properties() cil managed
  // SIG: 20 00 15 12 80 C9 02 0E 1C
  {
    .param [0]/*08000164*/ 
    .custom /*0C00018E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x24c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_Properties

  .method /*06000164*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x24c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_Proxy

  .method /*06000165*/ public hidebysig specialname 
          instance void  set_Proxy(class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x24c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_Proxy

  .method /*06000166*/ public hidebysig specialname 
          instance class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/> 
          get_RequestHeaderEncodingSelector() cil managed
  // SIG: 20 00 15 12 18 01 12 4C
  {
    .param [0]/*08000166*/ 
    .custom /*0C00018F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_RequestHeaderEncodingSelector

  .method /*06000167*/ public hidebysig specialname 
          instance void  set_RequestHeaderEncodingSelector(class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/> 'value') cil managed
  // SIG: 20 01 01 15 12 18 01 12 4C
  {
    .param [1]/*08000167*/ 
    .custom /*0C000190:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_RequestHeaderEncodingSelector

  .method /*06000168*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          get_ResponseDrainTimeout() cil managed
  // SIG: 20 00 11 80 99
  {
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_ResponseDrainTimeout

  .method /*06000169*/ public hidebysig specialname 
          instance void  set_ResponseDrainTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x24d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_ResponseDrainTimeout

  .method /*0600016A*/ public hidebysig specialname 
          instance class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/> 
          get_ResponseHeaderEncodingSelector() cil managed
  // SIG: 20 00 15 12 18 01 12 4C
  {
    .param [0]/*08000169*/ 
    .custom /*0C000191:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_ResponseHeaderEncodingSelector

  .method /*0600016B*/ public hidebysig specialname 
          instance void  set_ResponseHeaderEncodingSelector(class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/> 'value') cil managed
  // SIG: 20 01 01 15 12 18 01 12 4C
  {
    .param [1]/*0800016A*/ 
    .custom /*0C000192:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_ResponseHeaderEncodingSelector

  .method /*0600016C*/ public hidebysig specialname 
          instance class [System.Net.Security/*23000005*/]System.Net.Security.SslClientAuthenticationOptions/*0100004C*/ 
          get_SslOptions() cil managed
  // SIG: 20 00 12 81 31
  {
    .custom /*0C000194:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_SslOptions

  .method /*0600016D*/ public hidebysig specialname 
          instance void  set_SslOptions(class [System.Net.Security/*23000005*/]System.Net.Security.SslClientAuthenticationOptions/*0100004C*/ 'value') cil managed
  // SIG: 20 01 01 12 81 31
  {
    .custom /*0C000195:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800016B*/ 
    .custom /*0C000193:0A000033*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000046*/::.ctor() /* 0A000033 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_SslOptions

  .method /*0600016E*/ public hidebysig specialname 
          instance bool  get_UseCookies() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_UseCookies

  .method /*0600016F*/ public hidebysig specialname 
          instance void  set_UseCookies(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24df
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_UseCookies

  .method /*06000170*/ public hidebysig specialname 
          instance bool  get_UseProxy() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::get_UseProxy

  .method /*06000171*/ public hidebysig specialname 
          instance void  set_UseProxy(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::set_UseProxy

  .method /*06000172*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SocketsHttpHandler::Dispose

  .method /*06000173*/ famorassem hidebysig virtual 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          Send(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 12 58 12 4C 11 59
  {
    .custom /*0C000196:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::Send

  .method /*06000174*/ famorassem hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class System.Net.Http.HttpResponseMessage/*02000016*/> 
          SendAsync(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 5D 01 12 58 12 4C 11 59
  {
    .custom /*0C000197:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpHandler::SendAsync

  .property /*1700004F*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.DistributedContextPropagator/*01000049*/
          ActivityHeadersPropagator()
  {
    .custom /*0C00010C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.DistributedContextPropagator/*01000049*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_ActivityHeadersPropagator() /* 06000132 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_ActivityHeadersPropagator(class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.DistributedContextPropagator/*01000049*/) /* 06000133 */
  } // end of property SocketsHttpHandler::ActivityHeadersPropagator
  .property /*17000050*/ instance callconv(8) bool
          AllowAutoRedirect()
  {
    .get instance bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_AllowAutoRedirect() /* 06000134 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_AllowAutoRedirect(bool) /* 06000135 */
  } // end of property SocketsHttpHandler::AllowAutoRedirect
  .property /*17000051*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/
          AutomaticDecompression()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_AutomaticDecompression() /* 06000136 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_AutomaticDecompression(valuetype [System.Net.Primitives/*23000002*/]System.Net.DecompressionMethods/*01000029*/) /* 06000137 */
  } // end of property SocketsHttpHandler::AutomaticDecompression
  .property /*17000052*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpConnectionContext/*0200001C*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>>
          ConnectCallback()
  {
    .custom /*0C000111:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpConnectionContext/*0200001C*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>> System.Net.Http.SocketsHttpHandler/*0200001D*/::get_ConnectCallback() /* 06000138 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_ConnectCallback(class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpConnectionContext/*0200001C*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>>) /* 06000139 */
  } // end of property SocketsHttpHandler::ConnectCallback
  .property /*17000053*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          ConnectTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_ConnectTimeout() /* 0600013A */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_ConnectTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 0600013B */
  } // end of property SocketsHttpHandler::ConnectTimeout
  .property /*17000054*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/
          CookieContainer()
  {
    .custom /*0C000113:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_CookieContainer() /* 0600013C */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_CookieContainer(class [System.Net.Primitives/*23000002*/]System.Net.CookieContainer/*0100002B*/) /* 0600013D */
  } // end of property SocketsHttpHandler::CookieContainer
  .property /*17000055*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/
          Credentials()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_Credentials() /* 0600013E */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_Credentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/) /* 0600013F */
  } // end of property SocketsHttpHandler::Credentials
  .property /*17000056*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/
          DefaultProxyCredentials()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_DefaultProxyCredentials() /* 06000140 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_DefaultProxyCredentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*0100002C*/) /* 06000141 */
  } // end of property SocketsHttpHandler::DefaultProxyCredentials
  .property /*17000057*/ instance callconv(8) bool
          EnableMultipleHttp2Connections()
  {
    .get instance bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_EnableMultipleHttp2Connections() /* 06000142 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_EnableMultipleHttp2Connections(bool) /* 06000143 */
  } // end of property SocketsHttpHandler::EnableMultipleHttp2Connections
  .property /*17000058*/ instance callconv(8) bool
          EnableMultipleHttp3Connections()
  {
    .get instance bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_EnableMultipleHttp3Connections() /* 06000144 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_EnableMultipleHttp3Connections(bool) /* 06000145 */
  } // end of property SocketsHttpHandler::EnableMultipleHttp3Connections
  .property /*17000059*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          Expect100ContinueTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_Expect100ContinueTimeout() /* 06000146 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_Expect100ContinueTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 06000147 */
  } // end of property SocketsHttpHandler::Expect100ContinueTimeout
  .property /*1700005A*/ instance callconv(8) int32
          InitialHttp2StreamWindowSize()
  {
    .get instance int32 System.Net.Http.SocketsHttpHandler/*0200001D*/::get_InitialHttp2StreamWindowSize() /* 06000148 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_InitialHttp2StreamWindowSize(int32) /* 06000149 */
  } // end of property SocketsHttpHandler::InitialHttp2StreamWindowSize
  .property /*1700005B*/ callconv(8) bool
          IsSupported()
  {
    .custom /*0C000119:0A000034*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformGuardAttribute/*0100004D*/::.ctor(string) /* 0A000034 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_IsSupported() /* 0600014A */
  } // end of property SocketsHttpHandler::IsSupported
  .property /*1700005C*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          KeepAlivePingDelay()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_KeepAlivePingDelay() /* 0600014B */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_KeepAlivePingDelay(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 0600014C */
  } // end of property SocketsHttpHandler::KeepAlivePingDelay
  .property /*1700005D*/ instance callconv(8) valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/
          KeepAlivePingPolicy()
  {
    .get instance valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_KeepAlivePingPolicy() /* 0600014D */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_KeepAlivePingPolicy(valuetype System.Net.Http.HttpKeepAlivePingPolicy/*0200000C*/) /* 0600014E */
  } // end of property SocketsHttpHandler::KeepAlivePingPolicy
  .property /*1700005E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          KeepAlivePingTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_KeepAlivePingTimeout() /* 0600014F */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_KeepAlivePingTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 06000150 */
  } // end of property SocketsHttpHandler::KeepAlivePingTimeout
  .property /*1700005F*/ instance callconv(8) int32
          MaxAutomaticRedirections()
  {
    .get instance int32 System.Net.Http.SocketsHttpHandler/*0200001D*/::get_MaxAutomaticRedirections() /* 06000151 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_MaxAutomaticRedirections(int32) /* 06000152 */
  } // end of property SocketsHttpHandler::MaxAutomaticRedirections
  .property /*17000060*/ instance callconv(8) int32
          MaxConnectionsPerServer()
  {
    .get instance int32 System.Net.Http.SocketsHttpHandler/*0200001D*/::get_MaxConnectionsPerServer() /* 06000153 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_MaxConnectionsPerServer(int32) /* 06000154 */
  } // end of property SocketsHttpHandler::MaxConnectionsPerServer
  .property /*17000061*/ instance callconv(8) int32
          MaxResponseDrainSize()
  {
    .get instance int32 System.Net.Http.SocketsHttpHandler/*0200001D*/::get_MaxResponseDrainSize() /* 06000155 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_MaxResponseDrainSize(int32) /* 06000156 */
  } // end of property SocketsHttpHandler::MaxResponseDrainSize
  .property /*17000062*/ instance callconv(8) int32
          MaxResponseHeadersLength()
  {
    .get instance int32 System.Net.Http.SocketsHttpHandler/*0200001D*/::get_MaxResponseHeadersLength() /* 06000157 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_MaxResponseHeadersLength(int32) /* 06000158 */
  } // end of property SocketsHttpHandler::MaxResponseHeadersLength
  .property /*17000063*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/
          MeterFactory()
  {
    .custom /*0C00011F:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_MeterFactory() /* 06000159 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_MeterFactory(class [System.Diagnostics.DiagnosticSource/*23000004*/]System.Diagnostics.Metrics.IMeterFactory/*01000031*/) /* 0600015A */
  } // end of property SocketsHttpHandler::MeterFactory
  .property /*17000064*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>>
          PlaintextStreamFilter()
  {
    .custom /*0C000120:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>> System.Net.Http.SocketsHttpHandler/*0200001D*/::get_PlaintextStreamFilter() /* 0600015B */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_PlaintextStreamFilter(class [System.Runtime/*23000001*/]System.Func`3/*0100004A*/<class System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100004B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/>>) /* 0600015C */
  } // end of property SocketsHttpHandler::PlaintextStreamFilter
  .property /*17000065*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          PooledConnectionIdleTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_PooledConnectionIdleTimeout() /* 0600015D */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_PooledConnectionIdleTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 0600015E */
  } // end of property SocketsHttpHandler::PooledConnectionIdleTimeout
  .property /*17000066*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          PooledConnectionLifetime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_PooledConnectionLifetime() /* 0600015F */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_PooledConnectionLifetime(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 06000160 */
  } // end of property SocketsHttpHandler::PooledConnectionLifetime
  .property /*17000067*/ instance callconv(8) bool
          PreAuthenticate()
  {
    .get instance bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_PreAuthenticate() /* 06000161 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_PreAuthenticate(bool) /* 06000162 */
  } // end of property SocketsHttpHandler::PreAuthenticate
  .property /*17000068*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object>
          Properties()
  {
    .custom /*0C000124:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000032*/<string,object> System.Net.Http.SocketsHttpHandler/*0200001D*/::get_Properties() /* 06000163 */
  } // end of property SocketsHttpHandler::Properties
  .property /*17000069*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/
          Proxy()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_Proxy() /* 06000164 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_Proxy(class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000024*/) /* 06000165 */
  } // end of property SocketsHttpHandler::Proxy
  .property /*1700006A*/ instance callconv(8) class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/>
          RequestHeaderEncodingSelector()
  {
    .custom /*0C000126:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/> System.Net.Http.SocketsHttpHandler/*0200001D*/::get_RequestHeaderEncodingSelector() /* 06000166 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_RequestHeaderEncodingSelector(class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/>) /* 06000167 */
  } // end of property SocketsHttpHandler::RequestHeaderEncodingSelector
  .property /*1700006B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/
          ResponseDrainTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_ResponseDrainTimeout() /* 06000168 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_ResponseDrainTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/) /* 06000169 */
  } // end of property SocketsHttpHandler::ResponseDrainTimeout
  .property /*1700006C*/ instance callconv(8) class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/>
          ResponseHeaderEncodingSelector()
  {
    .custom /*0C000128:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/> System.Net.Http.SocketsHttpHandler/*0200001D*/::get_ResponseHeaderEncodingSelector() /* 0600016A */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_ResponseHeaderEncodingSelector(class System.Net.Http.HeaderEncodingSelector`1/*02000006*/<class System.Net.Http.HttpRequestMessage/*02000013*/>) /* 0600016B */
  } // end of property SocketsHttpHandler::ResponseHeaderEncodingSelector
  .property /*1700006D*/ instance callconv(8) class [System.Net.Security/*23000005*/]System.Net.Security.SslClientAuthenticationOptions/*0100004C*/
          SslOptions()
  {
    .custom /*0C000129:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.Security/*23000005*/]System.Net.Security.SslClientAuthenticationOptions/*0100004C*/ System.Net.Http.SocketsHttpHandler/*0200001D*/::get_SslOptions() /* 0600016C */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_SslOptions(class [System.Net.Security/*23000005*/]System.Net.Security.SslClientAuthenticationOptions/*0100004C*/) /* 0600016D */
  } // end of property SocketsHttpHandler::SslOptions
  .property /*1700006E*/ instance callconv(8) bool
          UseCookies()
  {
    .get instance bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_UseCookies() /* 0600016E */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_UseCookies(bool) /* 0600016F */
  } // end of property SocketsHttpHandler::UseCookies
  .property /*1700006F*/ instance callconv(8) bool
          UseProxy()
  {
    .get instance bool System.Net.Http.SocketsHttpHandler/*0200001D*/::get_UseProxy() /* 06000170 */
    .set instance void System.Net.Http.SocketsHttpHandler/*0200001D*/::set_UseProxy(bool) /* 06000171 */
  } // end of property SocketsHttpHandler::UseProxy
} // end of class System.Net.Http.SocketsHttpHandler

.class /*0200001E*/ public auto ansi sealed beforefieldinit System.Net.Http.SocketsHttpPlaintextStreamFilterContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
{
  .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000081:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000175*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24ee
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SocketsHttpPlaintextStreamFilterContext::.ctor

  .method /*06000176*/ public hidebysig specialname 
          instance class System.Net.Http.HttpRequestMessage/*02000013*/ 
          get_InitialRequestMessage() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x24f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpPlaintextStreamFilterContext::get_InitialRequestMessage

  .method /*06000177*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ 
          get_NegotiatedHttpVersion() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x24f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpPlaintextStreamFilterContext::get_NegotiatedHttpVersion

  .method /*06000178*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          get_PlaintextStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x24fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SocketsHttpPlaintextStreamFilterContext::get_PlaintextStream

  .property /*17000070*/ instance callconv(8) class System.Net.Http.HttpRequestMessage/*02000013*/
          InitialRequestMessage()
  {
    .get instance class System.Net.Http.HttpRequestMessage/*02000013*/ System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/::get_InitialRequestMessage() /* 06000176 */
  } // end of property SocketsHttpPlaintextStreamFilterContext::InitialRequestMessage
  .property /*17000071*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000025*/
          NegotiatedHttpVersion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000025*/ System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/::get_NegotiatedHttpVersion() /* 06000177 */
  } // end of property SocketsHttpPlaintextStreamFilterContext::NegotiatedHttpVersion
  .property /*17000072*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/
          PlaintextStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ System.Net.Http.SocketsHttpPlaintextStreamFilterContext/*0200001E*/::get_PlaintextStream() /* 06000178 */
  } // end of property SocketsHttpPlaintextStreamFilterContext::PlaintextStream
} // end of class System.Net.Http.SocketsHttpPlaintextStreamFilterContext

.class /*0200001F*/ public auto ansi beforefieldinit System.Net.Http.StreamContent
       extends System.Net.Http.HttpContent/*0200000A*/
{
  .custom /*0C000083:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000084:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000179*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ content) cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x24ff
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StreamContent::.ctor

  .method /*0600017A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ content,
                               int32 bufferSize) cil managed
  // SIG: 20 02 01 12 55 08
  {
    // Method begins at RVA 0x2507
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000084       */ call       instance void System.Net.Http.HttpContent/*0200000A*/::.ctor() /* 06000084 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StreamContent::.ctor

  .method /*0600017B*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          CreateContentReadStream(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 01 12 55 11 59
  {
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamContent::CreateContentReadStream

  .method /*0600017C*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000017*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/> 
          CreateContentReadStreamAsync() cil managed
  // SIG: 20 00 15 12 5D 01 12 55
  {
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamContent::CreateContentReadStreamAsync

  .method /*0600017D*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2515
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method StreamContent::Dispose

  .method /*0600017E*/ family hidebysig virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                           class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 55 12 61 11 59
  {
    .param [2]/*08000179*/ 
    .custom /*0C000198:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2517
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method StreamContent::SerializeToStream

  .method /*0600017F*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context) cil managed
  // SIG: 20 02 12 65 12 55 12 61
  {
    .param [2]/*0800017C*/ 
    .custom /*0C000199:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2519
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamContent::SerializeToStreamAsync

  .method /*06000180*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*0800017E*/ 
    .custom /*0C00019A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x251c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamContent::SerializeToStreamAsync

  .method /*06000181*/ famorassem hidebysig virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x251f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamContent::TryComputeLength

} // end of class System.Net.Http.StreamContent

.class /*02000020*/ public auto ansi beforefieldinit System.Net.Http.StringContent
       extends System.Net.Http.ByteArrayContent/*02000002*/
{
  .custom /*0C000088:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000182*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string content) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2522
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Net.Http.ByteArrayContent/*02000002*/::.ctor(uint8[]) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method StringContent::.ctor

  .method /*06000183*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string content,
                               class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ mediaType) cil managed
  // SIG: 20 02 01 0E 12 80 B8
  {
    .param [2]/*08000183*/ 
    .custom /*0C00019B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252b
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Net.Http.ByteArrayContent/*02000002*/::.ctor(uint8[]) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method StringContent::.ctor

  .method /*06000184*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string content,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*01000020*/ encoding) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    .param [2]/*08000185*/ 
    .custom /*0C00019D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2534
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Net.Http.ByteArrayContent/*02000002*/::.ctor(uint8[]) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method StringContent::.ctor

  .method /*06000185*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string content,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*01000020*/ encoding,
                               class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ mediaType) cil managed
  // SIG: 20 03 01 0E 12 80 81 12 80 B8
  {
    .custom /*0C00019C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000186*/ 
    .custom /*0C00019F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x253d
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Net.Http.ByteArrayContent/*02000002*/::.ctor(uint8[]) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method StringContent::.ctor

  .method /*06000186*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string content,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*01000020*/ encoding,
                               string mediaType) cil managed
  // SIG: 20 03 01 0E 12 80 81 0E
  {
    .custom /*0C00019E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000189*/ 
    .custom /*0C0001A1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2546
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Net.Http.ByteArrayContent/*02000002*/::.ctor(uint8[]) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method StringContent::.ctor

  .method /*06000187*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000019*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                 class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000018*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000016*/ cancellationToken) cil managed
  // SIG: 20 03 12 65 12 55 12 61 11 59
  {
    .param [2]/*0800018D*/ 
    .custom /*0C0001A3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x254f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringContent::SerializeToStreamAsync

} // end of class System.Net.Http.StringContent

.class /*02000021*/ public auto ansi sealed beforefieldinit System.Net.Http.Metrics.HttpMetricsEnrichmentContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
{
  .custom /*0C00008D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000188*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2552
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpMetricsEnrichmentContext::.ctor

  .method /*06000189*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*01000036*/ 
          get_Exception() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C0001A0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x255a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMetricsEnrichmentContext::get_Exception

  .method /*0600018A*/ public hidebysig specialname 
          instance class System.Net.Http.HttpRequestMessage/*02000013*/ 
          get_Request() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x255d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMetricsEnrichmentContext::get_Request

  .method /*0600018B*/ public hidebysig specialname 
          instance class System.Net.Http.HttpResponseMessage/*02000016*/ 
          get_Response() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C0001A2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2560
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpMetricsEnrichmentContext::get_Response

  .method /*0600018C*/ public hidebysig static 
          void  AddCallback(class System.Net.Http.HttpRequestMessage/*02000013*/ 'request',
                            class [System.Runtime/*23000001*/]System.Action`1/*0100004E*/<class System.Net.Http.Metrics.HttpMetricsEnrichmentContext/*02000021*/> callback) cil managed
  // SIG: 00 02 01 12 4C 15 12 81 39 01 12 80 84
  {
    // Method begins at RVA 0x2563
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpMetricsEnrichmentContext::AddCallback

  .method /*0600018D*/ public hidebysig instance void 
          AddCustomTag(string name,
                       object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000192*/ 
    .custom /*0C0001A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2565
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpMetricsEnrichmentContext::AddCustomTag

  .property /*17000073*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*01000036*/
          Exception()
  {
    .custom /*0C00012C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Exception/*01000036*/ System.Net.Http.Metrics.HttpMetricsEnrichmentContext/*02000021*/::get_Exception() /* 06000189 */
  } // end of property HttpMetricsEnrichmentContext::Exception
  .property /*17000074*/ instance callconv(8) class System.Net.Http.HttpRequestMessage/*02000013*/
          Request()
  {
    .get instance class System.Net.Http.HttpRequestMessage/*02000013*/ System.Net.Http.Metrics.HttpMetricsEnrichmentContext/*02000021*/::get_Request() /* 0600018A */
  } // end of property HttpMetricsEnrichmentContext::Request
  .property /*17000075*/ instance callconv(8) class System.Net.Http.HttpResponseMessage/*02000016*/
          Response()
  {
    .custom /*0C00012E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.HttpResponseMessage/*02000016*/ System.Net.Http.Metrics.HttpMetricsEnrichmentContext/*02000021*/::get_Response() /* 0600018B */
  } // end of property HttpMetricsEnrichmentContext::Response
} // end of class System.Net.Http.Metrics.HttpMetricsEnrichmentContext

.class /*02000022*/ public auto ansi beforefieldinit System.Net.Http.Headers.AuthenticationHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C000091:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000092:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600018E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scheme) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2567
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationHeaderValue::.ctor

  .method /*0600018F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scheme,
                               string parameter) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000195*/ 
    .custom /*0C0001A7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x256f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationHeaderValue::.ctor

  .method /*06000190*/ public hidebysig specialname 
          instance string  get_Parameter() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001A4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2577
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::get_Parameter

  .method /*06000191*/ public hidebysig specialname 
          instance string  get_Scheme() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x257a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::get_Scheme

  .method /*06000192*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001A5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000196*/ 
    .custom /*0C0001A8:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x257d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::Equals

  .method /*06000193*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2580
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::GetHashCode

  .method /*06000194*/ public hidebysig static 
          class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 88 0E
  {
    // Method begins at RVA 0x2583
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::Parse

  .method /*06000195*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2586
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::System.ICloneable.Clone

  .method /*06000196*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2589
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::ToString

  .method /*06000197*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 88
  {
    .custom /*0C0001A9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000198*/ 
    .custom /*0C0001AA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000199*/ 
    .custom /*0C0001AB:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x258c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationHeaderValue::TryParse

  .property /*17000076*/ instance callconv(8) string
          Parameter()
  {
    .custom /*0C000130:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/::get_Parameter() /* 06000190 */
  } // end of property AuthenticationHeaderValue::Parameter
  .property /*17000077*/ instance callconv(8) string
          Scheme()
  {
    .get instance string System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/::get_Scheme() /* 06000191 */
  } // end of property AuthenticationHeaderValue::Scheme
} // end of class System.Net.Http.Headers.AuthenticationHeaderValue

.class /*02000023*/ public auto ansi beforefieldinit System.Net.Http.Headers.CacheControlHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C000094:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000095:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000198*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x258f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::.ctor

  .method /*06000199*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Extensions() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 80 C0
  {
    // Method begins at RVA 0x2597
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_Extensions

  .method /*0600019A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          get_MaxAge() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x259a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_MaxAge

  .method /*0600019B*/ public hidebysig specialname 
          instance void  set_MaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x259d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_MaxAge

  .method /*0600019C*/ public hidebysig specialname 
          instance bool  get_MaxStale() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x259f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_MaxStale

  .method /*0600019D*/ public hidebysig specialname 
          instance void  set_MaxStale(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_MaxStale

  .method /*0600019E*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          get_MaxStaleLimit() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x25a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_MaxStaleLimit

  .method /*0600019F*/ public hidebysig specialname 
          instance void  set_MaxStaleLimit(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x25a7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_MaxStaleLimit

  .method /*060001A0*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          get_MinFresh() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x25a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_MinFresh

  .method /*060001A1*/ public hidebysig specialname 
          instance void  set_MinFresh(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x25ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_MinFresh

  .method /*060001A2*/ public hidebysig specialname 
          instance bool  get_MustRevalidate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_MustRevalidate

  .method /*060001A3*/ public hidebysig specialname 
          instance void  set_MustRevalidate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25b1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_MustRevalidate

  .method /*060001A4*/ public hidebysig specialname 
          instance bool  get_NoCache() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_NoCache

  .method /*060001A5*/ public hidebysig specialname 
          instance void  set_NoCache(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_NoCache

  .method /*060001A6*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> 
          get_NoCacheHeaders() cil managed
  // SIG: 20 00 15 12 80 F5 01 0E
  {
    // Method begins at RVA 0x25b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_NoCacheHeaders

  .method /*060001A7*/ public hidebysig specialname 
          instance bool  get_NoStore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_NoStore

  .method /*060001A8*/ public hidebysig specialname 
          instance void  set_NoStore(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_NoStore

  .method /*060001A9*/ public hidebysig specialname 
          instance bool  get_NoTransform() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_NoTransform

  .method /*060001AA*/ public hidebysig specialname 
          instance void  set_NoTransform(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_NoTransform

  .method /*060001AB*/ public hidebysig specialname 
          instance bool  get_OnlyIfCached() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_OnlyIfCached

  .method /*060001AC*/ public hidebysig specialname 
          instance void  set_OnlyIfCached(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_OnlyIfCached

  .method /*060001AD*/ public hidebysig specialname 
          instance bool  get_Private() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_Private

  .method /*060001AE*/ public hidebysig specialname 
          instance void  set_Private(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_Private

  .method /*060001AF*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> 
          get_PrivateHeaders() cil managed
  // SIG: 20 00 15 12 80 F5 01 0E
  {
    // Method begins at RVA 0x25cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_PrivateHeaders

  .method /*060001B0*/ public hidebysig specialname 
          instance bool  get_ProxyRevalidate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_ProxyRevalidate

  .method /*060001B1*/ public hidebysig specialname 
          instance void  set_ProxyRevalidate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_ProxyRevalidate

  .method /*060001B2*/ public hidebysig specialname 
          instance bool  get_Public() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_Public

  .method /*060001B3*/ public hidebysig specialname 
          instance void  set_Public(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_Public

  .method /*060001B4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          get_SharedMaxAge() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x25dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::get_SharedMaxAge

  .method /*060001B5*/ public hidebysig specialname 
          instance void  set_SharedMaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x25df
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CacheControlHeaderValue::set_SharedMaxAge

  .method /*060001B6*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001A7*/ 
    .custom /*0C0001AC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::Equals

  .method /*060001B7*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x25e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::GetHashCode

  .method /*060001B8*/ public hidebysig static 
          class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 8C 0E
  {
    .param [1]/*080001A8*/ 
    .custom /*0C0001AD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::Parse

  .method /*060001B9*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x25ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::System.ICloneable.Clone

  .method /*060001BA*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x25ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::ToString

  .method /*060001BB*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 8C
  {
    .custom /*0C0001B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001AA*/ 
    .custom /*0C0001AE:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CacheControlHeaderValue::TryParse

  .property /*17000078*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Extensions()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_Extensions() /* 06000199 */
  } // end of property CacheControlHeaderValue::Extensions
  .property /*17000079*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>
          MaxAge()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_MaxAge() /* 0600019A */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_MaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>) /* 0600019B */
  } // end of property CacheControlHeaderValue::MaxAge
  .property /*1700007A*/ instance callconv(8) bool
          MaxStale()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_MaxStale() /* 0600019C */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_MaxStale(bool) /* 0600019D */
  } // end of property CacheControlHeaderValue::MaxStale
  .property /*1700007B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>
          MaxStaleLimit()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_MaxStaleLimit() /* 0600019E */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_MaxStaleLimit(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>) /* 0600019F */
  } // end of property CacheControlHeaderValue::MaxStaleLimit
  .property /*1700007C*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>
          MinFresh()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_MinFresh() /* 060001A0 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_MinFresh(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>) /* 060001A1 */
  } // end of property CacheControlHeaderValue::MinFresh
  .property /*1700007D*/ instance callconv(8) bool
          MustRevalidate()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_MustRevalidate() /* 060001A2 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_MustRevalidate(bool) /* 060001A3 */
  } // end of property CacheControlHeaderValue::MustRevalidate
  .property /*1700007E*/ instance callconv(8) bool
          NoCache()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_NoCache() /* 060001A4 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_NoCache(bool) /* 060001A5 */
  } // end of property CacheControlHeaderValue::NoCache
  .property /*1700007F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string>
          NoCacheHeaders()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_NoCacheHeaders() /* 060001A6 */
  } // end of property CacheControlHeaderValue::NoCacheHeaders
  .property /*17000080*/ instance callconv(8) bool
          NoStore()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_NoStore() /* 060001A7 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_NoStore(bool) /* 060001A8 */
  } // end of property CacheControlHeaderValue::NoStore
  .property /*17000081*/ instance callconv(8) bool
          NoTransform()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_NoTransform() /* 060001A9 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_NoTransform(bool) /* 060001AA */
  } // end of property CacheControlHeaderValue::NoTransform
  .property /*17000082*/ instance callconv(8) bool
          OnlyIfCached()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_OnlyIfCached() /* 060001AB */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_OnlyIfCached(bool) /* 060001AC */
  } // end of property CacheControlHeaderValue::OnlyIfCached
  .property /*17000083*/ instance callconv(8) bool
          Private()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_Private() /* 060001AD */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_Private(bool) /* 060001AE */
  } // end of property CacheControlHeaderValue::Private
  .property /*17000084*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string>
          PrivateHeaders()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_PrivateHeaders() /* 060001AF */
  } // end of property CacheControlHeaderValue::PrivateHeaders
  .property /*17000085*/ instance callconv(8) bool
          ProxyRevalidate()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_ProxyRevalidate() /* 060001B0 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_ProxyRevalidate(bool) /* 060001B1 */
  } // end of property CacheControlHeaderValue::ProxyRevalidate
  .property /*17000086*/ instance callconv(8) bool
          Public()
  {
    .get instance bool System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_Public() /* 060001B2 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_Public(bool) /* 060001B3 */
  } // end of property CacheControlHeaderValue::Public
  .property /*17000087*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>
          SharedMaxAge()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::get_SharedMaxAge() /* 060001B4 */
    .set instance void System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/::set_SharedMaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>) /* 060001B5 */
  } // end of property CacheControlHeaderValue::SharedMaxAge
} // end of class System.Net.Http.Headers.CacheControlHeaderValue

.class /*02000024*/ public auto ansi beforefieldinit System.Net.Http.Headers.ContentDispositionHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C000098:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000099:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C000049:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060001BC*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/ source) cil managed
  // SIG: 20 01 01 12 80 90
  {
    .custom /*0C0001B4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25f3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::.ctor

  .method /*060001BD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string dispositionType) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001B5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25fb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::.ctor

  .method /*060001BE*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_CreationDate() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x2603
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_CreationDate

  .method /*060001BF*/ public hidebysig specialname 
          instance void  set_CreationDate(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x2606
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_CreationDate

  .method /*060001C0*/ public hidebysig specialname 
          instance string  get_DispositionType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001B6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2608
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_DispositionType

  .method /*060001C1*/ public hidebysig specialname 
          instance void  set_DispositionType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001B8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x260b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_DispositionType

  .method /*060001C2*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x260d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_FileName

  .method /*060001C3*/ public hidebysig specialname 
          instance void  set_FileName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2610
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_FileName

  .method /*060001C4*/ public hidebysig specialname 
          instance string  get_FileNameStar() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2612
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_FileNameStar

  .method /*060001C5*/ public hidebysig specialname 
          instance void  set_FileNameStar(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2615
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_FileNameStar

  .method /*060001C6*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_ModificationDate() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x2617
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_ModificationDate

  .method /*060001C7*/ public hidebysig specialname 
          instance void  set_ModificationDate(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x261a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_ModificationDate

  .method /*060001C8*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x261c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_Name

  .method /*060001C9*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x261f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_Name

  .method /*060001CA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 80 C0
  {
    .custom /*0C0001BD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2621
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_Parameters

  .method /*060001CB*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_ReadDate() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x2624
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_ReadDate

  .method /*060001CC*/ public hidebysig specialname 
          instance void  set_ReadDate(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x2627
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_ReadDate

  .method /*060001CD*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_Size() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x2629
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::get_Size

  .method /*060001CE*/ public hidebysig specialname 
          instance void  set_Size(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x262c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDispositionHeaderValue::set_Size

  .method /*060001CF*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*080001B5*/ 
    .custom /*0C0001AF:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x262e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::Equals

  .method /*060001D0*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2631
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::GetHashCode

  .method /*060001D1*/ public hidebysig static 
          class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 90 0E
  {
    .custom /*0C0001BF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2634
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::Parse

  .method /*060001D2*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2637
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::System.ICloneable.Clone

  .method /*060001D3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001C0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x263a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::ToString

  .method /*060001D4*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 90
  {
    .param [1]/*080001B7*/ 
    .custom /*0C0001B1:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001B8*/ 
    .custom /*0C0001B2:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x263d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDispositionHeaderValue::TryParse

  .property /*17000088*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          CreationDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_CreationDate() /* 060001BE */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_CreationDate(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 060001BF */
  } // end of property ContentDispositionHeaderValue::CreationDate
  .property /*17000089*/ instance callconv(8) string
          DispositionType()
  {
    .custom /*0C000137:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_DispositionType() /* 060001C0 */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_DispositionType(string) /* 060001C1 */
  } // end of property ContentDispositionHeaderValue::DispositionType
  .property /*1700008A*/ instance callconv(8) string
          FileName()
  {
    .get instance string System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_FileName() /* 060001C2 */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_FileName(string) /* 060001C3 */
  } // end of property ContentDispositionHeaderValue::FileName
  .property /*1700008B*/ instance callconv(8) string
          FileNameStar()
  {
    .get instance string System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_FileNameStar() /* 060001C4 */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_FileNameStar(string) /* 060001C5 */
  } // end of property ContentDispositionHeaderValue::FileNameStar
  .property /*1700008C*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          ModificationDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_ModificationDate() /* 060001C6 */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_ModificationDate(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 060001C7 */
  } // end of property ContentDispositionHeaderValue::ModificationDate
  .property /*1700008D*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_Name() /* 060001C8 */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_Name(string) /* 060001C9 */
  } // end of property ContentDispositionHeaderValue::Name
  .property /*1700008E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Parameters()
  {
    .custom /*0C000138:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_Parameters() /* 060001CA */
  } // end of property ContentDispositionHeaderValue::Parameters
  .property /*1700008F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          ReadDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_ReadDate() /* 060001CB */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_ReadDate(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 060001CC */
  } // end of property ContentDispositionHeaderValue::ReadDate
  .property /*17000090*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          Size()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::get_Size() /* 060001CD */
    .set instance void System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/::set_Size(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>) /* 060001CE */
  } // end of property ContentDispositionHeaderValue::Size
} // end of class System.Net.Http.Headers.ContentDispositionHeaderValue

.class /*02000025*/ public auto ansi beforefieldinit System.Net.Http.Headers.ContentRangeHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C00009B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060001D5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int64 length) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2640
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentRangeHeaderValue::.ctor

  .method /*060001D6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int64 from,
                               int64 'to') cil managed
  // SIG: 20 02 01 0A 0A
  {
    // Method begins at RVA 0x2648
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentRangeHeaderValue::.ctor

  .method /*060001D7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int64 from,
                               int64 'to',
                               int64 length) cil managed
  // SIG: 20 03 01 0A 0A 0A
  {
    // Method begins at RVA 0x2650
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentRangeHeaderValue::.ctor

  .method /*060001D8*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_From() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x2658
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::get_From

  .method /*060001D9*/ public hidebysig specialname 
          instance bool  get_HasLength() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x265b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::get_HasLength

  .method /*060001DA*/ public hidebysig specialname 
          instance bool  get_HasRange() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x265e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::get_HasRange

  .method /*060001DB*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_Length() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x2661
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::get_Length

  .method /*060001DC*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_To() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x2664
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::get_To

  .method /*060001DD*/ public hidebysig specialname 
          instance string  get_Unit() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2667
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::get_Unit

  .method /*060001DE*/ public hidebysig specialname 
          instance void  set_Unit(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x266a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentRangeHeaderValue::set_Unit

  .method /*060001DF*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001C6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C0*/ 
    .custom /*0C0001B7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x266c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::Equals

  .method /*060001E0*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x266f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::GetHashCode

  .method /*060001E1*/ public hidebysig static 
          class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 94 0E
  {
    // Method begins at RVA 0x2672
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::Parse

  .method /*060001E2*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2675
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::System.ICloneable.Clone

  .method /*060001E3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2678
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::ToString

  .method /*060001E4*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 94
  {
    .custom /*0C0001C9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C2*/ 
    .custom /*0C0001B9:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001C3*/ 
    .custom /*0C0001BA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x267b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentRangeHeaderValue::TryParse

  .property /*17000091*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          From()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::get_From() /* 060001D8 */
  } // end of property ContentRangeHeaderValue::From
  .property /*17000092*/ instance callconv(8) bool
          HasLength()
  {
    .get instance bool System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::get_HasLength() /* 060001D9 */
  } // end of property ContentRangeHeaderValue::HasLength
  .property /*17000093*/ instance callconv(8) bool
          HasRange()
  {
    .get instance bool System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::get_HasRange() /* 060001DA */
  } // end of property ContentRangeHeaderValue::HasRange
  .property /*17000094*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          Length()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::get_Length() /* 060001DB */
  } // end of property ContentRangeHeaderValue::Length
  .property /*17000095*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          To()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::get_To() /* 060001DC */
  } // end of property ContentRangeHeaderValue::To
  .property /*17000096*/ instance callconv(8) string
          Unit()
  {
    .get instance string System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::get_Unit() /* 060001DD */
    .set instance void System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/::set_Unit(string) /* 060001DE */
  } // end of property ContentRangeHeaderValue::Unit
} // end of class System.Net.Http.Headers.ContentRangeHeaderValue

.class /*02000026*/ public auto ansi beforefieldinit System.Net.Http.Headers.EntityTagHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C00009E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C000052:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060001E5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string tag) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x267e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EntityTagHeaderValue::.ctor

  .method /*060001E6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               bool isWeak) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x2686
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EntityTagHeaderValue::.ctor

  .method /*060001E7*/ public hidebysig specialname static 
          class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ 
          get_Any() cil managed
  // SIG: 00 00 12 80 98
  {
    // Method begins at RVA 0x268e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::get_Any

  .method /*060001E8*/ public hidebysig specialname 
          instance bool  get_IsWeak() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2691
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::get_IsWeak

  .method /*060001E9*/ public hidebysig specialname 
          instance string  get_Tag() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2694
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::get_Tag

  .method /*060001EA*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001CA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C7*/ 
    .custom /*0C0001BB:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2697
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::Equals

  .method /*060001EB*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x269a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::GetHashCode

  .method /*060001EC*/ public hidebysig static 
          class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 98 0E
  {
    // Method begins at RVA 0x269d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::Parse

  .method /*060001ED*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x26a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::System.ICloneable.Clone

  .method /*060001EE*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x26a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::ToString

  .method /*060001EF*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 98
  {
    .custom /*0C0001CB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C9*/ 
    .custom /*0C0001BC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001CA*/ 
    .custom /*0C0001BE:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EntityTagHeaderValue::TryParse

  .property /*17000097*/ callconv(8) class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/
          Any()
  {
    .get class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/::get_Any() /* 060001E7 */
  } // end of property EntityTagHeaderValue::Any
  .property /*17000098*/ instance callconv(8) bool
          IsWeak()
  {
    .get instance bool System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/::get_IsWeak() /* 060001E8 */
  } // end of property EntityTagHeaderValue::IsWeak
  .property /*17000099*/ instance callconv(8) string
          Tag()
  {
    .get instance string System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/::get_Tag() /* 060001E9 */
  } // end of property EntityTagHeaderValue::Tag
} // end of class System.Net.Http.Headers.EntityTagHeaderValue

.class /*02000027*/ public sequential ansi sealed beforefieldinit System.Net.Http.Headers.HeaderStringValues
       extends [System.Runtime/*23000001*/]System.ValueType/*01000045*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>/*1B000008*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<string>/*1B000009*/
{
  .custom /*0C0000A1:0A000032*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000044*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>/*1B000008*/
  .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<string>/*1B000009*/
  .custom /*0C00005E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .class /*0200003D*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000045*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<string>/*1B00000A*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
  {
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<string>/*1B00000A*/
    .custom /*0C0000BE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .field /*04000021*/ private object _dummy
    .field /*04000022*/ private int32 _dummyPrimitive
    .method /*06000318*/ public hidebysig newslot specialname virtual final 
            instance string  get_Current() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000249:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x2ac3
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000319*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/::get_Current /*01000042::0A000037*/ 
      // Method begins at RVA 0x2ac6
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*0600031A*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2ac9
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method Enumerator::Dispose

    .method /*0600031B*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x2acb
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*0600031C*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/::Reset /*01000042::0A000038*/ 
      // Method begins at RVA 0x2ace
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method Enumerator::System.Collections.IEnumerator.Reset

    .property /*17000101*/ instance callconv(8) string
            Current()
    {
      .custom /*0C000176:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
      .get instance string System.Net.Http.Headers.HeaderStringValues/*02000027*//Enumerator/*0200003D*/::get_Current() /* 06000318 */
    } // end of property Enumerator::Current
    .property /*17000102*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C000177:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Net.Http.Headers.HeaderStringValues/*02000027*//Enumerator/*0200003D*/::System.Collections.IEnumerator.get_Current() /* 06000319 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .field /*0400001D*/ private initonly object _dummy
  .field /*0400001E*/ private initonly int32 _dummyPrimitive
  .method /*060001F0*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x26a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeaderStringValues::get_Count

  .method /*060001F1*/ public hidebysig instance valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*//Enumerator/*0200003D*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 80 F4
  {
    // Method begins at RVA 0x26ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeaderStringValues::GetEnumerator

  .method /*060001F2*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<string> 
          'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 81 01 01 0E
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>/*1B000008*/::GetEnumerator() /* 0A000036 */
    // Method begins at RVA 0x26af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeaderStringValues::'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'

  .method /*060001F3*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 09
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/::GetEnumerator /*01000041::0A000030*/ 
    // Method begins at RVA 0x26b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeaderStringValues::System.Collections.IEnumerable.GetEnumerator

  .method /*060001F4*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001CC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeaderStringValues::ToString

  .property /*1700009A*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Net.Http.Headers.HeaderStringValues/*02000027*/::get_Count() /* 060001F0 */
  } // end of property HeaderStringValues::Count
} // end of class System.Net.Http.Headers.HeaderStringValues

.class /*02000028*/ public auto ansi sealed beforefieldinit System.Net.Http.Headers.HttpContentHeaders
       extends System.Net.Http.Headers.HttpHeaders/*02000029*/
{
  .custom /*0C0000A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000A4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060001F5*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26b8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000209       */ call       instance void System.Net.Http.Headers.HttpHeaders/*02000029*/::.ctor() /* 06000209 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::.ctor

  .method /*060001F6*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> 
          get_Allow() cil managed
  // SIG: 20 00 15 12 80 F5 01 0E
  {
    .custom /*0C0001CD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_Allow

  .method /*060001F7*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/ 
          get_ContentDisposition() cil managed
  // SIG: 20 00 12 80 90
  {
    // Method begins at RVA 0x26c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentDisposition

  .method /*060001F8*/ public hidebysig specialname 
          instance void  set_ContentDisposition(class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x26c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_ContentDisposition

  .method /*060001F9*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> 
          get_ContentEncoding() cil managed
  // SIG: 20 00 15 12 80 F5 01 0E
  {
    .custom /*0C0001CE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentEncoding

  .method /*060001FA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> 
          get_ContentLanguage() cil managed
  // SIG: 20 00 15 12 80 F5 01 0E
  {
    .custom /*0C0001CF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentLanguage

  .method /*060001FB*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_ContentLength() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x26ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentLength

  .method /*060001FC*/ public hidebysig specialname 
          instance void  set_ContentLength(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x26d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_ContentLength

  .method /*060001FD*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 
          get_ContentLocation() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x26d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentLocation

  .method /*060001FE*/ public hidebysig specialname 
          instance void  set_ContentLocation(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x26d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_ContentLocation

  .method /*060001FF*/ public hidebysig specialname 
          instance uint8[]  get_ContentMD5() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x26d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentMD5

  .method /*06000200*/ public hidebysig specialname 
          instance void  set_ContentMD5(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x26db
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_ContentMD5

  .method /*06000201*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/ 
          get_ContentRange() cil managed
  // SIG: 20 00 12 80 94
  {
    // Method begins at RVA 0x26dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentRange

  .method /*06000202*/ public hidebysig specialname 
          instance void  set_ContentRange(class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/ 'value') cil managed
  // SIG: 20 01 01 12 80 94
  {
    // Method begins at RVA 0x26e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_ContentRange

  .method /*06000203*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ 
          get_ContentType() cil managed
  // SIG: 20 00 12 80 B8
  {
    // Method begins at RVA 0x26e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_ContentType

  .method /*06000204*/ public hidebysig specialname 
          instance void  set_ContentType(class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B8
  {
    // Method begins at RVA 0x26e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_ContentType

  .method /*06000205*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_Expires() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x26e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_Expires

  .method /*06000206*/ public hidebysig specialname 
          instance void  set_Expires(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x26ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_Expires

  .method /*06000207*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_LastModified() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x26ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentHeaders::get_LastModified

  .method /*06000208*/ public hidebysig specialname 
          instance void  set_LastModified(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x26ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpContentHeaders::set_LastModified

  .property /*1700009B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string>
          Allow()
  {
    .custom /*0C00013A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_Allow() /* 060001F6 */
  } // end of property HttpContentHeaders::Allow
  .property /*1700009C*/ instance callconv(8) class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/
          ContentDisposition()
  {
    .get instance class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/ System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentDisposition() /* 060001F7 */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_ContentDisposition(class System.Net.Http.Headers.ContentDispositionHeaderValue/*02000024*/) /* 060001F8 */
  } // end of property HttpContentHeaders::ContentDisposition
  .property /*1700009D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string>
          ContentEncoding()
  {
    .custom /*0C00013B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentEncoding() /* 060001F9 */
  } // end of property HttpContentHeaders::ContentEncoding
  .property /*1700009E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string>
          ContentLanguage()
  {
    .custom /*0C00013C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<string> System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentLanguage() /* 060001FA */
  } // end of property HttpContentHeaders::ContentLanguage
  .property /*1700009F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          ContentLength()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentLength() /* 060001FB */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_ContentLength(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>) /* 060001FC */
  } // end of property HttpContentHeaders::ContentLength
  .property /*170000A0*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000023*/
          ContentLocation()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentLocation() /* 060001FD */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_ContentLocation(class [System.Runtime/*23000001*/]System.Uri/*01000023*/) /* 060001FE */
  } // end of property HttpContentHeaders::ContentLocation
  .property /*170000A1*/ instance callconv(8) uint8[]
          ContentMD5()
  {
    .get instance uint8[] System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentMD5() /* 060001FF */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_ContentMD5(uint8[]) /* 06000200 */
  } // end of property HttpContentHeaders::ContentMD5
  .property /*170000A2*/ instance callconv(8) class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/
          ContentRange()
  {
    .get instance class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/ System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentRange() /* 06000201 */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_ContentRange(class System.Net.Http.Headers.ContentRangeHeaderValue/*02000025*/) /* 06000202 */
  } // end of property HttpContentHeaders::ContentRange
  .property /*170000A3*/ instance callconv(8) class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/
          ContentType()
  {
    .get instance class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_ContentType() /* 06000203 */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_ContentType(class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/) /* 06000204 */
  } // end of property HttpContentHeaders::ContentType
  .property /*170000A4*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          Expires()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_Expires() /* 06000205 */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_Expires(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 06000206 */
  } // end of property HttpContentHeaders::Expires
  .property /*170000A5*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.HttpContentHeaders/*02000028*/::get_LastModified() /* 06000207 */
    .set instance void System.Net.Http.Headers.HttpContentHeaders/*02000028*/::set_LastModified(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 06000208 */
  } // end of property HttpContentHeaders::LastModified
} // end of class System.Net.Http.Headers.HttpContentHeaders

.class /*02000029*/ public abstract auto ansi beforefieldinit System.Net.Http.Headers.HttpHeaders
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>>>/*1B00000B*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
{
  .custom /*0C0000A5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>>>/*1B00000B*/
  .custom /*0C000060:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 05 00 00 00 00 00 01 01 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
  .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000209*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26f1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpHeaders::.ctor

  .method /*0600020A*/ public hidebysig specialname 
          instance valuetype System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/ 
          get_NonValidated() cil managed
  // SIG: 20 00 11 80 A8
  {
    // Method begins at RVA 0x26f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::get_NonValidated

  .method /*0600020B*/ public hidebysig instance void 
          Add(string name,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> values) cil managed
  // SIG: 20 02 01 0E 15 12 71 01 0E
  {
    .param [2]/*080001D4*/ 
    .custom /*0C0001C1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x26fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaders::Add

  .method /*0600020C*/ public hidebysig instance void 
          Add(string name,
              string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*080001D6*/ 
    .custom /*0C0001C2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaders::Add

  .method /*0600020D*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2700
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaders::Clear

  .method /*0600020E*/ public hidebysig instance bool 
          Contains(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2702
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::Contains

  .method /*0600020F*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 81 01 01 15 11 75 02 0E 15 12 71 01 0E
  {
    .param [0]/*080001D8*/ 
    .custom /*0C0001C3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 05 00 00 00 01 00 01 01 01 00 00 ) 
    // Method begins at RVA 0x2705
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::GetEnumerator

  .method /*06000210*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> 
          GetValues(string name) cil managed
  // SIG: 20 01 15 12 71 01 0E 0E
  {
    // Method begins at RVA 0x2708
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::GetValues

  .method /*06000211*/ public hidebysig instance bool 
          Remove(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x270b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::Remove

  .method /*06000212*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 09
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/::GetEnumerator /*01000041::0A000030*/ 
    // Method begins at RVA 0x270e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::System.Collections.IEnumerable.GetEnumerator

  .method /*06000213*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2711
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::ToString

  .method /*06000214*/ public hidebysig instance bool 
          TryAddWithoutValidation(string name,
                                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> values) cil managed
  // SIG: 20 02 02 0E 15 12 71 01 0E
  {
    .param [2]/*080001DC*/ 
    .custom /*0C0001C4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2714
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::TryAddWithoutValidation

  .method /*06000215*/ public hidebysig instance bool 
          TryAddWithoutValidation(string name,
                                  string 'value') cil managed
  // SIG: 20 02 02 0E 0E
  {
    .param [2]/*080001DE*/ 
    .custom /*0C0001C5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2717
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::TryAddWithoutValidation

  .method /*06000216*/ public hidebysig instance bool 
          TryGetValues(string name,
                       [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>& values) cil managed
  // SIG: 20 02 02 0E 10 15 12 71 01 0E
  {
    .param [2]/*080001E0*/ 
    .custom /*0C0001C7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0001C8:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x271a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaders::TryGetValues

  .property /*170000A6*/ instance callconv(8) valuetype System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/
          NonValidated()
  {
    .get instance valuetype System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/ System.Net.Http.Headers.HttpHeaders/*02000029*/::get_NonValidated() /* 0600020A */
  } // end of property HttpHeaders::NonValidated
} // end of class System.Net.Http.Headers.HttpHeaders

.class /*0200002A*/ public sequential ansi sealed beforefieldinit System.Net.Http.Headers.HttpHeadersNonValidated
       extends [System.Runtime/*23000001*/]System.ValueType/*01000045*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000D*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000E*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>/*1B00000C*/
{
  .custom /*0C0000A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000A9:0A000032*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000044*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
  .custom /*0C0000AA:0A000039*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000051*/::.ctor(string) /* 0A000039 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000D*/
  .custom /*0C00006A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
  .custom /*0C000071:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000E*/
  .custom /*0C000076:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>/*1B00000C*/
  .custom /*0C00007B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .class /*0200003E*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000045*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000F*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000034*/
  {
    .custom /*0C0000E6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000F*/
    .custom /*0C0000C7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 00 01 00 00 ) 
    .field /*04000023*/ private object _dummy
    .field /*04000024*/ private int32 _dummyPrimitive
    .method /*0600031D*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/> 
            get_Current() cil managed
    // SIG: 20 00 15 11 75 02 0E 11 80 9C
    {
      .param [0]/*0800027B*/ 
      .custom /*0C00021F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x2ad0
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*0600031E*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/::get_Current /*01000042::0A000037*/ 
      // Method begins at RVA 0x2ad3
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*0600031F*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2ad6
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method Enumerator::Dispose

    .method /*06000320*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x2ad8
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*06000321*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/::Reset /*01000042::0A000038*/ 
      // Method begins at RVA 0x2adb
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method Enumerator::System.Collections.IEnumerator.Reset

    .property /*17000103*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>
            Current()
    {
      .custom /*0C000178:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/> System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*//Enumerator/*0200003E*/::get_Current() /* 0600031D */
    } // end of property Enumerator::Current
    .property /*17000104*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C000179:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*//Enumerator/*0200003E*/::System.Collections.IEnumerator.get_Current() /* 0600031E */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .field /*0400001F*/ private initonly object _dummy
  .field /*04000020*/ private initonly int32 _dummyPrimitive
  .method /*06000217*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x271d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::get_Count

  .method /*06000218*/ public hidebysig newslot specialname virtual final 
          instance valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/ 
          get_Item(string headerName) cil managed
  // SIG: 20 01 11 80 9C 0E
  {
    // Method begins at RVA 0x2720
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::get_Item

  .method /*06000219*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> 
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.get_Keys'() cil managed
  // SIG: 20 00 15 12 71 01 0E
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>/*1B00000C*/::get_Keys() /* 0A00003A */
    // Method begins at RVA 0x2723
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.get_Keys'

  .method /*0600021A*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/> 
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.get_Values'() cil managed
  // SIG: 20 00 15 12 71 01 11 80 9C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<!1> class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>/*1B00000C*/::get_Values() /* 0A00003B */
    // Method begins at RVA 0x2726
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.get_Values'

  .method /*0600021B*/ public hidebysig instance bool 
          Contains(string headerName) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2729
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::Contains

  .method /*0600021C*/ public hidebysig instance valuetype System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*//Enumerator/*0200003E*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 80 F8
  {
    // Method begins at RVA 0x272c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::GetEnumerator

  .method /*0600021D*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Net.Http.Headers.HeaderStringValues>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 81 01 01 15 11 75 02 0E 11 80 9C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100001D*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>>/*1B00000D*/::GetEnumerator() /* 0A00003C */
    // Method begins at RVA 0x272f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Net.Http.Headers.HeaderStringValues>>.GetEnumerator'

  .method /*0600021E*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.ContainsKey'(string key) cil managed
  // SIG: 20 01 02 0E
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>/*1B00000C*/::ContainsKey(!0) /* 0A00003D */
    // Method begins at RVA 0x2732
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.ContainsKey'

  .method /*0600021F*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.TryGetValue'(string key,
                                                                                                                                                [out] valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/& 'value') cil managed
  // SIG: 20 02 02 0E 10 11 80 9C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100003F*/<string,valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>/*1B00000C*/::TryGetValue(!0,
                                                                                                                                                                                                                                     !1&) /* 0A00003E */
    // Method begins at RVA 0x2735
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.TryGetValue'

  .method /*06000220*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 09
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/::GetEnumerator /*01000041::0A000030*/ 
    // Method begins at RVA 0x2738
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::System.Collections.IEnumerable.GetEnumerator

  .method /*06000221*/ public hidebysig instance bool 
          TryGetValues(string headerName,
                       [out] valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/& values) cil managed
  // SIG: 20 02 02 0E 10 11 80 9C
  {
    // Method begins at RVA 0x273b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeadersNonValidated::TryGetValues

  .property /*170000A7*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/::get_Count() /* 06000217 */
  } // end of property HttpHeadersNonValidated::Count
  .property /*170000A8*/ instance callconv(8) valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/
          Item(string)
  {
    .get instance valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/ System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/::get_Item(string) /* 06000218 */
  } // end of property HttpHeadersNonValidated::Item
  .property /*170000A9*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string>
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.Keys'()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<string> System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.get_Keys'() /* 06000219 */
  } // end of property HttpHeadersNonValidated::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.Keys'
  .property /*170000AA*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/>
          'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.Values'()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<valuetype System.Net.Http.Headers.HeaderStringValues/*02000027*/> System.Net.Http.Headers.HttpHeadersNonValidated/*0200002A*/::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.get_Values'() /* 0600021A */
  } // end of property HttpHeadersNonValidated::'System.Collections.Generic.IReadOnlyDictionary<System.String,System.Net.Http.Headers.HeaderStringValues>.Values'
} // end of class System.Net.Http.Headers.HttpHeadersNonValidated

.class /*0200002B*/ public auto ansi sealed beforefieldinit System.Net.Http.Headers.HttpHeaderValueCollection`1<class T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<!T>/*1B000010*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<!T>/*1B000011*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
{
  .custom /*0C0000AC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<!T>/*1B000010*/
  .custom /*0C00007F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001C*/<!T>/*1B000011*/
  .custom /*0C000082:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/
  .custom /*0C000086:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000222*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x273e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpHeaderValueCollection`1::.ctor

  .method /*06000223*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2746
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::get_Count

  .method /*06000224*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2749
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::get_IsReadOnly

  .method /*06000225*/ public hidebysig newslot virtual final 
          instance void  Add(!T item) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x274c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaderValueCollection`1::Add

  .method /*06000226*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x274e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaderValueCollection`1::Clear

  .method /*06000227*/ public hidebysig newslot virtual final 
          instance bool  Contains(!T item) cil managed
  // SIG: 20 01 02 13 00
  {
    // Method begins at RVA 0x2750
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::Contains

  .method /*06000228*/ public hidebysig newslot virtual final 
          instance void  CopyTo(!T[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 13 00 08
  {
    // Method begins at RVA 0x2753
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaderValueCollection`1::CopyTo

  .method /*06000229*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000040*/<!T> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 81 01 01 13 00
  {
    // Method begins at RVA 0x2755
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::GetEnumerator

  .method /*0600022A*/ public hidebysig instance void 
          ParseAdd(string input) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001DF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2758
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpHeaderValueCollection`1::ParseAdd

  .method /*0600022B*/ public hidebysig newslot virtual final 
          instance bool  Remove(!T item) cil managed
  // SIG: 20 01 02 13 00
  {
    // Method begins at RVA 0x275a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::Remove

  .method /*0600022C*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000042*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 09
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000041*/::GetEnumerator /*01000041::0A000030*/ 
    // Method begins at RVA 0x275d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::System.Collections.IEnumerable.GetEnumerator

  .method /*0600022D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2760
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::ToString

  .method /*0600022E*/ public hidebysig instance bool 
          TryParseAdd(string input) cil managed
  // SIG: 20 01 02 0E
  {
    .custom /*0C0001E2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2763
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpHeaderValueCollection`1::TryParseAdd

  .property /*170000AB*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/::get_Count() /* 06000223 */
  } // end of property HttpHeaderValueCollection`1::Count
  .property /*170000AC*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/::get_IsReadOnly() /* 06000224 */
  } // end of property HttpHeaderValueCollection`1::IsReadOnly
} // end of class System.Net.Http.Headers.HttpHeaderValueCollection`1

.class /*0200002C*/ public auto ansi sealed beforefieldinit System.Net.Http.Headers.HttpRequestHeaders
       extends System.Net.Http.Headers.HttpHeaders/*02000029*/
{
  .custom /*0C0000AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600022F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2766
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000209       */ call       instance void System.Net.Http.Headers.HttpHeaders/*02000029*/::.ctor() /* 06000209 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::.ctor

  .method /*06000230*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/> 
          get_Accept() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 BC
  {
    // Method begins at RVA 0x276e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Accept

  .method /*06000231*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/> 
          get_AcceptCharset() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 E0
  {
    // Method begins at RVA 0x2771
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_AcceptCharset

  .method /*06000232*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/> 
          get_AcceptEncoding() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 E0
  {
    // Method begins at RVA 0x2774
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_AcceptEncoding

  .method /*06000233*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/> 
          get_AcceptLanguage() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 E0
  {
    // Method begins at RVA 0x2777
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_AcceptLanguage

  .method /*06000234*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ 
          get_Authorization() cil managed
  // SIG: 20 00 12 80 88
  {
    .custom /*0C0001E6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x277a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Authorization

  .method /*06000235*/ public hidebysig specialname 
          instance void  set_Authorization(class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ 'value') cil managed
  // SIG: 20 01 01 12 80 88
  {
    .custom /*0C0001E8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x277d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_Authorization

  .method /*06000236*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ 
          get_CacheControl() cil managed
  // SIG: 20 00 12 80 8C
  {
    .custom /*0C0001E9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x277f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_CacheControl

  .method /*06000237*/ public hidebysig specialname 
          instance void  set_CacheControl(class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8C
  {
    .custom /*0C0001EB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2782
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_CacheControl

  .method /*06000238*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> 
          get_Connection() cil managed
  // SIG: 20 00 15 12 80 AC 01 0E
  {
    // Method begins at RVA 0x2784
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Connection

  .method /*06000239*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 
          get_ConnectionClose() cil managed
  // SIG: 20 00 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x2787
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_ConnectionClose

  .method /*0600023A*/ public hidebysig specialname 
          instance void  set_ConnectionClose(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x278a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_ConnectionClose

  .method /*0600023B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_Date() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x278c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Date

  .method /*0600023C*/ public hidebysig specialname 
          instance void  set_Date(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x278f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_Date

  .method /*0600023D*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/> 
          get_Expect() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 C4
  {
    // Method begins at RVA 0x2791
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Expect

  .method /*0600023E*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 
          get_ExpectContinue() cil managed
  // SIG: 20 00 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x2794
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_ExpectContinue

  .method /*0600023F*/ public hidebysig specialname 
          instance void  set_ExpectContinue(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x2797
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_ExpectContinue

  .method /*06000240*/ public hidebysig specialname 
          instance string  get_From() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001F1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2799
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_From

  .method /*06000241*/ public hidebysig specialname 
          instance void  set_From(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001F2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x279c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_From

  .method /*06000242*/ public hidebysig specialname 
          instance string  get_Host() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x279e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Host

  .method /*06000243*/ public hidebysig specialname 
          instance void  set_Host(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27a1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_Host

  .method /*06000244*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/> 
          get_IfMatch() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 98
  {
    // Method begins at RVA 0x27a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_IfMatch

  .method /*06000245*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_IfModifiedSince() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x27a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_IfModifiedSince

  .method /*06000246*/ public hidebysig specialname 
          instance void  set_IfModifiedSince(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x27a9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_IfModifiedSince

  .method /*06000247*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/> 
          get_IfNoneMatch() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 98
  {
    // Method begins at RVA 0x27ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_IfNoneMatch

  .method /*06000248*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/ 
          get_IfRange() cil managed
  // SIG: 20 00 12 80 D0
  {
    .custom /*0C0001F8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_IfRange

  .method /*06000249*/ public hidebysig specialname 
          instance void  set_IfRange(class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D0
  {
    .custom /*0C0001FA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27b1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_IfRange

  .method /*0600024A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_IfUnmodifiedSince() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x27b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_IfUnmodifiedSince

  .method /*0600024B*/ public hidebysig specialname 
          instance void  set_IfUnmodifiedSince(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x27b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_IfUnmodifiedSince

  .method /*0600024C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int32> 
          get_MaxForwards() cil managed
  // SIG: 20 00 15 11 80 E9 01 08
  {
    // Method begins at RVA 0x27b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_MaxForwards

  .method /*0600024D*/ public hidebysig specialname 
          instance void  set_MaxForwards(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int32> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 08
  {
    // Method begins at RVA 0x27bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_MaxForwards

  .method /*0600024E*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Pragma() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 C0
  {
    // Method begins at RVA 0x27bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Pragma

  .method /*0600024F*/ public hidebysig specialname 
          instance string  get_Protocol() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001FE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Protocol

  .method /*06000250*/ public hidebysig specialname 
          instance void  set_Protocol(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_Protocol

  .method /*06000251*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ 
          get_ProxyAuthorization() cil managed
  // SIG: 20 00 12 80 88
  {
    .custom /*0C000200:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_ProxyAuthorization

  .method /*06000252*/ public hidebysig specialname 
          instance void  set_ProxyAuthorization(class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ 'value') cil managed
  // SIG: 20 01 01 12 80 88
  {
    .custom /*0C000201:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_ProxyAuthorization

  .method /*06000253*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.RangeHeaderValue/*02000035*/ 
          get_Range() cil managed
  // SIG: 20 00 12 80 D4
  {
    .custom /*0C000203:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Range

  .method /*06000254*/ public hidebysig specialname 
          instance void  set_Range(class System.Net.Http.Headers.RangeHeaderValue/*02000035*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D4
  {
    .custom /*0C000204:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_Range

  .method /*06000255*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 
          get_Referrer() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C000206:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Referrer

  .method /*06000256*/ public hidebysig specialname 
          instance void  set_Referrer(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C000208:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_Referrer

  .method /*06000257*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/> 
          get_TE() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 E8
  {
    // Method begins at RVA 0x27d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_TE

  .method /*06000258*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> 
          get_Trailer() cil managed
  // SIG: 20 00 15 12 80 AC 01 0E
  {
    // Method begins at RVA 0x27d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Trailer

  .method /*06000259*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/> 
          get_TransferEncoding() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 E4
  {
    // Method begins at RVA 0x27da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_TransferEncoding

  .method /*0600025A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 
          get_TransferEncodingChunked() cil managed
  // SIG: 20 00 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x27dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_TransferEncodingChunked

  .method /*0600025B*/ public hidebysig specialname 
          instance void  set_TransferEncodingChunked(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x27e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpRequestHeaders::set_TransferEncodingChunked

  .method /*0600025C*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductHeaderValue/*02000032*/> 
          get_Upgrade() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 C8
  {
    // Method begins at RVA 0x27e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Upgrade

  .method /*0600025D*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/> 
          get_UserAgent() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 CC
  {
    // Method begins at RVA 0x27e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_UserAgent

  .method /*0600025E*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/> 
          get_Via() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 EC
  {
    // Method begins at RVA 0x27e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Via

  .method /*0600025F*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/> 
          get_Warning() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 F0
  {
    // Method begins at RVA 0x27eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestHeaders::get_Warning

  .property /*170000AD*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/>
          Accept()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Accept() /* 06000230 */
  } // end of property HttpRequestHeaders::Accept
  .property /*170000AE*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/>
          AcceptCharset()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_AcceptCharset() /* 06000231 */
  } // end of property HttpRequestHeaders::AcceptCharset
  .property /*170000AF*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/>
          AcceptEncoding()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_AcceptEncoding() /* 06000232 */
  } // end of property HttpRequestHeaders::AcceptEncoding
  .property /*170000B0*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/>
          AcceptLanguage()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_AcceptLanguage() /* 06000233 */
  } // end of property HttpRequestHeaders::AcceptLanguage
  .property /*170000B1*/ instance callconv(8) class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/
          Authorization()
  {
    .custom /*0C000146:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Authorization() /* 06000234 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_Authorization(class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/) /* 06000235 */
  } // end of property HttpRequestHeaders::Authorization
  .property /*170000B2*/ instance callconv(8) class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/
          CacheControl()
  {
    .custom /*0C000147:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_CacheControl() /* 06000236 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_CacheControl(class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/) /* 06000237 */
  } // end of property HttpRequestHeaders::CacheControl
  .property /*170000B3*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string>
          Connection()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Connection() /* 06000238 */
  } // end of property HttpRequestHeaders::Connection
  .property /*170000B4*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>
          ConnectionClose()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_ConnectionClose() /* 06000239 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_ConnectionClose(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>) /* 0600023A */
  } // end of property HttpRequestHeaders::ConnectionClose
  .property /*170000B5*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          Date()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Date() /* 0600023B */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_Date(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 0600023C */
  } // end of property HttpRequestHeaders::Date
  .property /*170000B6*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/>
          Expect()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Expect() /* 0600023D */
  } // end of property HttpRequestHeaders::Expect
  .property /*170000B7*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>
          ExpectContinue()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_ExpectContinue() /* 0600023E */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_ExpectContinue(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>) /* 0600023F */
  } // end of property HttpRequestHeaders::ExpectContinue
  .property /*170000B8*/ instance callconv(8) string
          From()
  {
    .custom /*0C000149:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_From() /* 06000240 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_From(string) /* 06000241 */
  } // end of property HttpRequestHeaders::From
  .property /*170000B9*/ instance callconv(8) string
          Host()
  {
    .custom /*0C00014B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Host() /* 06000242 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_Host(string) /* 06000243 */
  } // end of property HttpRequestHeaders::Host
  .property /*170000BA*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/>
          IfMatch()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_IfMatch() /* 06000244 */
  } // end of property HttpRequestHeaders::IfMatch
  .property /*170000BB*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          IfModifiedSince()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_IfModifiedSince() /* 06000245 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_IfModifiedSince(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 06000246 */
  } // end of property HttpRequestHeaders::IfModifiedSince
  .property /*170000BC*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/>
          IfNoneMatch()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_IfNoneMatch() /* 06000247 */
  } // end of property HttpRequestHeaders::IfNoneMatch
  .property /*170000BD*/ instance callconv(8) class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/
          IfRange()
  {
    .custom /*0C00014F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/ System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_IfRange() /* 06000248 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_IfRange(class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/) /* 06000249 */
  } // end of property HttpRequestHeaders::IfRange
  .property /*170000BE*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          IfUnmodifiedSince()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_IfUnmodifiedSince() /* 0600024A */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_IfUnmodifiedSince(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 0600024B */
  } // end of property HttpRequestHeaders::IfUnmodifiedSince
  .property /*170000BF*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int32>
          MaxForwards()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int32> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_MaxForwards() /* 0600024C */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_MaxForwards(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int32>) /* 0600024D */
  } // end of property HttpRequestHeaders::MaxForwards
  .property /*170000C0*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Pragma()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Pragma() /* 0600024E */
  } // end of property HttpRequestHeaders::Pragma
  .property /*170000C1*/ instance callconv(8) string
          Protocol()
  {
    .custom /*0C000152:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Protocol() /* 0600024F */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_Protocol(string) /* 06000250 */
  } // end of property HttpRequestHeaders::Protocol
  .property /*170000C2*/ instance callconv(8) class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/
          ProxyAuthorization()
  {
    .custom /*0C000153:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/ System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_ProxyAuthorization() /* 06000251 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_ProxyAuthorization(class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/) /* 06000252 */
  } // end of property HttpRequestHeaders::ProxyAuthorization
  .property /*170000C3*/ instance callconv(8) class System.Net.Http.Headers.RangeHeaderValue/*02000035*/
          Range()
  {
    .custom /*0C000155:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.RangeHeaderValue/*02000035*/ System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Range() /* 06000253 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_Range(class System.Net.Http.Headers.RangeHeaderValue/*02000035*/) /* 06000254 */
  } // end of property HttpRequestHeaders::Range
  .property /*170000C4*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000023*/
          Referrer()
  {
    .custom /*0C000156:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Referrer() /* 06000255 */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_Referrer(class [System.Runtime/*23000001*/]System.Uri/*01000023*/) /* 06000256 */
  } // end of property HttpRequestHeaders::Referrer
  .property /*170000C5*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/>
          TE()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_TE() /* 06000257 */
  } // end of property HttpRequestHeaders::TE
  .property /*170000C6*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string>
          Trailer()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Trailer() /* 06000258 */
  } // end of property HttpRequestHeaders::Trailer
  .property /*170000C7*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/>
          TransferEncoding()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_TransferEncoding() /* 06000259 */
  } // end of property HttpRequestHeaders::TransferEncoding
  .property /*170000C8*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>
          TransferEncodingChunked()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_TransferEncodingChunked() /* 0600025A */
    .set instance void System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::set_TransferEncodingChunked(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>) /* 0600025B */
  } // end of property HttpRequestHeaders::TransferEncodingChunked
  .property /*170000C9*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductHeaderValue/*02000032*/>
          Upgrade()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductHeaderValue/*02000032*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Upgrade() /* 0600025C */
  } // end of property HttpRequestHeaders::Upgrade
  .property /*170000CA*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/>
          UserAgent()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_UserAgent() /* 0600025D */
  } // end of property HttpRequestHeaders::UserAgent
  .property /*170000CB*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/>
          Via()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Via() /* 0600025E */
  } // end of property HttpRequestHeaders::Via
  .property /*170000CC*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/>
          Warning()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/> System.Net.Http.Headers.HttpRequestHeaders/*0200002C*/::get_Warning() /* 0600025F */
  } // end of property HttpRequestHeaders::Warning
} // end of class System.Net.Http.Headers.HttpRequestHeaders

.class /*0200002D*/ public auto ansi sealed beforefieldinit System.Net.Http.Headers.HttpResponseHeaders
       extends System.Net.Http.Headers.HttpHeaders/*02000029*/
{
  .custom /*0C0000B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000260*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27ee
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000209       */ call       instance void System.Net.Http.Headers.HttpHeaders/*02000029*/::.ctor() /* 06000209 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::.ctor

  .method /*06000261*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> 
          get_AcceptRanges() cil managed
  // SIG: 20 00 15 12 80 AC 01 0E
  {
    // Method begins at RVA 0x27f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_AcceptRanges

  .method /*06000262*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          get_Age() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x27f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Age

  .method /*06000263*/ public hidebysig specialname 
          instance void  set_Age(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x27fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_Age

  .method /*06000264*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ 
          get_CacheControl() cil managed
  // SIG: 20 00 12 80 8C
  {
    .custom /*0C00020E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_CacheControl

  .method /*06000265*/ public hidebysig specialname 
          instance void  set_CacheControl(class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8C
  {
    .custom /*0C00020F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2801
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_CacheControl

  .method /*06000266*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> 
          get_Connection() cil managed
  // SIG: 20 00 15 12 80 AC 01 0E
  {
    // Method begins at RVA 0x2803
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Connection

  .method /*06000267*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 
          get_ConnectionClose() cil managed
  // SIG: 20 00 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x2806
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_ConnectionClose

  .method /*06000268*/ public hidebysig specialname 
          instance void  set_ConnectionClose(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x2809
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_ConnectionClose

  .method /*06000269*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_Date() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x280b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Date

  .method /*0600026A*/ public hidebysig specialname 
          instance void  set_Date(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x280e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_Date

  .method /*0600026B*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ 
          get_ETag() cil managed
  // SIG: 20 00 12 80 98
  {
    .custom /*0C000212:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2810
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_ETag

  .method /*0600026C*/ public hidebysig specialname 
          instance void  set_ETag(class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ 'value') cil managed
  // SIG: 20 01 01 12 80 98
  {
    .custom /*0C000214:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2813
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_ETag

  .method /*0600026D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 
          get_Location() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C000216:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2815
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Location

  .method /*0600026E*/ public hidebysig specialname 
          instance void  set_Location(class [System.Runtime/*23000001*/]System.Uri/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C000217:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2818
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_Location

  .method /*0600026F*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Pragma() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 C0
  {
    // Method begins at RVA 0x281a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Pragma

  .method /*06000270*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/> 
          get_ProxyAuthenticate() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 88
  {
    // Method begins at RVA 0x281d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_ProxyAuthenticate

  .method /*06000271*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/ 
          get_RetryAfter() cil managed
  // SIG: 20 00 12 80 DC
  {
    .custom /*0C00021A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2820
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_RetryAfter

  .method /*06000272*/ public hidebysig specialname 
          instance void  set_RetryAfter(class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/ 'value') cil managed
  // SIG: 20 01 01 12 80 DC
  {
    .custom /*0C00021B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2823
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_RetryAfter

  .method /*06000273*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/> 
          get_Server() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 CC
  {
    // Method begins at RVA 0x2825
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Server

  .method /*06000274*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> 
          get_Trailer() cil managed
  // SIG: 20 00 15 12 80 AC 01 0E
  {
    // Method begins at RVA 0x2828
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Trailer

  .method /*06000275*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/> 
          get_TransferEncoding() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 E4
  {
    // Method begins at RVA 0x282b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_TransferEncoding

  .method /*06000276*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 
          get_TransferEncodingChunked() cil managed
  // SIG: 20 00 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x282e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_TransferEncodingChunked

  .method /*06000277*/ public hidebysig specialname 
          instance void  set_TransferEncodingChunked(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 02
  {
    // Method begins at RVA 0x2831
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpResponseHeaders::set_TransferEncodingChunked

  .method /*06000278*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductHeaderValue/*02000032*/> 
          get_Upgrade() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 C8
  {
    // Method begins at RVA 0x2833
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Upgrade

  .method /*06000279*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> 
          get_Vary() cil managed
  // SIG: 20 00 15 12 80 AC 01 0E
  {
    // Method begins at RVA 0x2836
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Vary

  .method /*0600027A*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/> 
          get_Via() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 EC
  {
    // Method begins at RVA 0x2839
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Via

  .method /*0600027B*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/> 
          get_Warning() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 F0
  {
    // Method begins at RVA 0x283c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_Warning

  .method /*0600027C*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/> 
          get_WwwAuthenticate() cil managed
  // SIG: 20 00 15 12 80 AC 01 12 80 88
  {
    // Method begins at RVA 0x283f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseHeaders::get_WwwAuthenticate

  .property /*170000CD*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string>
          AcceptRanges()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_AcceptRanges() /* 06000261 */
  } // end of property HttpResponseHeaders::AcceptRanges
  .property /*170000CE*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>
          Age()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Age() /* 06000262 */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_Age(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>) /* 06000263 */
  } // end of property HttpResponseHeaders::Age
  .property /*170000CF*/ instance callconv(8) class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/
          CacheControl()
  {
    .custom /*0C00015B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/ System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_CacheControl() /* 06000264 */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_CacheControl(class System.Net.Http.Headers.CacheControlHeaderValue/*02000023*/) /* 06000265 */
  } // end of property HttpResponseHeaders::CacheControl
  .property /*170000D0*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string>
          Connection()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Connection() /* 06000266 */
  } // end of property HttpResponseHeaders::Connection
  .property /*170000D1*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>
          ConnectionClose()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_ConnectionClose() /* 06000267 */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_ConnectionClose(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>) /* 06000268 */
  } // end of property HttpResponseHeaders::ConnectionClose
  .property /*170000D2*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          Date()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Date() /* 06000269 */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_Date(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>) /* 0600026A */
  } // end of property HttpResponseHeaders::Date
  .property /*170000D3*/ instance callconv(8) class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/
          ETag()
  {
    .custom /*0C00015D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_ETag() /* 0600026B */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_ETag(class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/) /* 0600026C */
  } // end of property HttpResponseHeaders::ETag
  .property /*170000D4*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000023*/
          Location()
  {
    .custom /*0C00015F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000023*/ System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Location() /* 0600026D */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_Location(class [System.Runtime/*23000001*/]System.Uri/*01000023*/) /* 0600026E */
  } // end of property HttpResponseHeaders::Location
  .property /*170000D5*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Pragma()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Pragma() /* 0600026F */
  } // end of property HttpResponseHeaders::Pragma
  .property /*170000D6*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/>
          ProxyAuthenticate()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_ProxyAuthenticate() /* 06000270 */
  } // end of property HttpResponseHeaders::ProxyAuthenticate
  .property /*170000D7*/ instance callconv(8) class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/
          RetryAfter()
  {
    .custom /*0C000160:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/ System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_RetryAfter() /* 06000271 */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_RetryAfter(class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/) /* 06000272 */
  } // end of property HttpResponseHeaders::RetryAfter
  .property /*170000D8*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/>
          Server()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Server() /* 06000273 */
  } // end of property HttpResponseHeaders::Server
  .property /*170000D9*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string>
          Trailer()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Trailer() /* 06000274 */
  } // end of property HttpResponseHeaders::Trailer
  .property /*170000DA*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/>
          TransferEncoding()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_TransferEncoding() /* 06000275 */
  } // end of property HttpResponseHeaders::TransferEncoding
  .property /*170000DB*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>
          TransferEncodingChunked()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_TransferEncodingChunked() /* 06000276 */
    .set instance void System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::set_TransferEncodingChunked(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<bool>) /* 06000277 */
  } // end of property HttpResponseHeaders::TransferEncodingChunked
  .property /*170000DC*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductHeaderValue/*02000032*/>
          Upgrade()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ProductHeaderValue/*02000032*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Upgrade() /* 06000278 */
  } // end of property HttpResponseHeaders::Upgrade
  .property /*170000DD*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string>
          Vary()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<string> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Vary() /* 06000279 */
  } // end of property HttpResponseHeaders::Vary
  .property /*170000DE*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/>
          Via()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Via() /* 0600027A */
  } // end of property HttpResponseHeaders::Via
  .property /*170000DF*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/>
          Warning()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_Warning() /* 0600027B */
  } // end of property HttpResponseHeaders::Warning
  .property /*170000E0*/ instance callconv(8) class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/>
          WwwAuthenticate()
  {
    .get instance class System.Net.Http.Headers.HttpHeaderValueCollection`1/*0200002B*/<class System.Net.Http.Headers.AuthenticationHeaderValue/*02000022*/> System.Net.Http.Headers.HttpResponseHeaders/*0200002D*/::get_WwwAuthenticate() /* 0600027C */
  } // end of property HttpResponseHeaders::WwwAuthenticate
} // end of class System.Net.Http.Headers.HttpResponseHeaders

.class /*0200002E*/ public auto ansi beforefieldinit System.Net.Http.Headers.MediaTypeHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000B5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000B6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C00008B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600027D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ source) cil managed
  // SIG: 20 01 01 12 80 B8
  {
    .custom /*0C000220:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2842
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MediaTypeHeaderValue::.ctor

  .method /*0600027E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string mediaType) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000221:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x284a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MediaTypeHeaderValue::.ctor

  .method /*0600027F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string mediaType,
                               string charSet) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C000222:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800020A*/ 
    .custom /*0C0001D0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2852
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MediaTypeHeaderValue::.ctor

  .method /*06000280*/ public hidebysig specialname 
          instance string  get_CharSet() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x285a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::get_CharSet

  .method /*06000281*/ public hidebysig specialname 
          instance void  set_CharSet(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x285d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MediaTypeHeaderValue::set_CharSet

  .method /*06000282*/ public hidebysig specialname 
          instance string  get_MediaType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x285f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::get_MediaType

  .method /*06000283*/ public hidebysig specialname 
          instance void  set_MediaType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800020C*/ 
    .custom /*0C0001D1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2862
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MediaTypeHeaderValue::set_MediaType

  .method /*06000284*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 80 C0
  {
    .custom /*0C000223:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2864
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::get_Parameters

  .method /*06000285*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*0800020D*/ 
    .custom /*0C0001D2:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2867
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::Equals

  .method /*06000286*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x286a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::GetHashCode

  .method /*06000287*/ public hidebysig static 
          class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 B8 0E
  {
    .custom /*0C000224:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x286d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::Parse

  .method /*06000288*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2870
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::System.ICloneable.Clone

  .method /*06000289*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000225:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2873
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::ToString

  .method /*0600028A*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 B8
  {
    .param [1]/*0800020F*/ 
    .custom /*0C0001D3:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000210*/ 
    .custom /*0C0001D4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2876
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeHeaderValue::TryParse

  .property /*170000E1*/ instance callconv(8) string
          CharSet()
  {
    .get instance string System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::get_CharSet() /* 06000280 */
    .set instance void System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::set_CharSet(string) /* 06000281 */
  } // end of property MediaTypeHeaderValue::CharSet
  .property /*170000E2*/ instance callconv(8) string
          MediaType()
  {
    .get instance string System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::get_MediaType() /* 06000282 */
    .set instance void System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::set_MediaType(string) /* 06000283 */
  } // end of property MediaTypeHeaderValue::MediaType
  .property /*170000E3*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Parameters()
  {
    .custom /*0C000164:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::get_Parameters() /* 06000284 */
  } // end of property MediaTypeHeaderValue::Parameters
} // end of class System.Net.Http.Headers.MediaTypeHeaderValue

.class /*0200002F*/ public auto ansi sealed beforefieldinit System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
       extends System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000BA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C00008F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600028B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string mediaType) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2879
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)00027D       */ call       instance void System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::.ctor(class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/) /* 0600027D */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method MediaTypeWithQualityHeaderValue::.ctor

  .method /*0600028C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string mediaType,
                               float64 quality) cil managed
  // SIG: 20 02 01 0E 0D
  {
    // Method begins at RVA 0x2882
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)00027D       */ call       instance void System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/::.ctor(class System.Net.Http.Headers.MediaTypeHeaderValue/*0200002E*/) /* 0600027D */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method MediaTypeWithQualityHeaderValue::.ctor

  .method /*0600028D*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> 
          get_Quality() cil managed
  // SIG: 20 00 15 11 80 E9 01 0D
  {
    // Method begins at RVA 0x288b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeWithQualityHeaderValue::get_Quality

  .method /*0600028E*/ public hidebysig specialname 
          instance void  set_Quality(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 0D
  {
    // Method begins at RVA 0x288e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MediaTypeWithQualityHeaderValue::set_Quality

  .method /*0600028F*/ public hidebysig static 
          class System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 BC 0E
  {
    // Method begins at RVA 0x2890
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeWithQualityHeaderValue::Parse

  .method /*06000290*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2893
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeWithQualityHeaderValue::System.ICloneable.Clone

  .method /*06000291*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 BC
  {
    .custom /*0C000226:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000216*/ 
    .custom /*0C0001D5:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000217*/ 
    .custom /*0C0001D6:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2896
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MediaTypeWithQualityHeaderValue::TryParse

  .property /*170000E4*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64>
          Quality()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/::get_Quality() /* 0600028D */
    .set instance void System.Net.Http.Headers.MediaTypeWithQualityHeaderValue/*0200002F*/::set_Quality(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64>) /* 0600028E */
  } // end of property MediaTypeWithQualityHeaderValue::Quality
} // end of class System.Net.Http.Headers.MediaTypeWithQualityHeaderValue

.class /*02000030*/ public auto ansi beforefieldinit System.Net.Http.Headers.NameValueHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000BF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C000093:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000292*/ famorassem hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/ source) cil managed
  // SIG: 20 01 01 12 80 C0
  {
    // Method begins at RVA 0x2899
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NameValueHeaderValue::.ctor

  .method /*06000293*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28a1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NameValueHeaderValue::.ctor

  .method /*06000294*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*0800021B*/ 
    .custom /*0C0001D7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28a9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NameValueHeaderValue::.ctor

  .method /*06000295*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::get_Name

  .method /*06000296*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000227:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::get_Value

  .method /*06000297*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000228:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28b7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NameValueHeaderValue::set_Value

  .method /*06000298*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000229:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800021D*/ 
    .custom /*0C0001D8:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x28b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::Equals

  .method /*06000299*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x28bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::GetHashCode

  .method /*0600029A*/ public hidebysig static 
          class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 C0 0E
  {
    // Method begins at RVA 0x28bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::Parse

  .method /*0600029B*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x28c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::System.ICloneable.Clone

  .method /*0600029C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::ToString

  .method /*0600029D*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 C0
  {
    .custom /*0C00022A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800021F*/ 
    .custom /*0C0001D9:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000220*/ 
    .custom /*0C0001DA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x28c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueHeaderValue::TryParse

  .property /*170000E5*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Net.Http.Headers.NameValueHeaderValue/*02000030*/::get_Name() /* 06000295 */
  } // end of property NameValueHeaderValue::Name
  .property /*170000E6*/ instance callconv(8) string
          Value()
  {
    .custom /*0C000166:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.NameValueHeaderValue/*02000030*/::get_Value() /* 06000296 */
    .set instance void System.Net.Http.Headers.NameValueHeaderValue/*02000030*/::set_Value(string) /* 06000297 */
  } // end of property NameValueHeaderValue::Value
} // end of class System.Net.Http.Headers.NameValueHeaderValue

.class /*02000031*/ public auto ansi beforefieldinit System.Net.Http.Headers.NameValueWithParametersHeaderValue
       extends System.Net.Http.Headers.NameValueHeaderValue/*02000030*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000C1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C000097:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600029E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/ source) cil managed
  // SIG: 20 01 01 12 80 C4
  {
    // Method begins at RVA 0x28cb
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000293       */ call       instance void System.Net.Http.Headers.NameValueHeaderValue/*02000030*/::.ctor(string) /* 06000293 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method NameValueWithParametersHeaderValue::.ctor

  .method /*0600029F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28d4
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000293       */ call       instance void System.Net.Http.Headers.NameValueHeaderValue/*02000030*/::.ctor(string) /* 06000293 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method NameValueWithParametersHeaderValue::.ctor

  .method /*060002A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000224*/ 
    .custom /*0C0001DB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28dd
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000293       */ call       instance void System.Net.Http.Headers.NameValueHeaderValue/*02000030*/::.ctor(string) /* 06000293 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method NameValueWithParametersHeaderValue::.ctor

  .method /*060002A1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 80 C0
  {
    // Method begins at RVA 0x28e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::get_Parameters

  .method /*060002A2*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00022B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000225*/ 
    .custom /*0C0001DC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x28e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::Equals

  .method /*060002A3*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x28ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::GetHashCode

  .method /*060002A4*/ public hidebysig static 
          class System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 C4 0E
  {
    // Method begins at RVA 0x28ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::Parse

  .method /*060002A5*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x28f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::System.ICloneable.Clone

  .method /*060002A6*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::ToString

  .method /*060002A7*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 C4
  {
    .custom /*0C00022C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000227*/ 
    .custom /*0C0001DD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000228*/ 
    .custom /*0C0001DE:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x28f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameValueWithParametersHeaderValue::TryParse

  .property /*170000E7*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Parameters()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.NameValueWithParametersHeaderValue/*02000031*/::get_Parameters() /* 060002A1 */
  } // end of property NameValueWithParametersHeaderValue::Parameters
} // end of class System.Net.Http.Headers.NameValueWithParametersHeaderValue

.class /*02000032*/ public auto ansi beforefieldinit System.Net.Http.Headers.ProductHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000C4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C00009A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28fb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProductHeaderValue::.ctor

  .method /*060002A9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*0800022B*/ 
    .custom /*0C0001E0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2903
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProductHeaderValue::.ctor

  .method /*060002AA*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x290b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::get_Name

  .method /*060002AB*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00022D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x290e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::get_Version

  .method /*060002AC*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00022E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800022C*/ 
    .custom /*0C0001E1:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2911
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::Equals

  .method /*060002AD*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2914
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::GetHashCode

  .method /*060002AE*/ public hidebysig static 
          class System.Net.Http.Headers.ProductHeaderValue/*02000032*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 C8 0E
  {
    // Method begins at RVA 0x2917
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::Parse

  .method /*060002AF*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x291a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::System.ICloneable.Clone

  .method /*060002B0*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x291d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::ToString

  .method /*060002B1*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.ProductHeaderValue/*02000032*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 C8
  {
    .custom /*0C00022F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800022E*/ 
    .custom /*0C0001E3:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800022F*/ 
    .custom /*0C0001E4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2920
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductHeaderValue::TryParse

  .property /*170000E8*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Net.Http.Headers.ProductHeaderValue/*02000032*/::get_Name() /* 060002AA */
  } // end of property ProductHeaderValue::Name
  .property /*170000E9*/ instance callconv(8) string
          Version()
  {
    .custom /*0C000167:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.ProductHeaderValue/*02000032*/::get_Version() /* 060002AB */
  } // end of property ProductHeaderValue::Version
} // end of class System.Net.Http.Headers.ProductHeaderValue

.class /*02000033*/ public auto ansi beforefieldinit System.Net.Http.Headers.ProductInfoHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000C9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C00009D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002B2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.ProductHeaderValue/*02000032*/ product) cil managed
  // SIG: 20 01 01 12 80 C8
  {
    // Method begins at RVA 0x2923
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProductInfoHeaderValue::.ctor

  .method /*060002B3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string comment) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x292b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProductInfoHeaderValue::.ctor

  .method /*060002B4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string productName,
                               string productVersion) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000233*/ 
    .custom /*0C0001E5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2933
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProductInfoHeaderValue::.ctor

  .method /*060002B5*/ public hidebysig specialname 
          instance string  get_Comment() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000230:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x293b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::get_Comment

  .method /*060002B6*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.ProductHeaderValue/*02000032*/ 
          get_Product() cil managed
  // SIG: 20 00 12 80 C8
  {
    .custom /*0C000231:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x293e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::get_Product

  .method /*060002B7*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000232:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000234*/ 
    .custom /*0C0001E7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2941
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::Equals

  .method /*060002B8*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2944
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::GetHashCode

  .method /*060002B9*/ public hidebysig static 
          class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 CC 0E
  {
    // Method begins at RVA 0x2947
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::Parse

  .method /*060002BA*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x294a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::System.ICloneable.Clone

  .method /*060002BB*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x294d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::ToString

  .method /*060002BC*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 CC
  {
    .param [1]/*08000236*/ 
    .custom /*0C0001EA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000237*/ 
    .custom /*0C0001EC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001ED:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2950
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProductInfoHeaderValue::TryParse

  .property /*170000EA*/ instance callconv(8) string
          Comment()
  {
    .custom /*0C000168:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/::get_Comment() /* 060002B5 */
  } // end of property ProductInfoHeaderValue::Comment
  .property /*170000EB*/ instance callconv(8) class System.Net.Http.Headers.ProductHeaderValue/*02000032*/
          Product()
  {
    .custom /*0C000169:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.ProductHeaderValue/*02000032*/ System.Net.Http.Headers.ProductInfoHeaderValue/*02000033*/::get_Product() /* 060002B6 */
  } // end of property ProductInfoHeaderValue::Product
} // end of class System.Net.Http.Headers.ProductInfoHeaderValue

.class /*02000034*/ public auto ansi beforefieldinit System.Net.Http.Headers.RangeConditionHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000CB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000A0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002BD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/ 'date') cil managed
  // SIG: 20 01 01 11 81 41
  {
    // Method begins at RVA 0x2953
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RangeConditionHeaderValue::.ctor

  .method /*060002BE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ entityTag) cil managed
  // SIG: 20 01 01 12 80 98
  {
    // Method begins at RVA 0x295b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RangeConditionHeaderValue::.ctor

  .method /*060002BF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string entityTag) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2963
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RangeConditionHeaderValue::.ctor

  .method /*060002C0*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_Date() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x296b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::get_Date

  .method /*060002C1*/ public hidebysig specialname 
          instance class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ 
          get_EntityTag() cil managed
  // SIG: 20 00 12 80 98
  {
    .custom /*0C000233:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x296e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::get_EntityTag

  .method /*060002C2*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000234:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800023B*/ 
    .custom /*0C0001EE:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2971
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::Equals

  .method /*060002C3*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2974
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::GetHashCode

  .method /*060002C4*/ public hidebysig static 
          class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 D0 0E
  {
    // Method begins at RVA 0x2977
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::Parse

  .method /*060002C5*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x297a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::System.ICloneable.Clone

  .method /*060002C6*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x297d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::ToString

  .method /*060002C7*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 D0
  {
    .custom /*0C000235:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800023D*/ 
    .custom /*0C0001EF:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800023E*/ 
    .custom /*0C0001F0:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2980
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeConditionHeaderValue::TryParse

  .property /*170000EC*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          Date()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/::get_Date() /* 060002C0 */
  } // end of property RangeConditionHeaderValue::Date
  .property /*170000ED*/ instance callconv(8) class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/
          EntityTag()
  {
    .custom /*0C00016A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Http.Headers.EntityTagHeaderValue/*02000026*/ System.Net.Http.Headers.RangeConditionHeaderValue/*02000034*/::get_EntityTag() /* 060002C1 */
  } // end of property RangeConditionHeaderValue::EntityTag
} // end of class System.Net.Http.Headers.RangeConditionHeaderValue

.class /*02000035*/ public auto ansi beforefieldinit System.Net.Http.Headers.RangeHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000CD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000A2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2983
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RangeHeaderValue::.ctor

  .method /*060002C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> from,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 'to') cil managed
  // SIG: 20 02 01 15 11 80 E9 01 0A 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x298b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RangeHeaderValue::.ctor

  .method /*060002CA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.RangeItemHeaderValue/*02000036*/> 
          get_Ranges() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 80 D8
  {
    // Method begins at RVA 0x2993
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::get_Ranges

  .method /*060002CB*/ public hidebysig specialname 
          instance string  get_Unit() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2996
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::get_Unit

  .method /*060002CC*/ public hidebysig specialname 
          instance void  set_Unit(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2999
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RangeHeaderValue::set_Unit

  .method /*060002CD*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000236:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000242*/ 
    .custom /*0C0001F4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x299b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::Equals

  .method /*060002CE*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x299e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::GetHashCode

  .method /*060002CF*/ public hidebysig static 
          class System.Net.Http.Headers.RangeHeaderValue/*02000035*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 D4 0E
  {
    // Method begins at RVA 0x29a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::Parse

  .method /*060002D0*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x29a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::System.ICloneable.Clone

  .method /*060002D1*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x29a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::ToString

  .method /*060002D2*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.RangeHeaderValue/*02000035*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 D4
  {
    .custom /*0C000237:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000244*/ 
    .custom /*0C0001F6:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000245*/ 
    .custom /*0C0001F7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHeaderValue::TryParse

  .property /*170000EE*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.RangeItemHeaderValue/*02000036*/>
          Ranges()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.RangeItemHeaderValue/*02000036*/> System.Net.Http.Headers.RangeHeaderValue/*02000035*/::get_Ranges() /* 060002CA */
  } // end of property RangeHeaderValue::Ranges
  .property /*170000EF*/ instance callconv(8) string
          Unit()
  {
    .get instance string System.Net.Http.Headers.RangeHeaderValue/*02000035*/::get_Unit() /* 060002CB */
    .set instance void System.Net.Http.Headers.RangeHeaderValue/*02000035*/::set_Unit(string) /* 060002CC */
  } // end of property RangeHeaderValue::Unit
} // end of class System.Net.Http.Headers.RangeHeaderValue

.class /*02000036*/ public auto ansi beforefieldinit System.Net.Http.Headers.RangeItemHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .method /*060002D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> from,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 'to') cil managed
  // SIG: 20 02 01 15 11 80 E9 01 0A 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x29ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RangeItemHeaderValue::.ctor

  .method /*060002D4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_From() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x29b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeItemHeaderValue::get_From

  .method /*060002D5*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> 
          get_To() cil managed
  // SIG: 20 00 15 11 80 E9 01 0A
  {
    // Method begins at RVA 0x29b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeItemHeaderValue::get_To

  .method /*060002D6*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000238:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000248*/ 
    .custom /*0C0001F9:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeItemHeaderValue::Equals

  .method /*060002D7*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x29be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeItemHeaderValue::GetHashCode

  .method /*060002D8*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x29c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeItemHeaderValue::System.ICloneable.Clone

  .method /*060002D9*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000239:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeItemHeaderValue::ToString

  .property /*170000F0*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          From()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.RangeItemHeaderValue/*02000036*/::get_From() /* 060002D4 */
  } // end of property RangeItemHeaderValue::From
  .property /*170000F1*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64>
          To()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<int64> System.Net.Http.Headers.RangeItemHeaderValue/*02000036*/::get_To() /* 060002D5 */
  } // end of property RangeItemHeaderValue::To
} // end of class System.Net.Http.Headers.RangeItemHeaderValue

.class /*02000037*/ public auto ansi beforefieldinit System.Net.Http.Headers.RetryConditionHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .method /*060002DA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/ 'date') cil managed
  // SIG: 20 01 01 11 81 41
  {
    // Method begins at RVA 0x29c7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RetryConditionHeaderValue::.ctor

  .method /*060002DB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ delta) cil managed
  // SIG: 20 01 01 11 80 99
  {
    // Method begins at RVA 0x29cf
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RetryConditionHeaderValue::.ctor

  .method /*060002DC*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_Date() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x29d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::get_Date

  .method /*060002DD*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          get_Delta() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 80 99
  {
    // Method begins at RVA 0x29da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::get_Delta

  .method /*060002DE*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00023A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800024B*/ 
    .custom /*0C0001FB:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::Equals

  .method /*060002DF*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x29e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::GetHashCode

  .method /*060002E0*/ public hidebysig static 
          class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 DC 0E
  {
    .custom /*0C00023B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::Parse

  .method /*060002E1*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x29e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::System.ICloneable.Clone

  .method /*060002E2*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00023C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::ToString

  .method /*060002E3*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 DC
  {
    .custom /*0C00023D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800024D*/ 
    .custom /*0C0001FC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800024E*/ 
    .custom /*0C0001FD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RetryConditionHeaderValue::TryParse

  .property /*170000F2*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          Date()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/::get_Date() /* 060002DC */
  } // end of property RetryConditionHeaderValue::Date
  .property /*170000F3*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/>
          Delta()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> System.Net.Http.Headers.RetryConditionHeaderValue/*02000037*/::get_Delta() /* 060002DD */
  } // end of property RetryConditionHeaderValue::Delta
} // end of class System.Net.Http.Headers.RetryConditionHeaderValue

.class /*02000038*/ public auto ansi beforefieldinit System.Net.Http.Headers.StringWithQualityHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000D6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000AB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002E4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x29ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StringWithQualityHeaderValue::.ctor

  .method /*060002E5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value',
                               float64 quality) cil managed
  // SIG: 20 02 01 0E 0D
  {
    // Method begins at RVA 0x29f7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StringWithQualityHeaderValue::.ctor

  .method /*060002E6*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> 
          get_Quality() cil managed
  // SIG: 20 00 15 11 80 E9 01 0D
  {
    // Method begins at RVA 0x29ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::get_Quality

  .method /*060002E7*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a02
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::get_Value

  .method /*060002E8*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00023E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000252*/ 
    .custom /*0C000202:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a05
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::Equals

  .method /*060002E9*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2a08
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::GetHashCode

  .method /*060002EA*/ public hidebysig static 
          class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 E0 0E
  {
    // Method begins at RVA 0x2a0b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::Parse

  .method /*060002EB*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2a0e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::System.ICloneable.Clone

  .method /*060002EC*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a11
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::ToString

  .method /*060002ED*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 E0
  {
    .custom /*0C00023F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000254*/ 
    .custom /*0C000205:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000255*/ 
    .custom /*0C000207:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a14
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringWithQualityHeaderValue::TryParse

  .property /*170000F4*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64>
          Quality()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/::get_Quality() /* 060002E6 */
  } // end of property StringWithQualityHeaderValue::Quality
  .property /*170000F5*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Net.Http.Headers.StringWithQualityHeaderValue/*02000038*/::get_Value() /* 060002E7 */
  } // end of property StringWithQualityHeaderValue::Value
} // end of class System.Net.Http.Headers.StringWithQualityHeaderValue

.class /*02000039*/ public auto ansi beforefieldinit System.Net.Http.Headers.TransferCodingHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000DA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000AE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002EE*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/ source) cil managed
  // SIG: 20 01 01 12 80 E4
  {
    // Method begins at RVA 0x2a17
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransferCodingHeaderValue::.ctor

  .method /*060002EF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2a1f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransferCodingHeaderValue::.ctor

  .method /*060002F0*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 80 C0
  {
    // Method begins at RVA 0x2a27
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::get_Parameters

  .method /*060002F1*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a2a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::get_Value

  .method /*060002F2*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000240:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000258*/ 
    .custom /*0C000209:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a2d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::Equals

  .method /*060002F3*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2a30
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::GetHashCode

  .method /*060002F4*/ public hidebysig static 
          class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 E4 0E
  {
    // Method begins at RVA 0x2a33
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::Parse

  .method /*060002F5*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2a36
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::System.ICloneable.Clone

  .method /*060002F6*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a39
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::ToString

  .method /*060002F7*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 E4
  {
    .custom /*0C000241:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800025A*/ 
    .custom /*0C00020A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800025B*/ 
    .custom /*0C00020B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a3c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingHeaderValue::TryParse

  .property /*170000F6*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/>
          Parameters()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100003D*/<class System.Net.Http.Headers.NameValueHeaderValue/*02000030*/> System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/::get_Parameters() /* 060002F0 */
  } // end of property TransferCodingHeaderValue::Parameters
  .property /*170000F7*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/::get_Value() /* 060002F1 */
  } // end of property TransferCodingHeaderValue::Value
} // end of class System.Net.Http.Headers.TransferCodingHeaderValue

.class /*0200003A*/ public auto ansi sealed beforefieldinit System.Net.Http.Headers.TransferCodingWithQualityHeaderValue
       extends System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000DC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000B1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002F8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2a3f
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)0002EE       */ call       instance void System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/::.ctor(class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/) /* 060002EE */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TransferCodingWithQualityHeaderValue::.ctor

  .method /*060002F9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value',
                               float64 quality) cil managed
  // SIG: 20 02 01 0E 0D
  {
    // Method begins at RVA 0x2a48
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)0002EE       */ call       instance void System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/::.ctor(class System.Net.Http.Headers.TransferCodingHeaderValue/*02000039*/) /* 060002EE */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TransferCodingWithQualityHeaderValue::.ctor

  .method /*060002FA*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> 
          get_Quality() cil managed
  // SIG: 20 00 15 11 80 E9 01 0D
  {
    // Method begins at RVA 0x2a51
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingWithQualityHeaderValue::get_Quality

  .method /*060002FB*/ public hidebysig specialname 
          instance void  set_Quality(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 E9 01 0D
  {
    // Method begins at RVA 0x2a54
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransferCodingWithQualityHeaderValue::set_Quality

  .method /*060002FC*/ public hidebysig static 
          class System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 E8 0E
  {
    // Method begins at RVA 0x2a56
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingWithQualityHeaderValue::Parse

  .method /*060002FD*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2a59
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingWithQualityHeaderValue::System.ICloneable.Clone

  .method /*060002FE*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 E8
  {
    .custom /*0C000242:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000261*/ 
    .custom /*0C00020C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000262*/ 
    .custom /*0C00020D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a5c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransferCodingWithQualityHeaderValue::TryParse

  .property /*170000F8*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64>
          Quality()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64> System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/::get_Quality() /* 060002FA */
    .set instance void System.Net.Http.Headers.TransferCodingWithQualityHeaderValue/*0200003A*/::set_Quality(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<float64>) /* 060002FB */
  } // end of property TransferCodingWithQualityHeaderValue::Quality
} // end of class System.Net.Http.Headers.TransferCodingWithQualityHeaderValue

.class /*0200003B*/ public auto ansi beforefieldinit System.Net.Http.Headers.ViaHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000DE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000B4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060002FF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string protocolVersion,
                               string receivedBy) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2a5f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ViaHeaderValue::.ctor

  .method /*06000300*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string protocolVersion,
                               string receivedBy,
                               string protocolName) cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .param [3]/*08000267*/ 
    .custom /*0C000210:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2a67
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ViaHeaderValue::.ctor

  .method /*06000301*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string protocolVersion,
                               string receivedBy,
                               string protocolName,
                               string comment) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    .param [3]/*0800026A*/ 
    .custom /*0C000211:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800026B*/ 
    .custom /*0C000213:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2a6f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ViaHeaderValue::.ctor

  .method /*06000302*/ public hidebysig specialname 
          instance string  get_Comment() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000243:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2a77
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::get_Comment

  .method /*06000303*/ public hidebysig specialname 
          instance string  get_ProtocolName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000244:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2a7a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::get_ProtocolName

  .method /*06000304*/ public hidebysig specialname 
          instance string  get_ProtocolVersion() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a7d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::get_ProtocolVersion

  .method /*06000305*/ public hidebysig specialname 
          instance string  get_ReceivedBy() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a80
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::get_ReceivedBy

  .method /*06000306*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000245:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800026C*/ 
    .custom /*0C000215:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a83
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::Equals

  .method /*06000307*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2a86
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::GetHashCode

  .method /*06000308*/ public hidebysig static 
          class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 EC 0E
  {
    // Method begins at RVA 0x2a89
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::Parse

  .method /*06000309*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2a8c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::System.ICloneable.Clone

  .method /*0600030A*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a8f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::ToString

  .method /*0600030B*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.ViaHeaderValue/*0200003B*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 EC
  {
    .custom /*0C000246:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800026E*/ 
    .custom /*0C000218:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800026F*/ 
    .custom /*0C000219:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a92
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ViaHeaderValue::TryParse

  .property /*170000F9*/ instance callconv(8) string
          Comment()
  {
    .custom /*0C00016F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.ViaHeaderValue/*0200003B*/::get_Comment() /* 06000302 */
  } // end of property ViaHeaderValue::Comment
  .property /*170000FA*/ instance callconv(8) string
          ProtocolName()
  {
    .custom /*0C000170:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Http.Headers.ViaHeaderValue/*0200003B*/::get_ProtocolName() /* 06000303 */
  } // end of property ViaHeaderValue::ProtocolName
  .property /*170000FB*/ instance callconv(8) string
          ProtocolVersion()
  {
    .get instance string System.Net.Http.Headers.ViaHeaderValue/*0200003B*/::get_ProtocolVersion() /* 06000304 */
  } // end of property ViaHeaderValue::ProtocolVersion
  .property /*170000FC*/ instance callconv(8) string
          ReceivedBy()
  {
    .get instance string System.Net.Http.Headers.ViaHeaderValue/*0200003B*/::get_ReceivedBy() /* 06000305 */
  } // end of property ViaHeaderValue::ReceivedBy
} // end of class System.Net.Http.Headers.ViaHeaderValue

.class /*0200003C*/ public auto ansi beforefieldinit System.Net.Http.Headers.WarningHeaderValue
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
       implements [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
{
  .custom /*0C0000E2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000E3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/
  .custom /*0C0000B9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600030C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 code,
                               string agent,
                               string text) cil managed
  // SIG: 20 03 01 08 0E 0E
  {
    // Method begins at RVA 0x2a95
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WarningHeaderValue::.ctor

  .method /*0600030D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 code,
                               string agent,
                               string text,
                               valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/ 'date') cil managed
  // SIG: 20 04 01 08 0E 0E 11 81 41
  {
    // Method begins at RVA 0x2a9d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WarningHeaderValue::.ctor

  .method /*0600030E*/ public hidebysig specialname 
          instance string  get_Agent() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2aa5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::get_Agent

  .method /*0600030F*/ public hidebysig specialname 
          instance int32  get_Code() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2aa8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::get_Code

  .method /*06000310*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> 
          get_Date() cil managed
  // SIG: 20 00 15 11 80 E9 01 11 81 41
  {
    // Method begins at RVA 0x2aab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::get_Date

  .method /*06000311*/ public hidebysig specialname 
          instance string  get_Text() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2aae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::get_Text

  .method /*06000312*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000247:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000277*/ 
    .custom /*0C00021C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2ab1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::Equals

  .method /*06000313*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2ab4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::GetHashCode

  .method /*06000314*/ public hidebysig static 
          class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/ 
          Parse(string input) cil managed
  // SIG: 00 01 12 80 F0 0E
  {
    // Method begins at RVA 0x2ab7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::Parse

  .method /*06000315*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.ICloneable/*0100004F*/::Clone /*0100004F::0A000035*/ 
    // Method begins at RVA 0x2aba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::System.ICloneable.Clone

  .method /*06000316*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2abd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::ToString

  .method /*06000317*/ public hidebysig static 
          bool  TryParse(string input,
                         [out] class System.Net.Http.Headers.WarningHeaderValue/*0200003C*/& parsedValue) cil managed
  // SIG: 00 02 02 0E 10 12 80 F0
  {
    .custom /*0C000248:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000279*/ 
    .custom /*0C00021D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800027A*/ 
    .custom /*0C00021E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2ac0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WarningHeaderValue::TryParse

  .property /*170000FD*/ instance callconv(8) string
          Agent()
  {
    .get instance string System.Net.Http.Headers.WarningHeaderValue/*0200003C*/::get_Agent() /* 0600030E */
  } // end of property WarningHeaderValue::Agent
  .property /*170000FE*/ instance callconv(8) int32
          Code()
  {
    .get instance int32 System.Net.Http.Headers.WarningHeaderValue/*0200003C*/::get_Code() /* 0600030F */
  } // end of property WarningHeaderValue::Code
  .property /*170000FF*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/>
          Date()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003A*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000050*/> System.Net.Http.Headers.WarningHeaderValue/*0200003C*/::get_Date() /* 06000310 */
  } // end of property WarningHeaderValue::Date
  .property /*17000100*/ instance callconv(8) string
          Text()
  {
    .get instance string System.Net.Http.Headers.WarningHeaderValue/*0200003C*/::get_Text() /* 06000311 */
  } // end of property WarningHeaderValue::Text
} // end of class System.Net.Http.Headers.WarningHeaderValue


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
