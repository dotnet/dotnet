
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
// Time-date stamp:            0xfeffeb77
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00009200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000b1fa
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
// Checksum:                       0x00016776
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000b1a5 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x000004a4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00009c00 [0x00002388] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000b0b8 [0x00000054] address [size] of Debug Directory:           
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
//              0x00009200 Virtual Size
//              0x00002000 Virtual Address
//              0x00009200 Size of Raw Data
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
//              0x000004a4 Virtual Size
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00009400 Pointer to Raw Data
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
//              0x00009a00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000001fc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000b1e7 Import Name Table
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
// 0x0000284c [0x000087ec] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000b038 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000053f0 Size
//              '#~' Name
//    Stream 2:
//              0x0000545c Offset
//              0x000024a4 Size
//              '#Strings' Name
//    Stream 3:
//              0x00007900 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00007904 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00007914 Offset
//              0x00000ed8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000002890bb41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 49032
// PE header size       : 512 (496 used)    ( 1.04%)
// PE additional info   : 10539             (21.49%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.15%)
// CLR meta-data size  : 34796              (70.97%)
// CLR additional info : 128                ( 0.26%)
// CLR method headers  : 596                ( 1.22%)
// Managed code         : 1447              ( 2.95%)
// Data                 : 2048              ( 4.18%)
// Unaccounted          : -1106             (-2.26%)

// Num.of PE sections   : 3
//   .text    - 37376
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 34796
//   Module        -    1 (10 bytes)
//   TypeDef       -   65 (910 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   76 (456 bytes)
//   MethodDef     -  635 (8890 bytes)    7 abstract, 0 native, 596 bodies
//   FieldDef      -   16 (96 bytes)      0 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -  817 (4902 bytes)
//   MethodImpl    -    3 (18 bytes)
//   Constant      -    9 (54 bytes)
//   CustomAttribute-  521 (3126 bytes)
//   InterfaceImpl -    5 (20 bytes)
//   PropertyMap   -   40 (160 bytes)
//   Property      -  144 (864 bytes)
//   MethodSemantic-  228 (1368 bytes)
//   TypeSpec      -    2 (4 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   ExportedType  -   12 (168 bytes)
//   NestedClass   -    2 (8 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    4 (24 bytes)
//   Strings       -  9378 bytes
//   Blobs         -  3800 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   222 bytes

// CLR additional info : 128

// CLR method headers : 596
//   Num.of method bodies  - 596
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 596

// Managed code : 1447
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class CodeGenerationOptions          (public) (auto) (ansi) (sealed) 
// Class CodeIdentifier                 (public) (auto) (ansi) 
// Class CodeIdentifiers                (public) (auto) (ansi) 
// Class ImportContext                  (public) (auto) (ansi) 
// Interface IXmlTextParser                 (public) (abstract) (auto) (ansi) 
// Class SchemaImporter                 (public) (abstract) (auto) (ansi) 
// Class SoapAttributeAttribute         (public) (auto) (ansi) 
// Class SoapAttributeOverrides         (public) (auto) (ansi) 
// Class SoapAttributes                 (public) (auto) (ansi) 
// Class SoapElementAttribute           (public) (auto) (ansi) 
// Class SoapEnumAttribute              (public) (auto) (ansi) 
// Class SoapIgnoreAttribute            (public) (auto) (ansi) 
// Class SoapIncludeAttribute           (public) (auto) (ansi) 
// Class SoapReflectionImporter         (public) (auto) (ansi) 
// Class SoapSchemaMember               (public) (auto) (ansi) 
// Class SoapTypeAttribute              (public) (auto) (ansi) 
// Class UnreferencedObjectEventArgs    (public) (auto) (ansi) 
// Class UnreferencedObjectEventHandler (public) (auto) (ansi) (sealed) 
// Class XmlAnyElementAttributes        (public) (auto) (ansi) 
// Class XmlArrayAttribute              (public) (auto) (ansi) 
// Class XmlArrayItemAttribute          (public) (auto) (ansi) 
// Class XmlArrayItemAttributes         (public) (auto) (ansi) 
// Class XmlAttributeEventArgs          (public) (auto) (ansi) 
// Class XmlAttributeEventHandler       (public) (auto) (ansi) (sealed) 
// Class XmlAttributeOverrides          (public) (auto) (ansi) 
// Class XmlAttributes                  (public) (auto) (ansi) 
// Class XmlChoiceIdentifierAttribute   (public) (auto) (ansi) 
// Class XmlDeserializationEvents       (public) (sequential) (ansi) (sealed) 
// Class XmlElementAttributes           (public) (auto) (ansi) 
// Class XmlElementEventArgs            (public) (auto) (ansi) 
// Class XmlElementEventHandler         (public) (auto) (ansi) (sealed) 
// Class XmlIncludeAttribute            (public) (auto) (ansi) 
// Class XmlMapping                     (public) (abstract) (auto) (ansi) 
// Class XmlMappingAccess               (public) (auto) (ansi) (sealed) 
// Class XmlMemberMapping               (public) (auto) (ansi) 
// Class XmlMembersMapping              (public) (auto) (ansi) 
// Class XmlNodeEventArgs               (public) (auto) (ansi) 
// Class XmlNodeEventHandler            (public) (auto) (ansi) (sealed) 
// Class XmlReflectionImporter          (public) (auto) (ansi) 
// Class XmlReflectionMember            (public) (auto) (ansi) 
// Class XmlSchemaEnumerator            (public) (auto) (ansi) 
// Class XmlSchemaExporter              (public) (auto) (ansi) 
// Class XmlSchemaImporter              (public) (auto) (ansi) 
// Class XmlSchemas                     (public) (auto) (ansi) 
// Class XmlSerializationCollectionFixupCallback (public) (auto) (ansi) (sealed) 
// Class XmlSerializationFixupCallback  (public) (auto) (ansi) (sealed) 
// Class XmlSerializationGeneratedCode  (public) (abstract) (auto) (ansi) 
// Class XmlSerializationReadCallback   (public) (auto) (ansi) (sealed) 
// Class XmlSerializationReader         (public) (abstract) (auto) (ansi) 
// Class XmlSerializationWriteCallback  (public) (auto) (ansi) (sealed) 
// Class XmlSerializationWriter         (public) (abstract) (auto) (ansi) 
// Class XmlSerializer                  (public) (auto) (ansi) 
// Class XmlSerializerAssemblyAttribute (public) (auto) (ansi) (sealed) 
// Class XmlSerializerFactory           (public) (auto) (ansi) 
// Class XmlSerializerImplementation    (public) (abstract) (auto) (ansi) 
// Class XmlSerializerVersionAttribute  (public) (auto) (ansi) (sealed) 
// Class XmlTypeAttribute               (public) (auto) (ansi) 
// Class XmlTypeMapping                 (public) (auto) (ansi) 
// Class CollectionFixup                (auto) (ansi) (nested family) 
// Class Fixup                          (auto) (ansi) (nested family) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 5:0:0:0
}
.assembly extern /*23000002*/ System.Xml.ReaderWriter
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 5:0:0:0
}
.assembly extern /*23000003*/ System.Collections.Specialized
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 5:0:0:0
}
.assembly extern /*23000004*/ System.Collections.NonGeneric
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 5:0:0:0
}
.assembly /*20000001*/ noplatform System.Xml.XmlSerializer
{
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 35 2E 30 01 00 54 0E 14   // ersion=v5.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 53 79 73 74 65 6D 2E 58 6D 6C 2E 58 6D   // ...System.Xml.Xm
                                                                                                                                                                        6C 53 65 72 69 61 6C 69 7A 65 72 00 00 )          // lSerializer..
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 18 53 79 73 74 65 6D 2E 58 6D 6C 2E 58 6D   // ...System.Xml.Xm
                                                                                                                                                                       6C 53 65 72 69 61 6C 69 7A 65 72 00 00 )          // lSerializer..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 35 2E 30 2E 32 30 2E 35 31 39 30 34 00   // ...5.0.20.51904.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 35 2E 30 2E 30 2B 63 66 32 35 38 61 31   // ...5.0.0+cf258a1
                                                                                                                                                                                34 62 37 30 61 64 39 30 36 39 34 37 30 61 31 30   // 4b70ad9069470a10
                                                                                                                                                                                38 66 31 33 37 36 35 65 30 65 35 39 38 38 66 35   // 8f13765e0e5988f5
                                                                                                                                                                                31 00 00 )                                        // 1..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 18 53 79 73 74 65 6D 2E 58 6D 6C 2E 58 6D   // ...System.Xml.Xm
                                                                                                                                                                 6C 53 65 72 69 61 6C 69 7A 65 72 00 00 )          // lSerializer..
  .custom /*0C000014:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    1F 67 69 74 3A 2F 2F 67 69 74 68 75 62 2E 63 6F   // .git://github.co
                                                                                                                                                                    6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69 6D 65   // m/dotnet/runtime
                                                                                                                                                                    00 00 ) 
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
  .ver 5:0:0:0
}
.class extern /*27000001*/ forwarder System.Xml.Serialization.IXmlSerializable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000002*/ forwarder System.Xml.Serialization.XmlAnyAttributeAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000003*/ forwarder System.Xml.Serialization.XmlAnyElementAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000004*/ forwarder System.Xml.Serialization.XmlAttributeAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000005*/ forwarder System.Xml.Serialization.XmlElementAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000006*/ forwarder System.Xml.Serialization.XmlEnumAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000007*/ forwarder System.Xml.Serialization.XmlIgnoreAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000008*/ forwarder System.Xml.Serialization.XmlNamespaceDeclarationsAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000009*/ forwarder System.Xml.Serialization.XmlRootAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000A*/ forwarder System.Xml.Serialization.XmlSchemaProviderAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000B*/ forwarder System.Xml.Serialization.XmlSerializerNamespaces
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000C*/ forwarder System.Xml.Serialization.XmlTextAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.module System.Xml.XmlSerializer.dll
// MVID: {80be91b7-9c42-450b-92c3-6b8cb684d456}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FC24D677000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000016:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000018:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00004C       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100004C*/
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
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001C:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000021:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi sealed System.Xml.Serialization.CodeGenerationOptions
       extends [System.Runtime/*23000001*/]System.Enum/*01000016*/
{
  .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Xml.Serialization.CodeGenerationOptions/*02000006*/ None = int32(0x00000000)
  .custom /*0C000020:0A000013*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlIgnoreAttribute/*01000017*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ public static literal valuetype System.Xml.Serialization.CodeGenerationOptions/*02000006*/ GenerateProperties = int32(0x00000001)
  .custom /*0C000026:0A000014*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 0A 70 72 6F 70 65 72 74 69 65 73 00 00 )    // ...properties..
  .field /*04000007*/ public static literal valuetype System.Xml.Serialization.CodeGenerationOptions/*02000006*/ GenerateNewAsync = int32(0x00000002)
  .custom /*0C000028:0A000014*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 08 6E 65 77 41 73 79 6E 63 00 00 )          // ...newAsync..
  .field /*04000008*/ public static literal valuetype System.Xml.Serialization.CodeGenerationOptions/*02000006*/ GenerateOldAsync = int32(0x00000004)
  .custom /*0C00002C:0A000014*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 08 6F 6C 64 41 73 79 6E 63 00 00 )          // ...oldAsync..
  .field /*04000009*/ public static literal valuetype System.Xml.Serialization.CodeGenerationOptions/*02000006*/ GenerateOrder = int32(0x00000008)
  .custom /*0C00002F:0A000014*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 05 6F 72 64 65 72 00 00 )                   // ...order..
  .field /*0400000A*/ public static literal valuetype System.Xml.Serialization.CodeGenerationOptions/*02000006*/ EnableDataBinding = int32(0x00000010)
  .custom /*0C000033:0A000014*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 11 65 6E 61 62 6C 65 44 61 74 61 42 69 6E   // ...enableDataBin
                                                                                                                                                                           64 69 6E 67 00 00 )                               // ding..
} // end of class System.Xml.Serialization.CodeGenerationOptions

.class /*02000007*/ public auto ansi beforefieldinit System.Xml.Serialization.CodeIdentifier
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001A*/::.ctor(string) /* 0A000015 */ = ( 01 00 4B 54 68 69 73 20 63 6C 61 73 73 20 73 68   // ..KThis class sh
                                                                                                                                                   6F 75 6C 64 20 6E 65 76 65 72 20 67 65 74 20 63   // ould never get c
                                                                                                                                                   6F 6E 73 74 72 75 63 74 65 64 20 61 73 20 69 74   // onstructed as it
                                                                                                                                                   20 63 6F 6E 74 61 69 6E 73 20 6F 6E 6C 79 20 73   //  contains only s
                                                                                                                                                   74 61 74 69 63 20 6D 65 74 68 6F 64 73 2E 00 00 ) // tatic methods...
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CodeIdentifier::.ctor

  .method /*06000007*/ public hidebysig static 
          string  MakeCamel(string identifier) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifier::MakeCamel

  .method /*06000008*/ public hidebysig static 
          string  MakePascal(string identifier) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x20a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifier::MakePascal

  .method /*06000009*/ public hidebysig static 
          string  MakeValid(string identifier) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x20ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifier::MakeValid

} // end of class System.Xml.Serialization.CodeIdentifier

.class /*02000008*/ public auto ansi beforefieldinit System.Xml.Serialization.CodeIdentifiers
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ae
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool caseSensitive) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance bool  get_UseCamelCasing() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifiers::get_UseCamelCasing

  .method /*0600000D*/ public hidebysig specialname 
          instance void  set_UseCamelCasing(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::set_UseCamelCasing

  .method /*0600000E*/ public hidebysig instance void 
          Add(string identifier,
              object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000007*/ 
    .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::Add

  .method /*0600000F*/ public hidebysig instance void 
          AddReserved(string identifier) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::AddReserved

  .method /*06000010*/ public hidebysig instance string 
          AddUnique(string identifier,
                    object 'value') cil managed
  // SIG: 20 02 0E 0E 1C
  {
    .param [2]/*0800000A*/ 
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifiers::AddUnique

  .method /*06000011*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::Clear

  .method /*06000012*/ public hidebysig instance bool 
          IsInUse(string identifier) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifiers::IsInUse

  .method /*06000013*/ public hidebysig instance string 
          MakeRightCase(string identifier) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifiers::MakeRightCase

  .method /*06000014*/ public hidebysig instance string 
          MakeUnique(string identifier) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifiers::MakeUnique

  .method /*06000015*/ public hidebysig instance void 
          Remove(string identifier) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::Remove

  .method /*06000016*/ public hidebysig instance void 
          RemoveReserved(string identifier) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CodeIdentifiers::RemoveReserved

  .method /*06000017*/ public hidebysig instance object 
          ToArray(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 1C 12 6D
  {
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CodeIdentifiers::ToArray

  .property /*17000001*/ instance callconv(8) bool
          UseCamelCasing()
  {
    .get instance bool System.Xml.Serialization.CodeIdentifiers/*02000008*/::get_UseCamelCasing() /* 0600000C */
    .set instance void System.Xml.Serialization.CodeIdentifiers/*02000008*/::set_UseCamelCasing(bool) /* 0600000D */
  } // end of property CodeIdentifiers::UseCamelCasing
} // end of class System.Xml.Serialization.CodeIdentifiers

.class /*02000009*/ public auto ansi beforefieldinit System.Xml.Serialization.ImportContext
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.CodeIdentifiers/*02000008*/ identifiers,
                               bool shareTypes) cil managed
  // SIG: 20 02 01 12 20 02
  {
    .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20dc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ImportContext::.ctor

  .method /*06000019*/ public hidebysig specialname 
          instance bool  get_ShareTypes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImportContext::get_ShareTypes

  .method /*0600001A*/ public hidebysig specialname 
          instance class System.Xml.Serialization.CodeIdentifiers/*02000008*/ 
          get_TypeIdentifiers() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImportContext::get_TypeIdentifiers

  .method /*0600001B*/ public hidebysig specialname 
          instance class [System.Collections.Specialized/*23000003*/]System.Collections.Specialized.StringCollection/*0100001C*/ 
          get_Warnings() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImportContext::get_Warnings

  .property /*17000002*/ instance callconv(8) bool
          ShareTypes()
  {
    .get instance bool System.Xml.Serialization.ImportContext/*02000009*/::get_ShareTypes() /* 06000019 */
  } // end of property ImportContext::ShareTypes
  .property /*17000003*/ instance callconv(8) class System.Xml.Serialization.CodeIdentifiers/*02000008*/
          TypeIdentifiers()
  {
    .get instance class System.Xml.Serialization.CodeIdentifiers/*02000008*/ System.Xml.Serialization.ImportContext/*02000009*/::get_TypeIdentifiers() /* 0600001A */
  } // end of property ImportContext::TypeIdentifiers
  .property /*17000004*/ instance callconv(8) class [System.Collections.Specialized/*23000003*/]System.Collections.Specialized.StringCollection/*0100001C*/
          Warnings()
  {
    .get instance class [System.Collections.Specialized/*23000003*/]System.Collections.Specialized.StringCollection/*0100001C*/ System.Xml.Serialization.ImportContext/*02000009*/::get_Warnings() /* 0600001B */
  } // end of property ImportContext::Warnings
} // end of class System.Xml.Serialization.ImportContext

.class /*0200000A*/ interface public abstract auto ansi System.Xml.Serialization.IXmlTextParser
{
  .method /*0600001C*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_Normalized() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IXmlTextParser::get_Normalized

  .method /*0600001D*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Normalized(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IXmlTextParser::set_Normalized

  .method /*0600001E*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.WhitespaceHandling/*0100001D*/ 
          get_WhitespaceHandling() cil managed
  // SIG: 20 00 11 75
  {
    // Method begins at RVA 0x0
  } // end of method IXmlTextParser::get_WhitespaceHandling

  .method /*0600001F*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_WhitespaceHandling(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.WhitespaceHandling/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 11 75
  {
    // Method begins at RVA 0x0
  } // end of method IXmlTextParser::set_WhitespaceHandling

  .property /*17000005*/ instance callconv(8) bool
          Normalized()
  {
    .get instance bool System.Xml.Serialization.IXmlTextParser/*0200000A*/::get_Normalized() /* 0600001C */
    .set instance void System.Xml.Serialization.IXmlTextParser/*0200000A*/::set_Normalized(bool) /* 0600001D */
  } // end of property IXmlTextParser::Normalized
  .property /*17000006*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.WhitespaceHandling/*0100001D*/
          WhitespaceHandling()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.WhitespaceHandling/*0100001D*/ System.Xml.Serialization.IXmlTextParser/*0200000A*/::get_WhitespaceHandling() /* 0600001E */
    .set instance void System.Xml.Serialization.IXmlTextParser/*0200000A*/::set_WhitespaceHandling(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.WhitespaceHandling/*0100001D*/) /* 0600001F */
  } // end of property IXmlTextParser::WhitespaceHandling
} // end of class System.Xml.Serialization.IXmlTextParser

.class /*0200000B*/ public abstract auto ansi beforefieldinit System.Xml.Serialization.SchemaImporter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .method /*06000020*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ed
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SchemaImporter::.ctor

} // end of class System.Xml.Serialization.SchemaImporter

.class /*0200000C*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapAttributeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000039:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 80 29 00 00 00 00 )                         // ...)....
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapAttributeAttribute::.ctor

  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string attributeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20fd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapAttributeAttribute::.ctor

  .method /*06000023*/ public hidebysig specialname 
          instance string  get_AttributeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributeAttribute::get_AttributeName

  .method /*06000024*/ public hidebysig specialname 
          instance void  set_AttributeName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000016*/ 
    .custom /*0C000052:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2108
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributeAttribute::set_AttributeName

  .method /*06000025*/ public hidebysig specialname 
          instance string  get_DataType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributeAttribute::get_DataType

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_DataType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000017*/ 
    .custom /*0C000053:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributeAttribute::set_DataType

  .method /*06000027*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributeAttribute::get_Namespace

  .method /*06000028*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2112
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributeAttribute::set_Namespace

  .property /*17000007*/ instance callconv(8) string
          AttributeName()
  {
    .get instance string System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/::get_AttributeName() /* 06000023 */
    .set instance void System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/::set_AttributeName(string) /* 06000024 */
  } // end of property SoapAttributeAttribute::AttributeName
  .property /*17000008*/ instance callconv(8) string
          DataType()
  {
    .get instance string System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/::get_DataType() /* 06000025 */
    .set instance void System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/::set_DataType(string) /* 06000026 */
  } // end of property SoapAttributeAttribute::DataType
  .property /*17000009*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/::get_Namespace() /* 06000027 */
    .set instance void System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/::set_Namespace(string) /* 06000028 */
  } // end of property SoapAttributeAttribute::Namespace
} // end of class System.Xml.Serialization.SoapAttributeAttribute

.class /*0200000D*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapAttributeOverrides
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2114
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapAttributeOverrides::.ctor

  .method /*0600002A*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapAttributes/*0200000E*/ 
          get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 38 12 6D
  {
    .param [0]/*08000019*/ 
    .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributeOverrides::get_Item

  .method /*0600002B*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapAttributes/*0200000E*/ 
          get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                   string member) cil managed
  // SIG: 20 02 12 38 12 6D 0E
  {
    .param [0]/*0800001B*/ 
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributeOverrides::get_Item

  .method /*0600002C*/ public hidebysig instance void 
          Add(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
              string member,
              class System.Xml.Serialization.SoapAttributes/*0200000E*/ attributes) cil managed
  // SIG: 20 03 01 12 6D 0E 12 38
  {
    .param [3]/*08000020*/ 
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributeOverrides::Add

  .method /*0600002D*/ public hidebysig instance void 
          Add(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
              class System.Xml.Serialization.SoapAttributes/*0200000E*/ attributes) cil managed
  // SIG: 20 02 01 12 6D 12 38
  {
    .param [2]/*08000022*/ 
    .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2124
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributeOverrides::Add

  .property /*1700000A*/ instance callconv(8) class System.Xml.Serialization.SoapAttributes/*0200000E*/
          Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/)
  {
    .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Serialization.SoapAttributes/*0200000E*/ System.Xml.Serialization.SoapAttributeOverrides/*0200000D*/::get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0600002A */
  } // end of property SoapAttributeOverrides::Item
  .property /*1700000B*/ instance callconv(8) class System.Xml.Serialization.SoapAttributes/*0200000E*/
          Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/,
               string)
  {
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Serialization.SoapAttributes/*0200000E*/ System.Xml.Serialization.SoapAttributeOverrides/*0200000D*/::get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/,
                                                                                                                                                  string) /* 0600002B */
  } // end of property SoapAttributeOverrides::Item
} // end of class System.Xml.Serialization.SoapAttributeOverrides

