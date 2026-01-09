
//  .NET IL Disassembler.  Version 11.0.0-alpha.1.26055.101


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
// Time-date stamp:            0xf9e89767
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00006a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00008906
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
// Checksum:                       0x0001119f
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000088b4 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x000004e8] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00007400 [0x00002908] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00008898 [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000690c Virtual Size
//              0x00002000 Virtual Address
//              0x00006a00 Size of Raw Data
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
//              0x000004e8 Virtual Size
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00006c00 Pointer to Raw Data
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
//              0x00007200 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000908
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000088f6 Import Name Table
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
// 0x000024ec [0x0000632c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00008818 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003a08 Size
//              '#~' Name
//    Stream 2:
//              0x00003a74 Offset
//              0x00001aec Size
//              '#Strings' Name
//    Stream 3:
//              0x00005560 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00005564 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00005574 Offset
//              0x00000db8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000008901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 40200
// PE header size       : 512 (496 used)    ( 1.27%)
// PE additional info   : 11959             (29.75%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.18%)
// CLR meta-data size  : 25388              (63.15%)
// CLR additional info : 128                ( 0.32%)
// CLR method headers  : 371                ( 0.92%)
// Managed code         : 809               ( 2.01%)
// Data                 : 2048              ( 5.09%)
// Unaccounted          : -1087             (-2.70%)

// Num.of PE sections   : 3
//   .text    - 27136
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 25388
//   Module        -    1 (10 bytes)
//   TypeDef       -   33 (462 bytes)     8 interfaces, 0 explicit layout
//   TypeRef       -   63 (378 bytes)
//   MethodDef     -  399 (5586 bytes)    24 abstract, 0 native, 371 bodies
//   FieldDef      -    7 (42 bytes)      0 constant
//   MemberRef     -   30 (180 bytes)
//   ParamDef      -  746 (4476 bytes)
//   Constant      -    7 (42 bytes)
//   CustomAttribute-  407 (2442 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -   16 (64 bytes)
//   Property      -   71 (426 bytes)
//   MethodSemantic-   89 (534 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   ExportedType  -    1 (14 bytes)
//   Strings       -  6890 bytes
//   Blobs         -  3512 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   200 bytes

// CLR additional info : 128

// CLR method headers : 371
//   Num.of method bodies  - 371
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 371

// Managed code : 809
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Interface IFragmentCapableXmlDictionaryWriter (public) (abstract) (auto) (ansi) 
// Interface IStreamProvider                (public) (abstract) (auto) (ansi) 
// Interface IXmlBinaryReaderInitializer    (public) (abstract) (auto) (ansi) 
// Interface IXmlBinaryWriterInitializer    (public) (abstract) (auto) (ansi) 
// Interface IXmlDictionary                 (public) (abstract) (auto) (ansi) 
// Interface IXmlTextReaderInitializer      (public) (abstract) (auto) (ansi) 
// Interface IXmlTextWriterInitializer      (public) (abstract) (auto) (ansi) 
// Class OnXmlDictionaryReaderClose     (public) (auto) (ansi) (sealed) 
// Class UniqueId                       (public) (auto) (ansi) 
// Class XmlBinaryReaderSession         (public) (auto) (ansi) 
// Class XmlBinaryWriterSession         (public) (auto) (ansi) 
// Class XmlDictionary                  (public) (auto) (ansi) 
// Class XmlDictionaryReader            (public) (abstract) (auto) (ansi) 
// Class XmlDictionaryReaderQuotas      (public) (auto) (ansi) (sealed) 
// Class XmlDictionaryReaderQuotaTypes  (public) (auto) (ansi) (sealed) 
// Class XmlDictionaryString            (public) (auto) (ansi) 
// Class XmlDictionaryWriter            (public) (abstract) (auto) (ansi) 
// Class DataContractResolver           (public) (abstract) (auto) (ansi) 
// Class DataContractSerializer         (public) (auto) (ansi) (sealed) 
// Class DataContractSerializerExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class DataContractSerializerSettings (public) (auto) (ansi) 
// Class ExportOptions                  (public) (auto) (ansi) 
// Class ExtensionDataObject            (public) (auto) (ansi) (sealed) 
// Interface IExtensibleDataObject          (public) (abstract) (auto) (ansi) 
// Class XmlObjectSerializer            (public) (abstract) (auto) (ansi) 
// Class XmlSerializableServices        (public) (abstract) (auto) (ansi) (sealed) 
// Class XPathQueryGenerator            (public) (abstract) (auto) (ansi) (sealed) 
// Class XsdDataContractExporter        (public) (auto) (ansi) 
// Class DataContract                   (public) (abstract) (auto) (ansi) 
// Class DataContractSet                (public) (auto) (ansi) (sealed) 
// Class DataMember                     (public) (auto) (ansi) (sealed) 
// Class XmlDataContract                (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Xml.ReaderWriter
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000003*/ System.Runtime.Serialization.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000004*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Runtime.Serialization.Xml
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // .. System.Runtim
                                                                                                                                                                        65 2E 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 2E   // e.Serialization.
                                                                                                                                                                        58 6D 6C 00 00 )                                  // Xml..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // .. System.Runtim
                                                                                                                                                                       65 2E 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 2E   // e.Serialization.
                                                                                                                                                                       58 6D 6C 00 00 )                                  // Xml..
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // .. System.Runtim
                                                                                                                                                                 65 2E 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 2E   // e.Serialization.
                                                                                                                                                                 58 6D 6C 00 00 )                                  // Xml..
  .custom /*0C000017:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
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
  .ver 10:0:0:0
}
.class extern /*27000001*/ forwarder System.Runtime.Serialization.InvalidDataContractException
{
  .assembly extern System.Runtime.Serialization.Primitives /*23000003*/ 
}
.module System.Runtime.Serialization.Xml.dll
// MVID: {c7e796c5-b34d-4f48-8405-b62e5977b354}
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F0FC8402000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ interface public abstract auto ansi beforefieldinit System.Xml.IFragmentCapableXmlDictionaryWriter
{
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*06000001*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_CanFragment() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IFragmentCapableXmlDictionaryWriter::get_CanFragment

  .method /*06000002*/ public hidebysig newslot abstract virtual 
          instance void  EndFragment() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IFragmentCapableXmlDictionaryWriter::EndFragment

  .method /*06000003*/ public hidebysig newslot abstract virtual 
          instance void  StartFragment(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                       bool generateSelfContainedTextFragment) cil managed
  // SIG: 20 02 01 12 55 02
  {
    // Method begins at RVA 0x0
  } // end of method IFragmentCapableXmlDictionaryWriter::StartFragment

  .method /*06000004*/ public hidebysig newslot abstract virtual 
          instance void  WriteFragment(uint8[] buffer,
                                       int32 offset,
                                       int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IFragmentCapableXmlDictionaryWriter::WriteFragment

  .property /*17000001*/ instance callconv(8) bool
          CanFragment()
  {
    .get instance bool System.Xml.IFragmentCapableXmlDictionaryWriter/*02000002*/::get_CanFragment() /* 06000001 */
  } // end of property IFragmentCapableXmlDictionaryWriter::CanFragment
} // end of class System.Xml.IFragmentCapableXmlDictionaryWriter

.class /*02000003*/ interface public abstract auto ansi beforefieldinit System.Xml.IStreamProvider
{
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*06000005*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 
          GetStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x0
  } // end of method IStreamProvider::GetStream

  .method /*06000006*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x0
  } // end of method IStreamProvider::ReleaseStream

} // end of class System.Xml.IStreamProvider

.class /*02000004*/ interface public abstract auto ansi beforefieldinit System.Xml.IXmlBinaryReaderInitializer
{
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .method /*06000007*/ public hidebysig newslot abstract virtual 
          instance void  SetInput(uint8[] buffer,
                                  int32 offset,
                                  int32 count,
                                  class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                                  class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                                  class System.Xml.XmlBinaryReaderSession/*0200000B*/ session,
                                  class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 20 07 01 1D 05 08 08 12 18 12 3C 12 2C 12 24
  {
    .param [1]/*08000007*/ 
    .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800000B*/ 
    .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlBinaryReaderInitializer::SetInput

  .method /*06000008*/ public hidebysig newslot abstract virtual 
          instance void  SetInput(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                  class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                                  class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                                  class System.Xml.XmlBinaryReaderSession/*0200000B*/ session,
                                  class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 20 05 01 12 55 12 18 12 3C 12 2C 12 24
  {
    .param [1]/*0800000E*/ 
    .custom /*0C000030:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000010*/ 
    .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlBinaryReaderInitializer::SetInput

} // end of class System.Xml.IXmlBinaryReaderInitializer

.class /*02000005*/ interface public abstract auto ansi beforefieldinit System.Xml.IXmlBinaryWriterInitializer
{
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .method /*06000009*/ public hidebysig newslot abstract virtual 
          instance void  SetOutput(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                   class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                                   class System.Xml.XmlBinaryWriterSession/*0200000C*/ session,
                                   bool ownsStream) cil managed
  // SIG: 20 04 01 12 55 12 18 12 30 02
  {
    .param [1]/*08000013*/ 
    .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlBinaryWriterInitializer::SetOutput

} // end of class System.Xml.IXmlBinaryWriterInitializer

.class /*02000006*/ interface public abstract auto ansi beforefieldinit System.Xml.IXmlDictionary
{
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*0600000A*/ public hidebysig newslot abstract virtual 
          instance bool  TryLookup(int32 key,
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 08 10 12 44
  {
    .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000018*/ 
    .custom /*0C000047:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlDictionary::TryLookup

  .method /*0600000B*/ public hidebysig newslot abstract virtual 
          instance bool  TryLookup(string 'value',
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 0E 10 12 44
  {
    .param [2]/*0800001A*/ 
    .custom /*0C00004B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlDictionary::TryLookup

  .method /*0600000C*/ public hidebysig newslot abstract virtual 
          instance bool  TryLookup(class System.Xml.XmlDictionaryString/*02000011*/ 'value',
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 12 44 10 12 44
  {
    .param [2]/*0800001C*/ 
    .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000054:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlDictionary::TryLookup

} // end of class System.Xml.IXmlDictionary

.class /*02000007*/ interface public abstract auto ansi beforefieldinit System.Xml.IXmlTextReaderInitializer
{
  .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*0600000D*/ public hidebysig newslot abstract virtual 
          instance void  SetInput(uint8[] buffer,
                                  int32 offset,
                                  int32 count,
                                  class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                                  class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                                  class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 20 06 01 1D 05 08 08 12 61 12 3C 12 24
  {
    .param [4]/*08000020*/ 
    .custom /*0C00005F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000022*/ 
    .custom /*0C000062:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlTextReaderInitializer::SetInput

  .method /*0600000E*/ public hidebysig newslot abstract virtual 
          instance void  SetInput(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                  class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                                  class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                                  class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 20 04 01 12 55 12 61 12 3C 12 24
  {
    .param [2]/*08000024*/ 
    .custom /*0C000064:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000026*/ 
    .custom /*0C000066:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlTextReaderInitializer::SetInput

} // end of class System.Xml.IXmlTextReaderInitializer

.class /*02000008*/ interface public abstract auto ansi beforefieldinit System.Xml.IXmlTextWriterInitializer
{
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*0600000F*/ public hidebysig newslot abstract virtual 
          instance void  SetOutput(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                   class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                                   bool ownsStream) cil managed
  // SIG: 20 03 01 12 55 12 61 02
  {
    // Method begins at RVA 0x0
  } // end of method IXmlTextWriterInitializer::SetOutput

} // end of class System.Xml.IXmlTextWriterInitializer

.class /*02000009*/ public auto ansi sealed System.Xml.OnXmlDictionaryReaderClose
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000019*/
{
  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method OnXmlDictionaryReaderClose::.ctor

  .method /*06000011*/ public hidebysig newslot virtual 
          instance void  Invoke(class System.Xml.XmlDictionaryReader/*0200000E*/ reader) runtime managed
  // SIG: 20 01 01 12 38
  {
    .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method OnXmlDictionaryReaderClose::Invoke

  .method /*06000012*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001B*/ 
          BeginInvoke(class System.Xml.XmlDictionaryReader/*0200000E*/ reader,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001C*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 6D 12 38 12 71 1C
  {
    .param [1]/*0800002D*/ 
    .custom /*0C00006B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method OnXmlDictionaryReaderClose::BeginInvoke

  .method /*06000013*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001B*/ result) runtime managed
  // SIG: 20 01 01 12 6D
  {
  } // end of method OnXmlDictionaryReaderClose::EndInvoke

} // end of class System.Xml.OnXmlDictionaryReaderClose

.class /*0200000A*/ public auto ansi beforefieldinit System.Xml.UniqueId
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UniqueId::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] guid) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UniqueId::.ctor

  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] guid,
                               int32 offset) cil managed
  // SIG: 20 02 01 1D 05 08
  {
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UniqueId::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(char[] chars,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 03 08 08
  {
    // Method begins at RVA 0x2068
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UniqueId::.ctor

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/ guid) cil managed
  // SIG: 20 01 01 11 75
  {
    // Method begins at RVA 0x2070
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UniqueId::.ctor

  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2078
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UniqueId::.ctor

  .method /*0600001A*/ public hidebysig specialname 
          instance int32  get_CharArrayLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::get_CharArrayLength

  .method /*0600001B*/ public hidebysig specialname 
          instance bool  get_IsGuid() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::get_IsGuid

  .method /*0600001C*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000039*/ 
    .custom /*0C000072:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::Equals

  .method /*0600001D*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2089
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::GetHashCode

  .method /*0600001E*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xml.UniqueId/*0200000A*/ id1,
                            class System.Xml.UniqueId/*0200000A*/ id2) cil managed
  // SIG: 00 02 02 12 28 12 28
  {
    .custom /*0C000057:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::op_Equality

  .method /*0600001F*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xml.UniqueId/*0200000A*/ id1,
                              class System.Xml.UniqueId/*0200000A*/ id2) cil managed
  // SIG: 00 02 02 12 28 12 28
  {
    .custom /*0C00005A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::op_Inequality

  .method /*06000020*/ public hidebysig instance int32 
          ToCharArray(char[] chars,
                      int32 offset) cil managed
  // SIG: 20 02 08 1D 03 08
  {
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::ToCharArray

  .method /*06000021*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::ToString

  .method /*06000022*/ public hidebysig instance bool 
          TryGetGuid(uint8[] buffer,
                     int32 offset) cil managed
  // SIG: 20 02 02 1D 05 08
  {
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::TryGetGuid

  .method /*06000023*/ public hidebysig instance bool 
          TryGetGuid([out] valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/& guid) cil managed
  // SIG: 20 01 02 10 11 75
  {
    // Method begins at RVA 0x209b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UniqueId::TryGetGuid

  .property /*17000002*/ instance callconv(8) int32
          CharArrayLength()
  {
    .get instance int32 System.Xml.UniqueId/*0200000A*/::get_CharArrayLength() /* 0600001A */
  } // end of property UniqueId::CharArrayLength
  .property /*17000003*/ instance callconv(8) bool
          IsGuid()
  {
    .get instance bool System.Xml.UniqueId/*0200000A*/::get_IsGuid() /* 0600001B */
  } // end of property UniqueId::IsGuid
} // end of class System.Xml.UniqueId

.class /*0200000B*/ public auto ansi beforefieldinit System.Xml.XmlBinaryReaderSession
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements System.Xml.IXmlDictionary/*02000006*/
{
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Xml.IXmlDictionary/*02000006*/
  .custom /*0C000001:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlBinaryReaderSession::.ctor

  .method /*06000025*/ public hidebysig instance class System.Xml.XmlDictionaryString/*02000011*/ 
          Add(int32 id,
              string 'value') cil managed
  // SIG: 20 02 12 44 08 0E
  {
    // Method begins at RVA 0x20a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlBinaryReaderSession::Add

  .method /*06000026*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlBinaryReaderSession::Clear

  .method /*06000027*/ public hidebysig newslot virtual final 
          instance bool  TryLookup(int32 key,
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 08 10 12 44
  {
    .custom /*0C000068:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000046*/ 
    .custom /*0C000074:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlBinaryReaderSession::TryLookup

  .method /*06000028*/ public hidebysig newslot virtual final 
          instance bool  TryLookup(string 'value',
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 0E 10 12 44
  {
    .param [2]/*08000048*/ 
    .custom /*0C000075:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000076:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlBinaryReaderSession::TryLookup

  .method /*06000029*/ public hidebysig newslot virtual final 
          instance bool  TryLookup(class System.Xml.XmlDictionaryString/*02000011*/ 'value',
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 12 44 10 12 44
  {
    .param [2]/*0800004A*/ 
    .custom /*0C000077:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000078:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlBinaryReaderSession::TryLookup

} // end of class System.Xml.XmlBinaryReaderSession

.class /*0200000C*/ public auto ansi beforefieldinit System.Xml.XmlBinaryWriterSession
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlBinaryWriterSession::.ctor

  .method /*0600002B*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlBinaryWriterSession::Reset

  .method /*0600002C*/ public hidebysig newslot virtual 
          instance bool  TryAdd(class System.Xml.XmlDictionaryString/*02000011*/ 'value',
                                [out] int32& key) cil managed
  // SIG: 20 02 02 12 44 10 08
  {
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlBinaryWriterSession::TryAdd

} // end of class System.Xml.XmlBinaryWriterSession

.class /*0200000D*/ public auto ansi beforefieldinit System.Xml.XmlDictionary
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements System.Xml.IXmlDictionary/*02000006*/
{
  .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Xml.IXmlDictionary/*02000006*/
  .custom /*0C000019:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDictionary::.ctor

  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 capacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20c9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDictionary::.ctor

  .method /*0600002F*/ public hidebysig specialname static 
          class System.Xml.IXmlDictionary/*02000006*/ 
          get_Empty() cil managed
  // SIG: 00 00 12 18
  {
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionary::get_Empty

  .method /*06000030*/ public hidebysig newslot virtual 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          Add(string 'value') cil managed
  // SIG: 20 01 12 44 0E
  {
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionary::Add

  .method /*06000031*/ public hidebysig newslot virtual 
          instance bool  TryLookup(int32 key,
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 08 10 12 44
  {
    .custom /*0C00006F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000050*/ 
    .custom /*0C000079:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionary::TryLookup

  .method /*06000032*/ public hidebysig newslot virtual 
          instance bool  TryLookup(string 'value',
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 0E 10 12 44
  {
    .param [2]/*08000052*/ 
    .custom /*0C00007A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionary::TryLookup

  .method /*06000033*/ public hidebysig newslot virtual 
          instance bool  TryLookup(class System.Xml.XmlDictionaryString/*02000011*/ 'value',
                                   [out] class System.Xml.XmlDictionaryString/*02000011*/& result) cil managed
  // SIG: 20 02 02 12 44 10 12 44
  {
    .param [2]/*08000054*/ 
    .custom /*0C00007C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionary::TryLookup

  .property /*17000004*/ callconv(8) class System.Xml.IXmlDictionary/*02000006*/
          Empty()
  {
    .get class System.Xml.IXmlDictionary/*02000006*/ System.Xml.XmlDictionary/*0200000D*/::get_Empty() /* 0600002F */
  } // end of property XmlDictionary::Empty
} // end of class System.Xml.XmlDictionary

.class /*0200000E*/ public abstract auto ansi beforefieldinit System.Xml.XmlDictionaryReader
       extends [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/
{
  .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000034*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::.ctor

  .method /*06000035*/ public hidebysig newslot specialname virtual 
          instance bool  get_CanCanonicalize() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::get_CanCanonicalize

  .method /*06000036*/ public hidebysig newslot specialname virtual 
          instance class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ 
          get_Quotas() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::get_Quotas

  .method /*06000037*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(uint8[] buffer,
                             int32 offset,
                             int32 count,
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 05 12 38 1D 05 08 08 12 18 12 3C
  {
    .param [4]/*08000058*/ 
    .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*06000038*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(uint8[] buffer,
                             int32 offset,
                             int32 count,
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                             class System.Xml.XmlBinaryReaderSession/*0200000B*/ session) cil managed
  // SIG: 00 06 12 38 1D 05 08 08 12 18 12 3C 12 2C
  {
    .param [4]/*0800005D*/ 
    .custom /*0C000080:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800005F*/ 
    .custom /*0C000081:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*06000039*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(uint8[] buffer,
                             int32 offset,
                             int32 count,
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                             class System.Xml.XmlBinaryReaderSession/*0200000B*/ session,
                             class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 00 07 12 38 1D 05 08 08 12 18 12 3C 12 2C 12 24
  {
    .param [4]/*08000063*/ 
    .custom /*0C000082:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000065*/ 
    .custom /*0C000083:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [7]/*08000066*/ 
    .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*0600003A*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(uint8[] buffer,
                             int32 offset,
                             int32 count,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 04 12 38 1D 05 08 08 12 3C
  {
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*0600003B*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(uint8[] buffer,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 02 12 38 1D 05 12 3C
  {
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*0600003C*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 03 12 38 12 55 12 18 12 3C
  {
    .param [2]/*0800006E*/ 
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*0600003D*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                             class System.Xml.XmlBinaryReaderSession/*0200000B*/ session) cil managed
  // SIG: 00 04 12 38 12 55 12 18 12 3C 12 2C
  {
    .param [2]/*08000071*/ 
    .custom /*0C000086:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000073*/ 
    .custom /*0C000087:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*0600003E*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                             class System.Xml.XmlBinaryReaderSession/*0200000B*/ session,
                             class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 00 05 12 38 12 55 12 18 12 3C 12 2C 12 24
  {
    .param [2]/*08000075*/ 
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000077*/ 
    .custom /*0C000089:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000078*/ 
    .custom /*0C00008A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*0600003F*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateBinaryReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 02 12 38 12 55 12 3C
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateBinaryReader

  .method /*06000040*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateDictionaryReader(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader) cil managed
  // SIG: 00 01 12 38 12 79
  {
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateDictionaryReader

  .method /*06000041*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 05 12 38 1D 05 08 08 12 61 12 3C
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000042*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/[] encodings,
                           string contentType,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 06 12 38 1D 05 08 08 1D 12 61 0E 12 3C
  {
    .param [5]/*08000085*/ 
    .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000043*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/[] encodings,
                           string contentType,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                           int32 maxBufferSize,
                           class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 00 08 12 38 1D 05 08 08 1D 12 61 0E 12 3C 08 12 24
  {
    .param [5]/*0800008B*/ 
    .custom /*0C00008C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [8]/*0800008E*/ 
    .custom /*0C00008D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000044*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/[] encodings,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 05 12 38 1D 05 08 08 1D 12 61 12 3C
  {
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000045*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 03 12 38 12 55 12 61 12 3C
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000046*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/[] encodings,
                           string contentType,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 04 12 38 12 55 1D 12 61 0E 12 3C
  {
    .param [3]/*08000099*/ 
    .custom /*0C00008E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000047*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/[] encodings,
                           string contentType,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                           int32 maxBufferSize,
                           class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 00 06 12 38 12 55 1D 12 61 0E 12 3C 08 12 24
  {
    .param [3]/*0800009D*/ 
    .custom /*0C00008F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*080000A0*/ 
    .custom /*0C000090:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000048*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateMtomReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/[] encodings,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 03 12 38 12 55 1D 12 61 12 3C
  {
    // Method begins at RVA 0x2121
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateMtomReader

  .method /*06000049*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateTextReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                           class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 00 06 12 38 1D 05 08 08 12 61 12 3C 12 24
  {
    .param [4]/*080000A7*/ 
    .custom /*0C000091:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*080000A9*/ 
    .custom /*0C000092:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2124
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateTextReader

  .method /*0600004A*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateTextReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 04 12 38 1D 05 08 08 12 3C
  {
    // Method begins at RVA 0x2127
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateTextReader

  .method /*0600004B*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateTextReader(uint8[] buffer,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 02 12 38 1D 05 12 3C
  {
    // Method begins at RVA 0x212a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateTextReader

  .method /*0600004C*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateTextReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas,
                           class System.Xml.OnXmlDictionaryReaderClose/*02000009*/ onClose) cil managed
  // SIG: 00 04 12 38 12 55 12 61 12 3C 12 24
  {
    .param [2]/*080000B1*/ 
    .custom /*0C000096:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000B3*/ 
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateTextReader

  .method /*0600004D*/ public hidebysig static 
          class System.Xml.XmlDictionaryReader/*0200000E*/ 
          CreateTextReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 00 02 12 38 12 55 12 3C
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::CreateTextReader

  .method /*0600004E*/ public hidebysig newslot virtual 
          instance void  EndCanonicalization() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2133
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::EndCanonicalization

  .method /*0600004F*/ public hidebysig newslot virtual 
          instance string  GetAttribute(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                        class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 0E 12 44 12 44
  {
    .param [0]/*080000B6*/ 
    .custom /*0C000098:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::GetAttribute

  .method /*06000050*/ public hidebysig newslot virtual 
          instance void  GetNonAtomizedNames([out] string& localName,
                                             [out] string& namespaceUri) cil managed
  // SIG: 20 02 01 10 0E 10 0E
  {
    // Method begins at RVA 0x2138
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::GetNonAtomizedNames

  .method /*06000051*/ public hidebysig newslot virtual 
          instance int32  IndexOfLocalName(string[] localNames,
                                           string namespaceUri) cil managed
  // SIG: 20 02 08 1D 0E 0E
  {
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IndexOfLocalName

  .method /*06000052*/ public hidebysig newslot virtual 
          instance int32  IndexOfLocalName(class System.Xml.XmlDictionaryString/*02000011*/[] localNames,
                                           class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 08 1D 12 44 12 44
  {
    // Method begins at RVA 0x213e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IndexOfLocalName

  .method /*06000053*/ public hidebysig newslot virtual 
          instance bool  IsLocalName(string localName) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2141
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsLocalName

  .method /*06000054*/ public hidebysig newslot virtual 
          instance bool  IsLocalName(class System.Xml.XmlDictionaryString/*02000011*/ localName) cil managed
  // SIG: 20 01 02 12 44
  {
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsLocalName

  .method /*06000055*/ public hidebysig newslot virtual 
          instance bool  IsNamespaceUri(string namespaceUri) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsNamespaceUri

  .method /*06000056*/ public hidebysig newslot virtual 
          instance bool  IsNamespaceUri(class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 01 02 12 44
  {
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsNamespaceUri

  .method /*06000057*/ public hidebysig newslot virtual 
          instance bool  IsStartArray([out] class [System.Runtime/*23000001*/]System.Type/*0100001F*/& 'type') cil managed
  // SIG: 20 01 02 10 12 7D
  {
    .custom /*0C00007E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000C3*/ 
    .custom /*0C000099:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsStartArray

  .method /*06000058*/ public hidebysig newslot virtual 
          instance bool  IsStartElement(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                        class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 02 12 44 12 44
  {
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsStartElement

  .method /*06000059*/ family hidebysig instance bool 
          IsTextNode(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*01000020*/ nodeType) cil managed
  // SIG: 20 01 02 11 80 81
  {
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::IsTextNode

  .method /*0600005A*/ public hidebysig newslot virtual 
          instance void  MoveToStartElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2156
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::MoveToStartElement

  .method /*0600005B*/ public hidebysig newslot virtual 
          instance void  MoveToStartElement(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2158
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::MoveToStartElement

  .method /*0600005C*/ public hidebysig newslot virtual 
          instance void  MoveToStartElement(string localName,
                                            string namespaceUri) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x215a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::MoveToStartElement

  .method /*0600005D*/ public hidebysig newslot virtual 
          instance void  MoveToStartElement(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                            class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    // Method begins at RVA 0x215c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::MoveToStartElement

  .method /*0600005E*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    bool[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 02 08 08
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600005F*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 11 80 85 08 08
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000060*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 11 80 89 08 08
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000061*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    float64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 0D 08 08
  {
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000062*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 11 75 08 08
  {
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000063*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    int16[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 06 08 08
  {
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000064*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    int32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 08 08 08
  {
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000065*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    int64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 0A 08 08
  {
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000066*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    float32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 0C 08 08
  {
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000067*/ public hidebysig newslot virtual 
          instance int32  ReadArray(string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 0E 0E 1D 11 80 8D 08 08
  {
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000068*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    bool[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 02 08 08
  {
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000069*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 11 80 85 08 08
  {
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600006A*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 11 80 89 08 08
  {
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600006B*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    float64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 0D 08 08
  {
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600006C*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 11 75 08 08
  {
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600006D*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    int16[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 06 08 08
  {
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600006E*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    int32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 08 08 08
  {
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*0600006F*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    int64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 0A 08 08
  {
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000070*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    float32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 0C 08 08
  {
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000071*/ public hidebysig newslot virtual 
          instance int32  ReadArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 05 08 12 44 12 44 1D 11 80 8D 08 08
  {
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadArray

  .method /*06000072*/ public hidebysig newslot virtual 
          instance bool[]  ReadBooleanArray(string localName,
                                            string namespaceUri) cil managed
  // SIG: 20 02 1D 02 0E 0E
  {
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadBooleanArray

  .method /*06000073*/ public hidebysig newslot virtual 
          instance bool[]  ReadBooleanArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                            class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 02 12 44 12 44
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadBooleanArray

  .method /*06000074*/ public hidebysig virtual 
          instance object  ReadContentAs(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.IXmlNamespaceResolver/*01000024*/ namespaceResolver) cil managed
  // SIG: 20 02 1C 12 7D 12 80 91
  {
    .param [2]/*08000135*/ 
    .custom /*0C0000C8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAs

  .method /*06000075*/ public hidebysig newslot virtual 
          instance uint8[]  ReadContentAsBase64() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsBase64

  .method /*06000076*/ public hidebysig newslot virtual 
          instance uint8[]  ReadContentAsBinHex() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsBinHex

  .method /*06000077*/ family hidebysig instance uint8[] 
          ReadContentAsBinHex(int32 maxByteArrayContentLength) cil managed
  // SIG: 20 01 1D 05 08
  {
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsBinHex

  .method /*06000078*/ public hidebysig newslot virtual 
          instance int32  ReadContentAsChars(char[] chars,
                                             int32 offset,
                                             int32 count) cil managed
  // SIG: 20 03 08 1D 03 08 08
  {
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsChars

  .method /*06000079*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/ 
          ReadContentAsDecimal() cil managed
  // SIG: 20 00 11 80 89
  {
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsDecimal

  .method /*0600007A*/ public hidebysig virtual 
          instance float32  ReadContentAsFloat() cil managed
  // SIG: 20 00 0C
  {
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsFloat

  .method /*0600007B*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/ 
          ReadContentAsGuid() cil managed
  // SIG: 20 00 11 75
  {
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsGuid

  .method /*0600007C*/ public hidebysig newslot virtual 
          instance void  ReadContentAsQualifiedName([out] string& localName,
                                                    [out] string& namespaceUri) cil managed
  // SIG: 20 02 01 10 0E 10 0E
  {
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsQualifiedName

  .method /*0600007D*/ public hidebysig virtual 
          instance string  ReadContentAsString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsString

  .method /*0600007E*/ family hidebysig instance string 
          ReadContentAsString(int32 maxStringContentLength) cil managed
  // SIG: 20 01 0E 08
  {
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsString

  .method /*0600007F*/ public hidebysig newslot virtual 
          instance string  ReadContentAsString(string[] strings,
                                               [out] int32& index) cil managed
  // SIG: 20 02 0E 1D 0E 10 08
  {
    // Method begins at RVA 0x21c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsString

  .method /*06000080*/ public hidebysig newslot virtual 
          instance string  ReadContentAsString(class System.Xml.XmlDictionaryString/*02000011*/[] strings,
                                               [out] int32& index) cil managed
  // SIG: 20 02 0E 1D 12 44 10 08
  {
    // Method begins at RVA 0x21c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsString

  .method /*06000081*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/ 
          ReadContentAsTimeSpan() cil managed
  // SIG: 20 00 11 80 8D
  {
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsTimeSpan

  .method /*06000082*/ public hidebysig newslot virtual 
          instance class System.Xml.UniqueId/*0200000A*/ 
          ReadContentAsUniqueId() cil managed
  // SIG: 20 00 12 28
  {
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadContentAsUniqueId

  .method /*06000083*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/[] 
          ReadDateTimeArray(string localName,
                            string namespaceUri) cil managed
  // SIG: 20 02 1D 11 80 85 0E 0E
  {
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadDateTimeArray

  .method /*06000084*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/[] 
          ReadDateTimeArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                            class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 11 80 85 12 44 12 44
  {
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadDateTimeArray

  .method /*06000085*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/[] 
          ReadDecimalArray(string localName,
                           string namespaceUri) cil managed
  // SIG: 20 02 1D 11 80 89 0E 0E
  {
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadDecimalArray

  .method /*06000086*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/[] 
          ReadDecimalArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                           class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 11 80 89 12 44 12 44
  {
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadDecimalArray

  .method /*06000087*/ public hidebysig newslot virtual 
          instance float64[]  ReadDoubleArray(string localName,
                                              string namespaceUri) cil managed
  // SIG: 20 02 1D 0D 0E 0E
  {
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadDoubleArray

  .method /*06000088*/ public hidebysig newslot virtual 
          instance float64[]  ReadDoubleArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                              class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 0D 12 44 12 44
  {
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadDoubleArray

  .method /*06000089*/ public hidebysig newslot virtual 
          instance uint8[]  ReadElementContentAsBase64() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsBase64

  .method /*0600008A*/ public hidebysig newslot virtual 
          instance uint8[]  ReadElementContentAsBinHex() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsBinHex

  .method /*0600008B*/ public hidebysig virtual 
          instance bool  ReadElementContentAsBoolean() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsBoolean

  .method /*0600008C*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/ 
          ReadElementContentAsDateTime() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsDateTime

  .method /*0600008D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/ 
          ReadElementContentAsDecimal() cil managed
  // SIG: 20 00 11 80 89
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsDecimal

  .method /*0600008E*/ public hidebysig virtual 
          instance float64  ReadElementContentAsDouble() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsDouble

  .method /*0600008F*/ public hidebysig virtual 
          instance float32  ReadElementContentAsFloat() cil managed
  // SIG: 20 00 0C
  {
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsFloat

  .method /*06000090*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/ 
          ReadElementContentAsGuid() cil managed
  // SIG: 20 00 11 75
  {
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsGuid

  .method /*06000091*/ public hidebysig virtual 
          instance int32  ReadElementContentAsInt() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsInt

  .method /*06000092*/ public hidebysig virtual 
          instance int64  ReadElementContentAsLong() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsLong

  .method /*06000093*/ public hidebysig virtual 
          instance string  ReadElementContentAsString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsString

  .method /*06000094*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/ 
          ReadElementContentAsTimeSpan() cil managed
  // SIG: 20 00 11 80 8D
  {
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsTimeSpan

  .method /*06000095*/ public hidebysig newslot virtual 
          instance class System.Xml.UniqueId/*0200000A*/ 
          ReadElementContentAsUniqueId() cil managed
  // SIG: 20 00 12 28
  {
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadElementContentAsUniqueId

  .method /*06000096*/ public hidebysig newslot virtual 
          instance void  ReadFullStartElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2206
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::ReadFullStartElement

  .method /*06000097*/ public hidebysig newslot virtual 
          instance void  ReadFullStartElement(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2208
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::ReadFullStartElement

  .method /*06000098*/ public hidebysig newslot virtual 
          instance void  ReadFullStartElement(string localName,
                                              string namespaceUri) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x220a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::ReadFullStartElement

  .method /*06000099*/ public hidebysig newslot virtual 
          instance void  ReadFullStartElement(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                              class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    // Method begins at RVA 0x220c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::ReadFullStartElement

  .method /*0600009A*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/[] 
          ReadGuidArray(string localName,
                        string namespaceUri) cil managed
  // SIG: 20 02 1D 11 75 0E 0E
  {
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadGuidArray

  .method /*0600009B*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/[] 
          ReadGuidArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                        class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 11 75 12 44 12 44
  {
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadGuidArray

  .method /*0600009C*/ public hidebysig newslot virtual 
          instance int16[]  ReadInt16Array(string localName,
                                           string namespaceUri) cil managed
  // SIG: 20 02 1D 06 0E 0E
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadInt16Array

  .method /*0600009D*/ public hidebysig newslot virtual 
          instance int16[]  ReadInt16Array(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                           class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 06 12 44 12 44
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadInt16Array

  .method /*0600009E*/ public hidebysig newslot virtual 
          instance int32[]  ReadInt32Array(string localName,
                                           string namespaceUri) cil managed
  // SIG: 20 02 1D 08 0E 0E
  {
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadInt32Array

  .method /*0600009F*/ public hidebysig newslot virtual 
          instance int32[]  ReadInt32Array(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                           class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 08 12 44 12 44
  {
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadInt32Array

  .method /*060000A0*/ public hidebysig newslot virtual 
          instance int64[]  ReadInt64Array(string localName,
                                           string namespaceUri) cil managed
  // SIG: 20 02 1D 0A 0E 0E
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadInt64Array

  .method /*060000A1*/ public hidebysig newslot virtual 
          instance int64[]  ReadInt64Array(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                           class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 0A 12 44 12 44
  {
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadInt64Array

  .method /*060000A2*/ public hidebysig newslot virtual 
          instance float32[]  ReadSingleArray(string localName,
                                              string namespaceUri) cil managed
  // SIG: 20 02 1D 0C 0E 0E
  {
    // Method begins at RVA 0x2226
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadSingleArray

  .method /*060000A3*/ public hidebysig newslot virtual 
          instance float32[]  ReadSingleArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                              class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 0C 12 44 12 44
  {
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadSingleArray

  .method /*060000A4*/ public hidebysig newslot virtual 
          instance void  ReadStartElement(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                          class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    // Method begins at RVA 0x222c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::ReadStartElement

  .method /*060000A5*/ public hidebysig virtual 
          instance string  ReadString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x222e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadString

  .method /*060000A6*/ family hidebysig instance string 
          ReadString(int32 maxStringContentLength) cil managed
  // SIG: 20 01 0E 08
  {
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadString

  .method /*060000A7*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/[] 
          ReadTimeSpanArray(string localName,
                            string namespaceUri) cil managed
  // SIG: 20 02 1D 11 80 8D 0E 0E
  {
    // Method begins at RVA 0x2234
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadTimeSpanArray

  .method /*060000A8*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/[] 
          ReadTimeSpanArray(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                            class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 1D 11 80 8D 12 44 12 44
  {
    // Method begins at RVA 0x2237
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadTimeSpanArray

  .method /*060000A9*/ public hidebysig newslot virtual 
          instance int32  ReadValueAsBase64(uint8[] buffer,
                                            int32 offset,
                                            int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x223a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::ReadValueAsBase64

  .method /*060000AA*/ public hidebysig newslot virtual 
          instance void  StartCanonicalization(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                               bool includeComments,
                                               string[] inclusivePrefixes) cil managed
  // SIG: 20 03 01 12 55 02 1D 0E
  {
    .param [3]/*08000172*/ 
    .custom /*0C000115:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x223d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReader::StartCanonicalization

  .method /*060000AB*/ public hidebysig newslot virtual 
          instance bool  TryGetArrayLength([out] int32& count) cil managed
  // SIG: 20 01 02 10 08
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::TryGetArrayLength

  .method /*060000AC*/ public hidebysig newslot virtual 
          instance bool  TryGetBase64ContentLength([out] int32& length) cil managed
  // SIG: 20 01 02 10 08
  {
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::TryGetBase64ContentLength

  .method /*060000AD*/ public hidebysig newslot virtual 
          instance bool  TryGetLocalNameAsDictionaryString([out] class System.Xml.XmlDictionaryString/*02000011*/& localName) cil managed
  // SIG: 20 01 02 10 12 44
  {
    .custom /*0C000093:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000175*/ 
    .custom /*0C000116:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::TryGetLocalNameAsDictionaryString

  .method /*060000AE*/ public hidebysig newslot virtual 
          instance bool  TryGetNamespaceUriAsDictionaryString([out] class System.Xml.XmlDictionaryString/*02000011*/& namespaceUri) cil managed
  // SIG: 20 01 02 10 12 44
  {
    .custom /*0C000094:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000176*/ 
    .custom /*0C000119:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::TryGetNamespaceUriAsDictionaryString

  .method /*060000AF*/ public hidebysig newslot virtual 
          instance bool  TryGetValueAsDictionaryString([out] class System.Xml.XmlDictionaryString/*02000011*/& 'value') cil managed
  // SIG: 20 01 02 10 12 44
  {
    .custom /*0C000095:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000177*/ 
    .custom /*0C00011C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReader::TryGetValueAsDictionaryString

  .property /*17000005*/ instance callconv(8) bool
          CanCanonicalize()
  {
    .get instance bool System.Xml.XmlDictionaryReader/*0200000E*/::get_CanCanonicalize() /* 06000035 */
  } // end of property XmlDictionaryReader::CanCanonicalize
  .property /*17000006*/ instance callconv(8) class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/
          Quotas()
  {
    .get instance class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ System.Xml.XmlDictionaryReader/*0200000E*/::get_Quotas() /* 06000036 */
  } // end of property XmlDictionaryReader::Quotas
} // end of class System.Xml.XmlDictionaryReader

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Xml.XmlDictionaryReaderQuotas
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000B0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x224e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::.ctor

  .method /*060000B1*/ public hidebysig specialname static 
          class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ 
          get_Max() cil managed
  // SIG: 00 00 12 3C
  {
    // Method begins at RVA 0x2256
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_Max

  .method /*060000B2*/ public hidebysig specialname 
          instance int32  get_MaxArrayLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2259
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_MaxArrayLength

  .method /*060000B3*/ public hidebysig specialname 
          instance void  set_MaxArrayLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x225c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::set_MaxArrayLength

  .method /*060000B4*/ public hidebysig specialname 
          instance int32  get_MaxBytesPerRead() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x225e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_MaxBytesPerRead

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_MaxBytesPerRead(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2261
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::set_MaxBytesPerRead

  .method /*060000B6*/ public hidebysig specialname 
          instance int32  get_MaxDepth() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_MaxDepth

  .method /*060000B7*/ public hidebysig specialname 
          instance void  set_MaxDepth(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2266
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::set_MaxDepth

  .method /*060000B8*/ public hidebysig specialname 
          instance int32  get_MaxNameTableCharCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2268
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_MaxNameTableCharCount

  .method /*060000B9*/ public hidebysig specialname 
          instance void  set_MaxNameTableCharCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x226b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::set_MaxNameTableCharCount

  .method /*060000BA*/ public hidebysig specialname 
          instance int32  get_MaxStringContentLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x226d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_MaxStringContentLength

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_MaxStringContentLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2270
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::set_MaxStringContentLength

  .method /*060000BC*/ public hidebysig specialname 
          instance valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ 
          get_ModifiedQuotas() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryReaderQuotas::get_ModifiedQuotas

  .method /*060000BD*/ public hidebysig instance void 
          CopyTo(class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ quotas) cil managed
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x2275
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryReaderQuotas::CopyTo

  .property /*17000007*/ callconv(8) class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/
          Max()
  {
    .get class System.Xml.XmlDictionaryReaderQuotas/*0200000F*/ System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_Max() /* 060000B1 */
  } // end of property XmlDictionaryReaderQuotas::Max
  .property /*17000008*/ instance callconv(8) int32
          MaxArrayLength()
  {
    .custom /*0C000021:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.DefaultValueAttribute/*01000025*/::.ctor(int32) /* 0A000017 */ = ( 01 00 00 40 00 00 00 00 )                         // ...@....
    .get instance int32 System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_MaxArrayLength() /* 060000B2 */
    .set instance void System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::set_MaxArrayLength(int32) /* 060000B3 */
  } // end of property XmlDictionaryReaderQuotas::MaxArrayLength
  .property /*17000009*/ instance callconv(8) int32
          MaxBytesPerRead()
  {
    .custom /*0C000022:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.DefaultValueAttribute/*01000025*/::.ctor(int32) /* 0A000017 */ = ( 01 00 00 10 00 00 00 00 ) 
    .get instance int32 System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_MaxBytesPerRead() /* 060000B4 */
    .set instance void System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::set_MaxBytesPerRead(int32) /* 060000B5 */
  } // end of property XmlDictionaryReaderQuotas::MaxBytesPerRead
  .property /*1700000A*/ instance callconv(8) int32
          MaxDepth()
  {
    .custom /*0C000026:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.DefaultValueAttribute/*01000025*/::.ctor(int32) /* 0A000017 */ = ( 01 00 20 00 00 00 00 00 )                         // .. .....
    .get instance int32 System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_MaxDepth() /* 060000B6 */
    .set instance void System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::set_MaxDepth(int32) /* 060000B7 */
  } // end of property XmlDictionaryReaderQuotas::MaxDepth
  .property /*1700000B*/ instance callconv(8) int32
          MaxNameTableCharCount()
  {
    .custom /*0C00002A:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.DefaultValueAttribute/*01000025*/::.ctor(int32) /* 0A000017 */ = ( 01 00 00 40 00 00 00 00 )                         // ...@....
    .get instance int32 System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_MaxNameTableCharCount() /* 060000B8 */
    .set instance void System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::set_MaxNameTableCharCount(int32) /* 060000B9 */
  } // end of property XmlDictionaryReaderQuotas::MaxNameTableCharCount
  .property /*1700000C*/ instance callconv(8) int32
          MaxStringContentLength()
  {
    .custom /*0C00002B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.DefaultValueAttribute/*01000025*/::.ctor(int32) /* 0A000017 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .get instance int32 System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_MaxStringContentLength() /* 060000BA */
    .set instance void System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::set_MaxStringContentLength(int32) /* 060000BB */
  } // end of property XmlDictionaryReaderQuotas::MaxStringContentLength
  .property /*1700000D*/ instance callconv(8) valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/
          ModifiedQuotas()
  {
    .get instance valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ System.Xml.XmlDictionaryReaderQuotas/*0200000F*/::get_ModifiedQuotas() /* 060000BC */
  } // end of property XmlDictionaryReaderQuotas::ModifiedQuotas
} // end of class System.Xml.XmlDictionaryReaderQuotas

.class /*02000010*/ public auto ansi sealed System.Xml.XmlDictionaryReaderQuotaTypes
       extends [System.Runtime/*23000001*/]System.Enum/*01000027*/
{
  .custom /*0C000033:0A000018*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ MaxDepth = int32(0x00000001)
  .field /*04000003*/ public static literal valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ MaxStringContentLength = int32(0x00000002)
  .field /*04000004*/ public static literal valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ MaxArrayLength = int32(0x00000004)
  .field /*04000005*/ public static literal valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ MaxBytesPerRead = int32(0x00000008)
  .field /*04000006*/ public static literal valuetype System.Xml.XmlDictionaryReaderQuotaTypes/*02000010*/ MaxNameTableCharCount = int32(0x00000010)
} // end of class System.Xml.XmlDictionaryReaderQuotaTypes

.class /*02000011*/ public auto ansi beforefieldinit System.Xml.XmlDictionaryString
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000036:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000037:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000BE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                               string 'value',
                               int32 key) cil managed
  // SIG: 20 03 01 12 18 0E 08
  {
    // Method begins at RVA 0x2277
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryString::.ctor

  .method /*060000BF*/ public hidebysig specialname 
          instance class System.Xml.IXmlDictionary/*02000006*/ 
          get_Dictionary() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryString::get_Dictionary

  .method /*060000C0*/ public hidebysig specialname static 
          class System.Xml.XmlDictionaryString/*02000011*/ 
          get_Empty() cil managed
  // SIG: 00 00 12 44
  {
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryString::get_Empty

  .method /*060000C1*/ public hidebysig specialname 
          instance int32  get_Key() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryString::get_Key

  .method /*060000C2*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryString::get_Value

  .method /*060000C3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryString::ToString

  .property /*1700000E*/ instance callconv(8) class System.Xml.IXmlDictionary/*02000006*/
          Dictionary()
  {
    .get instance class System.Xml.IXmlDictionary/*02000006*/ System.Xml.XmlDictionaryString/*02000011*/::get_Dictionary() /* 060000BF */
  } // end of property XmlDictionaryString::Dictionary
  .property /*1700000F*/ callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          Empty()
  {
    .get class System.Xml.XmlDictionaryString/*02000011*/ System.Xml.XmlDictionaryString/*02000011*/::get_Empty() /* 060000C0 */
  } // end of property XmlDictionaryString::Empty
  .property /*17000010*/ instance callconv(8) int32
          Key()
  {
    .get instance int32 System.Xml.XmlDictionaryString/*02000011*/::get_Key() /* 060000C1 */
  } // end of property XmlDictionaryString::Key
  .property /*17000011*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.XmlDictionaryString/*02000011*/::get_Value() /* 060000C2 */
  } // end of property XmlDictionaryString::Value
} // end of class System.Xml.XmlDictionaryString

.class /*02000012*/ public abstract auto ansi beforefieldinit System.Xml.XmlDictionaryWriter
       extends [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/
{
  .custom /*0C000038:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000039:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000C4*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::.ctor

  .method /*060000C5*/ public hidebysig newslot specialname virtual 
          instance bool  get_CanCanonicalize() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::get_CanCanonicalize

  .method /*060000C6*/ public hidebysig virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2299
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::Close

  .method /*060000C7*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateBinaryWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 00 01 12 48 12 55
  {
    // Method begins at RVA 0x229b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateBinaryWriter

  .method /*060000C8*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateBinaryWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary) cil managed
  // SIG: 00 02 12 48 12 55 12 18
  {
    .param [2]/*08000183*/ 
    .custom /*0C000125:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x229e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateBinaryWriter

  .method /*060000C9*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateBinaryWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlBinaryWriterSession/*0200000C*/ session) cil managed
  // SIG: 00 03 12 48 12 55 12 18 12 30
  {
    .param [2]/*08000185*/ 
    .custom /*0C000126:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000186*/ 
    .custom /*0C000127:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateBinaryWriter

  .method /*060000CA*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateBinaryWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                             class System.Xml.IXmlDictionary/*02000006*/ dictionary,
                             class System.Xml.XmlBinaryWriterSession/*0200000C*/ session,
                             bool ownsStream) cil managed
  // SIG: 00 04 12 48 12 55 12 18 12 30 02
  {
    .param [2]/*08000188*/ 
    .custom /*0C000128:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000189*/ 
    .custom /*0C000129:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateBinaryWriter

  .method /*060000CB*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateDictionaryWriter(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer) cil managed
  // SIG: 00 01 12 48 12 80 A1
  {
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateDictionaryWriter

  .method /*060000CC*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateMtomWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           int32 maxSizeInBytes,
                           string startInfo) cil managed
  // SIG: 00 04 12 48 12 55 12 61 08 0E
  {
    // Method begins at RVA 0x22aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateMtomWriter

  .method /*060000CD*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateMtomWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           int32 maxSizeInBytes,
                           string startInfo,
                           string boundary,
                           string startUri,
                           bool writeMessageHeaders,
                           bool ownsStream) cil managed
  // SIG: 00 08 12 48 12 55 12 61 08 0E 0E 0E 02 02
  {
    .param [5]/*08000194*/ 
    .custom /*0C00012C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000195*/ 
    .custom /*0C00012D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateMtomWriter

  .method /*060000CE*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateTextWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 00 01 12 48 12 55
  {
    // Method begins at RVA 0x22b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateTextWriter

  .method /*060000CF*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateTextWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding) cil managed
  // SIG: 00 02 12 48 12 55 12 61
  {
    // Method begins at RVA 0x22b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateTextWriter

  .method /*060000D0*/ public hidebysig static 
          class System.Xml.XmlDictionaryWriter/*02000012*/ 
          CreateTextWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000018*/ encoding,
                           bool ownsStream) cil managed
  // SIG: 00 03 12 48 12 55 12 61 02
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::CreateTextWriter

  .method /*060000D1*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::Dispose

  .method /*060000D2*/ public hidebysig newslot virtual 
          instance void  EndCanonicalization() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::EndCanonicalization

  .method /*060000D3*/ public hidebysig newslot virtual 
          instance void  StartCanonicalization(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                               bool includeComments,
                                               string[] inclusivePrefixes) cil managed
  // SIG: 20 03 01 12 55 02 1D 0E
  {
    .param [3]/*080001A1*/ 
    .custom /*0C00012E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::StartCanonicalization

  .method /*060000D4*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    bool[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 02 08 08
  {
    .param [1]/*080001A2*/ 
    .custom /*0C00012F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001A4*/ 
    .custom /*0C000130:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22bf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000D5*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 11 80 85 08 08
  {
    .param [1]/*080001A8*/ 
    .custom /*0C000131:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001AA*/ 
    .custom /*0C000132:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000D6*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 11 80 89 08 08
  {
    .param [1]/*080001AE*/ 
    .custom /*0C000133:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001B0*/ 
    .custom /*0C000134:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000D7*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    float64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 0D 08 08
  {
    .param [1]/*080001B4*/ 
    .custom /*0C000135:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001B6*/ 
    .custom /*0C000136:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000D8*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 11 75 08 08
  {
    .param [1]/*080001BA*/ 
    .custom /*0C000137:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001BC*/ 
    .custom /*0C000138:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000D9*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    int16[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 06 08 08
  {
    .param [1]/*080001C0*/ 
    .custom /*0C000139:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001C2*/ 
    .custom /*0C00013A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000DA*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    int32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 08 08 08
  {
    .param [1]/*080001C6*/ 
    .custom /*0C00013B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001C8*/ 
    .custom /*0C00013C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000DB*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    int64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 0A 08 08
  {
    .param [1]/*080001CC*/ 
    .custom /*0C00013D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001CE*/ 
    .custom /*0C00013E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000DC*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    float32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 0C 08 08
  {
    .param [1]/*080001D2*/ 
    .custom /*0C00013F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001D4*/ 
    .custom /*0C000140:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000DD*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    string localName,
                                    string namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 0E 0E 1D 11 80 8D 08 08
  {
    .param [1]/*080001D8*/ 
    .custom /*0C000141:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001DA*/ 
    .custom /*0C000142:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000DE*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    bool[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 02 08 08
  {
    .param [1]/*080001DE*/ 
    .custom /*0C000143:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001E0*/ 
    .custom /*0C000144:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000DF*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 11 80 85 08 08
  {
    .param [1]/*080001E4*/ 
    .custom /*0C000145:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001E6*/ 
    .custom /*0C000146:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E0*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 11 80 89 08 08
  {
    .param [1]/*080001EA*/ 
    .custom /*0C000147:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001EC*/ 
    .custom /*0C000148:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E1*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    float64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 0D 08 08
  {
    .param [1]/*080001F0*/ 
    .custom /*0C000149:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001F2*/ 
    .custom /*0C00014A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E2*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 11 75 08 08
  {
    .param [1]/*080001F6*/ 
    .custom /*0C00014B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001F8*/ 
    .custom /*0C00014C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22db
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E3*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    int16[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 06 08 08
  {
    .param [1]/*080001FC*/ 
    .custom /*0C00014D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001FE*/ 
    .custom /*0C00014E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E4*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    int32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 08 08 08
  {
    .param [1]/*08000202*/ 
    .custom /*0C00014F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000204*/ 
    .custom /*0C000150:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22df
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E5*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    int64[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 0A 08 08
  {
    .param [1]/*08000208*/ 
    .custom /*0C000151:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800020A*/ 
    .custom /*0C000152:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E6*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    float32[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 0C 08 08
  {
    .param [1]/*0800020E*/ 
    .custom /*0C000153:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000210*/ 
    .custom /*0C000154:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E7*/ public hidebysig newslot virtual 
          instance void  WriteArray(string prefix,
                                    class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                    class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                                    valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/[] 'array',
                                    int32 offset,
                                    int32 count) cil managed
  // SIG: 20 06 01 0E 12 44 12 44 1D 11 80 8D 08 08
  {
    .param [1]/*08000214*/ 
    .custom /*0C000155:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000216*/ 
    .custom /*0C000156:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteArray

  .method /*060000E8*/ public hidebysig instance void 
          WriteAttributeString(string prefix,
                               class System.Xml.XmlDictionaryString/*02000011*/ localName,
                               class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                               string 'value') cil managed
  // SIG: 20 04 01 0E 12 44 12 44 0E
  {
    .custom /*0C00009A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800021B*/ 
    .custom /*0C000157:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22e7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteAttributeString

  .method /*060000E9*/ public hidebysig instance void 
          WriteAttributeString(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                               class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                               string 'value') cil managed
  // SIG: 20 03 01 12 44 12 44 0E
  {
    .custom /*0C00009B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800021E*/ 
    .custom /*0C000158:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteAttributeString

  .method /*060000EA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000029*/ 
          WriteBase64Async(uint8[] buffer,
                           int32 index,
                           int32 count) cil managed
  // SIG: 20 03 12 80 A5 1D 05 08 08
  {
    // Method begins at RVA 0x22eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::WriteBase64Async

  .method /*060000EB*/ public hidebysig instance void 
          WriteElementString(string prefix,
                             class System.Xml.XmlDictionaryString/*02000011*/ localName,
                             class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                             string 'value') cil managed
  // SIG: 20 04 01 0E 12 44 12 44 0E
  {
    .custom /*0C00009C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000225*/ 
    .custom /*0C000159:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteElementString

  .method /*060000EC*/ public hidebysig instance void 
          WriteElementString(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                             class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri,
                             string 'value') cil managed
  // SIG: 20 03 01 12 44 12 44 0E
  {
    .custom /*0C00009D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000228*/ 
    .custom /*0C00015A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteElementString

  .method /*060000ED*/ public hidebysig newslot virtual 
          instance void  WriteNode(class System.Xml.XmlDictionaryReader/*0200000E*/ reader,
                                   bool defattr) cil managed
  // SIG: 20 02 01 12 38 02
  {
    // Method begins at RVA 0x22f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteNode

  .method /*060000EE*/ public hidebysig virtual 
          instance void  WriteNode(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader,
                                   bool defattr) cil managed
  // SIG: 20 02 01 12 79 02
  {
    // Method begins at RVA 0x22f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteNode

  .method /*060000EF*/ public hidebysig newslot virtual 
          instance void  WriteQualifiedName(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                            class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    .param [2]/*08000230*/ 
    .custom /*0C00015B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteQualifiedName

  .method /*060000F0*/ public hidebysig newslot virtual 
          instance void  WriteStartAttribute(string prefix,
                                             class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                             class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 03 01 0E 12 44 12 44
  {
    .custom /*0C00009E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000232*/ 
    .custom /*0C00015C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteStartAttribute

  .method /*060000F1*/ public hidebysig instance void 
          WriteStartAttribute(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                              class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    .param [2]/*08000235*/ 
    .custom /*0C00015D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteStartAttribute

  .method /*060000F2*/ public hidebysig newslot virtual 
          instance void  WriteStartElement(string prefix,
                                           class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                           class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 03 01 0E 12 44 12 44
  {
    .custom /*0C00009F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000237*/ 
    .custom /*0C00015E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteStartElement

  .method /*060000F3*/ public hidebysig instance void 
          WriteStartElement(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                            class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    .param [2]/*0800023A*/ 
    .custom /*0C00015F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteStartElement

  .method /*060000F4*/ public hidebysig newslot virtual 
          instance void  WriteString(class System.Xml.XmlDictionaryString/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C0000A0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2300
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteString

  .method /*060000F5*/ family hidebysig newslot virtual 
          instance void  WriteTextNode(class System.Xml.XmlDictionaryReader/*0200000E*/ reader,
                                       bool isAttribute) cil managed
  // SIG: 20 02 01 12 38 02
  {
    // Method begins at RVA 0x2302
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteTextNode

  .method /*060000F6*/ public hidebysig newslot virtual 
          instance void  WriteValue(valuetype [System.Runtime/*23000001*/]System.Guid/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 11 75
  {
    // Method begins at RVA 0x2304
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteValue

  .method /*060000F7*/ public hidebysig newslot virtual 
          instance void  WriteValue(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 11 80 8D
  {
    // Method begins at RVA 0x2306
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteValue

  .method /*060000F8*/ public hidebysig newslot virtual 
          instance void  WriteValue(class System.Xml.IStreamProvider/*02000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0C
  {
    // Method begins at RVA 0x2308
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteValue

  .method /*060000F9*/ public hidebysig newslot virtual 
          instance void  WriteValue(class System.Xml.UniqueId/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x230a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteValue

  .method /*060000FA*/ public hidebysig newslot virtual 
          instance void  WriteValue(class System.Xml.XmlDictionaryString/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C0000A1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x230c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteValue

  .method /*060000FB*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000029*/ 
          WriteValueAsync(class System.Xml.IStreamProvider/*02000003*/ 'value') cil managed
  // SIG: 20 01 12 80 A5 12 0C
  {
    // Method begins at RVA 0x230e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDictionaryWriter::WriteValueAsync

  .method /*060000FC*/ public hidebysig newslot virtual 
          instance void  WriteXmlAttribute(string localName,
                                           string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000245*/ 
    .custom /*0C000160:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2311
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteXmlAttribute

  .method /*060000FD*/ public hidebysig newslot virtual 
          instance void  WriteXmlAttribute(class System.Xml.XmlDictionaryString/*02000011*/ localName,
                                           class System.Xml.XmlDictionaryString/*02000011*/ 'value') cil managed
  // SIG: 20 02 01 12 44 12 44
  {
    .param [2]/*08000247*/ 
    .custom /*0C000161:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2313
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteXmlAttribute

  .method /*060000FE*/ public hidebysig newslot virtual 
          instance void  WriteXmlnsAttribute(string prefix,
                                             string namespaceUri) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [1]/*08000248*/ 
    .custom /*0C000162:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2315
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteXmlnsAttribute

  .method /*060000FF*/ public hidebysig newslot virtual 
          instance void  WriteXmlnsAttribute(string prefix,
                                             class System.Xml.XmlDictionaryString/*02000011*/ namespaceUri) cil managed
  // SIG: 20 02 01 0E 12 44
  {
    .param [1]/*0800024A*/ 
    .custom /*0C000163:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2317
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDictionaryWriter::WriteXmlnsAttribute

  .property /*17000012*/ instance callconv(8) bool
          CanCanonicalize()
  {
    .get instance bool System.Xml.XmlDictionaryWriter/*02000012*/::get_CanCanonicalize() /* 060000C5 */
  } // end of property XmlDictionaryWriter::CanCanonicalize
} // end of class System.Xml.XmlDictionaryWriter

.class /*02000013*/ public abstract auto ansi beforefieldinit System.Runtime.Serialization.DataContractResolver
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000100*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2319
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractResolver::.ctor

  .method /*06000101*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 
          ResolveName(string typeName,
                      string typeNamespace,
                      class [System.Runtime/*23000001*/]System.Type/*0100001F*/ declaredType,
                      class System.Runtime.Serialization.DataContractResolver/*02000013*/ knownTypeResolver) cil managed
  // SIG: 20 04 12 7D 0E 0E 12 7D 12 4C
  {
    .custom /*0C0000A2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000A3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [1]/*0800024C*/ 
    .custom /*0C000164:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800024F*/ 
    .custom /*0C000165:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DataContractResolver::ResolveName

  .method /*06000102*/ public hidebysig newslot abstract virtual 
          instance bool  TryResolveType(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                                        class [System.Runtime/*23000001*/]System.Type/*0100001F*/ declaredType,
                                        class System.Runtime.Serialization.DataContractResolver/*02000013*/ knownTypeResolver,
                                        [out] class System.Xml.XmlDictionaryString/*02000011*/& typeName,
                                        [out] class System.Xml.XmlDictionaryString/*02000011*/& typeNamespace) cil managed
  // SIG: 20 05 02 12 7D 12 7D 12 4C 10 12 44 10 12 44
  {
    .custom /*0C0000A4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000A5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [1]/*08000250*/ 
    .custom /*0C000166:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000252*/ 
    .custom /*0C000167:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DataContractResolver::TryResolveType

} // end of class System.Runtime.Serialization.DataContractResolver

.class /*02000014*/ public auto ansi sealed beforefieldinit System.Runtime.Serialization.DataContractSerializer
       extends System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/
{
  .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000103*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x2321
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*06000104*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> knownTypes) cil managed
  // SIG: 20 02 01 12 7D 15 12 80 B1 01 12 7D
  {
    .param [2]/*08000257*/ 
    .custom /*0C000168:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2329
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*06000105*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               class System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/ settings) cil managed
  // SIG: 20 02 01 12 7D 12 58
  {
    .param [2]/*08000259*/ 
    .custom /*0C000169:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2331
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*06000106*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               string rootName,
                               string rootNamespace) cil managed
  // SIG: 20 03 01 12 7D 0E 0E
  {
    // Method begins at RVA 0x2339
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*06000107*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               string rootName,
                               string rootNamespace,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> knownTypes) cil managed
  // SIG: 20 04 01 12 7D 0E 0E 15 12 80 B1 01 12 7D
  {
    .param [4]/*08000260*/ 
    .custom /*0C00016A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2341
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*06000108*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               class System.Xml.XmlDictionaryString/*02000011*/ rootName,
                               class System.Xml.XmlDictionaryString/*02000011*/ rootNamespace) cil managed
  // SIG: 20 03 01 12 7D 12 44 12 44
  {
    // Method begins at RVA 0x2349
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*06000109*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               class System.Xml.XmlDictionaryString/*02000011*/ rootName,
                               class System.Xml.XmlDictionaryString/*02000011*/ rootNamespace,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> knownTypes) cil managed
  // SIG: 20 04 01 12 7D 12 44 12 44 15 12 80 B1 01 12 7D
  {
    .param [4]/*08000267*/ 
    .custom /*0C00016B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2351
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000138       */ call       instance void System.Runtime.Serialization.XmlObjectSerializer/*0200001A*/::.ctor() /* 06000138 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::.ctor

  .method /*0600010A*/ public hidebysig specialname 
          instance class System.Runtime.Serialization.DataContractResolver/*02000013*/ 
          get_DataContractResolver() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C0000A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2359
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::get_DataContractResolver

  .method /*0600010B*/ public hidebysig specialname 
          instance bool  get_IgnoreExtensionDataObject() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::get_IgnoreExtensionDataObject

  .method /*0600010C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> 
          get_KnownTypes() cil managed
  // SIG: 20 00 15 12 80 B5 01 12 7D
  {
    // Method begins at RVA 0x235f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::get_KnownTypes

  .method /*0600010D*/ public hidebysig specialname 
          instance int32  get_MaxItemsInObjectGraph() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2362
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::get_MaxItemsInObjectGraph

  .method /*0600010E*/ public hidebysig specialname 
          instance bool  get_PreserveObjectReferences() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::get_PreserveObjectReferences

  .method /*0600010F*/ public hidebysig specialname 
          instance bool  get_SerializeReadOnlyTypes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::get_SerializeReadOnlyTypes

  .method /*06000110*/ public hidebysig virtual 
          instance bool  IsStartObject(class System.Xml.XmlDictionaryReader/*0200000E*/ reader) cil managed
  // SIG: 20 01 02 12 38
  {
    .custom /*0C0000A7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000A8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::IsStartObject

  .method /*06000111*/ public hidebysig virtual 
          instance bool  IsStartObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader) cil managed
  // SIG: 20 01 02 12 79
  {
    .custom /*0C0000A9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000AA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::IsStartObject

  .method /*06000112*/ public hidebysig virtual 
          instance object  ReadObject(class System.Xml.XmlDictionaryReader/*0200000E*/ reader,
                                      bool verifyObjectName) cil managed
  // SIG: 20 02 1C 12 38 02
  {
    .custom /*0C0000AB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000AC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*0800026A*/ 
    .custom /*0C00016C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::ReadObject

  .method /*06000113*/ public hidebysig instance object 
          ReadObject(class System.Xml.XmlDictionaryReader/*0200000E*/ reader,
                     bool verifyObjectName,
                     class System.Runtime.Serialization.DataContractResolver/*02000013*/ dataContractResolver) cil managed
  // SIG: 20 03 1C 12 38 02 12 4C
  {
    .custom /*0C0000AD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000AE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000AF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [1]/*0800026D*/ 
    .custom /*0C00016D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::ReadObject

  .method /*06000114*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader) cil managed
  // SIG: 20 01 1C 12 79
  {
    .custom /*0C0000B0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000B1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000270*/ 
    .custom /*0C00016E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::ReadObject

  .method /*06000115*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader,
                                      bool verifyObjectName) cil managed
  // SIG: 20 02 1C 12 79 02
  {
    .custom /*0C0000B2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000B3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000272*/ 
    .custom /*0C00016F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializer::ReadObject

  .method /*06000116*/ public hidebysig virtual 
          instance void  WriteEndObject(class System.Xml.XmlDictionaryWriter/*02000012*/ writer) cil managed
  // SIG: 20 01 01 12 48
  {
    .custom /*0C0000B4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000B5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x237d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteEndObject

  .method /*06000117*/ public hidebysig virtual 
          instance void  WriteEndObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C0000B6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000B7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x237f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteEndObject

  .method /*06000118*/ public hidebysig instance void 
          WriteObject(class System.Xml.XmlDictionaryWriter/*02000012*/ writer,
                      object graph,
                      class System.Runtime.Serialization.DataContractResolver/*02000013*/ dataContractResolver) cil managed
  // SIG: 20 03 01 12 48 1C 12 4C
  {
    .custom /*0C0000B8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000BA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [1]/*08000277*/ 
    .custom /*0C000170:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2381
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteObject

  .method /*06000119*/ public hidebysig virtual 
          instance void  WriteObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer,
                                     object graph) cil managed
  // SIG: 20 02 01 12 80 A1 1C
  {
    .custom /*0C0000BB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000BC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800027B*/ 
    .custom /*0C000171:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2383
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteObject

  .method /*0600011A*/ public hidebysig virtual 
          instance void  WriteObjectContent(class System.Xml.XmlDictionaryWriter/*02000012*/ writer,
                                            object graph) cil managed
  // SIG: 20 02 01 12 48 1C
  {
    .custom /*0C0000BD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000BE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800027D*/ 
    .custom /*0C000172:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2385
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteObjectContent

  .method /*0600011B*/ public hidebysig virtual 
          instance void  WriteObjectContent(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer,
                                            object graph) cil managed
  // SIG: 20 02 01 12 80 A1 1C
  {
    .custom /*0C0000BF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000C0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800027F*/ 
    .custom /*0C000173:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2387
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteObjectContent

  .method /*0600011C*/ public hidebysig virtual 
          instance void  WriteStartObject(class System.Xml.XmlDictionaryWriter/*02000012*/ writer,
                                          object graph) cil managed
  // SIG: 20 02 01 12 48 1C
  {
    .custom /*0C0000C1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000C2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000281*/ 
    .custom /*0C000174:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2389
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteStartObject

  .method /*0600011D*/ public hidebysig virtual 
          instance void  WriteStartObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer,
                                          object graph) cil managed
  // SIG: 20 02 01 12 80 A1 1C
  {
    .custom /*0C0000C3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000C4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000283*/ 
    .custom /*0C000175:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializer::WriteStartObject

  .property /*17000013*/ instance callconv(8) class System.Runtime.Serialization.DataContractResolver/*02000013*/
          DataContractResolver()
  {
    .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Runtime.Serialization.DataContractResolver/*02000013*/ System.Runtime.Serialization.DataContractSerializer/*02000014*/::get_DataContractResolver() /* 0600010A */
  } // end of property DataContractSerializer::DataContractResolver
  .property /*17000014*/ instance callconv(8) bool
          IgnoreExtensionDataObject()
  {
    .get instance bool System.Runtime.Serialization.DataContractSerializer/*02000014*/::get_IgnoreExtensionDataObject() /* 0600010B */
  } // end of property DataContractSerializer::IgnoreExtensionDataObject
  .property /*17000015*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/>
          KnownTypes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> System.Runtime.Serialization.DataContractSerializer/*02000014*/::get_KnownTypes() /* 0600010C */
  } // end of property DataContractSerializer::KnownTypes
  .property /*17000016*/ instance callconv(8) int32
          MaxItemsInObjectGraph()
  {
    .get instance int32 System.Runtime.Serialization.DataContractSerializer/*02000014*/::get_MaxItemsInObjectGraph() /* 0600010D */
  } // end of property DataContractSerializer::MaxItemsInObjectGraph
  .property /*17000017*/ instance callconv(8) bool
          PreserveObjectReferences()
  {
    .get instance bool System.Runtime.Serialization.DataContractSerializer/*02000014*/::get_PreserveObjectReferences() /* 0600010E */
  } // end of property DataContractSerializer::PreserveObjectReferences
  .property /*17000018*/ instance callconv(8) bool
          SerializeReadOnlyTypes()
  {
    .get instance bool System.Runtime.Serialization.DataContractSerializer/*02000014*/::get_SerializeReadOnlyTypes() /* 0600010F */
  } // end of property DataContractSerializer::SerializeReadOnlyTypes
} // end of class System.Runtime.Serialization.DataContractSerializer

.class /*02000015*/ public abstract auto ansi sealed beforefieldinit System.Runtime.Serialization.DataContractSerializerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000040:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000042:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600011E*/ public hidebysig static 
          class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/ 
          GetSerializationSurrogateProvider(class System.Runtime.Serialization.DataContractSerializer/*02000014*/ serializer) cil managed
  // SIG: 00 01 12 80 B9 12 50
  {
    .custom /*0C0000C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000284*/ 
    .custom /*0C000176:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerExtensions::GetSerializationSurrogateProvider

  .method /*0600011F*/ public hidebysig static 
          void  SetSerializationSurrogateProvider(class System.Runtime.Serialization.DataContractSerializer/*02000014*/ serializer,
                                                  class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/ provider) cil managed
  // SIG: 00 02 01 12 50 12 80 B9
  {
    .custom /*0C0000C6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000287*/ 
    .custom /*0C000177:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2390
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerExtensions::SetSerializationSurrogateProvider

} // end of class System.Runtime.Serialization.DataContractSerializerExtensions

.class /*02000016*/ public auto ansi beforefieldinit System.Runtime.Serialization.DataContractSerializerSettings
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000120*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2392
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::.ctor

  .method /*06000121*/ public hidebysig specialname 
          instance class System.Runtime.Serialization.DataContractResolver/*02000013*/ 
          get_DataContractResolver() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_DataContractResolver

  .method /*06000122*/ public hidebysig specialname 
          instance void  set_DataContractResolver(class System.Runtime.Serialization.DataContractResolver/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x239d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_DataContractResolver

  .method /*06000123*/ public hidebysig specialname 
          instance bool  get_IgnoreExtensionDataObject() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x239f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_IgnoreExtensionDataObject

  .method /*06000124*/ public hidebysig specialname 
          instance void  set_IgnoreExtensionDataObject(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_IgnoreExtensionDataObject

  .method /*06000125*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> 
          get_KnownTypes() cil managed
  // SIG: 20 00 15 12 80 B1 01 12 7D
  {
    .param [0]/*0800028A*/ 
    .custom /*0C000178:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_KnownTypes

  .method /*06000126*/ public hidebysig specialname 
          instance void  set_KnownTypes(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 B1 01 12 7D
  {
    .param [1]/*0800028B*/ 
    .custom /*0C000179:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23a7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_KnownTypes

  .method /*06000127*/ public hidebysig specialname 
          instance int32  get_MaxItemsInObjectGraph() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_MaxItemsInObjectGraph

  .method /*06000128*/ public hidebysig specialname 
          instance void  set_MaxItemsInObjectGraph(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x23ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_MaxItemsInObjectGraph

  .method /*06000129*/ public hidebysig specialname 
          instance bool  get_PreserveObjectReferences() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_PreserveObjectReferences

  .method /*0600012A*/ public hidebysig specialname 
          instance void  set_PreserveObjectReferences(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23b1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_PreserveObjectReferences

  .method /*0600012B*/ public hidebysig specialname 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          get_RootName() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x23b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_RootName

  .method /*0600012C*/ public hidebysig specialname 
          instance void  set_RootName(class System.Xml.XmlDictionaryString/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x23b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_RootName

  .method /*0600012D*/ public hidebysig specialname 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          get_RootNamespace() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_RootNamespace

  .method /*0600012E*/ public hidebysig specialname 
          instance void  set_RootNamespace(class System.Xml.XmlDictionaryString/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x23bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_RootNamespace

  .method /*0600012F*/ public hidebysig specialname 
          instance bool  get_SerializeReadOnlyTypes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSerializerSettings::get_SerializeReadOnlyTypes

  .method /*06000130*/ public hidebysig specialname 
          instance void  set_SerializeReadOnlyTypes(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSerializerSettings::set_SerializeReadOnlyTypes

  .property /*17000019*/ instance callconv(8) class System.Runtime.Serialization.DataContractResolver/*02000013*/
          DataContractResolver()
  {
    .get instance class System.Runtime.Serialization.DataContractResolver/*02000013*/ System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_DataContractResolver() /* 06000121 */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_DataContractResolver(class System.Runtime.Serialization.DataContractResolver/*02000013*/) /* 06000122 */
  } // end of property DataContractSerializerSettings::DataContractResolver
  .property /*1700001A*/ instance callconv(8) bool
          IgnoreExtensionDataObject()
  {
    .get instance bool System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_IgnoreExtensionDataObject() /* 06000123 */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_IgnoreExtensionDataObject(bool) /* 06000124 */
  } // end of property DataContractSerializerSettings::IgnoreExtensionDataObject
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/>
          KnownTypes()
  {
    .custom /*0C00004F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_KnownTypes() /* 06000125 */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_KnownTypes(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/>) /* 06000126 */
  } // end of property DataContractSerializerSettings::KnownTypes
  .property /*1700001C*/ instance callconv(8) int32
          MaxItemsInObjectGraph()
  {
    .get instance int32 System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_MaxItemsInObjectGraph() /* 06000127 */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_MaxItemsInObjectGraph(int32) /* 06000128 */
  } // end of property DataContractSerializerSettings::MaxItemsInObjectGraph
  .property /*1700001D*/ instance callconv(8) bool
          PreserveObjectReferences()
  {
    .get instance bool System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_PreserveObjectReferences() /* 06000129 */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_PreserveObjectReferences(bool) /* 0600012A */
  } // end of property DataContractSerializerSettings::PreserveObjectReferences
  .property /*1700001E*/ instance callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          RootName()
  {
    .get instance class System.Xml.XmlDictionaryString/*02000011*/ System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_RootName() /* 0600012B */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_RootName(class System.Xml.XmlDictionaryString/*02000011*/) /* 0600012C */
  } // end of property DataContractSerializerSettings::RootName
  .property /*1700001F*/ instance callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          RootNamespace()
  {
    .get instance class System.Xml.XmlDictionaryString/*02000011*/ System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_RootNamespace() /* 0600012D */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_RootNamespace(class System.Xml.XmlDictionaryString/*02000011*/) /* 0600012E */
  } // end of property DataContractSerializerSettings::RootNamespace
  .property /*17000020*/ instance callconv(8) bool
          SerializeReadOnlyTypes()
  {
    .get instance bool System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::get_SerializeReadOnlyTypes() /* 0600012F */
    .set instance void System.Runtime.Serialization.DataContractSerializerSettings/*02000016*/::set_SerializeReadOnlyTypes(bool) /* 06000130 */
  } // end of property DataContractSerializerSettings::SerializeReadOnlyTypes
} // end of class System.Runtime.Serialization.DataContractSerializerSettings

.class /*02000017*/ public auto ansi beforefieldinit System.Runtime.Serialization.ExportOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23c2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExportOptions::.ctor

  .method /*06000132*/ public hidebysig specialname 
          instance class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/ 
          get_DataContractSurrogate() cil managed
  // SIG: 20 00 12 80 B9
  {
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExportOptions::get_DataContractSurrogate

  .method /*06000133*/ public hidebysig specialname 
          instance void  set_DataContractSurrogate(class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B9
  {
    // Method begins at RVA 0x23cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExportOptions::set_DataContractSurrogate

  .method /*06000134*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100002F*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> 
          get_KnownTypes() cil managed
  // SIG: 20 00 15 12 80 BD 01 12 7D
  {
    .custom /*0C0000C7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExportOptions::get_KnownTypes

  .property /*17000021*/ instance callconv(8) class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/
          DataContractSurrogate()
  {
    .get instance class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/ System.Runtime.Serialization.ExportOptions/*02000017*/::get_DataContractSurrogate() /* 06000132 */
    .set instance void System.Runtime.Serialization.ExportOptions/*02000017*/::set_DataContractSurrogate(class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/) /* 06000133 */
  } // end of property ExportOptions::DataContractSurrogate
  .property /*17000022*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100002F*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/>
          KnownTypes()
  {
    .custom /*0C000063:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100002F*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> System.Runtime.Serialization.ExportOptions/*02000017*/::get_KnownTypes() /* 06000134 */
  } // end of property ExportOptions::KnownTypes
} // end of class System.Runtime.Serialization.ExportOptions

.class /*02000018*/ public auto ansi sealed beforefieldinit System.Runtime.Serialization.ExtensionDataObject
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*06000135*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtensionDataObject::.ctor

} // end of class System.Runtime.Serialization.ExtensionDataObject

.class /*02000019*/ interface public abstract auto ansi beforefieldinit System.Runtime.Serialization.IExtensibleDataObject
{
  .custom /*0C000048:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .method /*06000136*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Runtime.Serialization.ExtensionDataObject/*02000018*/ 
          get_ExtensionData() cil managed
  // SIG: 20 00 12 60
  {
    // Method begins at RVA 0x0
  } // end of method IExtensibleDataObject::get_ExtensionData

  .method /*06000137*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ExtensionData(class System.Runtime.Serialization.ExtensionDataObject/*02000018*/ 'value') cil managed
  // SIG: 20 01 01 12 60
  {
    // Method begins at RVA 0x0
  } // end of method IExtensibleDataObject::set_ExtensionData

  .property /*17000023*/ instance callconv(8) class System.Runtime.Serialization.ExtensionDataObject/*02000018*/
          ExtensionData()
  {
    .get instance class System.Runtime.Serialization.ExtensionDataObject/*02000018*/ System.Runtime.Serialization.IExtensibleDataObject/*02000019*/::get_ExtensionData() /* 06000136 */
    .set instance void System.Runtime.Serialization.IExtensibleDataObject/*02000019*/::set_ExtensionData(class System.Runtime.Serialization.ExtensionDataObject/*02000018*/) /* 06000137 */
  } // end of property IExtensibleDataObject::ExtensionData
} // end of class System.Runtime.Serialization.IExtensibleDataObject

.class /*0200001A*/ public abstract auto ansi beforefieldinit System.Runtime.Serialization.XmlObjectSerializer
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000049:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000138*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23da
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::.ctor

  .method /*06000139*/ public hidebysig newslot abstract virtual 
          instance bool  IsStartObject(class System.Xml.XmlDictionaryReader/*0200000E*/ reader) cil managed
  // SIG: 20 01 02 12 38
  {
    .custom /*0C0000C9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000CA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x0
  } // end of method XmlObjectSerializer::IsStartObject

  .method /*0600013A*/ public hidebysig newslot virtual 
          instance bool  IsStartObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader) cil managed
  // SIG: 20 01 02 12 79
  {
    .custom /*0C0000CB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000CC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlObjectSerializer::IsStartObject

  .method /*0600013B*/ public hidebysig newslot virtual 
          instance object  ReadObject(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 01 1C 12 55
  {
    .custom /*0C0000CD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000CE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000295*/ 
    .custom /*0C00017A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlObjectSerializer::ReadObject

  .method /*0600013C*/ public hidebysig newslot virtual 
          instance object  ReadObject(class System.Xml.XmlDictionaryReader/*0200000E*/ reader) cil managed
  // SIG: 20 01 1C 12 38
  {
    .custom /*0C0000CF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000D0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000297*/ 
    .custom /*0C00017B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlObjectSerializer::ReadObject

  .method /*0600013D*/ public hidebysig newslot abstract virtual 
          instance object  ReadObject(class System.Xml.XmlDictionaryReader/*0200000E*/ reader,
                                      bool verifyObjectName) cil managed
  // SIG: 20 02 1C 12 38 02
  {
    .custom /*0C0000D1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000D2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000299*/ 
    .custom /*0C00017C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method XmlObjectSerializer::ReadObject

  .method /*0600013E*/ public hidebysig newslot virtual 
          instance object  ReadObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader) cil managed
  // SIG: 20 01 1C 12 79
  {
    .custom /*0C0000D3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000D4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*0800029C*/ 
    .custom /*0C00017D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlObjectSerializer::ReadObject

  .method /*0600013F*/ public hidebysig newslot virtual 
          instance object  ReadObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ reader,
                                      bool verifyObjectName) cil managed
  // SIG: 20 02 1C 12 79 02
  {
    .custom /*0C0000D5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000D6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*0800029E*/ 
    .custom /*0C00017E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlObjectSerializer::ReadObject

  .method /*06000140*/ public hidebysig newslot abstract virtual 
          instance void  WriteEndObject(class System.Xml.XmlDictionaryWriter/*02000012*/ writer) cil managed
  // SIG: 20 01 01 12 48
  {
    .custom /*0C0000D7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000D8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x0
  } // end of method XmlObjectSerializer::WriteEndObject

  .method /*06000141*/ public hidebysig newslot virtual 
          instance void  WriteEndObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C0000D9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000DA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x23f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::WriteEndObject

  .method /*06000142*/ public hidebysig newslot virtual 
          instance void  WriteObject(class [System.Runtime/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                                     object graph) cil managed
  // SIG: 20 02 01 12 55 1C
  {
    .custom /*0C0000DB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000DC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002A4*/ 
    .custom /*0C00017F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::WriteObject

  .method /*06000143*/ public hidebysig newslot virtual 
          instance void  WriteObject(class System.Xml.XmlDictionaryWriter/*02000012*/ writer,
                                     object graph) cil managed
  // SIG: 20 02 01 12 48 1C
  {
    .custom /*0C0000DD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000DE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002A6*/ 
    .custom /*0C000180:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::WriteObject

  .method /*06000144*/ public hidebysig newslot virtual 
          instance void  WriteObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer,
                                     object graph) cil managed
  // SIG: 20 02 01 12 80 A1 1C
  {
    .custom /*0C0000DF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000E0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002A8*/ 
    .custom /*0C000181:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::WriteObject

  .method /*06000145*/ public hidebysig newslot abstract virtual 
          instance void  WriteObjectContent(class System.Xml.XmlDictionaryWriter/*02000012*/ writer,
                                            object graph) cil managed
  // SIG: 20 02 01 12 48 1C
  {
    .custom /*0C0000E1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000E2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002AA*/ 
    .custom /*0C000182:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method XmlObjectSerializer::WriteObjectContent

  .method /*06000146*/ public hidebysig newslot virtual 
          instance void  WriteObjectContent(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer,
                                            object graph) cil managed
  // SIG: 20 02 01 12 80 A1 1C
  {
    .custom /*0C0000E3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000E4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002AC*/ 
    .custom /*0C000183:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::WriteObjectContent

  .method /*06000147*/ public hidebysig newslot abstract virtual 
          instance void  WriteStartObject(class System.Xml.XmlDictionaryWriter/*02000012*/ writer,
                                          object graph) cil managed
  // SIG: 20 02 01 12 48 1C
  {
    .custom /*0C0000E5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000E6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002AE*/ 
    .custom /*0C000184:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method XmlObjectSerializer::WriteStartObject

  .method /*06000148*/ public hidebysig newslot virtual 
          instance void  WriteStartObject(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ writer,
                                          object graph) cil managed
  // SIG: 20 02 01 12 80 A1 1C
  {
    .custom /*0C0000E7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000E8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002B0*/ 
    .custom /*0C000185:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlObjectSerializer::WriteStartObject

} // end of class System.Runtime.Serialization.XmlObjectSerializer

.class /*0200001B*/ public abstract auto ansi sealed beforefieldinit System.Runtime.Serialization.XmlSerializableServices
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000149*/ public hidebysig static 
          void  AddDefaultSchema(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/ schemas,
                                 class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ typeQName) cil managed
  // SIG: 00 02 01 12 80 C1 12 80 C5
  {
    // Method begins at RVA 0x23fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializableServices::AddDefaultSchema

  .method /*0600014A*/ public hidebysig static 
          class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000032*/[] 
          ReadNodes(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100001E*/ xmlReader) cil managed
  // SIG: 00 01 1D 12 80 C9 12 79
  {
    // Method begins at RVA 0x23ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializableServices::ReadNodes

  .method /*0600014B*/ public hidebysig static 
          void  WriteNodes(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000028*/ xmlWriter,
                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000032*/[] nodes) cil managed
  // SIG: 00 02 01 12 80 A1 1D 12 80 C9
  {
    .param [2]/*080002B5*/ 
    .custom /*0C000186:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2402
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializableServices::WriteNodes

} // end of class System.Runtime.Serialization.XmlSerializableServices

.class /*0200001C*/ public abstract auto ansi sealed beforefieldinit System.Runtime.Serialization.XPathQueryGenerator
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000051:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600014C*/ public hidebysig static 
          string  CreateFromDataContractSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                                                   class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000033*/[] pathToMember,
                                                   class [System.Runtime/*23000001*/]System.Text.StringBuilder/*01000034*/ rootElementXpath,
                                                   [out] class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNamespaceManager/*01000035*/& namespaces) cil managed
  // SIG: 00 04 0E 12 7D 1D 12 80 CD 12 80 D1 10 12 80 D5
  {
    .custom /*0C0000E9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000EA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [3]/*080002B8*/ 
    .custom /*0C000187:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2404
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XPathQueryGenerator::CreateFromDataContractSerializer

  .method /*0600014D*/ public hidebysig static 
          string  CreateFromDataContractSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                                                   class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000033*/[] pathToMember,
                                                   [out] class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNamespaceManager/*01000035*/& namespaces) cil managed
  // SIG: 00 03 0E 12 7D 1D 12 80 CD 10 12 80 D5
  {
    .custom /*0C0000EB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000EC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2407
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XPathQueryGenerator::CreateFromDataContractSerializer

} // end of class System.Runtime.Serialization.XPathQueryGenerator

.class /*0200001D*/ public auto ansi beforefieldinit System.Runtime.Serialization.XsdDataContractExporter
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000055:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000056:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600014E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x240a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XsdDataContractExporter::.ctor

  .method /*0600014F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/ schemas) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    .custom /*0C0000ED:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2412
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XsdDataContractExporter::.ctor

  .method /*06000150*/ public hidebysig specialname 
          instance class System.Runtime.Serialization.ExportOptions/*02000017*/ 
          get_Options() cil managed
  // SIG: 20 00 12 5C
  {
    .custom /*0C0000EE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x241a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::get_Options

  .method /*06000151*/ public hidebysig specialname 
          instance void  set_Options(class System.Runtime.Serialization.ExportOptions/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 12 5C
  {
    .custom /*0C0000EF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x241d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XsdDataContractExporter::set_Options

  .method /*06000152*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/ 
          get_Schemas() cil managed
  // SIG: 20 00 12 80 C1
  {
    // Method begins at RVA 0x241f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::get_Schemas

  .method /*06000153*/ public hidebysig instance bool 
          CanExport(class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000036*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000037*/> assemblies) cil managed
  // SIG: 20 01 02 15 12 80 D9 01 12 80 DD
  {
    .custom /*0C0000F0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000F1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2422
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::CanExport

  .method /*06000154*/ public hidebysig instance bool 
          CanExport(class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000036*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> types) cil managed
  // SIG: 20 01 02 15 12 80 D9 01 12 7D
  {
    .custom /*0C0000F2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000F3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2425
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::CanExport

  .method /*06000155*/ public hidebysig instance bool 
          CanExport(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 02 12 7D
  {
    .custom /*0C0000F4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000F5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2428
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::CanExport

  .method /*06000156*/ public hidebysig instance void 
          Export(class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000036*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000037*/> assemblies) cil managed
  // SIG: 20 01 01 15 12 80 D9 01 12 80 DD
  {
    .custom /*0C0000F6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000F7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x242b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XsdDataContractExporter::Export

  .method /*06000157*/ public hidebysig instance void 
          Export(class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000036*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> types) cil managed
  // SIG: 20 01 01 15 12 80 D9 01 12 7D
  {
    .custom /*0C0000F8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000F9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x242d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XsdDataContractExporter::Export

  .method /*06000158*/ public hidebysig instance void 
          Export(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 01 12 7D
  {
    .custom /*0C0000FA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000FB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x242f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XsdDataContractExporter::Export

  .method /*06000159*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ 
          GetRootElementName(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 12 80 C5 12 7D
  {
    .custom /*0C0000FC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000FD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002C5*/ 
    .custom /*0C000188:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2431
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::GetRootElementName

  .method /*0600015A*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaType/*01000038*/ 
          GetSchemaType(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 12 80 E1 12 7D
  {
    .custom /*0C0000FE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C0000FF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002C7*/ 
    .custom /*0C000189:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2434
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::GetSchemaType

  .method /*0600015B*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ 
          GetSchemaTypeName(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 12 80 C5 12 7D
  {
    .custom /*0C000100:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000101:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2437
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XsdDataContractExporter::GetSchemaTypeName

  .property /*17000024*/ instance callconv(8) class System.Runtime.Serialization.ExportOptions/*02000017*/
          Options()
  {
    .custom /*0C000065:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Runtime.Serialization.ExportOptions/*02000017*/ System.Runtime.Serialization.XsdDataContractExporter/*0200001D*/::get_Options() /* 06000150 */
    .set instance void System.Runtime.Serialization.XsdDataContractExporter/*0200001D*/::set_Options(class System.Runtime.Serialization.ExportOptions/*02000017*/) /* 06000151 */
  } // end of property XsdDataContractExporter::Options
  .property /*17000025*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/
          Schemas()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/ System.Runtime.Serialization.XsdDataContractExporter/*0200001D*/::get_Schemas() /* 06000152 */
  } // end of property XsdDataContractExporter::Schemas
} // end of class System.Runtime.Serialization.XsdDataContractExporter

.class /*0200001E*/ public abstract auto ansi beforefieldinit System.Runtime.Serialization.DataContracts.DataContract
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000058:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .field /*04000007*/ static assembly literal valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/ DataContractPreserveMemberTypes = int32(0x0000023F)
  .method /*0600015C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x243a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContract::.ctor

  .method /*0600015D*/ public hidebysig newslot specialname virtual 
          instance class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ 
          get_BaseContract() cil managed
  // SIG: 20 00 12 78
  {
    .custom /*0C000102:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000103:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000104:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2442
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_BaseContract

  .method /*0600015E*/ public hidebysig newslot specialname virtual 
          instance string  get_ContractType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000105:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2445
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_ContractType

  .method /*0600015F*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002D*/<class System.Runtime.Serialization.DataContracts.DataMember/*02000020*/> 
          get_DataMembers() cil managed
  // SIG: 20 00 15 12 80 B5 01 12 80 80
  {
    // Method begins at RVA 0x2448
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_DataMembers

  .method /*06000160*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsBuiltInDataContract() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_IsBuiltInDataContract

  .method /*06000161*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsISerializable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_IsISerializable

  .method /*06000162*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsReference() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2451
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_IsReference

  .method /*06000163*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsValueType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2454
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_IsValueType

  .method /*06000164*/ public hidebysig newslot specialname virtual 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> 
          get_KnownDataContracts() cil managed
  // SIG: 20 00 15 12 80 E9 02 12 80 C5 12 78
  {
    .custom /*0C000106:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000107:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002CA*/ 
    .custom /*0C00018A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2457
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_KnownDataContracts

  .method /*06000165*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 
          get_OriginalUnderlyingType() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x245a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_OriginalUnderlyingType

  .method /*06000166*/ public hidebysig newslot specialname virtual 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          get_TopLevelElementName() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C000108:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x245d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_TopLevelElementName

  .method /*06000167*/ public hidebysig newslot specialname virtual 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          get_TopLevelElementNamespace() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C000109:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2460
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_TopLevelElementNamespace

  .method /*06000168*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 
          get_UnderlyingType() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x2463
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_UnderlyingType

  .method /*06000169*/ public hidebysig newslot specialname virtual 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ 
          get_XmlName() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x2466
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::get_XmlName

  .method /*0600016A*/ public hidebysig virtual final 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00010A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2469
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::Equals

  .method /*0600016B*/ public hidebysig newslot virtual 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ 
          GetArrayTypeName(bool isNullable) cil managed
  // SIG: 20 01 12 80 C5 02
  {
    .custom /*0C00010B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00010C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x246c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::GetArrayTypeName

  .method /*0600016C*/ public hidebysig static 
          class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ 
          GetBuiltInDataContract(string name,
                                 string ns) cil managed
  // SIG: 00 02 12 78 0E 0E
  {
    .custom /*0C00010D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00010E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002CD*/ 
    .custom /*0C00018B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x246f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::GetBuiltInDataContract

  .method /*0600016D*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::GetHashCode

  .method /*0600016E*/ public hidebysig static 
          class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ 
          GetXmlName(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 00 01 12 80 C5 12 7D
  {
    .custom /*0C00010F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000110:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::GetXmlName

  .method /*0600016F*/ public hidebysig newslot virtual 
          instance bool  IsDictionaryLike([out] string& keyName,
                                          [out] string& valueName,
                                          [out] string& itemName) cil managed
  // SIG: 20 03 02 10 0E 10 0E 10 0E
  {
    .custom /*0C000111:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002D1*/ 
    .custom /*0C00018C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080002D2*/ 
    .custom /*0C00018D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080002D3*/ 
    .custom /*0C00018E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000017*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2478
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContract::IsDictionaryLike

  .property /*17000026*/ instance callconv(8) class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/
          BaseContract()
  {
    .custom /*0C000067:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_BaseContract() /* 0600015D */
  } // end of property DataContract::BaseContract
  .property /*17000027*/ instance callconv(8) string
          ContractType()
  {
    .custom /*0C000069:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_ContractType() /* 0600015E */
  } // end of property DataContract::ContractType
  .property /*17000028*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002D*/<class System.Runtime.Serialization.DataContracts.DataMember/*02000020*/>
          DataMembers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002D*/<class System.Runtime.Serialization.DataContracts.DataMember/*02000020*/> System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_DataMembers() /* 0600015F */
  } // end of property DataContract::DataMembers
  .property /*17000029*/ instance callconv(8) bool
          IsBuiltInDataContract()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_IsBuiltInDataContract() /* 06000160 */
  } // end of property DataContract::IsBuiltInDataContract
  .property /*1700002A*/ instance callconv(8) bool
          IsISerializable()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_IsISerializable() /* 06000161 */
  } // end of property DataContract::IsISerializable
  .property /*1700002B*/ instance callconv(8) bool
          IsReference()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_IsReference() /* 06000162 */
  } // end of property DataContract::IsReference
  .property /*1700002C*/ instance callconv(8) bool
          IsValueType()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_IsValueType() /* 06000163 */
  } // end of property DataContract::IsValueType
  .property /*1700002D*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/>
          KnownDataContracts()
  {
    .custom /*0C00006C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_KnownDataContracts() /* 06000164 */
  } // end of property DataContract::KnownDataContracts
  .property /*1700002E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001F*/
          OriginalUnderlyingType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001F*/ System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_OriginalUnderlyingType() /* 06000165 */
  } // end of property DataContract::OriginalUnderlyingType
  .property /*1700002F*/ instance callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          TopLevelElementName()
  {
    .custom /*0C00006D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.XmlDictionaryString/*02000011*/ System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_TopLevelElementName() /* 06000166 */
  } // end of property DataContract::TopLevelElementName
  .property /*17000030*/ instance callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          TopLevelElementNamespace()
  {
    .custom /*0C00006E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.XmlDictionaryString/*02000011*/ System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_TopLevelElementNamespace() /* 06000167 */
  } // end of property DataContract::TopLevelElementNamespace
  .property /*17000031*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001F*/
          UnderlyingType()
  {
    .custom /*0C000070:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001B */ = ( 01 00 3F 02 00 00 00 00 )                         // ..?.....
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001F*/ System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_UnderlyingType() /* 06000168 */
  } // end of property DataContract::UnderlyingType
  .property /*17000032*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/
          XmlName()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::get_XmlName() /* 06000169 */
  } // end of property DataContract::XmlName
} // end of class System.Runtime.Serialization.DataContracts.DataContract

.class /*0200001F*/ public auto ansi sealed beforefieldinit System.Runtime.Serialization.DataContracts.DataContractSet
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00005B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000170*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Runtime.Serialization.DataContracts.DataContractSet/*0200001F*/ dataContractSet) cil managed
  // SIG: 20 01 01 12 7C
  {
    .custom /*0C000112:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000113:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x247b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSet::.ctor

  .method /*06000171*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*0100002E*/ dataContractSurrogate,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> referencedTypes,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Runtime/*23000001*/]System.Type/*0100001F*/> referencedCollectionTypes) cil managed
  // SIG: 20 03 01 12 80 B9 15 12 80 B1 01 12 7D 15 12 80 B1 01 12 7D
  {
    .custom /*0C000114:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080002D6*/ 
    .custom /*0C00018F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080002D7*/ 
    .custom /*0C000190:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2483
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractSet::.ctor

  .method /*06000172*/ public hidebysig specialname 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> 
          get_Contracts() cil managed
  // SIG: 20 00 15 12 80 E9 02 12 80 C5 12 78
  {
    // Method begins at RVA 0x248b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::get_Contracts

  .method /*06000173*/ public hidebysig specialname 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> 
          get_KnownTypesForObject() cil managed
  // SIG: 20 00 15 12 80 E9 02 12 80 C5 12 78
  {
    .param [0]/*080002D8*/ 
    .custom /*0C000191:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x248e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::get_KnownTypesForObject

  .method /*06000174*/ public hidebysig specialname 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/,object> 
          get_ProcessedContracts() cil managed
  // SIG: 20 00 15 12 80 E9 02 12 78 1C
  {
    // Method begins at RVA 0x2491
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::get_ProcessedContracts

  .method /*06000175*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*0100003C*/ 
          get_SurrogateData() cil managed
  // SIG: 20 00 12 80 F1
  {
    // Method begins at RVA 0x2494
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::get_SurrogateData

  .method /*06000176*/ public hidebysig instance class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ 
          GetDataContract(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type') cil managed
  // SIG: 20 01 12 78 12 7D
  {
    .custom /*0C000117:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000118:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x2497
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::GetDataContract

  .method /*06000177*/ public hidebysig instance class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ 
          GetDataContract(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ key) cil managed
  // SIG: 20 01 12 78 12 80 C5
  {
    .custom /*0C00011A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00011B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002DA*/ 
    .custom /*0C000192:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x249a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::GetDataContract

  .method /*06000178*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 
          GetReferencedType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/ xmlName,
                            class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ dataContract,
                            [out] class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/& referencedContract,
                            [out] object[]& genericParameters,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003D*/<bool> supportGenericTypes) cil managed
  // SIG: 20 05 12 7D 12 80 C5 12 78 10 12 78 10 1D 1C 15 11 80 F5 01 02
  {
    .custom /*0C00011D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00011E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002DC*/ 
    .custom /*0C000193:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080002DF*/ 
    .custom /*0C000194:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080002E0*/ 
    .custom /*0C000195:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*080002E1*/  = nullref
    // Method begins at RVA 0x249d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::GetReferencedType

  .method /*06000179*/ public hidebysig instance class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*0100003E*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/> 
          ImportSchemaSet(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/ schemaSet,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaElement/*0100003F*/> elements,
                          bool importXmlDataType) cil managed
  // SIG: 20 03 15 12 80 F9 01 12 80 C5 12 80 C1 15 12 80 B1 01 12 80 FD 02
  {
    .custom /*0C00011F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000120:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x24a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractSet::ImportSchemaSet

  .method /*0600017A*/ public hidebysig instance void 
          ImportSchemaSet(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000030*/ schemaSet,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002C*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/> typeNames,
                          bool importXmlDataType) cil managed
  // SIG: 20 03 01 12 80 C1 15 12 80 B1 01 12 80 C5 02
  {
    .custom /*0C000121:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000122:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*080002E6*/ 
    .custom /*0C000196:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24a3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractSet::ImportSchemaSet

  .property /*17000033*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/>
          Contracts()
  {
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> System.Runtime.Serialization.DataContracts.DataContractSet/*0200001F*/::get_Contracts() /* 06000172 */
  } // end of property DataContractSet::Contracts
  .property /*17000034*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/>
          KnownTypesForObject()
  {
    .custom /*0C000071:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> System.Runtime.Serialization.DataContracts.DataContractSet/*0200001F*/::get_KnownTypesForObject() /* 06000173 */
  } // end of property DataContractSet::KnownTypesForObject
  .property /*17000035*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/,object>
          ProcessedContracts()
  {
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/,object> System.Runtime.Serialization.DataContracts.DataContractSet/*0200001F*/::get_ProcessedContracts() /* 06000174 */
  } // end of property DataContractSet::ProcessedContracts
  .property /*17000036*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Hashtable/*0100003C*/
          SurrogateData()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*0100003C*/ System.Runtime.Serialization.DataContracts.DataContractSet/*0200001F*/::get_SurrogateData() /* 06000175 */
  } // end of property DataContractSet::SurrogateData
} // end of class System.Runtime.Serialization.DataContracts.DataContractSet

.class /*02000020*/ public auto ansi sealed beforefieldinit System.Runtime.Serialization.DataContracts.DataMember
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00005D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600017B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataMember::.ctor

  .method /*0600017C*/ public hidebysig specialname 
          instance bool  get_EmitDefaultValue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataMember::get_EmitDefaultValue

  .method /*0600017D*/ public hidebysig specialname 
          instance bool  get_IsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataMember::get_IsNullable

  .method /*0600017E*/ public hidebysig specialname 
          instance bool  get_IsRequired() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataMember::get_IsRequired

  .method /*0600017F*/ public hidebysig specialname 
          instance class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ 
          get_MemberTypeContract() cil managed
  // SIG: 20 00 12 78
  {
    .custom /*0C000123:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000124:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x24b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataMember::get_MemberTypeContract

  .method /*06000180*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataMember::get_Name

  .method /*06000181*/ public hidebysig specialname 
          instance int64  get_Order() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x24bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataMember::get_Order

  .property /*17000037*/ instance callconv(8) bool
          EmitDefaultValue()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataMember/*02000020*/::get_EmitDefaultValue() /* 0600017C */
  } // end of property DataMember::EmitDefaultValue
  .property /*17000038*/ instance callconv(8) bool
          IsNullable()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataMember/*02000020*/::get_IsNullable() /* 0600017D */
  } // end of property DataMember::IsNullable
  .property /*17000039*/ instance callconv(8) bool
          IsRequired()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.DataMember/*02000020*/::get_IsRequired() /* 0600017E */
  } // end of property DataMember::IsRequired
  .property /*1700003A*/ instance callconv(8) class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/
          MemberTypeContract()
  {
    .get instance class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/ System.Runtime.Serialization.DataContracts.DataMember/*02000020*/::get_MemberTypeContract() /* 0600017F */
  } // end of property DataMember::MemberTypeContract
  .property /*1700003B*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Runtime.Serialization.DataContracts.DataMember/*02000020*/::get_Name() /* 06000180 */
  } // end of property DataMember::Name
  .property /*1700003C*/ instance callconv(8) int64
          Order()
  {
    .get instance int64 System.Runtime.Serialization.DataContracts.DataMember/*02000020*/::get_Order() /* 06000181 */
  } // end of property DataMember::Order
} // end of class System.Runtime.Serialization.DataContracts.DataMember

.class /*02000021*/ public auto ansi sealed beforefieldinit System.Runtime.Serialization.DataContracts.XmlDataContract
       extends System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/
{
  .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000061:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000182*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24bf
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00015C       */ call       instance void System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/::.ctor() /* 0600015C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlDataContract::.ctor

  .method /*06000183*/ public hidebysig specialname virtual 
          instance string  get_ContractType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_ContractType

  .method /*06000184*/ public hidebysig specialname 
          instance bool  get_HasRoot() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_HasRoot

  .method /*06000185*/ public hidebysig specialname 
          instance bool  get_IsAnonymous() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_IsAnonymous

  .method /*06000186*/ public hidebysig specialname virtual 
          instance bool  get_IsBuiltInDataContract() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_IsBuiltInDataContract

  .method /*06000187*/ public hidebysig specialname 
          instance bool  get_IsTopLevelElementNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_IsTopLevelElementNullable

  .method /*06000188*/ public hidebysig specialname 
          instance bool  get_IsTypeDefinedOnImport() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_IsTypeDefinedOnImport

  .method /*06000189*/ public hidebysig specialname 
          instance void  set_IsTypeDefinedOnImport(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDataContract::set_IsTypeDefinedOnImport

  .method /*0600018A*/ public hidebysig specialname 
          instance bool  get_IsValueType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_IsValueType

  .method /*0600018B*/ public hidebysig specialname 
          instance void  set_IsValueType(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDataContract::set_IsValueType

  .method /*0600018C*/ public hidebysig specialname virtual 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> 
          get_KnownDataContracts() cil managed
  // SIG: 20 00 15 12 80 E9 02 12 80 C5 12 78
  {
    .custom /*0C00012A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100002A*/::.ctor(string) /* 0A000019 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00012B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002B*/::.ctor(string) /* 0A00001A */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*080002EA*/ 
    .custom /*0C000197:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x24e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_KnownDataContracts

  .method /*0600018D*/ public hidebysig specialname virtual 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          get_TopLevelElementName() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x24e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_TopLevelElementName

  .method /*0600018E*/ public hidebysig specialname virtual 
          instance class System.Xml.XmlDictionaryString/*02000011*/ 
          get_TopLevelElementNamespace() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x24e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_TopLevelElementNamespace

  .method /*0600018F*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaType/*01000038*/ 
          get_XsdType() cil managed
  // SIG: 20 00 12 80 E1
  {
    // Method begins at RVA 0x24e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDataContract::get_XsdType

  .property /*1700003D*/ instance callconv(8) string
          ContractType()
  {
    .get instance string System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_ContractType() /* 06000183 */
  } // end of property XmlDataContract::ContractType
  .property /*1700003E*/ instance callconv(8) bool
          HasRoot()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_HasRoot() /* 06000184 */
  } // end of property XmlDataContract::HasRoot
  .property /*1700003F*/ instance callconv(8) bool
          IsAnonymous()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_IsAnonymous() /* 06000185 */
  } // end of property XmlDataContract::IsAnonymous
  .property /*17000040*/ instance callconv(8) bool
          IsBuiltInDataContract()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_IsBuiltInDataContract() /* 06000186 */
  } // end of property XmlDataContract::IsBuiltInDataContract
  .property /*17000041*/ instance callconv(8) bool
          IsTopLevelElementNullable()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_IsTopLevelElementNullable() /* 06000187 */
  } // end of property XmlDataContract::IsTopLevelElementNullable
  .property /*17000042*/ instance callconv(8) bool
          IsTypeDefinedOnImport()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_IsTypeDefinedOnImport() /* 06000188 */
    .set instance void System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::set_IsTypeDefinedOnImport(bool) /* 06000189 */
  } // end of property XmlDataContract::IsTypeDefinedOnImport
  .property /*17000043*/ instance callconv(8) bool
          IsValueType()
  {
    .get instance bool System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_IsValueType() /* 0600018A */
    .set instance void System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::set_IsValueType(bool) /* 0600018B */
  } // end of property XmlDataContract::IsValueType
  .property /*17000044*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/>
          KnownDataContracts()
  {
    .custom /*0C000073:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003A*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000031*/,class System.Runtime.Serialization.DataContracts.DataContract/*0200001E*/> System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_KnownDataContracts() /* 0600018C */
  } // end of property XmlDataContract::KnownDataContracts
  .property /*17000045*/ instance callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          TopLevelElementName()
  {
    .get instance class System.Xml.XmlDictionaryString/*02000011*/ System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_TopLevelElementName() /* 0600018D */
  } // end of property XmlDataContract::TopLevelElementName
  .property /*17000046*/ instance callconv(8) class System.Xml.XmlDictionaryString/*02000011*/
          TopLevelElementNamespace()
  {
    .get instance class System.Xml.XmlDictionaryString/*02000011*/ System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_TopLevelElementNamespace() /* 0600018E */
  } // end of property XmlDataContract::TopLevelElementNamespace
  .property /*17000047*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaType/*01000038*/
          XsdType()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaType/*01000038*/ System.Runtime.Serialization.DataContracts.XmlDataContract/*02000021*/::get_XsdType() /* 0600018F */
  } // end of property XmlDataContract::XsdType
} // end of class System.Runtime.Serialization.DataContracts.XmlDataContract


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
