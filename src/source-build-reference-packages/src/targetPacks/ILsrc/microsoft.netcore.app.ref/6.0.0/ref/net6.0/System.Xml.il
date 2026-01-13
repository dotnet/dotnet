


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
// Time-date stamp:            0xfe51567f
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00004a8a
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
// Checksum:                       0x0000f2f7
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00004a35 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000434] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003600 [0x00002470] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00004968 [0x00000054] address [size] of Debug Directory:           
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
//              0x00002a90 Virtual Size
//              0x00002000 Virtual Address
//              0x00002c00 Size of Raw Data
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
//              0x00000434 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002e00 Pointer to Raw Data
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
//              0x00003400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00004000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000a8c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00004a77 Import Name Table
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
// 0x00002050 [0x00002898] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000048e8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000fb0 Size
//              '#~' Name
//    Stream 2:
//              0x0000101c Offset
//              0x000015c0 Size
//              '#Strings' Name
//    Stream 3:
//              0x000025dc Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000025e0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000025f0 Offset
//              0x000002a8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000008900005407 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 23152
// PE header size       : 512 (496 used)    ( 2.21%)
// PE additional info   : 10659             (46.04%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.31%)
// CLR meta-data size  : 10392              (44.89%)
// CLR additional info : 128                ( 0.55%)
// CLR method headers  : 0                  ( 0.00%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 8.85%)
// Unaccounted          : -659              (-2.85%)

// Num.of PE sections   : 3
//   .text    - 11264
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 10392
//   Module        -    1 (10 bytes)
//   TypeRef       -   19 (114 bytes)
//   MemberRef     -   17 (102 bytes)
//   CustomAttribute-   20 (120 bytes)
//   Security      -    1 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   ExportedType  -  249 (3486 bytes)
//   TypeDef       -    1 (14 bytes)
//   Strings       -  5566 bytes
//   Blobs         -   680 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   172 bytes

// CLR additional info : 128

// CLR method headers : 0
//   Num.of method bodies  - 0
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 0

// Managed code : 0
// No classes defined in this module

// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000002*/ System.Xml.ReaderWriter
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000003*/ System.Xml.XmlSerializer
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000004*/ System.Xml.XPath
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly /*20000001*/ System.Xml
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
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000006*/::.ctor(bool) /* 0A000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 0A 53 79 73 74 65 6D 2E 58 6D 6C 00 00 )    // ...System.Xml..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 0A 53 79 73 74 65 6D 2E 58 6D 6C 00 00 )    // ...System.Xml..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 0A 53 79 73 74 65 6D 2E 58 6D 6C 00 00 )    // ...System.Xml..
  .custom /*0C000014:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000011*/ = {property bool 'SkipVerification' = bool(true)}}
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.class extern /*27000001*/ forwarder System.Xml.ConformanceLevel
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000002*/ forwarder System.Xml.DtdProcessing
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000003*/ forwarder System.Xml.EntityHandling
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000004*/ forwarder System.Xml.Formatting
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000005*/ forwarder System.Xml.IHasXmlNode
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000006*/ forwarder System.Xml.IXmlLineInfo
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000007*/ forwarder System.Xml.IXmlNamespaceResolver
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000008*/ forwarder System.Xml.NamespaceHandling
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000009*/ forwarder System.Xml.NameTable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000A*/ forwarder System.Xml.NewLineHandling
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000B*/ forwarder System.Xml.ReadState
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000C*/ forwarder System.Xml.Resolvers.XmlKnownDtds
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000D*/ forwarder System.Xml.Resolvers.XmlPreloadedResolver
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000E*/ forwarder System.Xml.Schema.IXmlSchemaInfo
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700000F*/ forwarder System.Xml.Schema.ValidationEventArgs
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000010*/ forwarder System.Xml.Schema.ValidationEventHandler
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000011*/ forwarder System.Xml.Schema.XmlAtomicValue
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000012*/ forwarder System.Xml.Schema.XmlSchema
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000013*/ forwarder System.Xml.Schema.XmlSchemaAll
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000014*/ forwarder System.Xml.Schema.XmlSchemaAnnotated
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000015*/ forwarder System.Xml.Schema.XmlSchemaAnnotation
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000016*/ forwarder System.Xml.Schema.XmlSchemaAny
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000017*/ forwarder System.Xml.Schema.XmlSchemaAnyAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000018*/ forwarder System.Xml.Schema.XmlSchemaAppInfo
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000019*/ forwarder System.Xml.Schema.XmlSchemaAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700001A*/ forwarder System.Xml.Schema.XmlSchemaAttributeGroup
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700001B*/ forwarder System.Xml.Schema.XmlSchemaAttributeGroupRef
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700001C*/ forwarder System.Xml.Schema.XmlSchemaChoice
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700001D*/ forwarder System.Xml.Schema.XmlSchemaCollection
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700001E*/ forwarder System.Xml.Schema.XmlSchemaCollectionEnumerator
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700001F*/ forwarder System.Xml.Schema.XmlSchemaCompilationSettings
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000020*/ forwarder System.Xml.Schema.XmlSchemaComplexContent
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000021*/ forwarder System.Xml.Schema.XmlSchemaComplexContentExtension
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000022*/ forwarder System.Xml.Schema.XmlSchemaComplexContentRestriction
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000023*/ forwarder System.Xml.Schema.XmlSchemaComplexType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000024*/ forwarder System.Xml.Schema.XmlSchemaContent
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000025*/ forwarder System.Xml.Schema.XmlSchemaContentModel
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000026*/ forwarder System.Xml.Schema.XmlSchemaContentProcessing
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000027*/ forwarder System.Xml.Schema.XmlSchemaContentType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000028*/ forwarder System.Xml.Schema.XmlSchemaDatatype
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000029*/ forwarder System.Xml.Schema.XmlSchemaDatatypeVariety
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700002A*/ forwarder System.Xml.Schema.XmlSchemaDerivationMethod
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700002B*/ forwarder System.Xml.Schema.XmlSchemaDocumentation
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700002C*/ forwarder System.Xml.Schema.XmlSchemaElement
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700002D*/ forwarder System.Xml.Schema.XmlSchemaEnumerationFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700002E*/ forwarder System.Xml.Schema.XmlSchemaException
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700002F*/ forwarder System.Xml.Schema.XmlSchemaExternal
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000030*/ forwarder System.Xml.Schema.XmlSchemaFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000031*/ forwarder System.Xml.Schema.XmlSchemaForm
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000032*/ forwarder System.Xml.Schema.XmlSchemaFractionDigitsFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000033*/ forwarder System.Xml.Schema.XmlSchemaGroup
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000034*/ forwarder System.Xml.Schema.XmlSchemaGroupBase
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000035*/ forwarder System.Xml.Schema.XmlSchemaGroupRef
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000036*/ forwarder System.Xml.Schema.XmlSchemaIdentityConstraint
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000037*/ forwarder System.Xml.Schema.XmlSchemaImport
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000038*/ forwarder System.Xml.Schema.XmlSchemaInclude
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000039*/ forwarder System.Xml.Schema.XmlSchemaInference
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700003A*/ InferenceOption
{
  .class extern System.Xml.Schema.XmlSchemaInference /*27000039*/ 
}
.class extern /*2700003B*/ forwarder System.Xml.Schema.XmlSchemaInferenceException
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700003C*/ forwarder System.Xml.Schema.XmlSchemaInfo
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700003D*/ forwarder System.Xml.Schema.XmlSchemaKey
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700003E*/ forwarder System.Xml.Schema.XmlSchemaKeyref
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700003F*/ forwarder System.Xml.Schema.XmlSchemaLengthFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000040*/ forwarder System.Xml.Schema.XmlSchemaMaxExclusiveFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000041*/ forwarder System.Xml.Schema.XmlSchemaMaxInclusiveFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000042*/ forwarder System.Xml.Schema.XmlSchemaMaxLengthFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000043*/ forwarder System.Xml.Schema.XmlSchemaMinExclusiveFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000044*/ forwarder System.Xml.Schema.XmlSchemaMinInclusiveFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000045*/ forwarder System.Xml.Schema.XmlSchemaMinLengthFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000046*/ forwarder System.Xml.Schema.XmlSchemaNotation
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000047*/ forwarder System.Xml.Schema.XmlSchemaNumericFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000048*/ forwarder System.Xml.Schema.XmlSchemaObject
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000049*/ forwarder System.Xml.Schema.XmlSchemaObjectCollection
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700004A*/ forwarder System.Xml.Schema.XmlSchemaObjectEnumerator
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700004B*/ forwarder System.Xml.Schema.XmlSchemaObjectTable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700004C*/ forwarder System.Xml.Schema.XmlSchemaParticle
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700004D*/ forwarder System.Xml.Schema.XmlSchemaPatternFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700004E*/ forwarder System.Xml.Schema.XmlSchemaRedefine
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700004F*/ forwarder System.Xml.Schema.XmlSchemaSequence
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000050*/ forwarder System.Xml.Schema.XmlSchemaSet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000051*/ forwarder System.Xml.Schema.XmlSchemaSimpleContent
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000052*/ forwarder System.Xml.Schema.XmlSchemaSimpleContentExtension
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000053*/ forwarder System.Xml.Schema.XmlSchemaSimpleContentRestriction
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000054*/ forwarder System.Xml.Schema.XmlSchemaSimpleType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000055*/ forwarder System.Xml.Schema.XmlSchemaSimpleTypeContent
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000056*/ forwarder System.Xml.Schema.XmlSchemaSimpleTypeList
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000057*/ forwarder System.Xml.Schema.XmlSchemaSimpleTypeRestriction
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000058*/ forwarder System.Xml.Schema.XmlSchemaSimpleTypeUnion
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000059*/ forwarder System.Xml.Schema.XmlSchemaTotalDigitsFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700005A*/ forwarder System.Xml.Schema.XmlSchemaType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700005B*/ forwarder System.Xml.Schema.XmlSchemaUnique
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700005C*/ forwarder System.Xml.Schema.XmlSchemaUse
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700005D*/ forwarder System.Xml.Schema.XmlSchemaValidationException
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700005E*/ forwarder System.Xml.Schema.XmlSchemaValidationFlags
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700005F*/ forwarder System.Xml.Schema.XmlSchemaValidator
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000060*/ forwarder System.Xml.Schema.XmlSchemaValidity
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000061*/ forwarder System.Xml.Schema.XmlSchemaWhiteSpaceFacet
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000062*/ forwarder System.Xml.Schema.XmlSchemaXPath
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000063*/ forwarder System.Xml.Schema.XmlSeverityType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000064*/ forwarder System.Xml.Schema.XmlTypeCode
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000065*/ forwarder System.Xml.Schema.XmlValueGetter
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000066*/ forwarder System.Xml.Serialization.CodeGenerationOptions
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000067*/ forwarder System.Xml.Serialization.CodeIdentifier
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000068*/ forwarder System.Xml.Serialization.CodeIdentifiers
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000069*/ forwarder System.Xml.Serialization.ImportContext
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700006A*/ forwarder System.Xml.Serialization.IXmlSerializable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700006B*/ forwarder System.Xml.Serialization.IXmlTextParser
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700006C*/ forwarder System.Xml.Serialization.SchemaImporter
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700006D*/ forwarder System.Xml.Serialization.SoapAttributeAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700006E*/ forwarder System.Xml.Serialization.SoapAttributeOverrides
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700006F*/ forwarder System.Xml.Serialization.SoapAttributes
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000070*/ forwarder System.Xml.Serialization.SoapElementAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000071*/ forwarder System.Xml.Serialization.SoapEnumAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000072*/ forwarder System.Xml.Serialization.SoapIgnoreAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000073*/ forwarder System.Xml.Serialization.SoapIncludeAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000074*/ forwarder System.Xml.Serialization.SoapReflectionImporter
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000075*/ forwarder System.Xml.Serialization.SoapSchemaMember
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000076*/ forwarder System.Xml.Serialization.SoapTypeAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000077*/ forwarder System.Xml.Serialization.UnreferencedObjectEventArgs
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000078*/ forwarder System.Xml.Serialization.UnreferencedObjectEventHandler
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000079*/ forwarder System.Xml.Serialization.XmlAnyAttributeAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700007A*/ forwarder System.Xml.Serialization.XmlAnyElementAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700007B*/ forwarder System.Xml.Serialization.XmlAnyElementAttributes
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700007C*/ forwarder System.Xml.Serialization.XmlArrayAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700007D*/ forwarder System.Xml.Serialization.XmlArrayItemAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700007E*/ forwarder System.Xml.Serialization.XmlArrayItemAttributes
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700007F*/ forwarder System.Xml.Serialization.XmlAttributeAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000080*/ forwarder System.Xml.Serialization.XmlAttributeEventArgs
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000081*/ forwarder System.Xml.Serialization.XmlAttributeEventHandler
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000082*/ forwarder System.Xml.Serialization.XmlAttributeOverrides
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000083*/ forwarder System.Xml.Serialization.XmlAttributes
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000084*/ forwarder System.Xml.Serialization.XmlChoiceIdentifierAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000085*/ forwarder System.Xml.Serialization.XmlDeserializationEvents
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000086*/ forwarder System.Xml.Serialization.XmlElementAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000087*/ forwarder System.Xml.Serialization.XmlElementAttributes
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000088*/ forwarder System.Xml.Serialization.XmlElementEventArgs
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000089*/ forwarder System.Xml.Serialization.XmlElementEventHandler
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700008A*/ forwarder System.Xml.Serialization.XmlEnumAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700008B*/ forwarder System.Xml.Serialization.XmlIgnoreAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700008C*/ forwarder System.Xml.Serialization.XmlIncludeAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700008D*/ forwarder System.Xml.Serialization.XmlMapping
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700008E*/ forwarder System.Xml.Serialization.XmlMappingAccess
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700008F*/ forwarder System.Xml.Serialization.XmlMemberMapping
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000090*/ forwarder System.Xml.Serialization.XmlMembersMapping
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000091*/ forwarder System.Xml.Serialization.XmlNamespaceDeclarationsAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000092*/ forwarder System.Xml.Serialization.XmlNodeEventArgs
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000093*/ forwarder System.Xml.Serialization.XmlNodeEventHandler
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000094*/ forwarder System.Xml.Serialization.XmlReflectionImporter
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000095*/ forwarder System.Xml.Serialization.XmlReflectionMember
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000096*/ forwarder System.Xml.Serialization.XmlRootAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*27000097*/ forwarder System.Xml.Serialization.XmlSchemaEnumerator
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000098*/ forwarder System.Xml.Serialization.XmlSchemaExporter
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*27000099*/ forwarder System.Xml.Serialization.XmlSchemaImporter
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700009A*/ forwarder System.Xml.Serialization.XmlSchemaProviderAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*2700009B*/ forwarder System.Xml.Serialization.XmlSchemas
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700009C*/ forwarder System.Xml.Serialization.XmlSerializationCollectionFixupCallback
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700009D*/ forwarder System.Xml.Serialization.XmlSerializationFixupCallback
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700009E*/ forwarder System.Xml.Serialization.XmlSerializationGeneratedCode
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*2700009F*/ forwarder System.Xml.Serialization.XmlSerializationReadCallback
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A0*/ forwarder System.Xml.Serialization.XmlSerializationReader
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A1*/ CollectionFixup
{
  .class extern System.Xml.Serialization.XmlSerializationReader /*270000A0*/ 
}
.class extern /*270000A2*/ Fixup
{
  .class extern System.Xml.Serialization.XmlSerializationReader /*270000A0*/ 
}
.class extern /*270000A3*/ forwarder System.Xml.Serialization.XmlSerializationWriteCallback
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A4*/ forwarder System.Xml.Serialization.XmlSerializationWriter
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A5*/ forwarder System.Xml.Serialization.XmlSerializer
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A6*/ forwarder System.Xml.Serialization.XmlSerializerAssemblyAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A7*/ forwarder System.Xml.Serialization.XmlSerializerFactory
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A8*/ forwarder System.Xml.Serialization.XmlSerializerImplementation
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000A9*/ forwarder System.Xml.Serialization.XmlSerializerNamespaces
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000AA*/ forwarder System.Xml.Serialization.XmlSerializerVersionAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000AB*/ forwarder System.Xml.Serialization.XmlTextAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000AC*/ forwarder System.Xml.Serialization.XmlTypeAttribute
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000AD*/ forwarder System.Xml.Serialization.XmlTypeMapping
{
  .assembly extern System.Xml.XmlSerializer /*23000003*/ 
}
.class extern /*270000AE*/ forwarder System.Xml.ValidationType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000AF*/ forwarder System.Xml.WhitespaceHandling
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B0*/ forwarder System.Xml.WriteState
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B1*/ forwarder System.Xml.XmlAttribute
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B2*/ forwarder System.Xml.XmlAttributeCollection
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B3*/ forwarder System.Xml.XmlCDataSection
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B4*/ forwarder System.Xml.XmlCharacterData
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B5*/ forwarder System.Xml.XmlComment
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B6*/ forwarder System.Xml.XmlConvert
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B7*/ forwarder System.Xml.XmlDateTimeSerializationMode
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B8*/ forwarder System.Xml.XmlDeclaration
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000B9*/ forwarder System.Xml.XmlDocument
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000BA*/ forwarder System.Xml.XmlDocumentFragment
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000BB*/ forwarder System.Xml.XmlDocumentType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000BC*/ forwarder System.Xml.XmlElement
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000BD*/ forwarder System.Xml.XmlEntity
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000BE*/ forwarder System.Xml.XmlEntityReference
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000BF*/ forwarder System.Xml.XmlException
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C0*/ forwarder System.Xml.XmlImplementation
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C1*/ forwarder System.Xml.XmlLinkedNode
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C2*/ forwarder System.Xml.XmlNamedNodeMap
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C3*/ forwarder System.Xml.XmlNamespaceManager
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C4*/ forwarder System.Xml.XmlNamespaceScope
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C5*/ forwarder System.Xml.XmlNameTable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C6*/ forwarder System.Xml.XmlNode
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C7*/ forwarder System.Xml.XmlNodeChangedAction
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C8*/ forwarder System.Xml.XmlNodeChangedEventArgs
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000C9*/ forwarder System.Xml.XmlNodeChangedEventHandler
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000CA*/ forwarder System.Xml.XmlNodeList
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000CB*/ forwarder System.Xml.XmlNodeOrder
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000CC*/ forwarder System.Xml.XmlNodeReader
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000CD*/ forwarder System.Xml.XmlNodeType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000CE*/ forwarder System.Xml.XmlNotation
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000CF*/ forwarder System.Xml.XmlOutputMethod
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D0*/ forwarder System.Xml.XmlParserContext
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D1*/ forwarder System.Xml.XmlProcessingInstruction
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D2*/ forwarder System.Xml.XmlQualifiedName
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D3*/ forwarder System.Xml.XmlReader
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D4*/ forwarder System.Xml.XmlReaderSettings
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D5*/ forwarder System.Xml.XmlResolver
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D6*/ forwarder System.Xml.XmlSecureResolver
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D7*/ forwarder System.Xml.XmlSignificantWhitespace
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D8*/ forwarder System.Xml.XmlSpace
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000D9*/ forwarder System.Xml.XmlText
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000DA*/ forwarder System.Xml.XmlTextReader
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000DB*/ forwarder System.Xml.XmlTextWriter
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000DC*/ forwarder System.Xml.XmlTokenizedType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000DD*/ forwarder System.Xml.XmlUrlResolver
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000DE*/ forwarder System.Xml.XmlValidatingReader
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000DF*/ forwarder System.Xml.XmlWhitespace
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E0*/ forwarder System.Xml.XmlWriter
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E1*/ forwarder System.Xml.XmlWriterSettings
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E2*/ forwarder System.Xml.XPath.IXPathNavigable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E3*/ forwarder System.Xml.XPath.XmlCaseOrder
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E4*/ forwarder System.Xml.XPath.XmlDataType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E5*/ forwarder System.Xml.XPath.XmlSortOrder
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E6*/ forwarder System.Xml.XPath.XPathDocument
{
  .assembly extern System.Xml.XPath /*23000004*/ 
}
.class extern /*270000E7*/ forwarder System.Xml.XPath.XPathException
{
  .assembly extern System.Xml.XPath /*23000004*/ 
}
.class extern /*270000E8*/ forwarder System.Xml.XPath.XPathExpression
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000E9*/ forwarder System.Xml.XPath.XPathItem
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000EA*/ forwarder System.Xml.XPath.XPathNamespaceScope
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000EB*/ forwarder System.Xml.XPath.XPathNavigator
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000EC*/ forwarder System.Xml.XPath.XPathNodeIterator
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000ED*/ forwarder System.Xml.XPath.XPathNodeType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000EE*/ forwarder System.Xml.XPath.XPathResultType
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000EF*/ forwarder System.Xml.Xsl.IXsltContextFunction
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F0*/ forwarder System.Xml.Xsl.IXsltContextVariable
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F1*/ forwarder System.Xml.Xsl.XslCompiledTransform
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F2*/ forwarder System.Xml.Xsl.XsltArgumentList
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F3*/ forwarder System.Xml.Xsl.XsltCompileException
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F4*/ forwarder System.Xml.Xsl.XsltContext
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F5*/ forwarder System.Xml.Xsl.XsltException
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F6*/ forwarder System.Xml.Xsl.XsltMessageEncounteredEventArgs
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F7*/ forwarder System.Xml.Xsl.XsltMessageEncounteredEventHandler
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F8*/ forwarder System.Xml.Xsl.XslTransform
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.class extern /*270000F9*/ forwarder System.Xml.Xsl.XsltSettings
{
  .assembly extern System.Xml.ReaderWriter /*23000002*/ 
}
.module System.Xml.dll
// MVID: {89694c92-4496-4368-a420-fdf414b0be55}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000012*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.SkipLocalsInitAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY

// *********** DISASSEMBLY COMPLETE ***********************