.class /*0200000E*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapAttributes
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2126
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::.ctor

  .method /*0600002F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000020*/ provider) cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C000092:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::.ctor

  .method /*06000030*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/ 
          get_SoapAttribute() cil managed
  // SIG: 20 00 12 30
  {
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributes::get_SoapAttribute

  .method /*06000031*/ public hidebysig specialname 
          instance void  set_SoapAttribute(class System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/ 'value') cil managed
  // SIG: 20 01 01 12 30
  {
    // Method begins at RVA 0x2139
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::set_SoapAttribute

  .method /*06000032*/ public hidebysig specialname 
          instance object  get_SoapDefaultValue() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributes::get_SoapDefaultValue

  .method /*06000033*/ public hidebysig specialname 
          instance void  set_SoapDefaultValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x213e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::set_SoapDefaultValue

  .method /*06000034*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapElementAttribute/*0200000F*/ 
          get_SoapElement() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributes::get_SoapElement

  .method /*06000035*/ public hidebysig specialname 
          instance void  set_SoapElement(class System.Xml.Serialization.SoapElementAttribute/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x2143
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::set_SoapElement

  .method /*06000036*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapEnumAttribute/*02000010*/ 
          get_SoapEnum() cil managed
  // SIG: 20 00 12 40
  {
    // Method begins at RVA 0x2145
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributes::get_SoapEnum

  .method /*06000037*/ public hidebysig specialname 
          instance void  set_SoapEnum(class System.Xml.Serialization.SoapEnumAttribute/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x2148
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::set_SoapEnum

  .method /*06000038*/ public hidebysig specialname 
          instance bool  get_SoapIgnore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributes::get_SoapIgnore

  .method /*06000039*/ public hidebysig specialname 
          instance void  set_SoapIgnore(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x214d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::set_SoapIgnore

  .method /*0600003A*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapTypeAttribute/*02000015*/ 
          get_SoapType() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapAttributes::get_SoapType

  .method /*0600003B*/ public hidebysig specialname 
          instance void  set_SoapType(class System.Xml.Serialization.SoapTypeAttribute/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x2152
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapAttributes::set_SoapType

  .property /*1700000C*/ instance callconv(8) class System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/
          SoapAttribute()
  {
    .get instance class System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/ System.Xml.Serialization.SoapAttributes/*0200000E*/::get_SoapAttribute() /* 06000030 */
    .set instance void System.Xml.Serialization.SoapAttributes/*0200000E*/::set_SoapAttribute(class System.Xml.Serialization.SoapAttributeAttribute/*0200000C*/) /* 06000031 */
  } // end of property SoapAttributes::SoapAttribute
  .property /*1700000D*/ instance callconv(8) object
          SoapDefaultValue()
  {
    .get instance object System.Xml.Serialization.SoapAttributes/*0200000E*/::get_SoapDefaultValue() /* 06000032 */
    .set instance void System.Xml.Serialization.SoapAttributes/*0200000E*/::set_SoapDefaultValue(object) /* 06000033 */
  } // end of property SoapAttributes::SoapDefaultValue
  .property /*1700000E*/ instance callconv(8) class System.Xml.Serialization.SoapElementAttribute/*0200000F*/
          SoapElement()
  {
    .get instance class System.Xml.Serialization.SoapElementAttribute/*0200000F*/ System.Xml.Serialization.SoapAttributes/*0200000E*/::get_SoapElement() /* 06000034 */
    .set instance void System.Xml.Serialization.SoapAttributes/*0200000E*/::set_SoapElement(class System.Xml.Serialization.SoapElementAttribute/*0200000F*/) /* 06000035 */
  } // end of property SoapAttributes::SoapElement
  .property /*1700000F*/ instance callconv(8) class System.Xml.Serialization.SoapEnumAttribute/*02000010*/
          SoapEnum()
  {
    .get instance class System.Xml.Serialization.SoapEnumAttribute/*02000010*/ System.Xml.Serialization.SoapAttributes/*0200000E*/::get_SoapEnum() /* 06000036 */
    .set instance void System.Xml.Serialization.SoapAttributes/*0200000E*/::set_SoapEnum(class System.Xml.Serialization.SoapEnumAttribute/*02000010*/) /* 06000037 */
  } // end of property SoapAttributes::SoapEnum
  .property /*17000010*/ instance callconv(8) bool
          SoapIgnore()
  {
    .get instance bool System.Xml.Serialization.SoapAttributes/*0200000E*/::get_SoapIgnore() /* 06000038 */
    .set instance void System.Xml.Serialization.SoapAttributes/*0200000E*/::set_SoapIgnore(bool) /* 06000039 */
  } // end of property SoapAttributes::SoapIgnore
  .property /*17000011*/ instance callconv(8) class System.Xml.Serialization.SoapTypeAttribute/*02000015*/
          SoapType()
  {
    .get instance class System.Xml.Serialization.SoapTypeAttribute/*02000015*/ System.Xml.Serialization.SoapAttributes/*0200000E*/::get_SoapType() /* 0600003A */
    .set instance void System.Xml.Serialization.SoapAttributes/*0200000E*/::set_SoapType(class System.Xml.Serialization.SoapTypeAttribute/*02000015*/) /* 0600003B */
  } // end of property SoapAttributes::SoapType
} // end of class System.Xml.Serialization.SoapAttributes

.class /*0200000F*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapElementAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000041:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 80 29 00 00 00 00 )                         // ...)....
  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2154
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapElementAttribute::.ctor

  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string elementName) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapElementAttribute::.ctor

  .method /*0600003E*/ public hidebysig specialname 
          instance string  get_DataType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapElementAttribute::get_DataType

  .method /*0600003F*/ public hidebysig specialname 
          instance void  set_DataType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800002B*/ 
    .custom /*0C00008A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2167
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapElementAttribute::set_DataType

  .method /*06000040*/ public hidebysig specialname 
          instance string  get_ElementName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2169
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapElementAttribute::get_ElementName

  .method /*06000041*/ public hidebysig specialname 
          instance void  set_ElementName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800002C*/ 
    .custom /*0C00008D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x216c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapElementAttribute::set_ElementName

  .method /*06000042*/ public hidebysig specialname 
          instance bool  get_IsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x216e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapElementAttribute::get_IsNullable

  .method /*06000043*/ public hidebysig specialname 
          instance void  set_IsNullable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2171
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapElementAttribute::set_IsNullable

  .property /*17000012*/ instance callconv(8) string
          DataType()
  {
    .get instance string System.Xml.Serialization.SoapElementAttribute/*0200000F*/::get_DataType() /* 0600003E */
    .set instance void System.Xml.Serialization.SoapElementAttribute/*0200000F*/::set_DataType(string) /* 0600003F */
  } // end of property SoapElementAttribute::DataType
  .property /*17000013*/ instance callconv(8) string
          ElementName()
  {
    .get instance string System.Xml.Serialization.SoapElementAttribute/*0200000F*/::get_ElementName() /* 06000040 */
    .set instance void System.Xml.Serialization.SoapElementAttribute/*0200000F*/::set_ElementName(string) /* 06000041 */
  } // end of property SoapElementAttribute::ElementName
  .property /*17000014*/ instance callconv(8) bool
          IsNullable()
  {
    .get instance bool System.Xml.Serialization.SoapElementAttribute/*0200000F*/::get_IsNullable() /* 06000042 */
    .set instance void System.Xml.Serialization.SoapElementAttribute/*0200000F*/::set_IsNullable(bool) /* 06000043 */
  } // end of property SoapElementAttribute::IsNullable
} // end of class System.Xml.Serialization.SoapElementAttribute

.class /*02000010*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapEnumAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000044:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 00 01 00 00 00 00 ) 
  .method /*06000044*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2173
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapEnumAttribute::.ctor

  .method /*06000045*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x217b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapEnumAttribute::.ctor

  .method /*06000046*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapEnumAttribute::get_Name

  .method /*06000047*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800002F*/ 
    .custom /*0C000095:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapEnumAttribute::set_Name

  .property /*17000015*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Xml.Serialization.SoapEnumAttribute/*02000010*/::get_Name() /* 06000046 */
    .set instance void System.Xml.Serialization.SoapEnumAttribute/*02000010*/::set_Name(string) /* 06000047 */
  } // end of property SoapEnumAttribute::Name
} // end of class System.Xml.Serialization.SoapEnumAttribute

.class /*02000011*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapIgnoreAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000045:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 80 29 00 00 00 00 )                         // ...)....
  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2188
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapIgnoreAttribute::.ctor

} // end of class System.Xml.Serialization.SoapIgnoreAttribute

.class /*02000012*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapIncludeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000048:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 04 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2190
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapIncludeAttribute::.ctor

  .method /*0600004A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 
          get_Type() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x2198
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapIncludeAttribute::get_Type

  .method /*0600004B*/ public hidebysig specialname 
          instance void  set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x219b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapIncludeAttribute::set_Type

  .property /*17000016*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001B*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ System.Xml.Serialization.SoapIncludeAttribute/*02000012*/::get_Type() /* 0600004A */
    .set instance void System.Xml.Serialization.SoapIncludeAttribute/*02000012*/::set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0600004B */
  } // end of property SoapIncludeAttribute::Type
} // end of class System.Xml.Serialization.SoapIncludeAttribute

.class /*02000013*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapReflectionImporter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x219d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapReflectionImporter::.ctor

  .method /*0600004D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string defaultNamespace) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapReflectionImporter::.ctor

  .method /*0600004E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.SoapAttributeOverrides/*0200000D*/ attributeOverrides) cil managed
  // SIG: 20 01 01 12 34
  {
    .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapReflectionImporter::.ctor

  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.SoapAttributeOverrides/*0200000D*/ attributeOverrides,
                               string defaultNamespace) cil managed
  // SIG: 20 02 01 12 34 0E
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapReflectionImporter::.ctor

  .method /*06000050*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members) cil managed
  // SIG: 20 03 12 80 A4 0E 0E 1D 12 80 B4
  {
    .param [1]/*08000036*/ 
    .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000037*/ 
    .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapReflectionImporter::ImportMembersMapping

  .method /*06000051*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement,
                               bool writeAccessors) cil managed
  // SIG: 20 05 12 80 A4 0E 0E 1D 12 80 B4 02 02
  {
    .param [1]/*08000039*/ 
    .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800003A*/ 
    .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapReflectionImporter::ImportMembersMapping

  .method /*06000052*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement,
                               bool writeAccessors,
                               bool validate) cil managed
  // SIG: 20 06 12 80 A4 0E 0E 1D 12 80 B4 02 02 02
  {
    .param [1]/*0800003E*/ 
    .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800003F*/ 
    .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapReflectionImporter::ImportMembersMapping

  .method /*06000053*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement,
                               bool writeAccessors,
                               bool validate,
                               valuetype System.Xml.Serialization.XmlMappingAccess/*02000027*/ access) cil managed
  // SIG: 20 07 12 80 A4 0E 0E 1D 12 80 B4 02 02 02 11 80 9C
  {
    .param [1]/*08000044*/ 
    .custom /*0C0000C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000045*/ 
    .custom /*0C0000C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapReflectionImporter::ImportMembersMapping

  .method /*06000054*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 80 FC 12 6D
  {
    // Method begins at RVA 0x21c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapReflectionImporter::ImportTypeMapping

  .method /*06000055*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                            string defaultNamespace) cil managed
  // SIG: 20 02 12 80 FC 12 6D 0E
  {
    .param [2]/*0800004D*/ 
    .custom /*0C0000C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapReflectionImporter::ImportTypeMapping

  .method /*06000056*/ public hidebysig instance void 
          IncludeType(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x21cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapReflectionImporter::IncludeType

  .method /*06000057*/ public hidebysig instance void 
          IncludeTypes(class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000020*/ provider) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x21d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapReflectionImporter::IncludeTypes

} // end of class System.Xml.Serialization.SoapReflectionImporter

.class /*02000014*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapSchemaMember
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapSchemaMember::.ctor

  .method /*06000059*/ public hidebysig specialname 
          instance string  get_MemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapSchemaMember::get_MemberName

  .method /*0600005A*/ public hidebysig specialname 
          instance void  set_MemberName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000050*/ 
    .custom /*0C0000C8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapSchemaMember::set_MemberName

  .method /*0600005B*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 
          get_MemberType() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C0000CE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapSchemaMember::get_MemberType

  .method /*0600005C*/ public hidebysig specialname 
          instance void  set_MemberType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value') cil managed
  // SIG: 20 01 01 12 80 85
  {
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapSchemaMember::set_MemberType

  .property /*17000017*/ instance callconv(8) string
          MemberName()
  {
    .get instance string System.Xml.Serialization.SoapSchemaMember/*02000014*/::get_MemberName() /* 06000059 */
    .set instance void System.Xml.Serialization.SoapSchemaMember/*02000014*/::set_MemberName(string) /* 0600005A */
  } // end of property SoapSchemaMember::MemberName
  .property /*17000018*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/
          MemberType()
  {
    .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ System.Xml.Serialization.SoapSchemaMember/*02000014*/::get_MemberType() /* 0600005B */
    .set instance void System.Xml.Serialization.SoapSchemaMember/*02000014*/::set_MemberType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/) /* 0600005C */
  } // end of property SoapSchemaMember::MemberType
} // end of class System.Xml.Serialization.SoapSchemaMember

.class /*02000015*/ public auto ansi beforefieldinit System.Xml.Serialization.SoapTypeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00004F:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 1C 04 00 00 00 00 ) 
  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapTypeAttribute::.ctor

  .method /*0600005E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21ed
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapTypeAttribute::.ctor

  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName,
                               string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x21f5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SoapTypeAttribute::.ctor

  .method /*06000060*/ public hidebysig specialname 
          instance bool  get_IncludeInSchema() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapTypeAttribute::get_IncludeInSchema

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_IncludeInSchema(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2200
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapTypeAttribute::set_IncludeInSchema

  .method /*06000062*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapTypeAttribute::get_Namespace

  .method /*06000063*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2205
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapTypeAttribute::set_Namespace

  .method /*06000064*/ public hidebysig specialname 
          instance string  get_TypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000D4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2207
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SoapTypeAttribute::get_TypeName

  .method /*06000065*/ public hidebysig specialname 
          instance void  set_TypeName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000D6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000057*/ 
    .custom /*0C0000CB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x220a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SoapTypeAttribute::set_TypeName

  .property /*17000019*/ instance callconv(8) bool
          IncludeInSchema()
  {
    .get instance bool System.Xml.Serialization.SoapTypeAttribute/*02000015*/::get_IncludeInSchema() /* 06000060 */
    .set instance void System.Xml.Serialization.SoapTypeAttribute/*02000015*/::set_IncludeInSchema(bool) /* 06000061 */
  } // end of property SoapTypeAttribute::IncludeInSchema
  .property /*1700001A*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Xml.Serialization.SoapTypeAttribute/*02000015*/::get_Namespace() /* 06000062 */
    .set instance void System.Xml.Serialization.SoapTypeAttribute/*02000015*/::set_Namespace(string) /* 06000063 */
  } // end of property SoapTypeAttribute::Namespace
  .property /*1700001B*/ instance callconv(8) string
          TypeName()
  {
    .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Xml.Serialization.SoapTypeAttribute/*02000015*/::get_TypeName() /* 06000064 */
    .set instance void System.Xml.Serialization.SoapTypeAttribute/*02000015*/::set_TypeName(string) /* 06000065 */
  } // end of property SoapTypeAttribute::TypeName
} // end of class System.Xml.Serialization.SoapTypeAttribute

.class /*02000016*/ public auto ansi beforefieldinit System.Xml.Serialization.UnreferencedObjectEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000022*/
{
  .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000066*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object o,
                               string id) cil managed
  // SIG: 20 02 01 1C 0E
  {
    // Method begins at RVA 0x220c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000022*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnreferencedObjectEventArgs::.ctor

  .method /*06000067*/ public hidebysig specialname 
          instance string  get_UnreferencedId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnreferencedObjectEventArgs::get_UnreferencedId

  .method /*06000068*/ public hidebysig specialname 
          instance object  get_UnreferencedObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnreferencedObjectEventArgs::get_UnreferencedObject

  .property /*1700001C*/ instance callconv(8) string
          UnreferencedId()
  {
    .get instance string System.Xml.Serialization.UnreferencedObjectEventArgs/*02000016*/::get_UnreferencedId() /* 06000067 */
  } // end of property UnreferencedObjectEventArgs::UnreferencedId
  .property /*1700001D*/ instance callconv(8) object
          UnreferencedObject()
  {
    .get instance object System.Xml.Serialization.UnreferencedObjectEventArgs/*02000016*/::get_UnreferencedObject() /* 06000068 */
  } // end of property UnreferencedObjectEventArgs::UnreferencedObject
} // end of class System.Xml.Serialization.UnreferencedObjectEventArgs

.class /*02000017*/ public auto ansi sealed System.Xml.Serialization.UnreferencedObjectEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UnreferencedObjectEventHandler::.ctor

  .method /*0600006A*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Xml.Serialization.UnreferencedObjectEventArgs/*02000016*/ e) runtime managed
  // SIG: 20 02 01 1C 12 58
  {
    .custom /*0C0000D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800005C*/ 
    .custom /*0C0000D0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method UnreferencedObjectEventHandler::Invoke

  .method /*0600006B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object sender,
                      class System.Xml.Serialization.UnreferencedObjectEventArgs/*02000016*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 91 1C 12 58 12 80 95 1C
  {
    .param [1]/*0800005E*/ 
    .custom /*0C0000D1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800005F*/ 
    .custom /*0C0000D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method UnreferencedObjectEventHandler::BeginInvoke

  .method /*0600006C*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method UnreferencedObjectEventHandler::EndInvoke

} // end of class System.Xml.Serialization.UnreferencedObjectEventHandler

.class /*02000018*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlAnyElementAttributes
       extends [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/
{
  .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000057:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*0600006D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x221a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlAnyElementAttributes::.ctor

  .method /*0600006E*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ 
          get_Item(int32 index) cil managed
  // SIG: 20 01 12 80 9D 08
  {
    // Method begins at RVA 0x2222
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAnyElementAttributes::get_Item

  .method /*0600006F*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ 'value') cil managed
  // SIG: 20 02 01 08 12 80 9D
  {
    // Method begins at RVA 0x2225
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAnyElementAttributes::set_Item

  .method /*06000070*/ public hidebysig instance int32 
          Add(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ attribute) cil managed
  // SIG: 20 01 08 12 80 9D
  {
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAnyElementAttributes::Add

  .method /*06000071*/ public hidebysig instance bool 
          Contains(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ attribute) cil managed
  // SIG: 20 01 02 12 80 9D
  {
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAnyElementAttributes::Contains

  .method /*06000072*/ public hidebysig instance void 
          CopyTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 80 9D 08
  {
    .custom /*0C0000DB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAnyElementAttributes::CopyTo

  .method /*06000073*/ public hidebysig instance int32 
          IndexOf(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ attribute) cil managed
  // SIG: 20 01 08 12 80 9D
  {
    // Method begins at RVA 0x222f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAnyElementAttributes::IndexOf

  .method /*06000074*/ public hidebysig instance void 
          Insert(int32 index,
                 class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ attribute) cil managed
  // SIG: 20 02 01 08 12 80 9D
  {
    // Method begins at RVA 0x2232
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAnyElementAttributes::Insert

  .method /*06000075*/ public hidebysig instance void 
          Remove(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ attribute) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2234
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAnyElementAttributes::Remove

  .property /*1700001E*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/
          Item(int32)
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/ System.Xml.Serialization.XmlAnyElementAttributes/*02000018*/::get_Item(int32) /* 0600006E */
    .set instance void System.Xml.Serialization.XmlAnyElementAttributes/*02000018*/::set_Item(int32,
                                                                                              class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyElementAttribute/*01000027*/) /* 0600006F */
  } // end of property XmlAnyElementAttributes::Item
} // end of class System.Xml.Serialization.XmlAnyElementAttributes

.class /*02000019*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlArrayAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005B:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 80 29 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ...)....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*06000076*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2236
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::.ctor

  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string elementName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x223e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::.ctor

  .method /*06000078*/ public hidebysig specialname 
          instance string  get_ElementName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000DD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2246
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayAttribute::get_ElementName

  .method /*06000079*/ public hidebysig specialname 
          instance void  set_ElementName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000DF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800006F*/ 
    .custom /*0C0000D9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2249
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::set_ElementName

  .method /*0600007A*/ public hidebysig specialname 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/ 
          get_Form() cil managed
  // SIG: 20 00 11 80 A1
  {
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayAttribute::get_Form

  .method /*0600007B*/ public hidebysig specialname 
          instance void  set_Form(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A1
  {
    // Method begins at RVA 0x224e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::set_Form

  .method /*0600007C*/ public hidebysig specialname 
          instance bool  get_IsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2250
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayAttribute::get_IsNullable

  .method /*0600007D*/ public hidebysig specialname 
          instance void  set_IsNullable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2253
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::set_IsNullable

  .method /*0600007E*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2255
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayAttribute::get_Namespace

  .method /*0600007F*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2258
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::set_Namespace

  .method /*06000080*/ public hidebysig specialname 
          instance int32  get_Order() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayAttribute::get_Order

  .method /*06000081*/ public hidebysig specialname 
          instance void  set_Order(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x225d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayAttribute::set_Order

  .property /*1700001F*/ instance callconv(8) string
          ElementName()
  {
    .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Xml.Serialization.XmlArrayAttribute/*02000019*/::get_ElementName() /* 06000078 */
    .set instance void System.Xml.Serialization.XmlArrayAttribute/*02000019*/::set_ElementName(string) /* 06000079 */
  } // end of property XmlArrayAttribute::ElementName
  .property /*17000020*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/
          Form()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/ System.Xml.Serialization.XmlArrayAttribute/*02000019*/::get_Form() /* 0600007A */
    .set instance void System.Xml.Serialization.XmlArrayAttribute/*02000019*/::set_Form(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/) /* 0600007B */
  } // end of property XmlArrayAttribute::Form
  .property /*17000021*/ instance callconv(8) bool
          IsNullable()
  {
    .get instance bool System.Xml.Serialization.XmlArrayAttribute/*02000019*/::get_IsNullable() /* 0600007C */
    .set instance void System.Xml.Serialization.XmlArrayAttribute/*02000019*/::set_IsNullable(bool) /* 0600007D */
  } // end of property XmlArrayAttribute::IsNullable
  .property /*17000022*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Xml.Serialization.XmlArrayAttribute/*02000019*/::get_Namespace() /* 0600007E */
    .set instance void System.Xml.Serialization.XmlArrayAttribute/*02000019*/::set_Namespace(string) /* 0600007F */
  } // end of property XmlArrayAttribute::Namespace
  .property /*17000023*/ instance callconv(8) int32
          Order()
  {
    .get instance int32 System.Xml.Serialization.XmlArrayAttribute/*02000019*/::get_Order() /* 06000080 */
    .set instance void System.Xml.Serialization.XmlArrayAttribute/*02000019*/::set_Order(int32) /* 06000081 */
  } // end of property XmlArrayAttribute::Order
} // end of class System.Xml.Serialization.XmlArrayAttribute

.class /*0200001A*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlArrayItemAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005F:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 80 29 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ...)....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*06000082*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::.ctor

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string elementName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2267
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::.ctor

  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string elementName,
                               class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x226f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::.ctor

  .method /*06000085*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2277
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::.ctor

  .method /*06000086*/ public hidebysig specialname 
          instance string  get_DataType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_DataType

  .method /*06000087*/ public hidebysig specialname 
          instance void  set_DataType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000E3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000078*/ 
    .custom /*0C0000DE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2282
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_DataType

  .method /*06000088*/ public hidebysig specialname 
          instance string  get_ElementName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_ElementName

  .method /*06000089*/ public hidebysig specialname 
          instance void  set_ElementName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000E5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000079*/ 
    .custom /*0C0000E0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2287
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_ElementName

  .method /*0600008A*/ public hidebysig specialname 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/ 
          get_Form() cil managed
  // SIG: 20 00 11 80 A1
  {
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_Form

  .method /*0600008B*/ public hidebysig specialname 
          instance void  set_Form(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A1
  {
    // Method begins at RVA 0x228c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_Form

  .method /*0600008C*/ public hidebysig specialname 
          instance bool  get_IsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_IsNullable

  .method /*0600008D*/ public hidebysig specialname 
          instance void  set_IsNullable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2291
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_IsNullable

  .method /*0600008E*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_Namespace

  .method /*0600008F*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2296
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_Namespace

  .method /*06000090*/ public hidebysig specialname 
          instance int32  get_NestingLevel() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_NestingLevel

  .method /*06000091*/ public hidebysig specialname 
          instance void  set_NestingLevel(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x229b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_NestingLevel

  .method /*06000092*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 
          get_Type() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttribute::get_Type

  .method /*06000093*/ public hidebysig specialname 
          instance void  set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x22a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttribute::set_Type

  .property /*17000024*/ instance callconv(8) string
          DataType()
  {
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_DataType() /* 06000086 */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_DataType(string) /* 06000087 */
  } // end of property XmlArrayItemAttribute::DataType
  .property /*17000025*/ instance callconv(8) string
          ElementName()
  {
    .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_ElementName() /* 06000088 */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_ElementName(string) /* 06000089 */
  } // end of property XmlArrayItemAttribute::ElementName
  .property /*17000026*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/
          Form()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/ System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_Form() /* 0600008A */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_Form(valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaForm/*01000028*/) /* 0600008B */
  } // end of property XmlArrayItemAttribute::Form
  .property /*17000027*/ instance callconv(8) bool
          IsNullable()
  {
    .get instance bool System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_IsNullable() /* 0600008C */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_IsNullable(bool) /* 0600008D */
  } // end of property XmlArrayItemAttribute::IsNullable
  .property /*17000028*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_Namespace() /* 0600008E */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_Namespace(string) /* 0600008F */
  } // end of property XmlArrayItemAttribute::Namespace
  .property /*17000029*/ instance callconv(8) int32
          NestingLevel()
  {
    .get instance int32 System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_NestingLevel() /* 06000090 */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_NestingLevel(int32) /* 06000091 */
  } // end of property XmlArrayItemAttribute::NestingLevel
  .property /*1700002A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001B*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::get_Type() /* 06000092 */
    .set instance void System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/::set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 06000093 */
  } // end of property XmlArrayItemAttribute::Type
} // end of class System.Xml.Serialization.XmlArrayItemAttribute

.class /*0200001B*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlArrayItemAttributes
       extends [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/
{
  .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000062:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000094*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22a2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttributes::.ctor

  .method /*06000095*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ 
          get_Item(int32 index) cil managed
  // SIG: 20 01 12 68 08
  {
    // Method begins at RVA 0x22aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttributes::get_Item

  .method /*06000096*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ 'value') cil managed
  // SIG: 20 02 01 08 12 68
  {
    // Method begins at RVA 0x22ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttributes::set_Item

  .method /*06000097*/ public hidebysig instance int32 
          Add(class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ attribute) cil managed
  // SIG: 20 01 08 12 68
  {
    // Method begins at RVA 0x22af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttributes::Add

  .method /*06000098*/ public hidebysig instance bool 
          Contains(class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ attribute) cil managed
  // SIG: 20 01 02 12 68
  {
    // Method begins at RVA 0x22b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttributes::Contains

  .method /*06000099*/ public hidebysig instance void 
          CopyTo(class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 68 08
  {
    .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22b5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttributes::CopyTo

  .method /*0600009A*/ public hidebysig instance int32 
          IndexOf(class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ attribute) cil managed
  // SIG: 20 01 08 12 68
  {
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlArrayItemAttributes::IndexOf

  .method /*0600009B*/ public hidebysig instance void 
          Insert(int32 index,
                 class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ attribute) cil managed
  // SIG: 20 02 01 08 12 68
  {
    // Method begins at RVA 0x22ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttributes::Insert

  .method /*0600009C*/ public hidebysig instance void 
          Remove(class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ attribute) cil managed
  // SIG: 20 01 01 12 68
  {
    // Method begins at RVA 0x22bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlArrayItemAttributes::Remove

  .property /*1700002B*/ instance callconv(8) class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/
          Item(int32)
  {
    .get instance class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/ System.Xml.Serialization.XmlArrayItemAttributes/*0200001B*/::get_Item(int32) /* 06000095 */
    .set instance void System.Xml.Serialization.XmlArrayItemAttributes/*0200001B*/::set_Item(int32,
                                                                                             class System.Xml.Serialization.XmlArrayItemAttribute/*0200001A*/) /* 06000096 */
  } // end of property XmlArrayItemAttributes::Item
} // end of class System.Xml.Serialization.XmlArrayItemAttributes

.class /*0200001C*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlAttributeEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000022*/
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600009D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22be
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000022*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlAttributeEventArgs::.ctor

  .method /*0600009E*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlAttribute/*01000029*/ 
          get_Attr() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeEventArgs::get_Attr

  .method /*0600009F*/ public hidebysig specialname 
          instance string  get_ExpectedAttributes() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeEventArgs::get_ExpectedAttributes

  .method /*060000A0*/ public hidebysig specialname 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeEventArgs::get_LineNumber

  .method /*060000A1*/ public hidebysig specialname 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeEventArgs::get_LinePosition

  .method /*060000A2*/ public hidebysig specialname 
          instance object  get_ObjectBeingDeserialized() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0000F1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeEventArgs::get_ObjectBeingDeserialized

  .property /*1700002C*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlAttribute/*01000029*/
          Attr()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlAttribute/*01000029*/ System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/::get_Attr() /* 0600009E */
  } // end of property XmlAttributeEventArgs::Attr
  .property /*1700002D*/ instance callconv(8) string
          ExpectedAttributes()
  {
    .get instance string System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/::get_ExpectedAttributes() /* 0600009F */
  } // end of property XmlAttributeEventArgs::ExpectedAttributes
  .property /*1700002E*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/::get_LineNumber() /* 060000A0 */
  } // end of property XmlAttributeEventArgs::LineNumber
  .property /*1700002F*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/::get_LinePosition() /* 060000A1 */
  } // end of property XmlAttributeEventArgs::LinePosition
  .property /*17000030*/ instance callconv(8) object
          ObjectBeingDeserialized()
  {
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/::get_ObjectBeingDeserialized() /* 060000A2 */
  } // end of property XmlAttributeEventArgs::ObjectBeingDeserialized
} // end of class System.Xml.Serialization.XmlAttributeEventArgs

.class /*0200001D*/ public auto ansi sealed System.Xml.Serialization.XmlAttributeEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*060000A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlAttributeEventHandler::.ctor

  .method /*060000A4*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/ e) runtime managed
  // SIG: 20 02 01 1C 12 70
  {
    .custom /*0C0000F2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800008C*/ 
    .custom /*0C0000E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlAttributeEventHandler::Invoke

  .method /*060000A5*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object sender,
                      class System.Xml.Serialization.XmlAttributeEventArgs/*0200001C*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 91 1C 12 70 12 80 95 1C
  {
    .param [1]/*0800008E*/ 
    .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800008F*/ 
    .custom /*0C0000EC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlAttributeEventHandler::BeginInvoke

  .method /*060000A6*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method XmlAttributeEventHandler::EndInvoke

} // end of class System.Xml.Serialization.XmlAttributeEventHandler

.class /*0200001E*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlAttributeOverrides
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000069:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*060000A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlAttributeOverrides::.ctor

  .method /*060000A8*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlAttributes/*0200001F*/ 
          get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 7C 12 6D
  {
    .param [0]/*08000093*/ 
    .custom /*0C0000ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeOverrides::get_Item

  .method /*060000A9*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlAttributes/*0200001F*/ 
          get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                   string member) cil managed
  // SIG: 20 02 12 7C 12 6D 0E
  {
    .param [0]/*08000095*/ 
    .custom /*0C0000EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributeOverrides::get_Item

  .method /*060000AA*/ public hidebysig instance void 
          Add(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
              string member,
              class System.Xml.Serialization.XmlAttributes/*0200001F*/ attributes) cil managed
  // SIG: 20 03 01 12 6D 0E 12 7C
  {
    .param [3]/*0800009A*/ 
    .custom /*0C0000F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributeOverrides::Add

  .method /*060000AB*/ public hidebysig instance void 
          Add(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
              class System.Xml.Serialization.XmlAttributes/*0200001F*/ attributes) cil managed
  // SIG: 20 02 01 12 6D 12 7C
  {
    // Method begins at RVA 0x22e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributeOverrides::Add

  .property /*17000031*/ instance callconv(8) class System.Xml.Serialization.XmlAttributes/*0200001F*/
          Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/)
  {
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Serialization.XmlAttributes/*0200001F*/ System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/::get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 060000A8 */
  } // end of property XmlAttributeOverrides::Item
  .property /*17000032*/ instance callconv(8) class System.Xml.Serialization.XmlAttributes/*0200001F*/
          Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/,
               string)
  {
    .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Serialization.XmlAttributes/*0200001F*/ System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/::get_Item(class [System.Runtime/*23000001*/]System.Type/*0100001B*/,
                                                                                                                                                string) /* 060000A9 */
  } // end of property XmlAttributeOverrides::Item
} // end of class System.Xml.Serialization.XmlAttributeOverrides

.class /*0200001F*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlAttributes
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::.ctor

  .method /*060000AD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000020*/ provider) cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C0000F4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::.ctor

  .method /*060000AE*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyAttributeAttribute/*0100002A*/ 
          get_XmlAnyAttribute() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x22f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlAnyAttribute

  .method /*060000AF*/ public hidebysig specialname 
          instance void  set_XmlAnyAttribute(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyAttributeAttribute/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x22fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlAnyAttribute

  .method /*060000B0*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlAnyElementAttributes/*02000018*/ 
          get_XmlAnyElements() cil managed
  // SIG: 20 00 12 60
  {
    .custom /*0C0000F5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlAnyElements

  .method /*060000B1*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlArrayAttribute/*02000019*/ 
          get_XmlArray() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlArray

  .method /*060000B2*/ public hidebysig specialname 
          instance void  set_XmlArray(class System.Xml.Serialization.XmlArrayAttribute/*02000019*/ 'value') cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x2302
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlArray

  .method /*060000B3*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlArrayItemAttributes/*0200001B*/ 
          get_XmlArrayItems() cil managed
  // SIG: 20 00 12 6C
  {
    .custom /*0C0000F6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2304
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlArrayItems

  .method /*060000B4*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAttributeAttribute/*0100002B*/ 
          get_XmlAttribute() cil managed
  // SIG: 20 00 12 80 AD
  {
    // Method begins at RVA 0x2307
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlAttribute

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_XmlAttribute(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAttributeAttribute/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    // Method begins at RVA 0x230a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlAttribute

  .method /*060000B6*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlChoiceIdentifierAttribute/*02000020*/ 
          get_XmlChoiceIdentifier() cil managed
  // SIG: 20 00 12 80 80
  {
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlChoiceIdentifier

  .method /*060000B7*/ public hidebysig specialname 
          instance object  get_XmlDefaultValue() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x230f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlDefaultValue

  .method /*060000B8*/ public hidebysig specialname 
          instance void  set_XmlDefaultValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2312
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlDefaultValue

  .method /*060000B9*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlElementAttributes/*02000022*/ 
          get_XmlElements() cil managed
  // SIG: 20 00 12 80 88
  {
    .custom /*0C0000F7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlElements

  .method /*060000BA*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/ 
          get_XmlEnum() cil managed
  // SIG: 20 00 12 61
  {
    // Method begins at RVA 0x2317
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlEnum

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_XmlEnum(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/ 'value') cil managed
  // SIG: 20 01 01 12 61
  {
    // Method begins at RVA 0x231a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlEnum

  .method /*060000BC*/ public hidebysig specialname 
          instance bool  get_XmlIgnore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlIgnore

  .method /*060000BD*/ public hidebysig specialname 
          instance void  set_XmlIgnore(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x231f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlIgnore

  .method /*060000BE*/ public hidebysig specialname 
          instance bool  get_Xmlns() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_Xmlns

  .method /*060000BF*/ public hidebysig specialname 
          instance void  set_Xmlns(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2324
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_Xmlns

  .method /*060000C0*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ 
          get_XmlRoot() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x2326
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlRoot

  .method /*060000C1*/ public hidebysig specialname 
          instance void  set_XmlRoot(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x2329
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlRoot

  .method /*060000C2*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlTextAttribute/*0100002D*/ 
          get_XmlText() cil managed
  // SIG: 20 00 12 80 B5
  {
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlText

  .method /*060000C3*/ public hidebysig specialname 
          instance void  set_XmlText(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlTextAttribute/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B5
  {
    // Method begins at RVA 0x232e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlText

  .method /*060000C4*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlTypeAttribute/*0200003E*/ 
          get_XmlType() cil managed
  // SIG: 20 00 12 80 F8
  {
    // Method begins at RVA 0x2330
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlAttributes::get_XmlType

  .method /*060000C5*/ public hidebysig specialname 
          instance void  set_XmlType(class System.Xml.Serialization.XmlTypeAttribute/*0200003E*/ 'value') cil managed
  // SIG: 20 01 01 12 80 F8
  {
    // Method begins at RVA 0x2333
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlAttributes::set_XmlType

  .property /*17000033*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyAttributeAttribute/*0100002A*/
          XmlAnyAttribute()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyAttributeAttribute/*0100002A*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlAnyAttribute() /* 060000AE */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlAnyAttribute(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAnyAttributeAttribute/*0100002A*/) /* 060000AF */
  } // end of property XmlAttributes::XmlAnyAttribute
  .property /*17000034*/ instance callconv(8) class System.Xml.Serialization.XmlAnyElementAttributes/*02000018*/
          XmlAnyElements()
  {
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class System.Xml.Serialization.XmlAnyElementAttributes/*02000018*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlAnyElements() /* 060000B0 */
  } // end of property XmlAttributes::XmlAnyElements
  .property /*17000035*/ instance callconv(8) class System.Xml.Serialization.XmlArrayAttribute/*02000019*/
          XmlArray()
  {
    .get instance class System.Xml.Serialization.XmlArrayAttribute/*02000019*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlArray() /* 060000B1 */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlArray(class System.Xml.Serialization.XmlArrayAttribute/*02000019*/) /* 060000B2 */
  } // end of property XmlAttributes::XmlArray
  .property /*17000036*/ instance callconv(8) class System.Xml.Serialization.XmlArrayItemAttributes/*0200001B*/
          XmlArrayItems()
  {
    .custom /*0C0000A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class System.Xml.Serialization.XmlArrayItemAttributes/*0200001B*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlArrayItems() /* 060000B3 */
  } // end of property XmlAttributes::XmlArrayItems
  .property /*17000037*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAttributeAttribute/*0100002B*/
          XmlAttribute()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAttributeAttribute/*0100002B*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlAttribute() /* 060000B4 */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlAttribute(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlAttributeAttribute/*0100002B*/) /* 060000B5 */
  } // end of property XmlAttributes::XmlAttribute
  .property /*17000038*/ instance callconv(8) class System.Xml.Serialization.XmlChoiceIdentifierAttribute/*02000020*/
          XmlChoiceIdentifier()
  {
    .get instance class System.Xml.Serialization.XmlChoiceIdentifierAttribute/*02000020*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlChoiceIdentifier() /* 060000B6 */
  } // end of property XmlAttributes::XmlChoiceIdentifier
  .property /*17000039*/ instance callconv(8) object
          XmlDefaultValue()
  {
    .get instance object System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlDefaultValue() /* 060000B7 */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlDefaultValue(object) /* 060000B8 */
  } // end of property XmlAttributes::XmlDefaultValue
  .property /*1700003A*/ instance callconv(8) class System.Xml.Serialization.XmlElementAttributes/*02000022*/
          XmlElements()
  {
    .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class System.Xml.Serialization.XmlElementAttributes/*02000022*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlElements() /* 060000B9 */
  } // end of property XmlAttributes::XmlElements
  .property /*1700003B*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/
          XmlEnum()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlEnum() /* 060000BA */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlEnum(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlEnumAttribute/*01000018*/) /* 060000BB */
  } // end of property XmlAttributes::XmlEnum
  .property /*1700003C*/ instance callconv(8) bool
          XmlIgnore()
  {
    .get instance bool System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlIgnore() /* 060000BC */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlIgnore(bool) /* 060000BD */
  } // end of property XmlAttributes::XmlIgnore
  .property /*1700003D*/ instance callconv(8) bool
          Xmlns()
  {
    .get instance bool System.Xml.Serialization.XmlAttributes/*0200001F*/::get_Xmlns() /* 060000BE */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_Xmlns(bool) /* 060000BF */
  } // end of property XmlAttributes::Xmlns
  .property /*1700003E*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/
          XmlRoot()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlRoot() /* 060000C0 */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlRoot(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/) /* 060000C1 */
  } // end of property XmlAttributes::XmlRoot
  .property /*1700003F*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlTextAttribute/*0100002D*/
          XmlText()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlTextAttribute/*0100002D*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlText() /* 060000C2 */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlText(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlTextAttribute/*0100002D*/) /* 060000C3 */
  } // end of property XmlAttributes::XmlText
  .property /*17000040*/ instance callconv(8) class System.Xml.Serialization.XmlTypeAttribute/*0200003E*/
          XmlType()
  {
    .get instance class System.Xml.Serialization.XmlTypeAttribute/*0200003E*/ System.Xml.Serialization.XmlAttributes/*0200001F*/::get_XmlType() /* 060000C4 */
    .set instance void System.Xml.Serialization.XmlAttributes/*0200001F*/::set_XmlType(class System.Xml.Serialization.XmlTypeAttribute/*0200003E*/) /* 060000C5 */
  } // end of property XmlAttributes::XmlType
} // end of class System.Xml.Serialization.XmlAttributes

.class /*02000020*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlChoiceIdentifierAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00006F:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 80 29 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ...)....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*060000C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2335
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlChoiceIdentifierAttribute::.ctor

  .method /*060000C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x233d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlChoiceIdentifierAttribute::.ctor

  .method /*060000C8*/ public hidebysig specialname 
          instance string  get_MemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2345
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlChoiceIdentifierAttribute::get_MemberName

  .method /*060000C9*/ public hidebysig specialname 
          instance void  set_MemberName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*080000A9*/ 
    .custom /*0C0000F3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2348
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlChoiceIdentifierAttribute::set_MemberName

  .property /*17000041*/ instance callconv(8) string
          MemberName()
  {
    .get instance string System.Xml.Serialization.XmlChoiceIdentifierAttribute/*02000020*/::get_MemberName() /* 060000C8 */
    .set instance void System.Xml.Serialization.XmlChoiceIdentifierAttribute/*02000020*/::set_MemberName(string) /* 060000C9 */
  } // end of property XmlChoiceIdentifierAttribute::MemberName
} // end of class System.Xml.Serialization.XmlChoiceIdentifierAttribute

.class /*02000021*/ public sequential ansi sealed beforefieldinit System.Xml.Serialization.XmlDeserializationEvents
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002E*/
{
  .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*0400000B*/ private object _dummy
  .field /*0400000C*/ private int32 _dummyPrimitive
  .method /*060000CA*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/ 
          get_OnUnknownAttribute() cil managed
  // SIG: 20 00 12 74
  {
    // Method begins at RVA 0x234a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDeserializationEvents::get_OnUnknownAttribute

  .method /*060000CB*/ public hidebysig specialname 
          instance void  set_OnUnknownAttribute(class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/ 'value') cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x234d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDeserializationEvents::set_OnUnknownAttribute

  .method /*060000CC*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlElementEventHandler/*02000024*/ 
          get_OnUnknownElement() cil managed
  // SIG: 20 00 12 80 90
  {
    // Method begins at RVA 0x234f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDeserializationEvents::get_OnUnknownElement

  .method /*060000CD*/ public hidebysig specialname 
          instance void  set_OnUnknownElement(class System.Xml.Serialization.XmlElementEventHandler/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x2352
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDeserializationEvents::set_OnUnknownElement

  .method /*060000CE*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/ 
          get_OnUnknownNode() cil managed
  // SIG: 20 00 12 80 AC
  {
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDeserializationEvents::get_OnUnknownNode

  .method /*060000CF*/ public hidebysig specialname 
          instance void  set_OnUnknownNode(class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AC
  {
    // Method begins at RVA 0x2357
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDeserializationEvents::set_OnUnknownNode

  .method /*060000D0*/ public hidebysig specialname 
          instance class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/ 
          get_OnUnreferencedObject() cil managed
  // SIG: 20 00 12 5C
  {
    // Method begins at RVA 0x2359
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlDeserializationEvents::get_OnUnreferencedObject

  .method /*060000D1*/ public hidebysig specialname 
          instance void  set_OnUnreferencedObject(class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x235c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlDeserializationEvents::set_OnUnreferencedObject

  .property /*17000042*/ instance callconv(8) class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/
          OnUnknownAttribute()
  {
    .get instance class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/ System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::get_OnUnknownAttribute() /* 060000CA */
    .set instance void System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::set_OnUnknownAttribute(class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/) /* 060000CB */
  } // end of property XmlDeserializationEvents::OnUnknownAttribute
  .property /*17000043*/ instance callconv(8) class System.Xml.Serialization.XmlElementEventHandler/*02000024*/
          OnUnknownElement()
  {
    .get instance class System.Xml.Serialization.XmlElementEventHandler/*02000024*/ System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::get_OnUnknownElement() /* 060000CC */
    .set instance void System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::set_OnUnknownElement(class System.Xml.Serialization.XmlElementEventHandler/*02000024*/) /* 060000CD */
  } // end of property XmlDeserializationEvents::OnUnknownElement
  .property /*17000044*/ instance callconv(8) class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/
          OnUnknownNode()
  {
    .get instance class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/ System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::get_OnUnknownNode() /* 060000CE */
    .set instance void System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::set_OnUnknownNode(class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/) /* 060000CF */
  } // end of property XmlDeserializationEvents::OnUnknownNode
  .property /*17000045*/ instance callconv(8) class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/
          OnUnreferencedObject()
  {
    .get instance class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/ System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::get_OnUnreferencedObject() /* 060000D0 */
    .set instance void System.Xml.Serialization.XmlDeserializationEvents/*02000021*/::set_OnUnreferencedObject(class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/) /* 060000D1 */
  } // end of property XmlDeserializationEvents::OnUnreferencedObject
} // end of class System.Xml.Serialization.XmlDeserializationEvents

.class /*02000022*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlElementAttributes
       extends [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/
{
  .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000075:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*060000D2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x235e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlElementAttributes::.ctor

  .method /*060000D3*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ 
          get_Item(int32 index) cil managed
  // SIG: 20 01 12 80 BD 08
  {
    // Method begins at RVA 0x2366
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementAttributes::get_Item

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ 'value') cil managed
  // SIG: 20 02 01 08 12 80 BD
  {
    // Method begins at RVA 0x2369
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlElementAttributes::set_Item

  .method /*060000D5*/ public hidebysig instance int32 
          Add(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ attribute) cil managed
  // SIG: 20 01 08 12 80 BD
  {
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementAttributes::Add

  .method /*060000D6*/ public hidebysig instance bool 
          Contains(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ attribute) cil managed
  // SIG: 20 01 02 12 80 BD
  {
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementAttributes::Contains

  .method /*060000D7*/ public hidebysig instance void 
          CopyTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 80 BD 08
  {
    .custom /*0C000101:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2371
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlElementAttributes::CopyTo

  .method /*060000D8*/ public hidebysig instance int32 
          IndexOf(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ attribute) cil managed
  // SIG: 20 01 08 12 80 BD
  {
    // Method begins at RVA 0x2373
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementAttributes::IndexOf

  .method /*060000D9*/ public hidebysig instance void 
          Insert(int32 index,
                 class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ attribute) cil managed
  // SIG: 20 02 01 08 12 80 BD
  {
    // Method begins at RVA 0x2376
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlElementAttributes::Insert

  .method /*060000DA*/ public hidebysig instance void 
          Remove(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ attribute) cil managed
  // SIG: 20 01 01 12 80 BD
  {
    // Method begins at RVA 0x2378
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlElementAttributes::Remove

  .property /*17000046*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/
          Item(int32)
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/ System.Xml.Serialization.XmlElementAttributes/*02000022*/::get_Item(int32) /* 060000D3 */
    .set instance void System.Xml.Serialization.XmlElementAttributes/*02000022*/::set_Item(int32,
                                                                                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlElementAttribute/*0100002F*/) /* 060000D4 */
  } // end of property XmlElementAttributes::Item
} // end of class System.Xml.Serialization.XmlElementAttributes

.class /*02000023*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlElementEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000022*/
{
  .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000DB*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x237a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000022*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlElementEventArgs::.ctor

  .method /*060000DC*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlElement/*01000030*/ 
          get_Element() cil managed
  // SIG: 20 00 12 80 C1
  {
    // Method begins at RVA 0x2382
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementEventArgs::get_Element

  .method /*060000DD*/ public hidebysig specialname 
          instance string  get_ExpectedElements() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementEventArgs::get_ExpectedElements

  .method /*060000DE*/ public hidebysig specialname 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementEventArgs::get_LineNumber

  .method /*060000DF*/ public hidebysig specialname 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementEventArgs::get_LinePosition

  .method /*060000E0*/ public hidebysig specialname 
          instance object  get_ObjectBeingDeserialized() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000106:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlElementEventArgs::get_ObjectBeingDeserialized

  .property /*17000047*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlElement/*01000030*/
          Element()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlElement/*01000030*/ System.Xml.Serialization.XmlElementEventArgs/*02000023*/::get_Element() /* 060000DC */
  } // end of property XmlElementEventArgs::Element
  .property /*17000048*/ instance callconv(8) string
          ExpectedElements()
  {
    .get instance string System.Xml.Serialization.XmlElementEventArgs/*02000023*/::get_ExpectedElements() /* 060000DD */
  } // end of property XmlElementEventArgs::ExpectedElements
  .property /*17000049*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xml.Serialization.XmlElementEventArgs/*02000023*/::get_LineNumber() /* 060000DE */
  } // end of property XmlElementEventArgs::LineNumber
  .property /*1700004A*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xml.Serialization.XmlElementEventArgs/*02000023*/::get_LinePosition() /* 060000DF */
  } // end of property XmlElementEventArgs::LinePosition
  .property /*1700004B*/ instance callconv(8) object
          ObjectBeingDeserialized()
  {
    .custom /*0C0000C2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Xml.Serialization.XmlElementEventArgs/*02000023*/::get_ObjectBeingDeserialized() /* 060000E0 */
  } // end of property XmlElementEventArgs::ObjectBeingDeserialized
} // end of class System.Xml.Serialization.XmlElementEventArgs

.class /*02000024*/ public auto ansi sealed System.Xml.Serialization.XmlElementEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*060000E1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlElementEventHandler::.ctor

  .method /*060000E2*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Xml.Serialization.XmlElementEventArgs/*02000023*/ e) runtime managed
  // SIG: 20 02 01 1C 12 80 8C
  {
    .custom /*0C000107:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000BB*/ 
    .custom /*0C0000F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlElementEventHandler::Invoke

  .method /*060000E3*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object sender,
                      class System.Xml.Serialization.XmlElementEventArgs/*02000023*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 91 1C 12 80 8C 12 80 95 1C
  {
    .param [1]/*080000BD*/ 
    .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000BE*/ 
    .custom /*0C0000FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlElementEventHandler::BeginInvoke

  .method /*060000E4*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method XmlElementEventHandler::EndInvoke

} // end of class System.Xml.Serialization.XmlElementEventHandler

.class /*02000025*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlIncludeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00007C:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 04 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*060000E5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2391
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlIncludeAttribute::.ctor

  .method /*060000E6*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 
          get_Type() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x2399
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlIncludeAttribute::get_Type

  .method /*060000E7*/ public hidebysig specialname 
          instance void  set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x239c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlIncludeAttribute::set_Type

  .property /*1700004C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001B*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ System.Xml.Serialization.XmlIncludeAttribute/*02000025*/::get_Type() /* 060000E6 */
    .set instance void System.Xml.Serialization.XmlIncludeAttribute/*02000025*/::set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 060000E7 */
  } // end of property XmlIncludeAttribute::Type
} // end of class System.Xml.Serialization.XmlIncludeAttribute

.class /*02000026*/ public abstract auto ansi beforefieldinit System.Xml.Serialization.XmlMapping
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000E8*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x239e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlMapping::.ctor

  .method /*060000E9*/ public hidebysig specialname 
          instance string  get_ElementName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMapping::get_ElementName

  .method /*060000EA*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00010A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMapping::get_Namespace

  .method /*060000EB*/ public hidebysig specialname 
          instance string  get_XsdElementName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMapping::get_XsdElementName

  .method /*060000EC*/ public hidebysig instance void 
          SetKey(string key) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00010C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlMapping::SetKey

  .property /*1700004D*/ instance callconv(8) string
          ElementName()
  {
    .get instance string System.Xml.Serialization.XmlMapping/*02000026*/::get_ElementName() /* 060000E9 */
  } // end of property XmlMapping::ElementName
  .property /*1700004E*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C0000C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlMapping/*02000026*/::get_Namespace() /* 060000EA */
  } // end of property XmlMapping::Namespace
  .property /*1700004F*/ instance callconv(8) string
          XsdElementName()
  {
    .get instance string System.Xml.Serialization.XmlMapping/*02000026*/::get_XsdElementName() /* 060000EB */
  } // end of property XmlMapping::XsdElementName
} // end of class System.Xml.Serialization.XmlMapping

.class /*02000027*/ public auto ansi sealed System.Xml.Serialization.XmlMappingAccess
       extends [System.Runtime/*23000001*/]System.Enum/*01000016*/
{
  .custom /*0C000081:0A000012*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ public specialname rtspecialname int32 value__
  .field /*0400000E*/ public static literal valuetype System.Xml.Serialization.XmlMappingAccess/*02000027*/ None = int32(0x00000000)
  .field /*0400000F*/ public static literal valuetype System.Xml.Serialization.XmlMappingAccess/*02000027*/ Read = int32(0x00000001)
  .field /*04000010*/ public static literal valuetype System.Xml.Serialization.XmlMappingAccess/*02000027*/ Write = int32(0x00000002)
} // end of class System.Xml.Serialization.XmlMappingAccess

.class /*02000028*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlMemberMapping
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000ED*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23b1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlMemberMapping::.ctor

  .method /*060000EE*/ public hidebysig specialname 
          instance bool  get_Any() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_Any

  .method /*060000EF*/ public hidebysig specialname 
          instance bool  get_CheckSpecified() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_CheckSpecified

  .method /*060000F0*/ public hidebysig specialname 
          instance string  get_ElementName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_ElementName

  .method /*060000F1*/ public hidebysig specialname 
          instance string  get_MemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_MemberName

  .method /*060000F2*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000110:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_Namespace

  .method /*060000F3*/ public hidebysig specialname 
          instance string  get_TypeFullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_TypeFullName

  .method /*060000F4*/ public hidebysig specialname 
          instance string  get_TypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_TypeName

  .method /*060000F5*/ public hidebysig specialname 
          instance string  get_TypeNamespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000112:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_TypeNamespace

  .method /*060000F6*/ public hidebysig specialname 
          instance string  get_XsdElementName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMemberMapping::get_XsdElementName

  .property /*17000050*/ instance callconv(8) bool
          Any()
  {
    .get instance bool System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_Any() /* 060000EE */
  } // end of property XmlMemberMapping::Any
  .property /*17000051*/ instance callconv(8) bool
          CheckSpecified()
  {
    .get instance bool System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_CheckSpecified() /* 060000EF */
  } // end of property XmlMemberMapping::CheckSpecified
  .property /*17000052*/ instance callconv(8) string
          ElementName()
  {
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_ElementName() /* 060000F0 */
  } // end of property XmlMemberMapping::ElementName
  .property /*17000053*/ instance callconv(8) string
          MemberName()
  {
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_MemberName() /* 060000F1 */
  } // end of property XmlMemberMapping::MemberName
  .property /*17000054*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C0000C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_Namespace() /* 060000F2 */
  } // end of property XmlMemberMapping::Namespace
  .property /*17000055*/ instance callconv(8) string
          TypeFullName()
  {
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_TypeFullName() /* 060000F3 */
  } // end of property XmlMemberMapping::TypeFullName
  .property /*17000056*/ instance callconv(8) string
          TypeName()
  {
    .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_TypeName() /* 060000F4 */
  } // end of property XmlMemberMapping::TypeName
  .property /*17000057*/ instance callconv(8) string
          TypeNamespace()
  {
    .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_TypeNamespace() /* 060000F5 */
  } // end of property XmlMemberMapping::TypeNamespace
  .property /*17000058*/ instance callconv(8) string
          XsdElementName()
  {
    .get instance string System.Xml.Serialization.XmlMemberMapping/*02000028*/::get_XsdElementName() /* 060000F6 */
  } // end of property XmlMemberMapping::XsdElementName
} // end of class System.Xml.Serialization.XmlMemberMapping

.class /*02000029*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlMembersMapping
       extends System.Xml.Serialization.XmlMapping/*02000026*/
{
  .custom /*0C000085:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000087:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*060000F7*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000E8       */ call       instance void System.Xml.Serialization.XmlMapping/*02000026*/::.ctor() /* 060000E8 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlMembersMapping::.ctor

  .method /*060000F8*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMembersMapping::get_Count

  .method /*060000F9*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlMemberMapping/*02000028*/ 
          get_Item(int32 index) cil managed
  // SIG: 20 01 12 80 A0 08
  {
    .custom /*0C000113:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMembersMapping::get_Item

  .method /*060000FA*/ public hidebysig specialname 
          instance string  get_TypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMembersMapping::get_TypeName

  .method /*060000FB*/ public hidebysig specialname 
          instance string  get_TypeNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlMembersMapping::get_TypeNamespace

  .property /*17000059*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Xml.Serialization.XmlMembersMapping/*02000029*/::get_Count() /* 060000F8 */
  } // end of property XmlMembersMapping::Count
  .property /*1700005A*/ instance callconv(8) class System.Xml.Serialization.XmlMemberMapping/*02000028*/
          Item(int32)
  {
    .custom /*0C0000CD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class System.Xml.Serialization.XmlMemberMapping/*02000028*/ System.Xml.Serialization.XmlMembersMapping/*02000029*/::get_Item(int32) /* 060000F9 */
  } // end of property XmlMembersMapping::Item
  .property /*1700005B*/ instance callconv(8) string
          TypeName()
  {
    .get instance string System.Xml.Serialization.XmlMembersMapping/*02000029*/::get_TypeName() /* 060000FA */
  } // end of property XmlMembersMapping::TypeName
  .property /*1700005C*/ instance callconv(8) string
          TypeNamespace()
  {
    .get instance string System.Xml.Serialization.XmlMembersMapping/*02000029*/::get_TypeNamespace() /* 060000FB */
  } // end of property XmlMembersMapping::TypeNamespace
} // end of class System.Xml.Serialization.XmlMembersMapping

.class /*0200002A*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlNodeEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000022*/
{
  .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000FC*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23e8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000022*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlNodeEventArgs::.ctor

  .method /*060000FD*/ public hidebysig specialname 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_LineNumber

  .method /*060000FE*/ public hidebysig specialname 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_LinePosition

  .method /*060000FF*/ public hidebysig specialname 
          instance string  get_LocalName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_LocalName

  .method /*06000100*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_Name

  .method /*06000101*/ public hidebysig specialname 
          instance string  get_NamespaceURI() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_NamespaceURI

  .method /*06000102*/ public hidebysig specialname 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*01000031*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 C5
  {
    // Method begins at RVA 0x23ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_NodeType

  .method /*06000103*/ public hidebysig specialname 
          instance object  get_ObjectBeingDeserialized() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000117:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2402
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_ObjectBeingDeserialized

  .method /*06000104*/ public hidebysig specialname 
          instance string  get_Text() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2405
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlNodeEventArgs::get_Text

  .property /*1700005D*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_LineNumber() /* 060000FD */
  } // end of property XmlNodeEventArgs::LineNumber
  .property /*1700005E*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_LinePosition() /* 060000FE */
  } // end of property XmlNodeEventArgs::LinePosition
  .property /*1700005F*/ instance callconv(8) string
          LocalName()
  {
    .get instance string System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_LocalName() /* 060000FF */
  } // end of property XmlNodeEventArgs::LocalName
  .property /*17000060*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_Name() /* 06000100 */
  } // end of property XmlNodeEventArgs::Name
  .property /*17000061*/ instance callconv(8) string
          NamespaceURI()
  {
    .get instance string System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_NamespaceURI() /* 06000101 */
  } // end of property XmlNodeEventArgs::NamespaceURI
  .property /*17000062*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*01000031*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*01000031*/ System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_NodeType() /* 06000102 */
  } // end of property XmlNodeEventArgs::NodeType
  .property /*17000063*/ instance callconv(8) object
          ObjectBeingDeserialized()
  {
    .custom /*0C0000D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_ObjectBeingDeserialized() /* 06000103 */
  } // end of property XmlNodeEventArgs::ObjectBeingDeserialized
  .property /*17000064*/ instance callconv(8) string
          Text()
  {
    .custom /*0C0000D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/::get_Text() /* 06000104 */
  } // end of property XmlNodeEventArgs::Text
} // end of class System.Xml.Serialization.XmlNodeEventArgs

.class /*0200002B*/ public auto ansi sealed System.Xml.Serialization.XmlNodeEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*06000105*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlNodeEventHandler::.ctor

  .method /*06000106*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/ e) runtime managed
  // SIG: 20 02 01 1C 12 80 A8
  {
    .custom /*0C000119:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000C8*/ 
    .custom /*0C0000FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlNodeEventHandler::Invoke

  .method /*06000107*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object sender,
                      class System.Xml.Serialization.XmlNodeEventArgs/*0200002A*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 91 1C 12 80 A8 12 80 95 1C
  {
    .param [1]/*080000CA*/ 
    .custom /*0C0000FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000CB*/ 
    .custom /*0C0000FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlNodeEventHandler::BeginInvoke

  .method /*06000108*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method XmlNodeEventHandler::EndInvoke

} // end of class System.Xml.Serialization.XmlNodeEventHandler

.class /*0200002C*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlReflectionImporter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000109*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2408
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlReflectionImporter::.ctor

  .method /*0600010A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string defaultNamespace) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2410
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlReflectionImporter::.ctor

  .method /*0600010B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ attributeOverrides) cil managed
  // SIG: 20 01 01 12 78
  {
    .custom /*0C00011D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2418
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlReflectionImporter::.ctor

  .method /*0600010C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ attributeOverrides,
                               string defaultNamespace) cil managed
  // SIG: 20 02 01 12 78 0E
  {
    .custom /*0C00011F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2420
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlReflectionImporter::.ctor

  .method /*0600010D*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement) cil managed
  // SIG: 20 04 12 80 A4 0E 0E 1D 12 80 B4 02
  {
    .param [1]/*080000D3*/ 
    .custom /*0C0000FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000D4*/ 
    .custom /*0C000100:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2428
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportMembersMapping

  .method /*0600010E*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement,
                               bool rpc) cil managed
  // SIG: 20 05 12 80 A4 0E 0E 1D 12 80 B4 02 02
  {
    .param [1]/*080000D7*/ 
    .custom /*0C000102:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000D8*/ 
    .custom /*0C000103:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x242b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportMembersMapping

  .method /*0600010F*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement,
                               bool rpc,
                               bool openModel) cil managed
  // SIG: 20 06 12 80 A4 0E 0E 1D 12 80 B4 02 02 02
  {
    .param [1]/*080000DC*/ 
    .custom /*0C000104:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000DD*/ 
    .custom /*0C000105:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x242e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportMembersMapping

  .method /*06000110*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string elementName,
                               string ns,
                               class System.Xml.Serialization.XmlReflectionMember/*0200002D*/[] members,
                               bool hasWrapperElement,
                               bool rpc,
                               bool openModel,
                               valuetype System.Xml.Serialization.XmlMappingAccess/*02000027*/ access) cil managed
  // SIG: 20 07 12 80 A4 0E 0E 1D 12 80 B4 02 02 02 11 80 9C
  {
    .param [1]/*080000E2*/ 
    .custom /*0C000108:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000E3*/ 
    .custom /*0C000109:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2431
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportMembersMapping

  .method /*06000111*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 80 FC 12 6D
  {
    // Method begins at RVA 0x2434
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportTypeMapping

  .method /*06000112*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                            string defaultNamespace) cil managed
  // SIG: 20 02 12 80 FC 12 6D 0E
  {
    .param [2]/*080000EB*/ 
    .custom /*0C00010B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2437
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportTypeMapping

  .method /*06000113*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                            class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root) cil managed
  // SIG: 20 02 12 80 FC 12 6D 12 80 B1
  {
    .param [2]/*080000ED*/ 
    .custom /*0C00010D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x243a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportTypeMapping

  .method /*06000114*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                            class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root,
                            string defaultNamespace) cil managed
  // SIG: 20 03 12 80 FC 12 6D 12 80 B1 0E
  {
    .param [2]/*080000EF*/ 
    .custom /*0C00010E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000F0*/ 
    .custom /*0C00010F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x243d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionImporter::ImportTypeMapping

  .method /*06000115*/ public hidebysig instance void 
          IncludeType(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2440
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionImporter::IncludeType

  .method /*06000116*/ public hidebysig instance void 
          IncludeTypes(class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000020*/ provider) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x2442
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionImporter::IncludeTypes

} // end of class System.Xml.Serialization.XmlReflectionImporter

.class /*0200002D*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlReflectionMember
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000117*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2444
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::.ctor

  .method /*06000118*/ public hidebysig specialname 
          instance bool  get_IsReturnValue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionMember::get_IsReturnValue

  .method /*06000119*/ public hidebysig specialname 
          instance void  set_IsReturnValue(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x244f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::set_IsReturnValue

  .method /*0600011A*/ public hidebysig specialname 
          instance string  get_MemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2451
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionMember::get_MemberName

  .method /*0600011B*/ public hidebysig specialname 
          instance void  set_MemberName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2454
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::set_MemberName

  .method /*0600011C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 
          get_MemberType() cil managed
  // SIG: 20 00 12 6D
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2456
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionMember::get_MemberType

  .method /*0600011D*/ public hidebysig specialname 
          instance void  set_MemberType(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6D
  {
    .custom /*0C000126:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2459
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::set_MemberType

  .method /*0600011E*/ public hidebysig specialname 
          instance bool  get_OverrideIsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x245b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionMember::get_OverrideIsNullable

  .method /*0600011F*/ public hidebysig specialname 
          instance void  set_OverrideIsNullable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x245e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::set_OverrideIsNullable

  .method /*06000120*/ public hidebysig specialname 
          instance class System.Xml.Serialization.SoapAttributes/*0200000E*/ 
          get_SoapAttributes() cil managed
  // SIG: 20 00 12 38
  {
    // Method begins at RVA 0x2460
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionMember::get_SoapAttributes

  .method /*06000121*/ public hidebysig specialname 
          instance void  set_SoapAttributes(class System.Xml.Serialization.SoapAttributes/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x2463
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::set_SoapAttributes

  .method /*06000122*/ public hidebysig specialname 
          instance class System.Xml.Serialization.XmlAttributes/*0200001F*/ 
          get_XmlAttributes() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2465
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlReflectionMember::get_XmlAttributes

  .method /*06000123*/ public hidebysig specialname 
          instance void  set_XmlAttributes(class System.Xml.Serialization.XmlAttributes/*0200001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7C
  {
    // Method begins at RVA 0x2468
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlReflectionMember::set_XmlAttributes

  .property /*17000065*/ instance callconv(8) bool
          IsReturnValue()
  {
    .get instance bool System.Xml.Serialization.XmlReflectionMember/*0200002D*/::get_IsReturnValue() /* 06000118 */
    .set instance void System.Xml.Serialization.XmlReflectionMember/*0200002D*/::set_IsReturnValue(bool) /* 06000119 */
  } // end of property XmlReflectionMember::IsReturnValue
  .property /*17000066*/ instance callconv(8) string
          MemberName()
  {
    .get instance string System.Xml.Serialization.XmlReflectionMember/*0200002D*/::get_MemberName() /* 0600011A */
    .set instance void System.Xml.Serialization.XmlReflectionMember/*0200002D*/::set_MemberName(string) /* 0600011B */
  } // end of property XmlReflectionMember::MemberName
  .property /*17000067*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001B*/
          MemberType()
  {
    .custom /*0C0000D7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ System.Xml.Serialization.XmlReflectionMember/*0200002D*/::get_MemberType() /* 0600011C */
    .set instance void System.Xml.Serialization.XmlReflectionMember/*0200002D*/::set_MemberType(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0600011D */
  } // end of property XmlReflectionMember::MemberType
  .property /*17000068*/ instance callconv(8) bool
          OverrideIsNullable()
  {
    .get instance bool System.Xml.Serialization.XmlReflectionMember/*0200002D*/::get_OverrideIsNullable() /* 0600011E */
    .set instance void System.Xml.Serialization.XmlReflectionMember/*0200002D*/::set_OverrideIsNullable(bool) /* 0600011F */
  } // end of property XmlReflectionMember::OverrideIsNullable
  .property /*17000069*/ instance callconv(8) class System.Xml.Serialization.SoapAttributes/*0200000E*/
          SoapAttributes()
  {
    .get instance class System.Xml.Serialization.SoapAttributes/*0200000E*/ System.Xml.Serialization.XmlReflectionMember/*0200002D*/::get_SoapAttributes() /* 06000120 */
    .set instance void System.Xml.Serialization.XmlReflectionMember/*0200002D*/::set_SoapAttributes(class System.Xml.Serialization.SoapAttributes/*0200000E*/) /* 06000121 */
  } // end of property XmlReflectionMember::SoapAttributes
  .property /*1700006A*/ instance callconv(8) class System.Xml.Serialization.XmlAttributes/*0200001F*/
          XmlAttributes()
  {
    .get instance class System.Xml.Serialization.XmlAttributes/*0200001F*/ System.Xml.Serialization.XmlReflectionMember/*0200002D*/::get_XmlAttributes() /* 06000122 */
    .set instance void System.Xml.Serialization.XmlReflectionMember/*0200002D*/::set_XmlAttributes(class System.Xml.Serialization.XmlAttributes/*0200001F*/) /* 06000123 */
  } // end of property XmlReflectionMember::XmlAttributes
} // end of class System.Xml.Serialization.XmlReflectionMember

.class /*0200002E*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlSchemaEnumerator
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/>/*1B000001*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000032*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000035*/
{
  .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/>/*1B000001*/
  .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000032*/
  .custom /*0C000017:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000035*/
  .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000124*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlSchemas/*02000031*/ list) cil managed
  // SIG: 20 01 01 12 80 C4
  {
    // Method begins at RVA 0x246a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSchemaEnumerator::.ctor

  .method /*06000125*/ public hidebysig newslot specialname virtual final 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ 
          get_Current() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaEnumerator::get_Current

  .method /*06000126*/ private hidebysig newslot specialname virtual final 
          instance object  System.Collections.IEnumerator.get_Current() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000032*/::get_Current /*01000032::0A000018*/ 
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaEnumerator::System.Collections.IEnumerator.get_Current

  .method /*06000127*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2478
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemaEnumerator::Dispose

  .method /*06000128*/ public hidebysig newslot virtual final 
          instance bool  MoveNext() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x247a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaEnumerator::MoveNext

  .method /*06000129*/ private hidebysig newslot virtual final 
          instance void  System.Collections.IEnumerator.Reset() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000032*/::Reset /*01000032::0A000019*/ 
    // Method begins at RVA 0x247d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemaEnumerator::System.Collections.IEnumerator.Reset

  .property /*1700006B*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/
          Current()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ System.Xml.Serialization.XmlSchemaEnumerator/*0200002E*/::get_Current() /* 06000125 */
  } // end of property XmlSchemaEnumerator::Current
  .property /*1700006C*/ instance callconv(8) object
          System.Collections.IEnumerator.Current()
  {
    .get instance object System.Xml.Serialization.XmlSchemaEnumerator/*0200002E*/::System.Collections.IEnumerator.get_Current() /* 06000126 */
  } // end of property XmlSchemaEnumerator::System.Collections.IEnumerator.Current
} // end of class System.Xml.Serialization.XmlSchemaEnumerator

.class /*0200002F*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlSchemaExporter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600012A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlSchemas/*02000031*/ schemas) cil managed
  // SIG: 20 01 01 12 80 C4
  {
    // Method begins at RVA 0x247f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSchemaExporter::.ctor

  .method /*0600012B*/ public hidebysig instance string 
          ExportAnyType(string ns) cil managed
  // SIG: 20 01 0E 0E
  {
    .param [1]/*080000FB*/ 
    .custom /*0C000114:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2487
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaExporter::ExportAnyType

  .method /*0600012C*/ public hidebysig instance string 
          ExportAnyType(class System.Xml.Serialization.XmlMembersMapping/*02000029*/ members) cil managed
  // SIG: 20 01 0E 12 80 A4
  {
    .param [0]/*080000FC*/ 
    .custom /*0C000115:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x248a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaExporter::ExportAnyType

  .method /*0600012D*/ public hidebysig instance void 
          ExportMembersMapping(class System.Xml.Serialization.XmlMembersMapping/*02000029*/ xmlMembersMapping) cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x248d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemaExporter::ExportMembersMapping

  .method /*0600012E*/ public hidebysig instance void 
          ExportMembersMapping(class System.Xml.Serialization.XmlMembersMapping/*02000029*/ xmlMembersMapping,
                               bool exportEnclosingType) cil managed
  // SIG: 20 02 01 12 80 A4 02
  {
    // Method begins at RVA 0x248f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemaExporter::ExportMembersMapping

  .method /*0600012F*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 
          ExportTypeMapping(class System.Xml.Serialization.XmlMembersMapping/*02000029*/ xmlMembersMapping) cil managed
  // SIG: 20 01 12 80 85 12 80 A4
  {
    .param [0]/*08000101*/ 
    .custom /*0C000116:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2491
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaExporter::ExportTypeMapping

  .method /*06000130*/ public hidebysig instance void 
          ExportTypeMapping(class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ xmlTypeMapping) cil managed
  // SIG: 20 01 01 12 80 FC
  {
    // Method begins at RVA 0x2494
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemaExporter::ExportTypeMapping

} // end of class System.Xml.Serialization.XmlSchemaExporter

.class /*02000030*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlSchemaImporter
       extends System.Xml.Serialization.SchemaImporter/*0200000B*/
{
  .custom /*0C000096:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlSchemas/*02000031*/ schemas) cil managed
  // SIG: 20 01 01 12 80 C4
  {
    // Method begins at RVA 0x2496
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000020       */ call       instance void System.Xml.Serialization.SchemaImporter/*0200000B*/::.ctor() /* 06000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSchemaImporter::.ctor

  .method /*06000132*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlSchemas/*02000031*/ schemas,
                               class System.Xml.Serialization.CodeIdentifiers/*02000008*/ typeIdentifiers) cil managed
  // SIG: 20 02 01 12 80 C4 12 20
  {
    .param [2]/*08000106*/ 
    .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x249e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000020       */ call       instance void System.Xml.Serialization.SchemaImporter/*0200000B*/::.ctor() /* 06000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSchemaImporter::.ctor

  .method /*06000133*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportAnyType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ typeName,
                        string elementName) cil managed
  // SIG: 20 02 12 80 A4 12 80 85 0E
  {
    .param [0]/*08000107*/ 
    .custom /*0C00011B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportAnyType

  .method /*06000134*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportDerivedTypeMapping(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ name,
                                   class [System.Runtime/*23000001*/]System.Type/*0100001B*/ baseType) cil managed
  // SIG: 20 02 12 80 FC 12 80 85 12 6D
  {
    .param [2]/*0800010B*/ 
    .custom /*0C00011E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportDerivedTypeMapping

  .method /*06000135*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportDerivedTypeMapping(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ name,
                                   class [System.Runtime/*23000001*/]System.Type/*0100001B*/ baseType,
                                   bool baseTypeCanBeIndirect) cil managed
  // SIG: 20 03 12 80 FC 12 80 85 12 6D 02
  {
    .param [2]/*0800010D*/ 
    .custom /*0C000120:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportDerivedTypeMapping

  .method /*06000136*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(string name,
                               string ns,
                               class System.Xml.Serialization.SoapSchemaMember/*02000014*/[] members) cil managed
  // SIG: 20 03 12 80 A4 0E 0E 1D 12 50
  {
    .param [2]/*08000110*/ 
    .custom /*0C000121:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportMembersMapping

  .method /*06000137*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ name) cil managed
  // SIG: 20 01 12 80 A4 12 80 85
  {
    // Method begins at RVA 0x24b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportMembersMapping

  .method /*06000138*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/[] names) cil managed
  // SIG: 20 01 12 80 A4 1D 12 80 85
  {
    // Method begins at RVA 0x24b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportMembersMapping

  .method /*06000139*/ public hidebysig instance class System.Xml.Serialization.XmlMembersMapping/*02000029*/ 
          ImportMembersMapping(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/[] names,
                               class [System.Runtime/*23000001*/]System.Type/*0100001B*/ baseType,
                               bool baseTypeCanBeIndirect) cil managed
  // SIG: 20 03 12 80 A4 1D 12 80 85 12 6D 02
  {
    .param [2]/*08000115*/ 
    .custom /*0C000122:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportMembersMapping

  .method /*0600013A*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportSchemaType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ typeName) cil managed
  // SIG: 20 01 12 80 FC 12 80 85
  {
    // Method begins at RVA 0x24bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportSchemaType

  .method /*0600013B*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportSchemaType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ typeName,
                           class [System.Runtime/*23000001*/]System.Type/*0100001B*/ baseType) cil managed
  // SIG: 20 02 12 80 FC 12 80 85 12 6D
  {
    .param [2]/*08000119*/ 
    .custom /*0C000123:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportSchemaType

  .method /*0600013C*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportSchemaType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ typeName,
                           class [System.Runtime/*23000001*/]System.Type/*0100001B*/ baseType,
                           bool baseTypeCanBeIndirect) cil managed
  // SIG: 20 03 12 80 FC 12 80 85 12 6D 02
  {
    .param [2]/*0800011B*/ 
    .custom /*0C000124:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportSchemaType

  .method /*0600013D*/ public hidebysig instance class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ 
          ImportTypeMapping(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ name) cil managed
  // SIG: 20 01 12 80 FC 12 80 85
  {
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemaImporter::ImportTypeMapping

} // end of class System.Xml.Serialization.XmlSchemaImporter

.class /*02000031*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlSchemas
       extends [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000036*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000037*/
{
  .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00009B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001F*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000036*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/>/*1B000002*/
  .custom /*0C00001F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000037*/
  .custom /*0C000024:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600013E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24c7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Collections.NonGeneric/*23000004*/]System.Collections.CollectionBase/*01000026*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::.ctor

  .method /*0600013F*/ public hidebysig specialname 
          instance bool  get_IsCompiled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::get_IsCompiled

  .method /*06000140*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ 
          get_Item(int32 index) cil managed
  // SIG: 20 01 12 80 D1 08
  {
    // Method begins at RVA 0x24d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::get_Item

  .method /*06000141*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ 'value') cil managed
  // SIG: 20 02 01 08 12 80 D1
  {
    // Method begins at RVA 0x24d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::set_Item

  .method /*06000142*/ public hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ 
          get_Item(string ns) cil managed
  // SIG: 20 01 12 80 D1 0E
  {
    .custom /*0C00012B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::get_Item

  .method /*06000143*/ public hidebysig instance int32 
          Add(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 20 01 08 12 80 D1
  {
    // Method begins at RVA 0x24da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::Add

  .method /*06000144*/ public hidebysig instance int32 
          Add(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema,
              class [System.Runtime/*23000001*/]System.Uri/*01000038*/ baseUri) cil managed
  // SIG: 20 02 08 12 80 D1 12 80 E1
  {
    .param [2]/*08000124*/ 
    .custom /*0C000127:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::Add

  .method /*06000145*/ public hidebysig instance void 
          Add(class System.Xml.Serialization.XmlSchemas/*02000031*/ schemas) cil managed
  // SIG: 20 01 01 12 80 C4
  {
    // Method begins at RVA 0x24e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::Add

  .method /*06000146*/ public hidebysig instance void 
          AddReference(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 20 01 01 12 80 D1
  {
    // Method begins at RVA 0x24e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::AddReference

  .method /*06000147*/ public hidebysig instance void 
          Compile(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*01000039*/ 'handler',
                  bool fullCompile) cil managed
  // SIG: 20 02 01 12 80 E5 02
  {
    .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::Compile

  .method /*06000148*/ public hidebysig instance bool 
          Contains(string targetNamespace) cil managed
  // SIG: 20 01 02 0E
  {
    .custom /*0C00012E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::Contains

  .method /*06000149*/ public hidebysig instance bool 
          Contains(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 20 01 02 12 80 D1
  {
    // Method begins at RVA 0x24e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::Contains

  .method /*0600014A*/ public hidebysig instance void 
          CopyTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 80 D1 08
  {
    // Method begins at RVA 0x24ec
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::CopyTo

  .method /*0600014B*/ public hidebysig instance object 
          Find(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ name,
               class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 02 1C 12 80 85 12 6D
  {
    .param [0]/*0800012D*/ 
    .custom /*0C000128:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::Find

  .method /*0600014C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.IList/*0100003A*/ 
          GetSchemas(string ns) cil managed
  // SIG: 20 01 12 80 E9 0E
  {
    .param [1]/*08000130*/ 
    .custom /*0C000129:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::GetSchemas

  .method /*0600014D*/ public hidebysig instance int32 
          IndexOf(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 20 01 08 12 80 D1
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::IndexOf

  .method /*0600014E*/ public hidebysig instance void 
          Insert(int32 index,
                 class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 20 02 01 08 12 80 D1
  {
    // Method begins at RVA 0x24f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::Insert

  .method /*0600014F*/ public hidebysig static 
          bool  IsDataSet(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 00 01 02 12 80 D1
  {
    // Method begins at RVA 0x24f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::IsDataSet

  .method /*06000150*/ family hidebysig virtual 
          instance void  OnClear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::OnClear

  .method /*06000151*/ family hidebysig virtual 
          instance void  OnInsert(int32 index,
                                  object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000130:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::OnInsert

  .method /*06000152*/ family hidebysig virtual 
          instance void  OnRemove(int32 index,
                                  object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000132:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2500
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::OnRemove

  .method /*06000153*/ family hidebysig virtual 
          instance void  OnSet(int32 index,
                               object oldValue,
                               object newValue) cil managed
  // SIG: 20 03 01 08 1C 1C
  {
    .custom /*0C000133:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2502
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::OnSet

  .method /*06000154*/ public hidebysig instance void 
          Remove(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ schema) cil managed
  // SIG: 20 01 01 12 80 D1
  {
    // Method begins at RVA 0x2504
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSchemas::Remove

  .method /*06000155*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/> 
          'System.Collections.Generic.IEnumerable<System.Xml.Schema.XmlSchema>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 80 CD 01 12 80 D1
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000036*/<class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/>/*1B000002*/::GetEnumerator() /* 0A00001A */
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSchemas::'System.Collections.Generic.IEnumerable<System.Xml.Schema.XmlSchema>.GetEnumerator'

  .property /*1700006D*/ instance callconv(8) bool
          IsCompiled()
  {
    .get instance bool System.Xml.Serialization.XmlSchemas/*02000031*/::get_IsCompiled() /* 0600013F */
  } // end of property XmlSchemas::IsCompiled
  .property /*1700006E*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/
          Item(int32)
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ System.Xml.Serialization.XmlSchemas/*02000031*/::get_Item(int32) /* 06000140 */
    .set instance void System.Xml.Serialization.XmlSchemas/*02000031*/::set_Item(int32,
                                                                                 class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/) /* 06000141 */
  } // end of property XmlSchemas::Item
  .property /*1700006F*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/
          Item(string)
  {
    .custom /*0C0000DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000034*/ System.Xml.Serialization.XmlSchemas/*02000031*/::get_Item(string) /* 06000142 */
  } // end of property XmlSchemas::Item
} // end of class System.Xml.Serialization.XmlSchemas

.class /*02000032*/ public auto ansi sealed System.Xml.Serialization.XmlSerializationCollectionFixupCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*06000156*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlSerializationCollectionFixupCallback::.ctor

  .method /*06000157*/ public hidebysig newslot virtual 
          instance void  Invoke(object collection,
                                object collectionItems) runtime managed
  // SIG: 20 02 01 1C 1C
  {
    .custom /*0C000134:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlSerializationCollectionFixupCallback::Invoke

  .method /*06000158*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object collection,
                      object collectionItems,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 91 1C 1C 12 80 95 1C
  {
    .param [1]/*08000141*/ 
    .custom /*0C00012A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000142*/ 
    .custom /*0C00012C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlSerializationCollectionFixupCallback::BeginInvoke

  .method /*06000159*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method XmlSerializationCollectionFixupCallback::EndInvoke

} // end of class System.Xml.Serialization.XmlSerializationCollectionFixupCallback

.class /*02000033*/ public auto ansi sealed System.Xml.Serialization.XmlSerializationFixupCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*0600015A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlSerializationFixupCallback::.ctor

  .method /*0600015B*/ public hidebysig newslot virtual 
          instance void  Invoke(object fixup) runtime managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlSerializationFixupCallback::Invoke

  .method /*0600015C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object fixup,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 91 1C 12 80 95 1C
  {
    .param [1]/*08000149*/ 
    .custom /*0C00012F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlSerializationFixupCallback::BeginInvoke

  .method /*0600015D*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method XmlSerializationFixupCallback::EndInvoke

} // end of class System.Xml.Serialization.XmlSerializationFixupCallback

.class /*02000034*/ public abstract auto ansi beforefieldinit System.Xml.Serialization.XmlSerializationGeneratedCode
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .method /*0600015E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2509
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializationGeneratedCode::.ctor

} // end of class System.Xml.Serialization.XmlSerializationGeneratedCode

.class /*02000035*/ public auto ansi sealed System.Xml.Serialization.XmlSerializationReadCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*0600015F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlSerializationReadCallback::.ctor

  .method /*06000160*/ public hidebysig newslot virtual 
          instance object  Invoke() runtime managed
  // SIG: 20 00 1C
  {
    .custom /*0C000137:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlSerializationReadCallback::Invoke

  .method /*06000161*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 02 12 80 91 12 80 95 1C
  {
  } // end of method XmlSerializationReadCallback::BeginInvoke

  .method /*06000162*/ public hidebysig newslot virtual 
          instance object  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 1C 12 80 91
  {
    .param [0]/*08000151*/ 
    .custom /*0C000131:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method XmlSerializationReadCallback::EndInvoke

} // end of class System.Xml.Serialization.XmlSerializationReadCallback

.class /*02000036*/ public abstract auto ansi beforefieldinit System.Xml.Serialization.XmlSerializationReader
       extends System.Xml.Serialization.XmlSerializationGeneratedCode/*02000034*/
{
  .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000040*/ auto ansi nested family beforefieldinit CollectionFixup
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
  {
    .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*06000272*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object collection,
                                 class System.Xml.Serialization.XmlSerializationCollectionFixupCallback/*02000032*/ callback,
                                 object collectionItems) cil managed
    // SIG: 20 03 01 1C 12 80 C8 1C
    {
      .param [1]/*08000328*/ 
      .custom /*0C000207:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x281f
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method CollectionFixup::.ctor

    .method /*06000273*/ public hidebysig specialname 
            instance class System.Xml.Serialization.XmlSerializationCollectionFixupCallback/*02000032*/ 
            get_Callback() cil managed
    // SIG: 20 00 12 80 C8
    {
      // Method begins at RVA 0x2827
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CollectionFixup::get_Callback

    .method /*06000274*/ public hidebysig specialname 
            instance object  get_Collection() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0001CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x282a
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CollectionFixup::get_Collection

    .method /*06000275*/ public hidebysig specialname 
            instance object  get_CollectionItems() cil managed
    // SIG: 20 00 1C
    {
      // Method begins at RVA 0x282d
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CollectionFixup::get_CollectionItems

    .property /*1700008B*/ instance callconv(8) class System.Xml.Serialization.XmlSerializationCollectionFixupCallback/*02000032*/
            Callback()
    {
      .get instance class System.Xml.Serialization.XmlSerializationCollectionFixupCallback/*02000032*/ System.Xml.Serialization.XmlSerializationReader/*02000036*//CollectionFixup/*02000040*/::get_Callback() /* 06000273 */
    } // end of property CollectionFixup::Callback
    .property /*1700008C*/ instance callconv(8) object
            Collection()
    {
      .custom /*0C0000E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .get instance object System.Xml.Serialization.XmlSerializationReader/*02000036*//CollectionFixup/*02000040*/::get_Collection() /* 06000274 */
    } // end of property CollectionFixup::Collection
    .property /*1700008D*/ instance callconv(8) object
            CollectionItems()
    {
      .get instance object System.Xml.Serialization.XmlSerializationReader/*02000036*//CollectionFixup/*02000040*/::get_CollectionItems() /* 06000275 */
    } // end of property CollectionFixup::CollectionItems
  } // end of class CollectionFixup

  .class /*02000041*/ auto ansi nested family beforefieldinit Fixup
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
  {
    .custom /*0C0000BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*06000276*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object o,
                                 class System.Xml.Serialization.XmlSerializationFixupCallback/*02000033*/ callback,
                                 int32 count) cil managed
    // SIG: 20 03 01 1C 12 80 CC 08
    {
      .custom /*0C0001D0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      .param [1]/*0800032B*/ 
      .custom /*0C000208:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x2830
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method Fixup::.ctor

    .method /*06000277*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object o,
                                 class System.Xml.Serialization.XmlSerializationFixupCallback/*02000033*/ callback,
                                 string[] ids) cil managed
    // SIG: 20 03 01 1C 12 80 CC 1D 0E
    {
      .param [2]/*0800032F*/ 
      .custom /*0C000209:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x2838
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method Fixup::.ctor

    .method /*06000278*/ public hidebysig specialname 
            instance class System.Xml.Serialization.XmlSerializationFixupCallback/*02000033*/ 
            get_Callback() cil managed
    // SIG: 20 00 12 80 CC
    {
      .custom /*0C0001D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x2840
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Fixup::get_Callback

    .method /*06000279*/ public hidebysig specialname 
            instance string[]  get_Ids() cil managed
    // SIG: 20 00 1D 0E
    {
      // Method begins at RVA 0x2843
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Fixup::get_Ids

    .method /*0600027A*/ public hidebysig specialname 
            instance object  get_Source() cil managed
    // SIG: 20 00 1C
    {
      // Method begins at RVA 0x2846
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Fixup::get_Source

    .method /*0600027B*/ public hidebysig specialname 
            instance void  set_Source(object 'value') cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x2849
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method Fixup::set_Source

    .property /*1700008E*/ instance callconv(8) class System.Xml.Serialization.XmlSerializationFixupCallback/*02000033*/
            Callback()
    {
      .custom /*0C0000EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .get instance class System.Xml.Serialization.XmlSerializationFixupCallback/*02000033*/ System.Xml.Serialization.XmlSerializationReader/*02000036*//Fixup/*02000041*/::get_Callback() /* 06000278 */
    } // end of property Fixup::Callback
    .property /*1700008F*/ instance callconv(8) string[]
            Ids()
    {
      .get instance string[] System.Xml.Serialization.XmlSerializationReader/*02000036*//Fixup/*02000041*/::get_Ids() /* 06000279 */
    } // end of property Fixup::Ids
    .property /*17000090*/ instance callconv(8) object
            Source()
    {
      .get instance object System.Xml.Serialization.XmlSerializationReader/*02000036*//Fixup/*02000041*/::get_Source() /* 0600027A */
      .set instance void System.Xml.Serialization.XmlSerializationReader/*02000036*//Fixup/*02000041*/::set_Source(object) /* 0600027B */
    } // end of property Fixup::Source
  } // end of class Fixup

  .method /*06000163*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2511
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00015E       */ call       instance void System.Xml.Serialization.XmlSerializationGeneratedCode/*02000034*/::.ctor() /* 0600015E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::.ctor

  .method /*06000164*/ family hidebysig specialname 
          instance bool  get_DecodeName() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2519
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::get_DecodeName

  .method /*06000165*/ family hidebysig specialname 
          instance void  set_DecodeName(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x251c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::set_DecodeName

  .method /*06000166*/ family hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlDocument/*0100003B*/ 
          get_Document() cil managed
  // SIG: 20 00 12 80 ED
  {
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::get_Document

  .method /*06000167*/ family hidebysig specialname 
          instance bool  get_IsReturnValue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::get_IsReturnValue

  .method /*06000168*/ family hidebysig specialname 
          instance void  set_IsReturnValue(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2524
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::set_IsReturnValue

  .method /*06000169*/ family hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ 
          get_Reader() cil managed
  // SIG: 20 00 12 80 F1
  {
    // Method begins at RVA 0x2526
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::get_Reader

  .method /*0600016A*/ family hidebysig specialname 
          instance int32  get_ReaderCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2529
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::get_ReaderCount

  .method /*0600016B*/ family hidebysig instance void 
          AddFixup(class System.Xml.Serialization.XmlSerializationReader/*02000036*//CollectionFixup/*02000040*/ fixup) cil managed
  // SIG: 20 01 01 12 81 00
  {
    .custom /*0C00013A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::AddFixup

  .method /*0600016C*/ family hidebysig instance void 
          AddFixup(class System.Xml.Serialization.XmlSerializationReader/*02000036*//Fixup/*02000041*/ fixup) cil managed
  // SIG: 20 01 01 12 81 04
  {
    .custom /*0C00013B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::AddFixup

  .method /*0600016D*/ family hidebysig instance void 
          AddReadCallback(string name,
                          string ns,
                          class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                          class System.Xml.Serialization.XmlSerializationReadCallback/*02000035*/ read) cil managed
  // SIG: 20 04 01 0E 0E 12 6D 12 80 D4
  {
    // Method begins at RVA 0x2530
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::AddReadCallback

  .method /*0600016E*/ family hidebysig instance void 
          AddTarget(string id,
                    object o) cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2532
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::AddTarget

  .method /*0600016F*/ family hidebysig instance void 
          CheckReaderCount(int32& whileIterations,
                           int32& readerCount) cil managed
  // SIG: 20 02 01 10 08 10 08
  {
    // Method begins at RVA 0x2534
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::CheckReaderCount

  .method /*06000170*/ family hidebysig instance string 
          CollapseWhitespace(string 'value') cil managed
  // SIG: 20 01 0E 0E
  {
    .custom /*0C00013F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800015F*/ 
    .custom /*0C000136:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x2536
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CollapseWhitespace

  .method /*06000171*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateAbstractTypeException(string name,
                                      string ns) cil managed
  // SIG: 20 02 12 80 F9 0E 0E
  {
    .param [2]/*08000162*/ 
    .custom /*0C000138:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2539
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateAbstractTypeException

  .method /*06000172*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateBadDerivationException(string xsdDerived,
                                       string nsDerived,
                                       string xsdBase,
                                       string nsBase,
                                       string clrDerived,
                                       string clrBase) cil managed
  // SIG: 20 06 12 80 F9 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C000142:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000163*/ 
    .custom /*0C000139:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x253c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateBadDerivationException

  .method /*06000173*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateCtorHasSecurityException(string typeName) cil managed
  // SIG: 20 01 12 80 F9 0E
  {
    // Method begins at RVA 0x253f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateCtorHasSecurityException

  .method /*06000174*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInaccessibleConstructorException(string typeName) cil managed
  // SIG: 20 01 12 80 F9 0E
  {
    // Method begins at RVA 0x2542
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateInaccessibleConstructorException

  .method /*06000175*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInvalidCastException(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                                     object 'value') cil managed
  // SIG: 20 02 12 80 F9 12 6D 1C
  {
    .param [2]/*0800016D*/ 
    .custom /*0C00013C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2545
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateInvalidCastException

  .method /*06000176*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInvalidCastException(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                                     object 'value',
                                     string id) cil managed
  // SIG: 20 03 12 80 F9 12 6D 1C 0E
  {
    .param [2]/*0800016F*/ 
    .custom /*0C00013E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000170*/ 
    .custom /*0C000140:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2548
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateInvalidCastException

  .method /*06000177*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateMissingIXmlSerializableType(string name,
                                            string ns,
                                            string clrType) cil managed
  // SIG: 20 03 12 80 F9 0E 0E 0E
  {
    .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000171*/ 
    .custom /*0C000141:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x254b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateMissingIXmlSerializableType

  .method /*06000178*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateReadOnlyCollectionException(string name) cil managed
  // SIG: 20 01 12 80 F9 0E
  {
    // Method begins at RVA 0x254e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateReadOnlyCollectionException

  .method /*06000179*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateUnknownConstantException(string 'value',
                                         class [System.Runtime/*23000001*/]System.Type/*0100001B*/ enumType) cil managed
  // SIG: 20 02 12 80 F9 0E 12 6D
  {
    .param [1]/*08000176*/ 
    .custom /*0C000143:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2551
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateUnknownConstantException

  .method /*0600017A*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateUnknownNodeException() cil managed
  // SIG: 20 00 12 80 F9
  {
    // Method begins at RVA 0x2554
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateUnknownNodeException

  .method /*0600017B*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateUnknownTypeException(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'type') cil managed
  // SIG: 20 01 12 80 F9 12 80 85
  {
    // Method begins at RVA 0x2557
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::CreateUnknownTypeException

  .method /*0600017C*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Array/*0100003F*/ 
          EnsureArrayIndex(class [System.Runtime/*23000001*/]System.Array/*0100003F*/ a,
                           int32 index,
                           class [System.Runtime/*23000001*/]System.Type/*0100001B*/ elementType) cil managed
  // SIG: 20 03 12 80 FD 12 80 FD 08 12 6D
  {
    .param [1]/*08000179*/ 
    .custom /*0C000145:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x255a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::EnsureArrayIndex

  .method /*0600017D*/ family hidebysig instance void 
          FixupArrayRefs(object fixup) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x255d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::FixupArrayRefs

  .method /*0600017E*/ family hidebysig instance int32 
          GetArrayLength(string name,
                         string ns) cil managed
  // SIG: 20 02 08 0E 0E
  {
    // Method begins at RVA 0x255f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::GetArrayLength

  .method /*0600017F*/ family hidebysig instance bool 
          GetNullAttr() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2562
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::GetNullAttr

  .method /*06000180*/ family hidebysig instance object 
          GetTarget(string id) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x2565
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::GetTarget

  .method /*06000181*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 
          GetXsiType() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C000146:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2568
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::GetXsiType

  .method /*06000182*/ family hidebysig newslot abstract virtual 
          instance void  InitCallbacks() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XmlSerializationReader::InitCallbacks

  .method /*06000183*/ family hidebysig newslot abstract virtual 
          instance void  InitIDs() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XmlSerializationReader::InitIDs

  .method /*06000184*/ family hidebysig instance bool 
          IsXmlnsAttribute(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x256b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::IsXmlnsAttribute

  .method /*06000185*/ family hidebysig instance void 
          ParseWsdlArrayType(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlAttribute/*01000029*/ attr) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x256e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::ParseWsdlArrayType

  .method /*06000186*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 
          ReadElementQualifiedName() cil managed
  // SIG: 20 00 12 80 85
  {
    // Method begins at RVA 0x2570
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadElementQualifiedName

  .method /*06000187*/ family hidebysig instance void 
          ReadEndElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2573
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::ReadEndElement

  .method /*06000188*/ family hidebysig instance bool 
          ReadNull() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2575
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadNull

  .method /*06000189*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 
          ReadNullableQualifiedName() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C000148:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2578
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadNullableQualifiedName

  .method /*0600018A*/ family hidebysig instance string 
          ReadNullableString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000149:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x257b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadNullableString

  .method /*0600018B*/ family hidebysig instance bool 
          ReadReference([out] string& fixupReference) cil managed
  // SIG: 20 01 02 10 0E
  {
    .custom /*0C00014A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000182*/ 
    .custom /*0C000147:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000040*/::.ctor(bool) /* 0A00001C */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x257e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadReference

  .method /*0600018C*/ family hidebysig instance object 
          ReadReferencedElement() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00014B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2581
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadReferencedElement

  .method /*0600018D*/ family hidebysig instance object 
          ReadReferencedElement(string name,
                                string ns) cil managed
  // SIG: 20 02 1C 0E 0E
  {
    .custom /*0C00014C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2584
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadReferencedElement

  .method /*0600018E*/ family hidebysig instance void 
          ReadReferencedElements() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2587
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::ReadReferencedElements

  .method /*0600018F*/ family hidebysig instance object 
          ReadReferencingElement(string name,
                                 string ns,
                                 bool elementCanBeType,
                                 [out] string& fixupReference) cil managed
  // SIG: 20 04 1C 0E 0E 02 10 0E
  {
    .custom /*0C00014D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2589
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadReferencingElement

  .method /*06000190*/ family hidebysig instance object 
          ReadReferencingElement(string name,
                                 string ns,
                                 [out] string& fixupReference) cil managed
  // SIG: 20 03 1C 0E 0E 10 0E
  {
    .custom /*0C00014E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x258c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadReferencingElement

  .method /*06000191*/ family hidebysig instance object 
          ReadReferencingElement([out] string& fixupReference) cil managed
  // SIG: 20 01 1C 10 0E
  {
    .custom /*0C000150:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x258f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadReferencingElement

  .method /*06000192*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000041*/ 
          ReadSerializable(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000041*/ 'serializable') cil managed
  // SIG: 20 01 12 81 05 12 81 05
  {
    // Method begins at RVA 0x2592
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadSerializable

  .method /*06000193*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000041*/ 
          ReadSerializable(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000041*/ 'serializable',
                           bool wrappedAny) cil managed
  // SIG: 20 02 12 81 05 12 81 05 02
  {
    // Method begins at RVA 0x2595
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadSerializable

  .method /*06000194*/ family hidebysig instance string 
          ReadString(string 'value') cil managed
  // SIG: 20 01 0E 0E
  {
    .custom /*0C000152:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000190*/ 
    .custom /*0C00014F:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x2598
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadString

  .method /*06000195*/ family hidebysig instance string 
          ReadString(string 'value',
                     bool trim) cil managed
  // SIG: 20 02 0E 0E 02
  {
    .custom /*0C000153:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000192*/ 
    .custom /*0C000151:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x259b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadString

  .method /*06000196*/ family hidebysig instance object 
          ReadTypedNull(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'type') cil managed
  // SIG: 20 01 1C 12 80 85
  {
    .param [0]/*08000195*/ 
    .custom /*0C000154:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x259e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadTypedNull

  .method /*06000197*/ family hidebysig instance object 
          ReadTypedPrimitive(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'type') cil managed
  // SIG: 20 01 1C 12 80 85
  {
    .param [0]/*08000197*/ 
    .custom /*0C000155:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadTypedPrimitive

  .method /*06000198*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlDocument/*0100003B*/ 
          ReadXmlDocument(bool wrapped) cil managed
  // SIG: 20 01 12 80 ED 02
  {
    .custom /*0C000156:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadXmlDocument

  .method /*06000199*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000042*/ 
          ReadXmlNode(bool wrapped) cil managed
  // SIG: 20 01 12 81 09 02
  {
    .custom /*0C000157:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ReadXmlNode

  .method /*0600019A*/ family hidebysig instance void 
          Referenced(object o) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000158:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::Referenced

  .method /*0600019B*/ family hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000043*/ 
          ResolveDynamicAssembly(string assemblyFullName) cil managed
  // SIG: 00 01 12 81 0D 0E
  {
    .param [0]/*0800019C*/ 
    .custom /*0C00015A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ResolveDynamicAssembly

  .method /*0600019C*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Array/*0100003F*/ 
          ShrinkArray(class [System.Runtime/*23000001*/]System.Array/*0100003F*/ a,
                      int32 length,
                      class [System.Runtime/*23000001*/]System.Type/*0100001B*/ elementType,
                      bool isNullable) cil managed
  // SIG: 20 04 12 80 FD 12 80 FD 08 12 6D 02
  {
    .custom /*0C000159:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001A0*/ 
    .custom /*0C00015F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ShrinkArray

  .method /*0600019D*/ family hidebysig instance uint8[] 
          ToByteArrayBase64(bool isNull) cil managed
  // SIG: 20 01 1D 05 02
  {
    .custom /*0C00015B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToByteArrayBase64

  .method /*0600019E*/ family hidebysig static 
          uint8[]  ToByteArrayBase64(string 'value') cil managed
  // SIG: 00 01 1D 05 0E
  {
    .custom /*0C00015C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001A3*/ 
    .custom /*0C000160:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x25b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToByteArrayBase64

  .method /*0600019F*/ family hidebysig instance uint8[] 
          ToByteArrayHex(bool isNull) cil managed
  // SIG: 20 01 1D 05 02
  {
    .custom /*0C00015D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToByteArrayHex

  .method /*060001A0*/ family hidebysig static 
          uint8[]  ToByteArrayHex(string 'value') cil managed
  // SIG: 00 01 1D 05 0E
  {
    .custom /*0C00015E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001A6*/ 
    .custom /*0C000162:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x25bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToByteArrayHex

  .method /*060001A1*/ family hidebysig static 
          char  ToChar(string 'value') cil managed
  // SIG: 00 01 03 0E
  {
    // Method begins at RVA 0x25be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToChar

  .method /*060001A2*/ family hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 
          ToDate(string 'value') cil managed
  // SIG: 00 01 11 81 11 0E
  {
    // Method begins at RVA 0x25c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToDate

  .method /*060001A3*/ family hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 
          ToDateTime(string 'value') cil managed
  // SIG: 00 01 11 81 11 0E
  {
    // Method begins at RVA 0x25c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToDateTime

  .method /*060001A4*/ family hidebysig static 
          int64  ToEnum(string 'value',
                        class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ h,
                        string typeName) cil managed
  // SIG: 00 03 0A 0E 12 81 15 0E
  {
    // Method begins at RVA 0x25c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToEnum

  .method /*060001A5*/ family hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 
          ToTime(string 'value') cil managed
  // SIG: 00 01 11 81 11 0E
  {
    // Method begins at RVA 0x25ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToTime

  .method /*060001A6*/ family hidebysig static 
          string  ToXmlName(string 'value') cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000161:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001AF*/ 
    .custom /*0C000169:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x25cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToXmlName

  .method /*060001A7*/ family hidebysig static 
          string  ToXmlNCName(string 'value') cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001B1*/ 
    .custom /*0C00016C:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x25d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToXmlNCName

  .method /*060001A8*/ family hidebysig static 
          string  ToXmlNmToken(string 'value') cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000164:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001B3*/ 
    .custom /*0C00016E:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x25d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToXmlNmToken

  .method /*060001A9*/ family hidebysig static 
          string  ToXmlNmTokens(string 'value') cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000165:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001B5*/ 
    .custom /*0C00016F:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x25d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToXmlNmTokens

  .method /*060001AA*/ family hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 
          ToXmlQualifiedName(string 'value') cil managed
  // SIG: 20 01 12 80 85 0E
  {
    .param [1]/*080001B7*/ 
    .custom /*0C000170:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationReader::ToXmlQualifiedName

  .method /*060001AB*/ family hidebysig instance void 
          UnknownAttribute(object o,
                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlAttribute/*01000029*/ attr) cil managed
  // SIG: 20 02 01 1C 12 80 A5
  {
    .param [1]/*080001B8*/ 
    .custom /*0C000171:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnknownAttribute

  .method /*060001AC*/ family hidebysig instance void 
          UnknownAttribute(object o,
                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlAttribute/*01000029*/ attr,
                           string qnames) cil managed
  // SIG: 20 03 01 1C 12 80 A5 0E
  {
    .custom /*0C000166:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001BB*/ 
    .custom /*0C000173:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnknownAttribute

  .method /*060001AD*/ family hidebysig instance void 
          UnknownElement(object o,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlElement/*01000030*/ elem) cil managed
  // SIG: 20 02 01 1C 12 80 C1
  {
    .param [1]/*080001BD*/ 
    .custom /*0C000176:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnknownElement

  .method /*060001AE*/ family hidebysig instance void 
          UnknownElement(object o,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlElement/*01000030*/ elem,
                         string qnames) cil managed
  // SIG: 20 03 01 1C 12 80 C1 0E
  {
    .custom /*0C000167:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001C0*/ 
    .custom /*0C00017A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnknownElement

  .method /*060001AF*/ family hidebysig instance void 
          UnknownNode(object o) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000168:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnknownNode

  .method /*060001B0*/ family hidebysig instance void 
          UnknownNode(object o,
                      string qnames) cil managed
  // SIG: 20 02 01 1C 0E
  {
    .custom /*0C00016A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnknownNode

  .method /*060001B1*/ family hidebysig instance void 
          UnreferencedObject(string id,
                             object o) cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C00016B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationReader::UnreferencedObject

  .property /*17000070*/ instance callconv(8) bool
          DecodeName()
  {
    .get instance bool System.Xml.Serialization.XmlSerializationReader/*02000036*/::get_DecodeName() /* 06000164 */
    .set instance void System.Xml.Serialization.XmlSerializationReader/*02000036*/::set_DecodeName(bool) /* 06000165 */
  } // end of property XmlSerializationReader::DecodeName
  .property /*17000071*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlDocument/*0100003B*/
          Document()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlDocument/*0100003B*/ System.Xml.Serialization.XmlSerializationReader/*02000036*/::get_Document() /* 06000166 */
  } // end of property XmlSerializationReader::Document
  .property /*17000072*/ instance callconv(8) bool
          IsReturnValue()
  {
    .get instance bool System.Xml.Serialization.XmlSerializationReader/*02000036*/::get_IsReturnValue() /* 06000167 */
    .set instance void System.Xml.Serialization.XmlSerializationReader/*02000036*/::set_IsReturnValue(bool) /* 06000168 */
  } // end of property XmlSerializationReader::IsReturnValue
  .property /*17000073*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/
          Reader()
  {
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ System.Xml.Serialization.XmlSerializationReader/*02000036*/::get_Reader() /* 06000169 */
  } // end of property XmlSerializationReader::Reader
  .property /*17000074*/ instance callconv(8) int32
          ReaderCount()
  {
    .get instance int32 System.Xml.Serialization.XmlSerializationReader/*02000036*/::get_ReaderCount() /* 0600016A */
  } // end of property XmlSerializationReader::ReaderCount
} // end of class System.Xml.Serialization.XmlSerializationReader

.class /*02000037*/ public auto ansi sealed System.Xml.Serialization.XmlSerializationWriteCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
{
  .method /*060001B2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method XmlSerializationWriteCallback::.ctor

  .method /*060001B3*/ public hidebysig newslot virtual 
          instance void  Invoke(object o) runtime managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00016D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlSerializationWriteCallback::Invoke

  .method /*060001B4*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
          BeginInvoke(object o,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 91 1C 12 80 95 1C
  {
    .param [1]/*080001CA*/ 
    .custom /*0C000184:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method XmlSerializationWriteCallback::BeginInvoke

  .method /*060001B5*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
  // SIG: 20 01 01 12 80 91
  {
  } // end of method XmlSerializationWriteCallback::EndInvoke

} // end of class System.Xml.Serialization.XmlSerializationWriteCallback

.class /*02000038*/ public abstract auto ansi beforefieldinit System.Xml.Serialization.XmlSerializationWriter
       extends System.Xml.Serialization.XmlSerializationGeneratedCode/*02000034*/
{
  .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060001B6*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x25ea
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00015E       */ call       instance void System.Xml.Serialization.XmlSerializationGeneratedCode/*02000034*/::.ctor() /* 0600015E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::.ctor

  .method /*060001B7*/ family hidebysig specialname 
          instance bool  get_EscapeName() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::get_EscapeName

  .method /*060001B8*/ family hidebysig specialname 
          instance void  set_EscapeName(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::set_EscapeName

  .method /*060001B9*/ family hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000046*/ 
          get_Namespaces() cil managed
  // SIG: 20 00 12 81 19
  {
    // Method begins at RVA 0x25f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::get_Namespaces

  .method /*060001BA*/ family hidebysig specialname 
          instance void  set_Namespaces(class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000046*/ 'value') cil managed
  // SIG: 20 01 01 12 81 19
  {
    // Method begins at RVA 0x25fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::set_Namespaces

  .method /*060001BB*/ family hidebysig specialname 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ 
          get_Writer() cil managed
  // SIG: 20 00 12 81 1D
  {
    .custom /*0C000172:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::get_Writer

  .method /*060001BC*/ family hidebysig specialname 
          instance void  set_Writer(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ 'value') cil managed
  // SIG: 20 01 01 12 81 1D
  {
    .custom /*0C000174:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::set_Writer

  .method /*060001BD*/ family hidebysig instance void 
          AddWriteCallback(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           string typeName,
                           string typeNs,
                           class System.Xml.Serialization.XmlSerializationWriteCallback/*02000037*/ callback) cil managed
  // SIG: 20 04 01 12 6D 0E 0E 12 80 DC
  {
    .custom /*0C000175:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080001D3*/ 
    .custom /*0C000189:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2601
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::AddWriteCallback

  .method /*060001BE*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateChoiceIdentifierValueException(string 'value',
                                               string identifier,
                                               string name,
                                               string ns) cil managed
  // SIG: 20 04 12 80 F9 0E 0E 0E 0E
  {
    .custom /*0C000177:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2603
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateChoiceIdentifierValueException

  .method /*060001BF*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInvalidAnyTypeException(object o) cil managed
  // SIG: 20 01 12 80 F9 1C
  {
    .custom /*0C000178:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2606
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateInvalidAnyTypeException

  .method /*060001C0*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInvalidAnyTypeException(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 80 F9 12 6D
  {
    .custom /*0C000179:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2609
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateInvalidAnyTypeException

  .method /*060001C1*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInvalidChoiceIdentifierValueException(string 'type',
                                                      string identifier) cil managed
  // SIG: 20 02 12 80 F9 0E 0E
  {
    .custom /*0C00017B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x260c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateInvalidChoiceIdentifierValueException

  .method /*060001C2*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateInvalidEnumValueException(object 'value',
                                          string typeName) cil managed
  // SIG: 20 02 12 80 F9 1C 0E
  {
    .custom /*0C00017C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x260f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateInvalidEnumValueException

  .method /*060001C3*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateMismatchChoiceException(string 'value',
                                        string elementName,
                                        string enumValue) cil managed
  // SIG: 20 03 12 80 F9 0E 0E 0E
  {
    .custom /*0C00017D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2612
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateMismatchChoiceException

  .method /*060001C4*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateUnknownAnyElementException(string name,
                                           string ns) cil managed
  // SIG: 20 02 12 80 F9 0E 0E
  {
    .custom /*0C00017E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2615
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateUnknownAnyElementException

  .method /*060001C5*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateUnknownTypeException(object o) cil managed
  // SIG: 20 01 12 80 F9 1C
  {
    .custom /*0C00017F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2618
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateUnknownTypeException

  .method /*060001C6*/ family hidebysig instance class [System.Runtime/*23000001*/]System.Exception/*0100003E*/ 
          CreateUnknownTypeException(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 80 F9 12 6D
  {
    .custom /*0C000180:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x261b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::CreateUnknownTypeException

  .method /*060001C7*/ family hidebysig static 
          uint8[]  FromByteArrayBase64(uint8[] 'value') cil managed
  // SIG: 00 01 1D 05 1D 05
  {
    .custom /*0C000181:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x261e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromByteArrayBase64

  .method /*060001C8*/ family hidebysig static 
          string  FromByteArrayHex(uint8[] 'value') cil managed
  // SIG: 00 01 0E 1D 05
  {
    .param [0]/*080001E7*/ 
    .custom /*0C000191:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x2621
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromByteArrayHex

  .method /*060001C9*/ family hidebysig static 
          string  FromChar(char 'value') cil managed
  // SIG: 00 01 0E 03
  {
    .custom /*0C000182:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2624
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromChar

  .method /*060001CA*/ family hidebysig static 
          string  FromDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 'value') cil managed
  // SIG: 00 01 0E 11 81 11
  {
    .custom /*0C000183:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2627
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromDate

  .method /*060001CB*/ family hidebysig static 
          string  FromDateTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 'value') cil managed
  // SIG: 00 01 0E 11 81 11
  {
    .custom /*0C000185:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x262a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromDateTime

  .method /*060001CC*/ family hidebysig static 
          string  FromEnum(int64 'value',
                           string[] values,
                           int64[] ids) cil managed
  // SIG: 00 03 0E 0A 1D 0E 1D 0A
  {
    .custom /*0C000186:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x262d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromEnum

  .method /*060001CD*/ family hidebysig static 
          string  FromEnum(int64 'value',
                           string[] values,
                           int64[] ids,
                           string typeName) cil managed
  // SIG: 00 04 0E 0A 1D 0E 1D 0A 0E
  {
    .custom /*0C000187:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2630
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromEnum

  .method /*060001CE*/ family hidebysig static 
          string  FromTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 'value') cil managed
  // SIG: 00 01 0E 11 81 11
  {
    .custom /*0C000188:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2633
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromTime

  .method /*060001CF*/ family hidebysig static 
          string  FromXmlName(string name) cil managed
  // SIG: 00 01 0E 0E
  {
    .param [0]/*080001F4*/ 
    .custom /*0C000194:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 04 6E 61 6D 65 00 00 )                      // ...name..
    // Method begins at RVA 0x2636
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromXmlName

  .method /*060001D0*/ family hidebysig static 
          string  FromXmlNCName(string ncName) cil managed
  // SIG: 00 01 0E 0E
  {
    .param [0]/*080001F6*/ 
    .custom /*0C000195:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 06 6E 63 4E 61 6D 65 00 00 )                // ...ncName..
    // Method begins at RVA 0x2639
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromXmlNCName

  .method /*060001D1*/ family hidebysig static 
          string  FromXmlNmToken(string nmToken) cil managed
  // SIG: 00 01 0E 0E
  {
    .param [0]/*080001F8*/ 
    .custom /*0C000196:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 07 6E 6D 54 6F 6B 65 6E 00 00 )             // ...nmToken..
    // Method begins at RVA 0x263c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromXmlNmToken

  .method /*060001D2*/ family hidebysig static 
          string  FromXmlNmTokens(string nmTokens) cil managed
  // SIG: 00 01 0E 0E
  {
    .param [0]/*080001FA*/ 
    .custom /*0C000197:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100003D*/::.ctor(string) /* 0A00001B */ = ( 01 00 08 6E 6D 54 6F 6B 65 6E 73 00 00 )          // ...nmTokens..
    // Method begins at RVA 0x263f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromXmlNmTokens

  .method /*060001D3*/ family hidebysig instance string 
          FromXmlQualifiedName(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xmlQualifiedName) cil managed
  // SIG: 20 01 0E 12 80 85
  {
    // Method begins at RVA 0x2642
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromXmlQualifiedName

  .method /*060001D4*/ family hidebysig instance string 
          FromXmlQualifiedName(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xmlQualifiedName,
                               bool ignoreEmpty) cil managed
  // SIG: 20 02 0E 12 80 85 02
  {
    // Method begins at RVA 0x2645
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::FromXmlQualifiedName

  .method /*060001D5*/ family hidebysig newslot abstract virtual 
          instance void  InitCallbacks() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XmlSerializationWriter::InitCallbacks

  .method /*060001D6*/ family hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000043*/ 
          ResolveDynamicAssembly(string assemblyFullName) cil managed
  // SIG: 00 01 12 81 0D 0E
  {
    .custom /*0C00018A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080001FF*/ 
    .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2648
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializationWriter::ResolveDynamicAssembly

  .method /*060001D7*/ family hidebysig instance void 
          TopLevelElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x264b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::TopLevelElement

  .method /*060001D8*/ family hidebysig instance void 
          WriteAttribute(string localName,
                         uint8[] 'value') cil managed
  // SIG: 20 02 01 0E 1D 05
  {
    .custom /*0C00018B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000202*/ 
    .custom /*0C000199:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x264d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteAttribute

  .method /*060001D9*/ family hidebysig instance void 
          WriteAttribute(string localName,
                         string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C00018C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000204*/ 
    .custom /*0C00019A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x264f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteAttribute

  .method /*060001DA*/ family hidebysig instance void 
          WriteAttribute(string localName,
                         string ns,
                         uint8[] 'value') cil managed
  // SIG: 20 03 01 0E 0E 1D 05
  {
    .custom /*0C00018D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000207*/ 
    .custom /*0C00019B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2651
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteAttribute

  .method /*060001DB*/ family hidebysig instance void 
          WriteAttribute(string localName,
                         string ns,
                         string 'value') cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .param [1]/*08000208*/ 
    .custom /*0C00019D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2653
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteAttribute

  .method /*060001DC*/ family hidebysig instance void 
          WriteAttribute(string prefix,
                         string localName,
                         string ns,
                         string 'value') cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    .param [2]/*0800020C*/ 
    .custom /*0C0001A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2655
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteAttribute

  .method /*060001DD*/ family hidebysig instance void 
          WriteElementEncoded(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000042*/ node,
                              string name,
                              string ns,
                              bool isNullable,
                              bool 'any') cil managed
  // SIG: 20 05 01 12 81 09 0E 0E 02 02
  {
    .param [2]/*08000210*/ 
    .custom /*0C0001A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2657
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementEncoded

  .method /*060001DE*/ family hidebysig instance void 
          WriteElementLiteral(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000042*/ node,
                              string name,
                              string ns,
                              bool isNullable,
                              bool 'any') cil managed
  // SIG: 20 05 01 12 81 09 0E 0E 02 02
  {
    .param [2]/*08000215*/ 
    .custom /*0C0001A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2659
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementLiteral

  .method /*060001DF*/ family hidebysig instance void 
          WriteElementQualifiedName(string localName,
                                    string ns,
                                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value') cil managed
  // SIG: 20 03 01 0E 0E 12 80 85
  {
    .param [1]/*08000219*/ 
    .custom /*0C0001A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x265b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementQualifiedName

  .method /*060001E0*/ family hidebysig instance void 
          WriteElementQualifiedName(string localName,
                                    string ns,
                                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value',
                                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 12 80 85 12 80 85
  {
    .param [1]/*0800021C*/ 
    .custom /*0C0001A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x265d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementQualifiedName

  .method /*060001E1*/ family hidebysig instance void 
          WriteElementQualifiedName(string localName,
                                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value') cil managed
  // SIG: 20 02 01 0E 12 80 85
  {
    .custom /*0C00018E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000221*/ 
    .custom /*0C0001AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x265f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementQualifiedName

  .method /*060001E2*/ family hidebysig instance void 
          WriteElementQualifiedName(string localName,
                                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value',
                                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 03 01 0E 12 80 85 12 80 85
  {
    .param [1]/*08000222*/ 
    .custom /*0C0001AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2661
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementQualifiedName

  .method /*060001E3*/ family hidebysig instance void 
          WriteElementString(string localName,
                             string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C00018F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000226*/ 
    .custom /*0C0001AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2663
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementString

  .method /*060001E4*/ family hidebysig instance void 
          WriteElementString(string localName,
                             string ns,
                             string 'value') cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .param [1]/*08000227*/ 
    .custom /*0C0001AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2665
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementString

  .method /*060001E5*/ family hidebysig instance void 
          WriteElementString(string localName,
                             string ns,
                             string 'value',
                             class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 0E 12 80 85
  {
    .param [1]/*0800022A*/ 
    .custom /*0C0001B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2667
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementString

  .method /*060001E6*/ family hidebysig instance void 
          WriteElementString(string localName,
                             string 'value',
                             class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 03 01 0E 0E 12 80 85
  {
    .param [1]/*0800022E*/ 
    .custom /*0C0001B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2669
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementString

  .method /*060001E7*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                uint8[] 'value') cil managed
  // SIG: 20 02 01 0E 1D 05
  {
    .custom /*0C000190:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000232*/ 
    .custom /*0C0001B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x266b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001E8*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                uint8[] 'value',
                                class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 03 01 0E 1D 05 12 80 85
  {
    .param [1]/*08000233*/ 
    .custom /*0C0001B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x266d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001E9*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C000192:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000237*/ 
    .custom /*0C0001B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x266f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001EA*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                string ns,
                                uint8[] 'value') cil managed
  // SIG: 20 03 01 0E 0E 1D 05
  {
    .param [1]/*08000238*/ 
    .custom /*0C0001B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2671
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001EB*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                string ns,
                                uint8[] 'value',
                                class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 1D 05 12 80 85
  {
    .param [1]/*0800023B*/ 
    .custom /*0C0001BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2673
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001EC*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                string ns,
                                string 'value') cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .param [1]/*0800023F*/ 
    .custom /*0C0001BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2675
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001ED*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                string ns,
                                string 'value',
                                class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 0E 12 80 85
  {
    .param [1]/*08000242*/ 
    .custom /*0C0001BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2677
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001EE*/ family hidebysig instance void 
          WriteElementStringRaw(string localName,
                                string 'value',
                                class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 03 01 0E 0E 12 80 85
  {
    .param [1]/*08000246*/ 
    .custom /*0C0001C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2679
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteElementStringRaw

  .method /*060001EF*/ family hidebysig instance void 
          WriteEmptyTag(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x267b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteEmptyTag

  .method /*060001F0*/ family hidebysig instance void 
          WriteEmptyTag(string name,
                        string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x267d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteEmptyTag

  .method /*060001F1*/ family hidebysig instance void 
          WriteEndElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x267f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteEndElement

  .method /*060001F2*/ family hidebysig instance void 
          WriteEndElement(object o) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2681
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteEndElement

  .method /*060001F3*/ family hidebysig instance void 
          WriteId(object o) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000193:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2683
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteId

  .method /*060001F4*/ family hidebysig instance void 
          WriteNamespaceDeclarations(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ xmlns) cil managed
  // SIG: 20 01 01 12 81 21
  {
    // Method begins at RVA 0x2685
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNamespaceDeclarations

  .method /*060001F5*/ family hidebysig instance void 
          WriteNullableQualifiedNameEncoded(string name,
                                            string ns,
                                            class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value',
                                            class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 12 80 85 12 80 85
  {
    .param [1]/*0800024F*/ 
    .custom /*0C0001C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2687
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableQualifiedNameEncoded

  .method /*060001F6*/ family hidebysig instance void 
          WriteNullableQualifiedNameLiteral(string name,
                                            string ns,
                                            class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ 'value') cil managed
  // SIG: 20 03 01 0E 0E 12 80 85
  {
    .param [1]/*08000253*/ 
    .custom /*0C0001C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2689
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableQualifiedNameLiteral

  .method /*060001F7*/ family hidebysig instance void 
          WriteNullableStringEncoded(string name,
                                     string ns,
                                     string 'value',
                                     class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 0E 12 80 85
  {
    .param [1]/*08000256*/ 
    .custom /*0C0001C5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x268b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableStringEncoded

  .method /*060001F8*/ family hidebysig instance void 
          WriteNullableStringEncodedRaw(string name,
                                        string ns,
                                        uint8[] 'value',
                                        class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 1D 05 12 80 85
  {
    .param [1]/*0800025A*/ 
    .custom /*0C0001C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x268d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableStringEncodedRaw

  .method /*060001F9*/ family hidebysig instance void 
          WriteNullableStringEncodedRaw(string name,
                                        string ns,
                                        string 'value',
                                        class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlQualifiedName/*01000021*/ xsiType) cil managed
  // SIG: 20 04 01 0E 0E 0E 12 80 85
  {
    .param [1]/*0800025E*/ 
    .custom /*0C0001C7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x268f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableStringEncodedRaw

  .method /*060001FA*/ family hidebysig instance void 
          WriteNullableStringLiteral(string name,
                                     string ns,
                                     string 'value') cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .param [1]/*08000262*/ 
    .custom /*0C0001C8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2691
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableStringLiteral

  .method /*060001FB*/ family hidebysig instance void 
          WriteNullableStringLiteralRaw(string name,
                                        string ns,
                                        uint8[] 'value') cil managed
  // SIG: 20 03 01 0E 0E 1D 05
  {
    .param [1]/*08000265*/ 
    .custom /*0C0001C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2693
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableStringLiteralRaw

  .method /*060001FC*/ family hidebysig instance void 
          WriteNullableStringLiteralRaw(string name,
                                        string ns,
                                        string 'value') cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .param [1]/*08000268*/ 
    .custom /*0C0001CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2695
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullableStringLiteralRaw

  .method /*060001FD*/ family hidebysig instance void 
          WriteNullTagEncoded(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2697
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullTagEncoded

  .method /*060001FE*/ family hidebysig instance void 
          WriteNullTagEncoded(string name,
                              string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2699
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullTagEncoded

  .method /*060001FF*/ family hidebysig instance void 
          WriteNullTagLiteral(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x269b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullTagLiteral

  .method /*06000200*/ family hidebysig instance void 
          WriteNullTagLiteral(string name,
                              string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x269d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteNullTagLiteral

  .method /*06000201*/ family hidebysig instance void 
          WritePotentiallyReferencingElement(string n,
                                             string ns,
                                             object o) cil managed
  // SIG: 20 03 01 0E 0E 1C
  {
    // Method begins at RVA 0x269f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WritePotentiallyReferencingElement

  .method /*06000202*/ family hidebysig instance void 
          WritePotentiallyReferencingElement(string n,
                                             string ns,
                                             object o,
                                             class [System.Runtime/*23000001*/]System.Type/*0100001B*/ ambientType) cil managed
  // SIG: 20 04 01 0E 0E 1C 12 6D
  {
    // Method begins at RVA 0x26a1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WritePotentiallyReferencingElement

  .method /*06000203*/ family hidebysig instance void 
          WritePotentiallyReferencingElement(string n,
                                             string ns,
                                             object o,
                                             class [System.Runtime/*23000001*/]System.Type/*0100001B*/ ambientType,
                                             bool suppressReference) cil managed
  // SIG: 20 05 01 0E 0E 1C 12 6D 02
  {
    .param [1]/*08000278*/ 
    .custom /*0C0001D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26a3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WritePotentiallyReferencingElement

  .method /*06000204*/ family hidebysig instance void 
          WritePotentiallyReferencingElement(string n,
                                             string ns,
                                             object o,
                                             class [System.Runtime/*23000001*/]System.Type/*0100001B*/ ambientType,
                                             bool suppressReference,
                                             bool isNullable) cil managed
  // SIG: 20 06 01 0E 0E 1C 12 6D 02 02
  {
    // Method begins at RVA 0x26a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WritePotentiallyReferencingElement

  .method /*06000205*/ family hidebysig instance void 
          WriteReferencedElements() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26a7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteReferencedElements

  .method /*06000206*/ family hidebysig instance void 
          WriteReferencingElement(string n,
                                  string ns,
                                  object o) cil managed
  // SIG: 20 03 01 0E 0E 1C
  {
    .param [1]/*08000283*/ 
    .custom /*0C0001D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26a9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteReferencingElement

  .method /*06000207*/ family hidebysig instance void 
          WriteReferencingElement(string n,
                                  string ns,
                                  object o,
                                  bool isNullable) cil managed
  // SIG: 20 04 01 0E 0E 1C 02
  {
    .param [1]/*08000286*/ 
    .custom /*0C0001D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26ab
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteReferencingElement

  .method /*06000208*/ family hidebysig instance void 
          WriteRpcResult(string name,
                         string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C00019C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800028B*/ 
    .custom /*0C0001D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteRpcResult

  .method /*06000209*/ family hidebysig instance void 
          WriteSerializable(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000041*/ 'serializable',
                            string name,
                            string ns,
                            bool isNullable) cil managed
  // SIG: 20 04 01 12 81 05 0E 0E 02
  {
    .custom /*0C00019E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800028C*/ 
    .custom /*0C0001D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteSerializable

  .method /*0600020A*/ family hidebysig instance void 
          WriteSerializable(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000041*/ 'serializable',
                            string name,
                            string ns,
                            bool isNullable,
                            bool wrapped) cil managed
  // SIG: 20 05 01 12 81 05 0E 0E 02 02
  {
    .param [2]/*08000291*/ 
    .custom /*0C0001D7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26b1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteSerializable

  .method /*0600020B*/ family hidebysig instance void 
          WriteStartDocument() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26b3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartDocument

  .method /*0600020C*/ family hidebysig instance void 
          WriteStartElement(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00019F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26b5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartElement

  .method /*0600020D*/ family hidebysig instance void 
          WriteStartElement(string name,
                            string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C0001A1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000297*/ 
    .custom /*0C0001D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26b7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartElement

  .method /*0600020E*/ family hidebysig instance void 
          WriteStartElement(string name,
                            string ns,
                            bool writePrefixed) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    .custom /*0C0001A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000299*/ 
    .custom /*0C0001D9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartElement

  .method /*0600020F*/ family hidebysig instance void 
          WriteStartElement(string name,
                            string ns,
                            object o) cil managed
  // SIG: 20 03 01 0E 0E 1C
  {
    .param [1]/*0800029B*/ 
    .custom /*0C0001DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartElement

  .method /*06000210*/ family hidebysig instance void 
          WriteStartElement(string name,
                            string ns,
                            object o,
                            bool writePrefixed) cil managed
  // SIG: 20 04 01 0E 0E 1C 02
  {
    .param [1]/*0800029E*/ 
    .custom /*0C0001DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26bd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartElement

  .method /*06000211*/ family hidebysig instance void 
          WriteStartElement(string name,
                            string ns,
                            object o,
                            bool writePrefixed,
                            class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ xmlns) cil managed
  // SIG: 20 05 01 0E 0E 1C 02 12 81 21
  {
    .param [1]/*080002A2*/ 
    .custom /*0C0001DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26bf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteStartElement

  .method /*06000212*/ family hidebysig instance void 
          WriteTypedPrimitive(string name,
                              string ns,
                              object o,
                              bool xsiType) cil managed
  // SIG: 20 04 01 0E 0E 1C 02
  {
    .param [3]/*080002A9*/ 
    .custom /*0C0001DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteTypedPrimitive

  .method /*06000213*/ family hidebysig instance void 
          WriteValue(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x26c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteValue

  .method /*06000214*/ family hidebysig instance void 
          WriteValue(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x26c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteValue

  .method /*06000215*/ family hidebysig instance void 
          WriteXmlAttribute(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000042*/ node) cil managed
  // SIG: 20 01 01 12 81 09
  {
    .custom /*0C0001A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteXmlAttribute

  .method /*06000216*/ family hidebysig instance void 
          WriteXmlAttribute(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNode/*01000042*/ node,
                            object container) cil managed
  // SIG: 20 02 01 12 81 09 1C
  {
    .custom /*0C0001A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080002AF*/ 
    .custom /*0C0001DE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26c9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteXmlAttribute

  .method /*06000217*/ family hidebysig instance void 
          WriteXsiType(string name,
                       string ns) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C0001A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080002B1*/ 
    .custom /*0C0001DF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializationWriter::WriteXsiType

  .property /*17000075*/ instance callconv(8) bool
          EscapeName()
  {
    .get instance bool System.Xml.Serialization.XmlSerializationWriter/*02000038*/::get_EscapeName() /* 060001B7 */
    .set instance void System.Xml.Serialization.XmlSerializationWriter/*02000038*/::set_EscapeName(bool) /* 060001B8 */
  } // end of property XmlSerializationWriter::EscapeName
  .property /*17000076*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000046*/
          Namespaces()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000046*/ System.Xml.Serialization.XmlSerializationWriter/*02000038*/::get_Namespaces() /* 060001B9 */
    .set instance void System.Xml.Serialization.XmlSerializationWriter/*02000038*/::set_Namespaces(class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000046*/) /* 060001BA */
  } // end of property XmlSerializationWriter::Namespaces
  .property /*17000077*/ instance callconv(8) class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/
          Writer()
  {
    .custom /*0C0000DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ System.Xml.Serialization.XmlSerializationWriter/*02000038*/::get_Writer() /* 060001BB */
    .set instance void System.Xml.Serialization.XmlSerializationWriter/*02000038*/::set_Writer(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/) /* 060001BC */
  } // end of property XmlSerializationWriter::Writer
} // end of class System.Xml.Serialization.XmlSerializationWriter

.class /*02000039*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlSerializer
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C0000A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000218*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26cd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*06000219*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x26d5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*0600021A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                               string defaultNamespace) cil managed
  // SIG: 20 02 01 12 6D 0E
  {
    .param [2]/*080002B4*/ 
    .custom /*0C0001E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26dd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*0600021B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                               class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] extraTypes) cil managed
  // SIG: 20 02 01 12 6D 1D 12 6D
  {
    .param [2]/*080002B6*/ 
    .custom /*0C0001E1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x26e5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*0600021C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                               class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ overrides) cil managed
  // SIG: 20 02 01 12 6D 12 78
  {
    .param [2]/*080002B8*/ 
    .custom /*0C0001E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26ed
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*0600021D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                               class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ overrides,
                               class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] extraTypes,
                               class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root,
                               string defaultNamespace) cil managed
  // SIG: 20 05 01 12 6D 12 78 1D 12 6D 12 80 B1 0E
  {
    .custom /*0C0001AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002B9*/ 
    .custom /*0C0001E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080002BB*/ 
    .custom /*0C0001E4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x26f5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*0600021E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                               class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ overrides,
                               class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] extraTypes,
                               class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root,
                               string defaultNamespace,
                               string location) cil managed
  // SIG: 20 06 01 12 6D 12 78 1D 12 6D 12 80 B1 0E 0E
  {
    .custom /*0C0001AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002BE*/ 
    .custom /*0C0001E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080002C0*/ 
    .custom /*0C0001E6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x26fd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*0600021F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                               class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root) cil managed
  // SIG: 20 02 01 12 6D 12 80 B1
  {
    .param [2]/*080002C5*/ 
    .custom /*0C0001E7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2705
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*06000220*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ xmlTypeMapping) cil managed
  // SIG: 20 01 01 12 80 FC
  {
    // Method begins at RVA 0x270d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::.ctor

  .method /*06000221*/ public hidebysig specialname 
          instance void  add_UnknownAttribute(class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/ 'value') cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x2715
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::add_UnknownAttribute

  .method /*06000222*/ public hidebysig specialname 
          instance void  remove_UnknownAttribute(class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/ 'value') cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x2717
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::remove_UnknownAttribute

  .method /*06000223*/ public hidebysig specialname 
          instance void  add_UnknownElement(class System.Xml.Serialization.XmlElementEventHandler/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x2719
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::add_UnknownElement

  .method /*06000224*/ public hidebysig specialname 
          instance void  remove_UnknownElement(class System.Xml.Serialization.XmlElementEventHandler/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x271b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::remove_UnknownElement

  .method /*06000225*/ public hidebysig specialname 
          instance void  add_UnknownNode(class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AC
  {
    // Method begins at RVA 0x271d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::add_UnknownNode

  .method /*06000226*/ public hidebysig specialname 
          instance void  remove_UnknownNode(class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AC
  {
    // Method begins at RVA 0x271f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::remove_UnknownNode

  .method /*06000227*/ public hidebysig specialname 
          instance void  add_UnreferencedObject(class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x2721
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::add_UnreferencedObject

  .method /*06000228*/ public hidebysig specialname 
          instance void  remove_UnreferencedObject(class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x2723
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::remove_UnreferencedObject

  .method /*06000229*/ public hidebysig newslot virtual 
          instance bool  CanDeserialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ xmlReader) cil managed
  // SIG: 20 01 02 12 80 F1
  {
    // Method begins at RVA 0x2725
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::CanDeserialize

  .method /*0600022A*/ family hidebysig newslot virtual 
          instance class System.Xml.Serialization.XmlSerializationReader/*02000036*/ 
          CreateReader() cil managed
  // SIG: 20 00 12 80 D8
  {
    // Method begins at RVA 0x2728
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::CreateReader

  .method /*0600022B*/ family hidebysig newslot virtual 
          instance class System.Xml.Serialization.XmlSerializationWriter/*02000038*/ 
          CreateWriter() cil managed
  // SIG: 20 00 12 80 E0
  {
    // Method begins at RVA 0x272b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::CreateWriter

  .method /*0600022C*/ public hidebysig instance object 
          Deserialize(class [System.Runtime/*23000001*/]System.IO.Stream/*01000049*/ 'stream') cil managed
  // SIG: 20 01 1C 12 81 25
  {
    .param [0]/*080002D0*/ 
    .custom /*0C0001E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x272e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*0600022D*/ public hidebysig instance object 
          Deserialize(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100004A*/ textReader) cil managed
  // SIG: 20 01 1C 12 81 29
  {
    .param [0]/*080002D2*/ 
    .custom /*0C0001E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2731
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*0600022E*/ family hidebysig newslot virtual 
          instance object  Deserialize(class System.Xml.Serialization.XmlSerializationReader/*02000036*/ reader) cil managed
  // SIG: 20 01 1C 12 80 D8
  {
    // Method begins at RVA 0x2734
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*0600022F*/ public hidebysig instance object 
          Deserialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ xmlReader) cil managed
  // SIG: 20 01 1C 12 80 F1
  {
    .param [0]/*080002D5*/ 
    .custom /*0C0001EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2737
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*06000230*/ public hidebysig instance object 
          Deserialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ xmlReader,
                      string encodingStyle) cil managed
  // SIG: 20 02 1C 12 80 F1 0E
  {
    .custom /*0C0001B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002D7*/ 
    .custom /*0C0001EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x273a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*06000231*/ public hidebysig instance object 
          Deserialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ xmlReader,
                      string encodingStyle,
                      valuetype System.Xml.Serialization.XmlDeserializationEvents/*02000021*/ events) cil managed
  // SIG: 20 03 1C 12 80 F1 0E 11 80 84
  {
    .custom /*0C0001B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002D9*/ 
    .custom /*0C0001EC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x273d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*06000232*/ public hidebysig instance object 
          Deserialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100003C*/ xmlReader,
                      valuetype System.Xml.Serialization.XmlDeserializationEvents/*02000021*/ events) cil managed
  // SIG: 20 02 1C 12 80 F1 11 80 84
  {
    .param [0]/*080002DC*/ 
    .custom /*0C0001ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2740
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::Deserialize

  .method /*06000233*/ public hidebysig static 
          class System.Xml.Serialization.XmlSerializer/*02000039*/[] 
          FromMappings(class System.Xml.Serialization.XmlMapping/*02000026*/[] mappings) cil managed
  // SIG: 00 01 1D 12 80 E4 1D 12 80 98
  {
    .param [1]/*080002DF*/ 
    .custom /*0C0001EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2743
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::FromMappings

  .method /*06000234*/ public hidebysig static 
          class System.Xml.Serialization.XmlSerializer/*02000039*/[] 
          FromMappings(class System.Xml.Serialization.XmlMapping/*02000026*/[] mappings,
                       class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 00 02 1D 12 80 E4 1D 12 80 98 12 6D
  {
    .param [1]/*080002E0*/ 
    .custom /*0C0001EF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*080002E1*/ 
    .custom /*0C0001F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2746
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::FromMappings

  .method /*06000235*/ public hidebysig static 
          class System.Xml.Serialization.XmlSerializer/*02000039*/[] 
          FromTypes(class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] types) cil managed
  // SIG: 00 01 1D 12 80 E4 1D 12 6D
  {
    .param [1]/*080002E2*/ 
    .custom /*0C0001F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2749
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::FromTypes

  .method /*06000236*/ public hidebysig static 
          string  GetXmlSerializerAssemblyName(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 00 01 0E 12 6D
  {
    // Method begins at RVA 0x274c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::GetXmlSerializerAssemblyName

  .method /*06000237*/ public hidebysig static 
          string  GetXmlSerializerAssemblyName(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                                               string defaultNamespace) cil managed
  // SIG: 00 02 0E 12 6D 0E
  {
    .param [2]/*080002E5*/ 
    .custom /*0C0001F2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x274f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializer::GetXmlSerializerAssemblyName

  .method /*06000238*/ public hidebysig instance void 
          Serialize(class [System.Runtime/*23000001*/]System.IO.Stream/*01000049*/ 'stream',
                    object o) cil managed
  // SIG: 20 02 01 12 81 25 1C
  {
    .param [2]/*080002E7*/ 
    .custom /*0C0001F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2752
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*06000239*/ public hidebysig instance void 
          Serialize(class [System.Runtime/*23000001*/]System.IO.Stream/*01000049*/ 'stream',
                    object o,
                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ namespaces) cil managed
  // SIG: 20 03 01 12 81 25 1C 12 81 21
  {
    .custom /*0C0001B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002E8*/ 
    .custom /*0C0001F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2754
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*0600023A*/ public hidebysig instance void 
          Serialize(class [System.Runtime/*23000001*/]System.IO.TextWriter/*0100004B*/ textWriter,
                    object o) cil managed
  // SIG: 20 02 01 12 81 2D 1C
  {
    .param [2]/*080002EC*/ 
    .custom /*0C0001F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2756
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*0600023B*/ public hidebysig instance void 
          Serialize(class [System.Runtime/*23000001*/]System.IO.TextWriter/*0100004B*/ textWriter,
                    object o,
                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ namespaces) cil managed
  // SIG: 20 03 01 12 81 2D 1C 12 81 21
  {
    .custom /*0C0001B9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002ED*/ 
    .custom /*0C0001F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2758
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*0600023C*/ family hidebysig newslot virtual 
          instance void  Serialize(object o,
                                   class System.Xml.Serialization.XmlSerializationWriter/*02000038*/ writer) cil managed
  // SIG: 20 02 01 1C 12 80 E0
  {
    .param [1]/*080002F0*/ 
    .custom /*0C0001F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x275a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*0600023D*/ public hidebysig instance void 
          Serialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ xmlWriter,
                    object o) cil managed
  // SIG: 20 02 01 12 81 1D 1C
  {
    .param [2]/*080002F3*/ 
    .custom /*0C0001F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x275c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*0600023E*/ public hidebysig instance void 
          Serialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ xmlWriter,
                    object o,
                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ namespaces) cil managed
  // SIG: 20 03 01 12 81 1D 1C 12 81 21
  {
    .custom /*0C0001BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002F4*/ 
    .custom /*0C0001F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x275e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*0600023F*/ public hidebysig instance void 
          Serialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ xmlWriter,
                    object o,
                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ namespaces,
                    string encodingStyle) cil managed
  // SIG: 20 04 01 12 81 1D 1C 12 81 21 0E
  {
    .custom /*0C0001BC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002F7*/ 
    .custom /*0C0001FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2760
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .method /*06000240*/ public hidebysig instance void 
          Serialize(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000047*/ xmlWriter,
                    object o,
                    class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSerializerNamespaces/*01000048*/ namespaces,
                    string encodingStyle,
                    string id) cil managed
  // SIG: 20 05 01 12 81 1D 1C 12 81 21 0E 0E
  {
    .custom /*0C0001BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002FB*/ 
    .custom /*0C0001FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2762
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializer::Serialize

  .event /*14000001*/ System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/ UnknownAttribute
  {
    .addon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::add_UnknownAttribute(class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/) /* 06000221 */
    .removeon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::remove_UnknownAttribute(class System.Xml.Serialization.XmlAttributeEventHandler/*0200001D*/) /* 06000222 */
  } // end of event XmlSerializer::UnknownAttribute
  .event /*14000002*/ System.Xml.Serialization.XmlElementEventHandler/*02000024*/ UnknownElement
  {
    .addon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::add_UnknownElement(class System.Xml.Serialization.XmlElementEventHandler/*02000024*/) /* 06000223 */
    .removeon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::remove_UnknownElement(class System.Xml.Serialization.XmlElementEventHandler/*02000024*/) /* 06000224 */
  } // end of event XmlSerializer::UnknownElement
  .event /*14000003*/ System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/ UnknownNode
  {
    .addon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::add_UnknownNode(class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/) /* 06000225 */
    .removeon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::remove_UnknownNode(class System.Xml.Serialization.XmlNodeEventHandler/*0200002B*/) /* 06000226 */
  } // end of event XmlSerializer::UnknownNode
  .event /*14000004*/ System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/ UnreferencedObject
  {
    .addon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::add_UnreferencedObject(class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/) /* 06000227 */
    .removeon instance void System.Xml.Serialization.XmlSerializer/*02000039*/::remove_UnreferencedObject(class System.Xml.Serialization.UnreferencedObjectEventHandler/*02000017*/) /* 06000228 */
  } // end of event XmlSerializer::UnreferencedObject
} // end of class System.Xml.Serialization.XmlSerializer

.class /*0200003A*/ public auto ansi sealed beforefieldinit System.Xml.Serialization.XmlSerializerAssemblyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C0000A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000AB:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 1C 04 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*06000241*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2764
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerAssemblyAttribute::.ctor

  .method /*06000242*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string assemblyName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x276c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerAssemblyAttribute::.ctor

  .method /*06000243*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string assemblyName,
                               string codeBase) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2774
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerAssemblyAttribute::.ctor

  .method /*06000244*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x277c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerAssemblyAttribute::get_AssemblyName

  .method /*06000245*/ public hidebysig specialname 
          instance void  set_AssemblyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x277f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializerAssemblyAttribute::set_AssemblyName

  .method /*06000246*/ public hidebysig specialname 
          instance string  get_CodeBase() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2781
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerAssemblyAttribute::get_CodeBase

  .method /*06000247*/ public hidebysig specialname 
          instance void  set_CodeBase(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2784
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializerAssemblyAttribute::set_CodeBase

  .property /*17000078*/ instance callconv(8) string
          AssemblyName()
  {
    .get instance string System.Xml.Serialization.XmlSerializerAssemblyAttribute/*0200003A*/::get_AssemblyName() /* 06000244 */
    .set instance void System.Xml.Serialization.XmlSerializerAssemblyAttribute/*0200003A*/::set_AssemblyName(string) /* 06000245 */
  } // end of property XmlSerializerAssemblyAttribute::AssemblyName
  .property /*17000079*/ instance callconv(8) string
          CodeBase()
  {
    .get instance string System.Xml.Serialization.XmlSerializerAssemblyAttribute/*0200003A*/::get_CodeBase() /* 06000246 */
    .set instance void System.Xml.Serialization.XmlSerializerAssemblyAttribute/*0200003A*/::set_CodeBase(string) /* 06000247 */
  } // end of property XmlSerializerAssemblyAttribute::CodeBase
} // end of class System.Xml.Serialization.XmlSerializerAssemblyAttribute

.class /*0200003B*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlSerializerFactory
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000248*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2786
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerFactory::.ctor

  .method /*06000249*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 80 E4 12 6D
  {
    // Method begins at RVA 0x278e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*0600024A*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           string defaultNamespace) cil managed
  // SIG: 20 02 12 80 E4 12 6D 0E
  {
    .param [2]/*08000307*/ 
    .custom /*0C0001FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2791
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*0600024B*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] extraTypes) cil managed
  // SIG: 20 02 12 80 E4 12 6D 1D 12 6D
  {
    .param [2]/*08000309*/ 
    .custom /*0C0001FD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2794
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*0600024C*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ overrides) cil managed
  // SIG: 20 02 12 80 E4 12 6D 12 78
  {
    .param [2]/*0800030B*/ 
    .custom /*0C0001FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2797
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*0600024D*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ overrides,
                           class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] extraTypes,
                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root,
                           string defaultNamespace) cil managed
  // SIG: 20 05 12 80 E4 12 6D 12 78 1D 12 6D 12 80 B1 0E
  {
    .custom /*0C0001C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800030C*/ 
    .custom /*0C0001FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800030D*/ 
    .custom /*0C000200:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800030F*/ 
    .custom /*0C000201:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x279a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*0600024E*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           class System.Xml.Serialization.XmlAttributeOverrides/*0200001E*/ overrides,
                           class [System.Runtime/*23000001*/]System.Type/*0100001B*/[] extraTypes,
                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root,
                           string defaultNamespace,
                           string location) cil managed
  // SIG: 20 06 12 80 E4 12 6D 12 78 1D 12 6D 12 80 B1 0E 0E
  {
    .custom /*0C0001C2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000312*/ 
    .custom /*0C000202:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000313*/ 
    .custom /*0C000203:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000315*/ 
    .custom /*0C000204:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x279d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*0600024F*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type',
                           class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlRootAttribute/*0100002C*/ root) cil managed
  // SIG: 20 02 12 80 E4 12 6D 12 80 B1
  {
    .param [2]/*0800031A*/ 
    .custom /*0C000205:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

  .method /*06000250*/ public hidebysig instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          CreateSerializer(class System.Xml.Serialization.XmlTypeMapping/*0200003F*/ xmlTypeMapping) cil managed
  // SIG: 20 01 12 80 E4 12 80 FC
  {
    // Method begins at RVA 0x27a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerFactory::CreateSerializer

} // end of class System.Xml.Serialization.XmlSerializerFactory

.class /*0200003C*/ public abstract auto ansi beforefieldinit System.Xml.Serialization.XmlSerializerImplementation
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000251*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27a6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerImplementation::.ctor

  .method /*06000252*/ public hidebysig newslot specialname virtual 
          instance class System.Xml.Serialization.XmlSerializationReader/*02000036*/ 
          get_Reader() cil managed
  // SIG: 20 00 12 80 D8
  {
    // Method begins at RVA 0x27ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::get_Reader

  .method /*06000253*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ 
          get_ReadMethods() cil managed
  // SIG: 20 00 12 81 15
  {
    // Method begins at RVA 0x27b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::get_ReadMethods

  .method /*06000254*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ 
          get_TypedSerializers() cil managed
  // SIG: 20 00 12 81 15
  {
    // Method begins at RVA 0x27b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::get_TypedSerializers

  .method /*06000255*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ 
          get_WriteMethods() cil managed
  // SIG: 20 00 12 81 15
  {
    // Method begins at RVA 0x27b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::get_WriteMethods

  .method /*06000256*/ public hidebysig newslot specialname virtual 
          instance class System.Xml.Serialization.XmlSerializationWriter/*02000038*/ 
          get_Writer() cil managed
  // SIG: 20 00 12 80 E0
  {
    // Method begins at RVA 0x27ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::get_Writer

  .method /*06000257*/ public hidebysig newslot virtual 
          instance bool  CanSerialize(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 02 12 6D
  {
    // Method begins at RVA 0x27bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::CanSerialize

  .method /*06000258*/ public hidebysig newslot virtual 
          instance class System.Xml.Serialization.XmlSerializer/*02000039*/ 
          GetSerializer(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 12 80 E4 12 6D
  {
    // Method begins at RVA 0x27c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerImplementation::GetSerializer

  .property /*1700007A*/ instance callconv(8) class System.Xml.Serialization.XmlSerializationReader/*02000036*/
          Reader()
  {
    .get instance class System.Xml.Serialization.XmlSerializationReader/*02000036*/ System.Xml.Serialization.XmlSerializerImplementation/*0200003C*/::get_Reader() /* 06000252 */
  } // end of property XmlSerializerImplementation::Reader
  .property /*1700007B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/
          ReadMethods()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ System.Xml.Serialization.XmlSerializerImplementation/*0200003C*/::get_ReadMethods() /* 06000253 */
  } // end of property XmlSerializerImplementation::ReadMethods
  .property /*1700007C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/
          TypedSerializers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ System.Xml.Serialization.XmlSerializerImplementation/*0200003C*/::get_TypedSerializers() /* 06000254 */
  } // end of property XmlSerializerImplementation::TypedSerializers
  .property /*1700007D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/
          WriteMethods()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Hashtable/*01000045*/ System.Xml.Serialization.XmlSerializerImplementation/*0200003C*/::get_WriteMethods() /* 06000255 */
  } // end of property XmlSerializerImplementation::WriteMethods
  .property /*1700007E*/ instance callconv(8) class System.Xml.Serialization.XmlSerializationWriter/*02000038*/
          Writer()
  {
    .get instance class System.Xml.Serialization.XmlSerializationWriter/*02000038*/ System.Xml.Serialization.XmlSerializerImplementation/*0200003C*/::get_Writer() /* 06000256 */
  } // end of property XmlSerializerImplementation::Writer
} // end of class System.Xml.Serialization.XmlSerializerImplementation

.class /*0200003D*/ public auto ansi sealed beforefieldinit System.Xml.Serialization.XmlSerializerVersionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B5:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000259*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27c3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerVersionAttribute::.ctor

  .method /*0600025A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'type') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x27cb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlSerializerVersionAttribute::.ctor

  .method /*0600025B*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x27d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerVersionAttribute::get_Namespace

  .method /*0600025C*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x27d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializerVersionAttribute::set_Namespace

  .method /*0600025D*/ public hidebysig specialname 
          instance string  get_ParentAssemblyId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x27d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerVersionAttribute::get_ParentAssemblyId

  .method /*0600025E*/ public hidebysig specialname 
          instance void  set_ParentAssemblyId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x27db
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializerVersionAttribute::set_ParentAssemblyId

  .method /*0600025F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 
          get_Type() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x27dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerVersionAttribute::get_Type

  .method /*06000260*/ public hidebysig specialname 
          instance void  set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x27e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializerVersionAttribute::set_Type

  .method /*06000261*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x27e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlSerializerVersionAttribute::get_Version

  .method /*06000262*/ public hidebysig specialname 
          instance void  set_Version(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x27e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlSerializerVersionAttribute::set_Version

  .property /*1700007F*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::get_Namespace() /* 0600025B */
    .set instance void System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::set_Namespace(string) /* 0600025C */
  } // end of property XmlSerializerVersionAttribute::Namespace
  .property /*17000080*/ instance callconv(8) string
          ParentAssemblyId()
  {
    .get instance string System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::get_ParentAssemblyId() /* 0600025D */
    .set instance void System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::set_ParentAssemblyId(string) /* 0600025E */
  } // end of property XmlSerializerVersionAttribute::ParentAssemblyId
  .property /*17000081*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001B*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::get_Type() /* 0600025F */
    .set instance void System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::set_Type(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 06000260 */
  } // end of property XmlSerializerVersionAttribute::Type
  .property /*17000082*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::get_Version() /* 06000261 */
    .set instance void System.Xml.Serialization.XmlSerializerVersionAttribute/*0200003D*/::set_Version(string) /* 06000262 */
  } // end of property XmlSerializerVersionAttribute::Version
} // end of class System.Xml.Serialization.XmlSerializerVersionAttribute

.class /*0200003E*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlTypeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B8:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 1C 04 00 00 00 00 ) 
  .method /*06000263*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlTypeAttribute::.ctor

  .method /*06000264*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x27ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlTypeAttribute::.ctor

  .method /*06000265*/ public hidebysig specialname 
          instance bool  get_AnonymousType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeAttribute::get_AnonymousType

  .method /*06000266*/ public hidebysig specialname 
          instance void  set_AnonymousType(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x27fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlTypeAttribute::set_AnonymousType

  .method /*06000267*/ public hidebysig specialname 
          instance bool  get_IncludeInSchema() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeAttribute::get_IncludeInSchema

  .method /*06000268*/ public hidebysig specialname 
          instance void  set_IncludeInSchema(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x27ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlTypeAttribute::set_IncludeInSchema

  .method /*06000269*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2801
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeAttribute::get_Namespace

  .method /*0600026A*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2804
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlTypeAttribute::set_Namespace

  .method /*0600026B*/ public hidebysig specialname 
          instance string  get_TypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2806
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeAttribute::get_TypeName

  .method /*0600026C*/ public hidebysig specialname 
          instance void  set_TypeName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001CC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000327*/ 
    .custom /*0C000206:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100001E*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2809
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XmlTypeAttribute::set_TypeName

  .property /*17000083*/ instance callconv(8) bool
          AnonymousType()
  {
    .get instance bool System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::get_AnonymousType() /* 06000265 */
    .set instance void System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::set_AnonymousType(bool) /* 06000266 */
  } // end of property XmlTypeAttribute::AnonymousType
  .property /*17000084*/ instance callconv(8) bool
          IncludeInSchema()
  {
    .get instance bool System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::get_IncludeInSchema() /* 06000267 */
    .set instance void System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::set_IncludeInSchema(bool) /* 06000268 */
  } // end of property XmlTypeAttribute::IncludeInSchema
  .property /*17000085*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::get_Namespace() /* 06000269 */
    .set instance void System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::set_Namespace(string) /* 0600026A */
  } // end of property XmlTypeAttribute::Namespace
  .property /*17000086*/ instance callconv(8) string
          TypeName()
  {
    .custom /*0C0000E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::get_TypeName() /* 0600026B */
    .set instance void System.Xml.Serialization.XmlTypeAttribute/*0200003E*/::set_TypeName(string) /* 0600026C */
  } // end of property XmlTypeAttribute::TypeName
} // end of class System.Xml.Serialization.XmlTypeAttribute

.class /*0200003F*/ public auto ansi beforefieldinit System.Xml.Serialization.XmlTypeMapping
       extends System.Xml.Serialization.XmlMapping/*02000026*/
{
  .custom /*0C0000BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600026D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x280b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000E8       */ call       instance void System.Xml.Serialization.XmlMapping/*02000026*/::.ctor() /* 060000E8 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XmlTypeMapping::.ctor

  .method /*0600026E*/ public hidebysig specialname 
          instance string  get_TypeFullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2813
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeMapping::get_TypeFullName

  .method /*0600026F*/ public hidebysig specialname 
          instance string  get_TypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2816
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeMapping::get_TypeName

  .method /*06000270*/ public hidebysig specialname 
          instance string  get_XsdTypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2819
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeMapping::get_XsdTypeName

  .method /*06000271*/ public hidebysig specialname 
          instance string  get_XsdTypeNamespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001CE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x281c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XmlTypeMapping::get_XsdTypeNamespace

  .property /*17000087*/ instance callconv(8) string
          TypeFullName()
  {
    .get instance string System.Xml.Serialization.XmlTypeMapping/*0200003F*/::get_TypeFullName() /* 0600026E */
  } // end of property XmlTypeMapping::TypeFullName
  .property /*17000088*/ instance callconv(8) string
          TypeName()
  {
    .get instance string System.Xml.Serialization.XmlTypeMapping/*0200003F*/::get_TypeName() /* 0600026F */
  } // end of property XmlTypeMapping::TypeName
  .property /*17000089*/ instance callconv(8) string
          XsdTypeName()
  {
    .custom /*0C0000E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlTypeMapping/*0200003F*/::get_XsdTypeName() /* 06000270 */
  } // end of property XmlTypeMapping::XsdTypeName
  .property /*1700008A*/ instance callconv(8) string
          XsdTypeNamespace()
  {
    .custom /*0C0000E7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Xml.Serialization.XmlTypeMapping/*0200003F*/::get_XsdTypeNamespace() /* 06000271 */
  } // end of property XmlTypeMapping::XsdTypeNamespace
} // end of class System.Xml.Serialization.XmlTypeMapping


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
