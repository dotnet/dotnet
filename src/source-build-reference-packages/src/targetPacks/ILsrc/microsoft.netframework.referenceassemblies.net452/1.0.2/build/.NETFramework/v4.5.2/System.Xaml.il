
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
// Time-date stamp:            0x5385a777
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x0b
// Minor linker version:           0x00
// Size of code:                   0x00016400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00018206
// Base of code:                   0x00002000
// Base of data:                   0x0001a000
// Image base:                     0x580d0000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0006
// Minor subsystem version:        0x0000
// Size of image:                  0x0001e000
// Size of headers:                0x00000200
// Checksum:                       0x0002abe3
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000181b4 [0x0000004f] address [size] of Import Directory:          
// 0x0001a000 [0x00000444] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00016e00 [0x00003eb0] address [size] of Security Directory:        
// 0x0001c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00000000 [0x00000000] address [size] of Debug Directory:           
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
//              0x00016234 Virtual Size
//              0x00002000 Virtual Address
//              0x00016400 Size of Raw Data
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
//              0x00000444 Virtual Size
//              0x0001a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00016600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0001c000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00016c00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00018000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000208
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000181f6 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 0D 58 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00002348 [0x0000a26c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x0000c5b4 [0x0000bb80] address [size] of Resources Directory:       
// 0x00018134 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00005af0 Size
//              '#~' Name
//    Stream 2:
//              0x00005b5c Offset
//              0x00003300 Size
//              '#Strings' Name
//    Stream 3:
//              0x00008e5c Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x00008e64 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00008e74 Offset
//              0x000013f8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000007892bb4df57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 109744
// PE header size       : 512 (496 used)    ( 0.47%)
// PE additional info   : 17311             (15.77%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.07%)
// CLR meta-data size  : 41580              (37.89%)
// CLR additional info : 48128              (43.85%)
// CLR method headers  : 756                ( 0.69%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 1.87%)
// Unaccounted          : -663              (-0.60%)

// Num.of PE sections   : 3
//   .text    - 91136
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 41580
//   Module        -    1 (10 bytes)
//   TypeDef       -  111 (1554 bytes)    21 interfaces, 1 explicit layout
//   TypeRef       -   87 (522 bytes)
//   MethodDef     -  814 (11396 bytes)   69 abstract, 0 native, 745 bodies
//   FieldDef      -   30 (180 bytes)     0 constant
//   MemberRef     -   36 (216 bytes)
//   ParamDef      -  569 (3414 bytes)
//   MethodImpl    -    2 (12 bytes)
//   Constant      -   21 (126 bytes)
//   CustomAttribute-  232 (1392 bytes)
//   ClassLayout   -    2 (16 bytes)
//   InterfaceImpl -   17 (68 bytes)
//   PropertyMap   -   70 (280 bytes)
//   Property      -  279 (1674 bytes)
//   MethodSemantic-  350 (2100 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    7 (14 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   ExportedType  -    1 (14 bytes)
//   ManifestResource-    1 (12 bytes)
//   NestedClass   -    2 (8 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    1 (6 bytes)
//   FieldRVA      -    3 (18 bytes)
//   GenericParam  -    3 (24 bytes)
//   Strings       - 13056 bytes
//   Blobs         -  5112 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   240 bytes

// CLR additional info : 48128
//   Resources         - 48000

// CLR method headers : 756
//   Num.of method bodies  - 745
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 744

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class ValueSerializer                (public) (abstract) (auto) (ansi) 
// Class MemberDefinition               (public) (abstract) (auto) (ansi) 
// Class PropertyDefinition             (public) (auto) (ansi) 
// Class AcceptedMarkupExtensionExpressionTypeAttribute (public) (auto) (ansi) 
// Class AmbientAttribute               (public) (auto) (ansi) (sealed) 
// Class UsableDuringInitializationAttribute (public) (auto) (ansi) (sealed) 
// Class ConstructorArgumentAttribute   (public) (auto) (ansi) (sealed) 
// Class ContentPropertyAttribute       (public) (auto) (ansi) (sealed) 
// Class ContentWrapperAttribute        (public) (auto) (ansi) (sealed) 
// Class DependsOnAttribute             (public) (auto) (ansi) (sealed) 
// Class MarkupExtensionReturnTypeAttribute (public) (auto) (ansi) (sealed) 
// Class NameScopePropertyAttribute     (public) (auto) (ansi) (sealed) 
// Class RootNamespaceAttribute         (public) (auto) (ansi) (sealed) 
// Class TrimSurroundingWhitespaceAttribute (public) (auto) (ansi) (sealed) 
// Class UidPropertyAttribute           (public) (auto) (ansi) (sealed) 
// Class WhitespaceSignificantCollectionAttribute (public) (auto) (ansi) (sealed) 
// Class XamlSetMarkupExtensionAttribute (public) (auto) (ansi) (sealed) 
// Class XmlLangPropertyAttribute       (public) (auto) (ansi) (sealed) 
// Class XmlnsCompatibleWithAttribute   (public) (auto) (ansi) (sealed) 
// Class XmlnsDefinitionAttribute       (public) (auto) (ansi) (sealed) 
// Class XmlnsPrefixAttribute           (public) (auto) (ansi) (sealed) 
// Class DateTimeValueSerializer        (public) (auto) (ansi) 
// Interface INameScope                     (public) (abstract) (auto) (ansi) 
// Interface INameScopeDictionary           (public) (abstract) (auto) (ansi) 
// Interface IProvideValueTarget            (public) (abstract) (auto) (ansi) 
// Interface IQueryAmbient                  (public) (abstract) (auto) (ansi) 
// Interface IUriContext                    (public) (abstract) (auto) (ansi) 
// Interface IValueSerializerContext        (public) (abstract) (auto) (ansi) 
// Interface IXamlTypeResolver              (public) (abstract) (auto) (ansi) 
// Class XamlSetValueEventArgs          (public) (auto) (ansi) 
// Class XamlSetMarkupExtensionEventArgs (public) (auto) (ansi) 
// Class XamlSetTypeConverterAttribute  (public) (auto) (ansi) (sealed) 
// Class XamlSetTypeConverterEventArgs  (public) (auto) (ansi) 
// Class MarkupExtension                (public) (abstract) (auto) (ansi) 
// Class ArrayExtension                 (public) (auto) (ansi) 
// Class DictionaryKeyPropertyAttribute (public) (auto) (ansi) (sealed) 
// Interface IComponentConnector            (public) (abstract) (auto) (ansi) 
// Class NullExtension                  (public) (auto) (ansi) 
// Class StaticExtension                (public) (auto) (ansi) 
// Class TypeExtension                  (public) (auto) (ansi) 
// Class StaticExtensionConverter       (auto) (ansi) 
// Class TypeExtensionConverter         (auto) (ansi) 
// Class RuntimeNamePropertyAttribute   (public) (auto) (ansi) (sealed) 
// Class AllowedMemberLocations         (public) (auto) (ansi) (sealed) 
// Class AttachableMemberIdentifier     (public) (auto) (ansi) 
// Class AttachablePropertyServices     (public) (abstract) (auto) (ansi) (sealed) 
// Interface IXamlNameResolver              (public) (abstract) (auto) (ansi) 
// Interface IAmbientProvider               (public) (abstract) (auto) (ansi) 
// Interface IXamlSchemaContextProvider     (public) (abstract) (auto) (ansi) 
// Interface IRootObjectProvider            (public) (abstract) (auto) (ansi) 
// Interface IXamlNamespaceResolver         (public) (abstract) (auto) (ansi) 
// Interface IDestinationTypeProvider       (public) (abstract) (auto) (ansi) 
// Interface IXamlLineInfo                  (public) (abstract) (auto) (ansi) 
// Interface IXamlObjectWriterFactory       (public) (abstract) (auto) (ansi) 
// Class XamlObjectEventArgs            (public) (auto) (ansi) 
// Class AmbientPropertyValue           (public) (auto) (ansi) 
// Interface IAttachedPropertyStore         (public) (abstract) (auto) (ansi) 
// Interface INamespacePrefixLookup         (public) (abstract) (auto) (ansi) 
// Class XamlWriter                     (public) (abstract) (auto) (ansi) 
// Interface IXamlLineInfoConsumer          (public) (abstract) (auto) (ansi) 
// Class XamlNodeType                   (public) (auto) (ansi) (sealed) 
// Class XamlReaderSettings             (public) (auto) (ansi) 
// Class XamlObjectWriter               (public) (auto) (ansi) 
// Class XamlWriterSettings             (public) (auto) (ansi) 
// Class XamlObjectWriterSettings       (public) (auto) (ansi) 
// Class XamlReader                     (public) (abstract) (auto) (ansi) 
// Class XamlXmlReader                  (public) (auto) (ansi) 
// Class XamlXmlReaderSettings          (public) (auto) (ansi) 
// Interface IXamlNameProvider              (public) (abstract) (auto) (ansi) 
// Class NameReferenceConverter         (public) (auto) (ansi) 
// Class NamespaceDeclaration           (public) (auto) (ansi) 
// Class XamlException                  (public) (auto) (ansi) 
// Class XamlParseException             (public) (auto) (ansi) 
// Class XamlAccessLevel                (public) (auto) (ansi) 
// Class XamlLoadPermission             (public) (auto) (ansi) (sealed) 
// Interface IXamlIndexingReader            (public) (abstract) (auto) (ansi) 
// Class XamlBackgroundReader           (public) (auto) (ansi) 
// Class XamlNodeList                   (public) (auto) (ansi) 
// Class XamlNodeQueue                  (public) (auto) (ansi) 
// Class Reference                      (public) (auto) (ansi) 
// Class XamlValueConverter`1           (public) (auto) (ansi) 
// Class XamlCollectionKind             (public) (auto) (ansi) (sealed) 
// Class XamlMember                     (public) (auto) (ansi) 
// Class XamlDirective                  (public) (auto) (ansi) 
// Class XamlMemberInvoker              (public) (auto) (ansi) 
// Class ShouldSerializeResult          (public) (auto) (ansi) (sealed) 
// Class XamlTypeInvoker                (public) (auto) (ansi) 
// Class XamlDeferLoadAttribute         (public) (auto) (ansi) (sealed) 
// Class XamlDeferringLoader            (public) (abstract) (auto) (ansi) 
// Class XamlObjectWriterException      (public) (auto) (ansi) 
// Class XamlDuplicateMemberException   (public) (auto) (ansi) 
// Class XamlInternalException          (public) (auto) (ansi) 
// Class XamlSchemaException            (public) (auto) (ansi) 
// Class XamlObjectReaderException      (public) (auto) (ansi) 
// Class XamlXmlWriterException         (public) (auto) (ansi) 
// Class XamlLanguage                   (public) (abstract) (auto) (ansi) (sealed) 
// Class XamlSchemaContext              (public) (auto) (ansi) 
// Class XamlSchemaContextSettings      (public) (auto) (ansi) 
// Class XamlServices                   (public) (abstract) (auto) (ansi) (sealed) 
// Class XamlType                       (public) (auto) (ansi) 
// Class XamlTypeName                   (public) (auto) (ansi) 
// Class XamlTypeTypeConverter          (public) (auto) (ansi) 
// Class XamlObjectReader               (public) (auto) (ansi) 
// Class XamlObjectReaderSettings       (public) (auto) (ansi) 
// Class XamlXmlWriter                  (public) (auto) (ansi) 
// Class XamlXmlWriterSettings          (public) (auto) (ansi) 
// Class XData                          (public) (auto) (ansi) (sealed) 
// Class <PrivateImplementationDetails>{55D021A1-CF45-4D2E-812A-0BA1C23EF42C} (auto) (ansi) 
// Class __StaticArrayInitTypeSize=10   (explicit) (ansi) (sealed) (nested private) 
// Class __StaticArrayInitTypeSize=16   (explicit) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000003*/ System.Xml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ noplatform System.Xaml
{
  .custom /*0C000002:0A000002*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000031*/::.ctor(string) /* 0A000002 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C000003:0A000003*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000032*/::.ctor(string) /* 0A000003 */ = ( 01 00 0F 34 2E 30 2E 33 30 33 31 39 2E 33 34 32   // ...4.0.30319.342
                                                                                                                                                                          31 31 00 00 )                                     // 11..
  .custom /*0C000004:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000033*/::.ctor(string) /* 0A000004 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C000005:06000043*/ instance void System.Windows.Markup.XmlnsDefinitionAttribute/*02000015*/::.ctor(string,
                                                                                                                string) /* 06000043 */ = ( 01 00 2C 68 74 74 70 3A 2F 2F 73 63 68 65 6D 61   // ..,http://schema
                                                                                                                                           73 2E 6D 69 63 72 6F 73 6F 66 74 2E 63 6F 6D 2F   // s.microsoft.com/
                                                                                                                                           77 69 6E 66 78 2F 32 30 30 36 2F 78 61 6D 6C 15   // winfx/2006/xaml.
                                                                                                                                           53 79 73 74 65 6D 2E 57 69 6E 64 6F 77 73 2E 4D   // System.Windows.M
                                                                                                                                           61 72 6B 75 70 00 00 )                            // arkup..
  .custom /*0C000006:0A000005*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000034*/::.ctor(string) /* 0A000005 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000007:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000035*/::.ctor(string) /* 0A000006 */ = ( 01 00 0F 34 2E 30 2E 33 30 33 31 39 2E 33 34 32   // ...4.0.30319.342
                                                                                                                                                                 31 31 00 00 )                                     // 11..
  .custom /*0C000008:0A000007*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*01000036*/::.ctor(string) /* 0A000007 */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000009:0A000008*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000037*/::.ctor(string) /* 0A000008 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C00000A:0A000009*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*01000038*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000B:0A00000A*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000039*/::.ctor(string) /* 0A00000A */ = ( 01 00 0F 53 79 73 74 65 6D 2E 58 61 6D 6C 2E 64   // ...System.Xaml.d
                                                                                                                                                                  6C 6C 00 00 )                                     // ll..
  .custom /*0C00000C:0A00000B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*0100003A*/::.ctor() /* 0A00000B */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C00000D:0A00000C*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.DependencyAttribute/*0100003B*/::.ctor(string,
                                                                                                                                           valuetype [mscorlib/*23000001*/]System.Runtime.CompilerServices.LoadHint/*0100003C*/) /* 0A00000C */ = ( 01 00 09 6D 73 63 6F 72 6C 69 62 2C 01 00 00 00   // ...mscorlib,....
                                                                                                                                                                                                                                                    00 00 ) 
  .custom /*0C00000E:0A00000C*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.DependencyAttribute/*0100003B*/::.ctor(string,
                                                                                                                                           valuetype [mscorlib/*23000001*/]System.Runtime.CompilerServices.LoadHint/*0100003C*/) /* 0A00000C */ = ( 01 00 07 53 79 73 74 65 6D 2C 01 00 00 00 00 00 ) // ...System,......
  .custom /*0C00000F:0A00000C*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.DependencyAttribute/*0100003B*/::.ctor(string,
                                                                                                                                           valuetype [mscorlib/*23000001*/]System.Runtime.CompilerServices.LoadHint/*0100003C*/) /* 0A00000C */ = ( 01 00 0B 53 79 73 74 65 6D 2E 58 6D 6C 2C 02 00   // ...System.Xml,..
                                                                                                                                                                                                                                                    00 00 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000010:0A00000D*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*0100003D*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*0100003D*//DebuggingModes/*0100003E*/) /* 0A00000D */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000011:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*0100003F*/::.ctor(int32) /* 0A00000E */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000012:0A00000F*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*01000040*/::.ctor(string) /* 0A00000F */ = ( 01 00 24 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..$f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 45 63 6D 61 50 75 62 6C 69 63   // evdiv\EcmaPublic
                                                                                                                                                             4B 65 79 2E 73 6E 6B 00 00 )                      // Key.snk..
  .custom /*0C000013:0A000010*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000041*/::.ctor(bool) /* 0A000010 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000014:0A000011*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000042*/::.ctor(bool) /* 0A000011 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000015:0A000012*/ instance void [mscorlib/*23000001*/]System.Security.AllowPartiallyTrustedCallersAttribute/*01000043*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000016:0A000013*/ instance void [mscorlib/*23000001*/]System.Security.SecurityRulesAttribute/*01000044*/::.ctor(valuetype [mscorlib/*23000001*/]System.Security.SecurityRuleSet/*01000045*/) /* 0A000013 */ = ( 01 00 02 01 00 54 02 1B 53 6B 69 70 56 65 72 69   // .....T..SkipVeri
                                                                                                                                                                                                                              66 69 63 61 74 69 6F 6E 49 6E 46 75 6C 6C 54 72   // ficationInFullTr
                                                                                                                                                                                                                              75 73 74 01 )                                     // ust.
  .custom /*0C000017:0A000014*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000046*/::.ctor(string) /* 0A000014 */ = ( 01 00 0F 53 79 73 74 65 6D 2E 58 61 6D 6C 2E 64   // ...System.Xaml.d
                                                                                                                                                           6C 6C 00 00 )                                     // ll..
  .custom /*0C000018:0A000015*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000047*/::.ctor(string) /* 0A000015 */ = ( 01 00 0F 53 79 73 74 65 6D 2E 58 61 6D 6C 2E 64   // ...System.Xaml.d
                                                                                                                                                                 6C 6C 00 00 )                                     // ll..
  .custom /*0C000019:0A000016*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000048*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
  .permissionset reqmin
             = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Execution' = bool(true)},
                class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SkipVerification' = bool(true)}}
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.class extern /*27000001*/ forwarder System.Windows.Markup.ValueSerializerAttribute
{
  .assembly extern System /*23000002*/ 
}
.module System.Xaml.dll
// MVID: {390b5755-0fdd-4303-96d0-1c95cf833571}
.custom /*0C000001:0A000017*/ instance void [mscorlib/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000049*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
.imagebase 0x580d0000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F48E04F9000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi System.Windows.Markup.ValueSerializer
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00001A:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000001*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method ValueSerializer::.ctor

  .method /*06000002*/ public hidebysig newslot virtual 
          instance bool  CanConvertToString(object 'value',
                                            class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 02 1C 12 74
  {
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method ValueSerializer::CanConvertToString

  .method /*06000003*/ public hidebysig newslot virtual 
          instance bool  CanConvertFromString(string 'value',
                                              class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 02 0E 12 74
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method ValueSerializer::CanConvertFromString

  .method /*06000004*/ public hidebysig newslot virtual 
          instance string  ConvertToString(object 'value',
                                           class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 0E 1C 12 74
  {
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method ValueSerializer::ConvertToString

  .method /*06000005*/ public hidebysig newslot virtual 
          instance object  ConvertFromString(string 'value',
                                             class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 1C 0E 12 74
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method ValueSerializer::ConvertFromString

  .method /*06000006*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class [mscorlib/*23000001*/]System.Type/*01000003*/> 
          TypeReferences(object 'value',
                         class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 15 12 09 01 12 0D 1C 12 74
  {
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method ValueSerializer::TypeReferences

  .method /*06000007*/ public hidebysig static 
          class System.Windows.Markup.ValueSerializer/*02000002*/ 
          GetSerializerFor(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 00 01 12 08 12 0D
  {
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method ValueSerializer::GetSerializerFor

  .method /*06000008*/ public hidebysig static 
          class System.Windows.Markup.ValueSerializer/*02000002*/ 
          GetSerializerFor(class [System/*23000002*/]System.ComponentModel.PropertyDescriptor/*01000004*/ descriptor) cil managed
  // SIG: 00 01 12 08 12 11
  {
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method ValueSerializer::GetSerializerFor

  .method /*06000009*/ public hidebysig static 
          class System.Windows.Markup.ValueSerializer/*02000002*/ 
          GetSerializerFor(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type',
                           class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 00 02 12 08 12 0D 12 74
  {
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method ValueSerializer::GetSerializerFor

  .method /*0600000A*/ public hidebysig static 
          class System.Windows.Markup.ValueSerializer/*02000002*/ 
          GetSerializerFor(class [System/*23000002*/]System.ComponentModel.PropertyDescriptor/*01000004*/ descriptor,
                           class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 00 02 12 08 12 11 12 74
  {
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method ValueSerializer::GetSerializerFor

  .method /*0600000B*/ family hidebysig instance class [mscorlib/*23000001*/]System.Exception/*01000005*/ 
          GetConvertToException(object 'value',
                                class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 02 12 15 1C 12 0D
  {
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method ValueSerializer::GetConvertToException

  .method /*0600000C*/ family hidebysig instance class [mscorlib/*23000001*/]System.Exception/*01000005*/ 
          GetConvertFromException(object 'value') cil managed
  // SIG: 20 01 12 15 1C
  {
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method ValueSerializer::GetConvertFromException

  .method /*0600000D*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method ValueSerializer::.cctor

} // end of class System.Windows.Markup.ValueSerializer

.class /*02000003*/ public abstract auto ansi beforefieldinit System.Windows.Markup.MemberDefinition
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600000E*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method MemberDefinition::get_Name

  .method /*0600000F*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method MemberDefinition::set_Name

  .method /*06000010*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method MemberDefinition::.ctor

  .property /*17000001*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.MemberDefinition/*02000003*/::get_Name() /* 0600000E */
    .set instance void System.Windows.Markup.MemberDefinition/*02000003*/::set_Name(string) /* 0600000F */
  } // end of property MemberDefinition::Name
} // end of class System.Windows.Markup.MemberDefinition

.class /*02000004*/ public auto ansi beforefieldinit System.Windows.Markup.PropertyDefinition
       extends System.Windows.Markup.MemberDefinition/*02000003*/
{
  .method /*06000011*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000034:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::get_Name

  .method /*06000012*/ public hidebysig specialname virtual 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000037:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::set_Name

  .method /*06000013*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 20 00 12 81 94
  {
    .custom /*0C000039:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::get_Type

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_Type(class System.Xaml.XamlType/*02000065*/ 'value') cil managed
  // SIG: 20 01 01 12 81 94
  {
    .custom /*0C00003C:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::set_Type

  .method /*06000015*/ public hidebysig specialname 
          instance string  get_Modifier() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00003F:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::get_Modifier

  .method /*06000016*/ public hidebysig specialname 
          instance void  set_Modifier(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000042:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::set_Modifier

  .method /*06000017*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class [mscorlib/*23000001*/]System.Attribute/*01000007*/> 
          get_Attributes() cil managed
  // SIG: 20 00 15 12 19 01 12 1D
  {
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::get_Attributes

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method PropertyDefinition::.ctor

  .property /*17000002*/ instance callconv(8) string
          Name()
  {
    .set instance void System.Windows.Markup.PropertyDefinition/*02000004*/::set_Name(string) /* 06000012 */
    .get instance string System.Windows.Markup.PropertyDefinition/*02000004*/::get_Name() /* 06000011 */
  } // end of property PropertyDefinition::Name
  .property /*17000003*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .custom /*0C00001B:0A00001A*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*0100004C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A00001A */ = ( 01 00 28 53 79 73 74 65 6D 2E 58 61 6D 6C 2E 53   // ..(System.Xaml.S
                                                                                                                                                                                                            63 68 65 6D 61 2E 58 61 6D 6C 54 79 70 65 54 79   // chema.XamlTypeTy
                                                                                                                                                                                                            70 65 43 6F 6E 76 65 72 74 65 72 00 00 )          // peConverter..
    .set instance void System.Windows.Markup.PropertyDefinition/*02000004*/::set_Type(class System.Xaml.XamlType/*02000065*/) /* 06000014 */
    .get instance class System.Xaml.XamlType/*02000065*/ System.Windows.Markup.PropertyDefinition/*02000004*/::get_Type() /* 06000013 */
  } // end of property PropertyDefinition::Type
  .property /*17000004*/ instance callconv(8) string
          Modifier()
  {
    .custom /*0C00001C:0A00001B*/ instance void [System/*23000002*/]System.ComponentModel.DefaultValueAttribute/*0100004D*/::.ctor(string) /* 0A00001B */ = ( 01 00 FF 00 00 ) 
    .set instance void System.Windows.Markup.PropertyDefinition/*02000004*/::set_Modifier(string) /* 06000016 */
    .get instance string System.Windows.Markup.PropertyDefinition/*02000004*/::get_Modifier() /* 06000015 */
  } // end of property PropertyDefinition::Modifier
  .property /*17000005*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class [mscorlib/*23000001*/]System.Attribute/*01000007*/>
          Attributes()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class [mscorlib/*23000001*/]System.Attribute/*01000007*/> System.Windows.Markup.PropertyDefinition/*02000004*/::get_Attributes() /* 06000017 */
  } // end of property PropertyDefinition::Attributes
} // end of class System.Windows.Markup.PropertyDefinition

.class /*02000005*/ public auto ansi beforefieldinit System.Windows.Markup.AcceptedMarkupExtensionExpressionTypeAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00001D:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .custom /*0C00001E:0A00001D*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*01000050*/::.ctor(string) /* 0A00001D */ = ( 01 00 54 54 68 69 73 20 69 73 20 6E 6F 74 20 75   // ..TThis is not u
                                                                                                                                           73 65 64 20 62 79 20 74 68 65 20 58 41 4D 4C 20   // sed by the XAML 
                                                                                                                                           70 61 72 73 65 72 2E 20 50 6C 65 61 73 65 20 6C   // parser. Please l
                                                                                                                                           6F 6F 6B 20 61 74 20 58 61 6D 6C 53 65 74 4D 61   // ook at XamlSetMa
                                                                                                                                           72 6B 75 70 45 78 74 65 6E 73 69 6F 6E 41 74 74   // rkupExtensionAtt
                                                                                                                                           72 69 62 75 74 65 2E 00 00 )                      // ribute...
  .method /*06000019*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_Type() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C000047:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method AcceptedMarkupExtensionExpressionTypeAttribute::get_Type

  .method /*0600001A*/ public hidebysig specialname 
          instance void  set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    .custom /*0C000048:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method AcceptedMarkupExtensionExpressionTypeAttribute::set_Type

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method AcceptedMarkupExtensionExpressionTypeAttribute::.ctor

  .property /*17000006*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          Type()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.AcceptedMarkupExtensionExpressionTypeAttribute/*02000005*/::get_Type() /* 06000019 */
    .set instance void System.Windows.Markup.AcceptedMarkupExtensionExpressionTypeAttribute/*02000005*/::set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600001A */
  } // end of property AcceptedMarkupExtensionExpressionTypeAttribute::Type
} // end of class System.Windows.Markup.AcceptedMarkupExtensionExpressionTypeAttribute

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Windows.Markup.AmbientAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00001F:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000020:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 C4 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 01 )                                  // ited.
  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method AmbientAttribute::.ctor

} // end of class System.Windows.Markup.AmbientAttribute

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Windows.Markup.UsableDuringInitializationAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000021:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool usable) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method UsableDuringInitializationAttribute::.ctor

  .method /*0600001E*/ public hidebysig specialname 
          instance bool  get_Usable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method UsableDuringInitializationAttribute::get_Usable

  .property /*17000007*/ instance callconv(8) bool
          Usable()
  {
    .get instance bool System.Windows.Markup.UsableDuringInitializationAttribute/*02000007*/::get_Usable() /* 0600001E */
  } // end of property UsableDuringInitializationAttribute::Usable
} // end of class System.Windows.Markup.UsableDuringInitializationAttribute

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Windows.Markup.ConstructorArgumentAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000022:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000023:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 80 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 00 )                               // rited.
  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string argumentName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method ConstructorArgumentAttribute::.ctor

  .method /*06000020*/ public hidebysig specialname 
          instance string  get_ArgumentName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method ConstructorArgumentAttribute::get_ArgumentName

  .property /*17000008*/ instance callconv(8) string
          ArgumentName()
  {
    .get instance string System.Windows.Markup.ConstructorArgumentAttribute/*02000008*/::get_ArgumentName() /* 06000020 */
  } // end of property ConstructorArgumentAttribute::ArgumentName
} // end of class System.Windows.Markup.ConstructorArgumentAttribute

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Windows.Markup.ContentPropertyAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000024:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000025:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method ContentPropertyAttribute::.ctor

  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method ContentPropertyAttribute::.ctor

  .method /*06000023*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method ContentPropertyAttribute::get_Name

  .property /*17000009*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.ContentPropertyAttribute/*02000009*/::get_Name() /* 06000023 */
  } // end of property ContentPropertyAttribute::Name
} // end of class System.Windows.Markup.ContentPropertyAttribute

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Windows.Markup.ContentWrapperAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000026:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .custom /*0C000027:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ contentWrapper) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method ContentWrapperAttribute::.ctor

  .method /*06000025*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ContentWrapper() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method ContentWrapperAttribute::get_ContentWrapper

  .method /*06000026*/ public hidebysig specialname virtual 
          instance object  get_TypeId() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method ContentWrapperAttribute::get_TypeId

  .method /*06000027*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method ContentWrapperAttribute::Equals

  .method /*06000028*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method ContentWrapperAttribute::GetHashCode

  .property /*1700000A*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ContentWrapper()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.ContentWrapperAttribute/*0200000A*/::get_ContentWrapper() /* 06000025 */
  } // end of property ContentWrapperAttribute::ContentWrapper
  .property /*1700000B*/ instance callconv(8) object
          TypeId()
  {
    .get instance object System.Windows.Markup.ContentWrapperAttribute/*0200000A*/::get_TypeId() /* 06000026 */
  } // end of property ContentWrapperAttribute::TypeId
} // end of class System.Windows.Markup.ContentWrapperAttribute

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Windows.Markup.DependsOnAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000028:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 C0 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .custom /*0C000029:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method DependsOnAttribute::.ctor

  .method /*0600002A*/ public hidebysig specialname virtual 
          instance object  get_TypeId() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method DependsOnAttribute::get_TypeId

  .method /*0600002B*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method DependsOnAttribute::get_Name

  .property /*1700000C*/ instance callconv(8) object
          TypeId()
  {
    .get instance object System.Windows.Markup.DependsOnAttribute/*0200000B*/::get_TypeId() /* 0600002A */
  } // end of property DependsOnAttribute::TypeId
  .property /*1700000D*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.DependsOnAttribute/*0200000B*/::get_Name() /* 0600002B */
  } // end of property DependsOnAttribute::Name
} // end of class System.Windows.Markup.DependsOnAttribute

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Windows.Markup.MarkupExtensionReturnTypeAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00002A:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ returnType) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method MarkupExtensionReturnTypeAttribute::.ctor

  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ returnType,
                               class [mscorlib/*23000001*/]System.Type/*01000003*/ expressionType) cil managed
  // SIG: 20 02 01 12 0D 12 0D
  {
    .custom /*0C000062:0A00001D*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*01000050*/::.ctor(string) /* 0A00001D */ = ( 01 00 80 E4 54 68 65 20 65 78 70 72 65 73 73 69   // ....The expressi
                                                                                                                                             6F 6E 54 79 70 65 20 61 72 67 75 6D 65 6E 74 20   // onType argument 
                                                                                                                                             69 73 20 6E 6F 74 20 75 73 65 64 20 62 79 20 74   // is not used by t
                                                                                                                                             68 65 20 58 41 4D 4C 20 70 61 72 73 65 72 2E 20   // he XAML parser. 
                                                                                                                                             54 6F 20 73 70 65 63 69 66 79 20 74 68 65 20 65   // To specify the e
                                                                                                                                             78 70 65 63 74 65 64 20 72 65 74 75 72 6E 20 74   // xpected return t
                                                                                                                                             79 70 65 2C 20 75 73 65 20 4D 61 72 6B 75 70 45   // ype, use MarkupE
                                                                                                                                             78 74 65 6E 73 69 6F 6E 52 65 74 75 72 6E 54 79   // xtensionReturnTy
                                                                                                                                             70 65 41 74 74 72 69 62 75 74 65 28 54 79 70 65   // peAttribute(Type
                                                                                                                                             29 2E 20 54 6F 20 73 70 65 63 69 66 79 20 63 75   // ). To specify cu
                                                                                                                                             73 74 6F 6D 20 68 61 6E 64 6C 69 6E 67 20 66 6F   // stom handling fo
                                                                                                                                             72 20 65 78 70 72 65 73 73 69 6F 6E 20 74 79 70   // r expression typ
                                                                                                                                             65 73 2C 20 75 73 65 20 58 61 6D 6C 53 65 74 4D   // es, use XamlSetM
                                                                                                                                             61 72 6B 75 70 45 78 74 65 6E 73 69 6F 6E 41 74   // arkupExtensionAt
                                                                                                                                             74 72 69 62 75 74 65 2E 00 00 )                   // tribute...
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method MarkupExtensionReturnTypeAttribute::.ctor

  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method MarkupExtensionReturnTypeAttribute::.ctor

  .method /*0600002F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method MarkupExtensionReturnTypeAttribute::get_ReturnType

  .method /*06000030*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ExpressionType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method MarkupExtensionReturnTypeAttribute::get_ExpressionType

  .property /*1700000E*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ReturnType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.MarkupExtensionReturnTypeAttribute/*0200000C*/::get_ReturnType() /* 0600002F */
  } // end of property MarkupExtensionReturnTypeAttribute::ReturnType
  .property /*1700000F*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ExpressionType()
  {
    .custom /*0C000031:0A00001D*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*01000050*/::.ctor(string) /* 0A00001D */ = ( 01 00 54 54 68 69 73 20 69 73 20 6E 6F 74 20 75   // ..TThis is not u
                                                                                                                                             73 65 64 20 62 79 20 74 68 65 20 58 41 4D 4C 20   // sed by the XAML 
                                                                                                                                             70 61 72 73 65 72 2E 20 50 6C 65 61 73 65 20 6C   // parser. Please l
                                                                                                                                             6F 6F 6B 20 61 74 20 58 61 6D 6C 53 65 74 4D 61   // ook at XamlSetMa
                                                                                                                                             72 6B 75 70 45 78 74 65 6E 73 69 6F 6E 41 74 74   // rkupExtensionAtt
                                                                                                                                             72 69 62 75 74 65 2E 00 00 )                      // ribute...
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.MarkupExtensionReturnTypeAttribute/*0200000C*/::get_ExpressionType() /* 06000030 */
  } // end of property MarkupExtensionReturnTypeAttribute::ExpressionType
} // end of class System.Windows.Markup.MarkupExtensionReturnTypeAttribute

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Windows.Markup.NameScopePropertyAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00002B:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .custom /*0C00002C:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000031*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method NameScopePropertyAttribute::.ctor

  .method /*06000032*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 20 02 01 0E 12 0D
  {
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method NameScopePropertyAttribute::.ctor

  .method /*06000033*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method NameScopePropertyAttribute::get_Name

  .method /*06000034*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_Type() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method NameScopePropertyAttribute::get_Type

  .property /*17000010*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.NameScopePropertyAttribute/*0200000D*/::get_Name() /* 06000033 */
  } // end of property NameScopePropertyAttribute::Name
  .property /*17000011*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          Type()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.NameScopePropertyAttribute/*0200000D*/::get_Type() /* 06000034 */
  } // end of property NameScopePropertyAttribute::Type
} // end of class System.Windows.Markup.NameScopePropertyAttribute

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.Windows.Markup.RootNamespaceAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00002D:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00002E:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string nameSpace) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method RootNamespaceAttribute::.ctor

  .method /*06000036*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method RootNamespaceAttribute::get_Namespace

  .property /*17000012*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Windows.Markup.RootNamespaceAttribute/*0200000E*/::get_Namespace() /* 06000036 */
  } // end of property RootNamespaceAttribute::Namespace
} // end of class System.Windows.Markup.RootNamespaceAttribute

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Windows.Markup.TrimSurroundingWhitespaceAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00002F:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000030:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .method /*06000037*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method TrimSurroundingWhitespaceAttribute::.ctor

} // end of class System.Windows.Markup.TrimSurroundingWhitespaceAttribute

.class /*02000010*/ public auto ansi sealed beforefieldinit System.Windows.Markup.UidPropertyAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000032:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000033:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*06000038*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method UidPropertyAttribute::.ctor

  .method /*06000039*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method UidPropertyAttribute::get_Name

  .property /*17000013*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.UidPropertyAttribute/*02000010*/::get_Name() /* 06000039 */
  } // end of property UidPropertyAttribute::Name
} // end of class System.Windows.Markup.UidPropertyAttribute

.class /*02000011*/ public auto ansi sealed beforefieldinit System.Windows.Markup.WhitespaceSignificantCollectionAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000035:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .custom /*0C000036:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*0600003A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method WhitespaceSignificantCollectionAttribute::.ctor

} // end of class System.Windows.Markup.WhitespaceSignificantCollectionAttribute

.class /*02000012*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XamlSetMarkupExtensionAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000038:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 01 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .method /*0600003B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xamlSetMarkupExtensionHandler) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionAttribute::.ctor

  .method /*0600003C*/ public hidebysig specialname 
          instance string  get_XamlSetMarkupExtensionHandler() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000068:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionAttribute::get_XamlSetMarkupExtensionHandler

  .method /*0600003D*/ private hidebysig specialname 
          instance void  set_XamlSetMarkupExtensionHandler(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000069:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionAttribute::set_XamlSetMarkupExtensionHandler

  .property /*17000014*/ instance callconv(8) string
          XamlSetMarkupExtensionHandler()
  {
    .get instance string System.Windows.Markup.XamlSetMarkupExtensionAttribute/*02000012*/::get_XamlSetMarkupExtensionHandler() /* 0600003C */
    .set instance void System.Windows.Markup.XamlSetMarkupExtensionAttribute/*02000012*/::set_XamlSetMarkupExtensionHandler(string) /* 0600003D */
  } // end of property XamlSetMarkupExtensionAttribute::XamlSetMarkupExtensionHandler
} // end of class System.Windows.Markup.XamlSetMarkupExtensionAttribute

.class /*02000013*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XmlLangPropertyAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00003A:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C00003B:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method XmlLangPropertyAttribute::.ctor

  .method /*0600003F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method XmlLangPropertyAttribute::get_Name

  .property /*17000015*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.XmlLangPropertyAttribute/*02000013*/::get_Name() /* 0600003F */
  } // end of property XmlLangPropertyAttribute::Name
} // end of class System.Windows.Markup.XmlLangPropertyAttribute

.class /*02000014*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XmlnsCompatibleWithAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00003D:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C00003E:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 01 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string oldNamespace,
                               string newNamespace) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method XmlnsCompatibleWithAttribute::.ctor

  .method /*06000041*/ public hidebysig specialname 
          instance string  get_OldNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method XmlnsCompatibleWithAttribute::get_OldNamespace

  .method /*06000042*/ public hidebysig specialname 
          instance string  get_NewNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method XmlnsCompatibleWithAttribute::get_NewNamespace

  .property /*17000016*/ instance callconv(8) string
          OldNamespace()
  {
    .get instance string System.Windows.Markup.XmlnsCompatibleWithAttribute/*02000014*/::get_OldNamespace() /* 06000041 */
  } // end of property XmlnsCompatibleWithAttribute::OldNamespace
  .property /*17000017*/ instance callconv(8) string
          NewNamespace()
  {
    .get instance string System.Windows.Markup.XmlnsCompatibleWithAttribute/*02000014*/::get_NewNamespace() /* 06000042 */
  } // end of property XmlnsCompatibleWithAttribute::NewNamespace
} // end of class System.Windows.Markup.XmlnsCompatibleWithAttribute

.class /*02000015*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XmlnsDefinitionAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000040:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000041:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 01 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*06000043*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xmlNamespace,
                               string clrNamespace) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method XmlnsDefinitionAttribute::.ctor

  .method /*06000044*/ public hidebysig specialname 
          instance string  get_XmlNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method XmlnsDefinitionAttribute::get_XmlNamespace

  .method /*06000045*/ public hidebysig specialname 
          instance string  get_ClrNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method XmlnsDefinitionAttribute::get_ClrNamespace

  .method /*06000046*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method XmlnsDefinitionAttribute::get_AssemblyName

  .method /*06000047*/ public hidebysig specialname 
          instance void  set_AssemblyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method XmlnsDefinitionAttribute::set_AssemblyName

  .property /*17000018*/ instance callconv(8) string
          XmlNamespace()
  {
    .get instance string System.Windows.Markup.XmlnsDefinitionAttribute/*02000015*/::get_XmlNamespace() /* 06000044 */
  } // end of property XmlnsDefinitionAttribute::XmlNamespace
  .property /*17000019*/ instance callconv(8) string
          ClrNamespace()
  {
    .get instance string System.Windows.Markup.XmlnsDefinitionAttribute/*02000015*/::get_ClrNamespace() /* 06000045 */
  } // end of property XmlnsDefinitionAttribute::ClrNamespace
  .property /*1700001A*/ instance callconv(8) string
          AssemblyName()
  {
    .get instance string System.Windows.Markup.XmlnsDefinitionAttribute/*02000015*/::get_AssemblyName() /* 06000046 */
    .set instance void System.Windows.Markup.XmlnsDefinitionAttribute/*02000015*/::set_AssemblyName(string) /* 06000047 */
  } // end of property XmlnsDefinitionAttribute::AssemblyName
} // end of class System.Windows.Markup.XmlnsDefinitionAttribute

.class /*02000016*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XmlnsPrefixAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000043:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 01 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .custom /*0C000044:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xmlNamespace,
                               string prefix) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method XmlnsPrefixAttribute::.ctor

  .method /*06000049*/ public hidebysig specialname 
          instance string  get_XmlNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method XmlnsPrefixAttribute::get_XmlNamespace

  .method /*0600004A*/ public hidebysig specialname 
          instance string  get_Prefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method XmlnsPrefixAttribute::get_Prefix

  .property /*1700001B*/ instance callconv(8) string
          XmlNamespace()
  {
    .get instance string System.Windows.Markup.XmlnsPrefixAttribute/*02000016*/::get_XmlNamespace() /* 06000049 */
  } // end of property XmlnsPrefixAttribute::XmlNamespace
  .property /*1700001C*/ instance callconv(8) string
          Prefix()
  {
    .get instance string System.Windows.Markup.XmlnsPrefixAttribute/*02000016*/::get_Prefix() /* 0600004A */
  } // end of property XmlnsPrefixAttribute::Prefix
} // end of class System.Windows.Markup.XmlnsPrefixAttribute

.class /*02000017*/ public auto ansi beforefieldinit System.Windows.Markup.DateTimeValueSerializer
       extends System.Windows.Markup.ValueSerializer/*02000002*/
{
  .custom /*0C000045:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*0600004B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method DateTimeValueSerializer::.ctor

  .method /*0600004C*/ public hidebysig virtual 
          instance bool  CanConvertFromString(string 'value',
                                              class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 02 0E 12 74
  {
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method DateTimeValueSerializer::CanConvertFromString

  .method /*0600004D*/ public hidebysig virtual 
          instance bool  CanConvertToString(object 'value',
                                            class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 02 1C 12 74
  {
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method DateTimeValueSerializer::CanConvertToString

  .method /*0600004E*/ public hidebysig virtual 
          instance object  ConvertFromString(string 'value',
                                             class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 1C 0E 12 74
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method DateTimeValueSerializer::ConvertFromString

  .method /*0600004F*/ public hidebysig virtual 
          instance string  ConvertToString(object 'value',
                                           class System.Windows.Markup.IValueSerializerContext/*0200001D*/ context) cil managed
  // SIG: 20 02 0E 1C 12 74
  {
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method DateTimeValueSerializer::ConvertToString

} // end of class System.Windows.Markup.DateTimeValueSerializer

.class /*02000018*/ interface public abstract auto ansi System.Windows.Markup.INameScope
{
  .custom /*0C000046:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000050*/ public hidebysig newslot abstract virtual 
          instance void  RegisterName(string name,
                                      object scopedElement) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x0
  } // end of method INameScope::RegisterName

  .method /*06000051*/ public hidebysig newslot abstract virtual 
          instance void  UnregisterName(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method INameScope::UnregisterName

  .method /*06000052*/ public hidebysig newslot abstract virtual 
          instance object  FindName(string name) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x0
  } // end of method INameScope::FindName

} // end of class System.Windows.Markup.INameScope

.class /*02000019*/ interface public abstract auto ansi System.Windows.Markup.INameScopeDictionary
       implements System.Windows.Markup.INameScope/*02000018*/,
                  class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000008*/<string,object>/*1B000001*/,
                  class [mscorlib/*23000001*/]System.Collections.Generic.ICollection`1/*01000009*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100000A*/<string,object>>/*1B000002*/,
                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100000A*/<string,object>>/*1B000003*/,
                  [mscorlib/*23000001*/]System.Collections.IEnumerable/*0100000B*/
{
} // end of class System.Windows.Markup.INameScopeDictionary

.class /*0200001A*/ interface public abstract auto ansi System.Windows.Markup.IProvideValueTarget
{
  .custom /*0C000049:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 58 50 72 65 73 65 6E 74 61 74 69 6F 6E 46   // ..XPresentationF
                                                                                                                                                                             72 61 6D 65 77 6F 72 6B 2C 20 56 65 72 73 69 6F   // ramework, Versio
                                                                                                                                                                             6E 3D 33 2E 35 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=3.5.0.0, Cultu
                                                                                                                                                                             72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                                                             69 63 4B 65 79 54 6F 6B 65 6E 3D 33 31 62 66 33   // icKeyToken=31bf3
                                                                                                                                                                             38 35 36 61 64 33 36 34 65 33 35 00 00 )          // 856ad364e35..
  .method /*06000053*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_TargetObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method IProvideValueTarget::get_TargetObject

  .method /*06000054*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_TargetProperty() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method IProvideValueTarget::get_TargetProperty

  .property /*1700001D*/ instance callconv(8) object
          TargetObject()
  {
    .get instance object System.Windows.Markup.IProvideValueTarget/*0200001A*/::get_TargetObject() /* 06000053 */
  } // end of property IProvideValueTarget::TargetObject
  .property /*1700001E*/ instance callconv(8) object
          TargetProperty()
  {
    .get instance object System.Windows.Markup.IProvideValueTarget/*0200001A*/::get_TargetProperty() /* 06000054 */
  } // end of property IProvideValueTarget::TargetProperty
} // end of class System.Windows.Markup.IProvideValueTarget

.class /*0200001B*/ interface public abstract auto ansi System.Windows.Markup.IQueryAmbient
{
  .method /*06000055*/ public hidebysig newslot abstract virtual 
          instance bool  IsAmbientPropertyAvailable(string propertyName) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x0
  } // end of method IQueryAmbient::IsAmbientPropertyAvailable

} // end of class System.Windows.Markup.IQueryAmbient

.class /*0200001C*/ interface public abstract auto ansi System.Windows.Markup.IUriContext
{
  .custom /*0C00004A:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 53 50 72 65 73 65 6E 74 61 74 69 6F 6E 43   // ..SPresentationC
                                                                                                                                                                             6F 72 65 2C 20 56 65 72 73 69 6F 6E 3D 33 2E 35   // ore, Version=3.5
                                                                                                                                                                             2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65   // .0.0, Culture=ne
                                                                                                                                                                             75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79   // utral, PublicKey
                                                                                                                                                                             54 6F 6B 65 6E 3D 33 31 62 66 33 38 35 36 61 64   // Token=31bf3856ad
                                                                                                                                                                             33 36 34 65 33 35 00 00 )                         // 364e35..
  .method /*06000056*/ public hidebysig newslot specialname abstract virtual 
          instance class [System/*23000002*/]System.Uri/*0100000C*/ 
          get_BaseUri() cil managed
  // SIG: 20 00 12 31
  {
    // Method begins at RVA 0x0
  } // end of method IUriContext::get_BaseUri

  .method /*06000057*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_BaseUri(class [System/*23000002*/]System.Uri/*0100000C*/ 'value') cil managed
  // SIG: 20 01 01 12 31
  {
    // Method begins at RVA 0x0
  } // end of method IUriContext::set_BaseUri

  .property /*1700001F*/ instance callconv(8) class [System/*23000002*/]System.Uri/*0100000C*/
          BaseUri()
  {
    .get instance class [System/*23000002*/]System.Uri/*0100000C*/ System.Windows.Markup.IUriContext/*0200001C*/::get_BaseUri() /* 06000056 */
    .set instance void System.Windows.Markup.IUriContext/*0200001C*/::set_BaseUri(class [System/*23000002*/]System.Uri/*0100000C*/) /* 06000057 */
  } // end of property IUriContext::BaseUri
} // end of class System.Windows.Markup.IUriContext

.class /*0200001D*/ interface public abstract auto ansi System.Windows.Markup.IValueSerializerContext
       implements [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/,
                  [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/
{
  .custom /*0C00004B:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000058*/ public hidebysig newslot abstract virtual 
          instance class System.Windows.Markup.ValueSerializer/*02000002*/ 
          GetValueSerializerFor(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 20 01 12 08 12 0D
  {
    // Method begins at RVA 0x0
  } // end of method IValueSerializerContext::GetValueSerializerFor

  .method /*06000059*/ public hidebysig newslot abstract virtual 
          instance class System.Windows.Markup.ValueSerializer/*02000002*/ 
          GetValueSerializerFor(class [System/*23000002*/]System.ComponentModel.PropertyDescriptor/*01000004*/ descriptor) cil managed
  // SIG: 20 01 12 08 12 11
  {
    // Method begins at RVA 0x0
  } // end of method IValueSerializerContext::GetValueSerializerFor

} // end of class System.Windows.Markup.IValueSerializerContext

.class /*0200001E*/ interface public abstract auto ansi System.Windows.Markup.IXamlTypeResolver
{
  .custom /*0C00004C:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*0600005A*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          Resolve(string qualifiedTypeName) cil managed
  // SIG: 20 01 12 0D 0E
  {
    // Method begins at RVA 0x0
  } // end of method IXamlTypeResolver::Resolve

} // end of class System.Windows.Markup.IXamlTypeResolver

.class /*0200001F*/ public auto ansi beforefieldinit System.Windows.Markup.XamlSetValueEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000F*/
{
  .method /*0600005B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlMember/*02000054*/ member,
                               object 'value') cil managed
  // SIG: 20 02 01 12 81 50 1C
  {
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::.ctor

  .method /*0600005C*/ public hidebysig specialname 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_Member() cil managed
  // SIG: 20 00 12 81 50
  {
    .custom /*0C00006D:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::get_Member

  .method /*0600005D*/ private hidebysig specialname 
          instance void  set_Member(class System.Xaml.XamlMember/*02000054*/ 'value') cil managed
  // SIG: 20 01 01 12 81 50
  {
    .custom /*0C00006E:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::set_Member

  .method /*0600005E*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00006F:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::get_Value

  .method /*0600005F*/ private hidebysig specialname 
          instance void  set_Value(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000070:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::set_Value

  .method /*06000060*/ public hidebysig specialname 
          instance bool  get_Handled() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000071:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::get_Handled

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_Handled(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000072:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::set_Handled

  .method /*06000062*/ public hidebysig newslot virtual 
          instance void  CallBase() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method XamlSetValueEventArgs::CallBase

  .property /*17000020*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          Member()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/::get_Member() /* 0600005C */
    .set instance void System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/::set_Member(class System.Xaml.XamlMember/*02000054*/) /* 0600005D */
  } // end of property XamlSetValueEventArgs::Member
  .property /*17000021*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/::get_Value() /* 0600005E */
    .set instance void System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/::set_Value(object) /* 0600005F */
  } // end of property XamlSetValueEventArgs::Value
  .property /*17000022*/ instance callconv(8) bool
          Handled()
  {
    .get instance bool System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/::get_Handled() /* 06000060 */
    .set instance void System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/::set_Handled(bool) /* 06000061 */
  } // end of property XamlSetValueEventArgs::Handled
} // end of class System.Windows.Markup.XamlSetValueEventArgs

.class /*02000020*/ public auto ansi beforefieldinit System.Windows.Markup.XamlSetMarkupExtensionEventArgs
       extends System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/
{
  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlMember/*02000054*/ member,
                               class System.Windows.Markup.MarkupExtension/*02000023*/ 'value',
                               class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 03 01 12 81 50 12 80 8C 12 39
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionEventArgs::.ctor

  .method /*06000064*/ public hidebysig specialname 
          instance class System.Windows.Markup.MarkupExtension/*02000023*/ 
          get_MarkupExtension() cil managed
  // SIG: 20 00 12 80 8C
  {
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionEventArgs::get_MarkupExtension

  .method /*06000065*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ 
          get_ServiceProvider() cil managed
  // SIG: 20 00 12 39
  {
    .custom /*0C000073:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionEventArgs::get_ServiceProvider

  .method /*06000066*/ private hidebysig specialname 
          instance void  set_ServiceProvider(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ 'value') cil managed
  // SIG: 20 01 01 12 39
  {
    .custom /*0C000074:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionEventArgs::set_ServiceProvider

  .method /*06000067*/ public hidebysig virtual 
          instance void  CallBase() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method XamlSetMarkupExtensionEventArgs::CallBase

  .property /*17000023*/ instance callconv(8) class System.Windows.Markup.MarkupExtension/*02000023*/
          MarkupExtension()
  {
    .get instance class System.Windows.Markup.MarkupExtension/*02000023*/ System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/::get_MarkupExtension() /* 06000064 */
  } // end of property XamlSetMarkupExtensionEventArgs::MarkupExtension
  .property /*17000024*/ instance callconv(8) class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/
          ServiceProvider()
  {
    .set instance void System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/::set_ServiceProvider(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/) /* 06000066 */
    .get instance class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/::get_ServiceProvider() /* 06000065 */
  } // end of property XamlSetMarkupExtensionEventArgs::ServiceProvider
} // end of class System.Windows.Markup.XamlSetMarkupExtensionEventArgs

.class /*02000021*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XamlSetTypeConverterAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00004D:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 01 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xamlSetTypeConverterHandler) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterAttribute::.ctor

  .method /*06000069*/ public hidebysig specialname 
          instance string  get_XamlSetTypeConverterHandler() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000076:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterAttribute::get_XamlSetTypeConverterHandler

  .method /*0600006A*/ private hidebysig specialname 
          instance void  set_XamlSetTypeConverterHandler(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000077:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterAttribute::set_XamlSetTypeConverterHandler

  .property /*17000025*/ instance callconv(8) string
          XamlSetTypeConverterHandler()
  {
    .set instance void System.Windows.Markup.XamlSetTypeConverterAttribute/*02000021*/::set_XamlSetTypeConverterHandler(string) /* 0600006A */
    .get instance string System.Windows.Markup.XamlSetTypeConverterAttribute/*02000021*/::get_XamlSetTypeConverterHandler() /* 06000069 */
  } // end of property XamlSetTypeConverterAttribute::XamlSetTypeConverterHandler
} // end of class System.Windows.Markup.XamlSetTypeConverterAttribute

.class /*02000022*/ public auto ansi beforefieldinit System.Windows.Markup.XamlSetTypeConverterEventArgs
       extends System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/
{
  .method /*0600006B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlMember/*02000054*/ member,
                               class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/ typeConverter,
                               object 'value',
                               class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ serviceProvider,
                               class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ cultureInfo) cil managed
  // SIG: 20 05 01 12 81 50 12 41 1C 12 35 12 45
  {
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::.ctor

  .method /*0600006C*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/ 
          get_TypeConverter() cil managed
  // SIG: 20 00 12 41
  {
    .custom /*0C000078:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::get_TypeConverter

  .method /*0600006D*/ private hidebysig specialname 
          instance void  set_TypeConverter(class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/ 'value') cil managed
  // SIG: 20 01 01 12 41
  {
    .custom /*0C00007A:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::set_TypeConverter

  .method /*0600006E*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ 
          get_ServiceProvider() cil managed
  // SIG: 20 00 12 35
  {
    .custom /*0C00007C:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::get_ServiceProvider

  .method /*0600006F*/ private hidebysig specialname 
          instance void  set_ServiceProvider(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ 'value') cil managed
  // SIG: 20 01 01 12 35
  {
    .custom /*0C00007D:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::set_ServiceProvider

  .method /*06000070*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ 
          get_CultureInfo() cil managed
  // SIG: 20 00 12 45
  {
    .custom /*0C00007E:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::get_CultureInfo

  .method /*06000071*/ private hidebysig specialname 
          instance void  set_CultureInfo(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 12 45
  {
    .custom /*0C00007F:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::set_CultureInfo

  .method /*06000072*/ public hidebysig virtual 
          instance void  CallBase() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method XamlSetTypeConverterEventArgs::CallBase

  .property /*17000026*/ instance callconv(8) class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/
          TypeConverter()
  {
    .set instance void System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/::set_TypeConverter(class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/) /* 0600006D */
    .get instance class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/ System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/::get_TypeConverter() /* 0600006C */
  } // end of property XamlSetTypeConverterEventArgs::TypeConverter
  .property /*17000027*/ instance callconv(8) class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/
          ServiceProvider()
  {
    .get instance class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/::get_ServiceProvider() /* 0600006E */
    .set instance void System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/::set_ServiceProvider(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/) /* 0600006F */
  } // end of property XamlSetTypeConverterEventArgs::ServiceProvider
  .property /*17000028*/ instance callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/
          CultureInfo()
  {
    .set instance void System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/::set_CultureInfo(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/) /* 06000071 */
    .get instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/::get_CultureInfo() /* 06000070 */
  } // end of property XamlSetTypeConverterEventArgs::CultureInfo
} // end of class System.Windows.Markup.XamlSetTypeConverterEventArgs

.class /*02000023*/ public abstract auto ansi beforefieldinit System.Windows.Markup.MarkupExtension
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004E:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000073*/ public hidebysig newslot abstract virtual 
          instance object  ProvideValue(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 01 1C 12 39
  {
    // Method begins at RVA 0x0
  } // end of method MarkupExtension::ProvideValue

  .method /*06000074*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method MarkupExtension::.ctor

} // end of class System.Windows.Markup.MarkupExtension

.class /*02000024*/ public auto ansi beforefieldinit System.Windows.Markup.ArrayExtension
       extends System.Windows.Markup.MarkupExtension/*02000023*/
{
  .custom /*0C00004F:0600002C*/ instance void System.Windows.Markup.MarkupExtensionReturnTypeAttribute/*0200000C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600002C */ = ( 01 00 59 53 79 73 74 65 6D 2E 41 72 72 61 79 2C   // ..YSystem.Array,
                                                                                                                                                                                                  20 6D 73 63 6F 72 6C 69 62 2C 20 56 65 72 73 69   //  mscorlib, Versi
                                                                                                                                                                                                  6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74   // on=4.0.0.0, Cult
                                                                                                                                                                                                  75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62   // ure=neutral, Pub
                                                                                                                                                                                                  6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 37 37 61   // licKeyToken=b77a
                                                                                                                                                                                                  35 63 35 36 31 39 33 34 65 30 38 39 00 00 )       // 5c561934e089..
  .custom /*0C000050:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 58 50 72 65 73 65 6E 74 61 74 69 6F 6E 46   // ..XPresentationF
                                                                                                                                                                             72 61 6D 65 77 6F 72 6B 2C 20 56 65 72 73 69 6F   // ramework, Versio
                                                                                                                                                                             6E 3D 33 2E 35 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=3.5.0.0, Cultu
                                                                                                                                                                             72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                                                             69 63 4B 65 79 54 6F 6B 65 6E 3D 33 31 62 66 33   // icKeyToken=31bf3
                                                                                                                                                                             38 35 36 61 64 33 36 34 65 33 35 00 00 )          // 856ad364e35..
  .custom /*0C000051:06000022*/ instance void System.Windows.Markup.ContentPropertyAttribute/*02000009*/::.ctor(string) /* 06000022 */ = ( 01 00 05 49 74 65 6D 73 00 00 )                   // ...Items..
  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method ArrayExtension::.ctor

  .method /*06000076*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ arrayType) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method ArrayExtension::.ctor

  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Array/*01000012*/ elements) cil managed
  // SIG: 20 01 01 12 49
  {
    // Method begins at RVA 0x20b8
    // Code size       0 (0x0)
  } // end of method ArrayExtension::.ctor

  .method /*06000078*/ public hidebysig instance void 
          AddChild(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20b9
    // Code size       0 (0x0)
  } // end of method ArrayExtension::AddChild

  .method /*06000079*/ public hidebysig instance void 
          AddText(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20ba
    // Code size       0 (0x0)
  } // end of method ArrayExtension::AddText

  .method /*0600007A*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_Type() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x20bb
    // Code size       0 (0x0)
  } // end of method ArrayExtension::get_Type

  .method /*0600007B*/ public hidebysig specialname 
          instance void  set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method ArrayExtension::set_Type

  .method /*0600007C*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.IList/*01000013*/ 
          get_Items() cil managed
  // SIG: 20 00 12 4D
  {
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method ArrayExtension::get_Items

  .method /*0600007D*/ public hidebysig virtual 
          instance object  ProvideValue(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 01 1C 12 39
  {
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method ArrayExtension::ProvideValue

  .property /*17000029*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          Type()
  {
    .custom /*0C00005D:0600001F*/ instance void System.Windows.Markup.ConstructorArgumentAttribute/*02000008*/::.ctor(string) /* 0600001F */ = ( 01 00 04 74 79 70 65 00 00 )                      // ...type..
    .set instance void System.Windows.Markup.ArrayExtension/*02000024*/::set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600007B */
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.ArrayExtension/*02000024*/::get_Type() /* 0600007A */
  } // end of property ArrayExtension::Type
  .property /*1700002A*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.IList/*01000013*/
          Items()
  {
    .custom /*0C00005E:0A00001E*/ instance void [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibilityAttribute/*01000051*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibility/*01000029*/) /* 0A00001E */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance class [mscorlib/*23000001*/]System.Collections.IList/*01000013*/ System.Windows.Markup.ArrayExtension/*02000024*/::get_Items() /* 0600007C */
  } // end of property ArrayExtension::Items
} // end of class System.Windows.Markup.ArrayExtension

.class /*02000025*/ public auto ansi sealed beforefieldinit System.Windows.Markup.DictionaryKeyPropertyAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C000052:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .custom /*0C000053:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .method /*0600007E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method DictionaryKeyPropertyAttribute::.ctor

  .method /*0600007F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method DictionaryKeyPropertyAttribute::get_Name

  .property /*1700002B*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.DictionaryKeyPropertyAttribute/*02000025*/::get_Name() /* 0600007F */
  } // end of property DictionaryKeyPropertyAttribute::Name
} // end of class System.Windows.Markup.DictionaryKeyPropertyAttribute

.class /*02000026*/ interface public abstract auto ansi System.Windows.Markup.IComponentConnector
{
  .custom /*0C000054:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000080*/ public hidebysig newslot abstract virtual 
          instance void  Connect(int32 connectionId,
                                 object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    // Method begins at RVA 0x0
  } // end of method IComponentConnector::Connect

  .method /*06000081*/ public hidebysig newslot abstract virtual 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IComponentConnector::InitializeComponent

} // end of class System.Windows.Markup.IComponentConnector

.class /*02000027*/ public auto ansi beforefieldinit System.Windows.Markup.NullExtension
       extends System.Windows.Markup.MarkupExtension/*02000023*/
{
  .custom /*0C000055:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 58 50 72 65 73 65 6E 74 61 74 69 6F 6E 46   // ..XPresentationF
                                                                                                                                                                             72 61 6D 65 77 6F 72 6B 2C 20 56 65 72 73 69 6F   // ramework, Versio
                                                                                                                                                                             6E 3D 33 2E 35 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=3.5.0.0, Cultu
                                                                                                                                                                             72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                                                             69 63 4B 65 79 54 6F 6B 65 6E 3D 33 31 62 66 33   // icKeyToken=31bf3
                                                                                                                                                                             38 35 36 61 64 33 36 34 65 33 35 00 00 )          // 856ad364e35..
  .custom /*0C000056:0600002C*/ instance void System.Windows.Markup.MarkupExtensionReturnTypeAttribute/*0200000C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600002C */ = ( 01 00 5A 53 79 73 74 65 6D 2E 4F 62 6A 65 63 74   // ..ZSystem.Object
                                                                                                                                                                                                  2C 20 6D 73 63 6F 72 6C 69 62 2C 20 56 65 72 73   // , mscorlib, Vers
                                                                                                                                                                                                  69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=4.0.0.0, Cul
                                                                                                                                                                                                  74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                                  62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 37 37   // blicKeyToken=b77
                                                                                                                                                                                                  61 35 63 35 36 31 39 33 34 65 30 38 39 00 00 )    // a5c561934e089..
  .method /*06000082*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method NullExtension::.ctor

  .method /*06000083*/ public hidebysig virtual 
          instance object  ProvideValue(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 01 1C 12 39
  {
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method NullExtension::ProvideValue

} // end of class System.Windows.Markup.NullExtension

.class /*02000028*/ public auto ansi beforefieldinit System.Windows.Markup.StaticExtension
       extends System.Windows.Markup.MarkupExtension/*02000023*/
{
  .custom /*0C000057:0600002C*/ instance void System.Windows.Markup.MarkupExtensionReturnTypeAttribute/*0200000C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600002C */ = ( 01 00 5A 53 79 73 74 65 6D 2E 4F 62 6A 65 63 74   // ..ZSystem.Object
                                                                                                                                                                                                  2C 20 6D 73 63 6F 72 6C 69 62 2C 20 56 65 72 73   // , mscorlib, Vers
                                                                                                                                                                                                  69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=4.0.0.0, Cul
                                                                                                                                                                                                  74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                                  62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 37 37   // blicKeyToken=b77
                                                                                                                                                                                                  61 35 63 35 36 31 39 33 34 65 30 38 39 00 00 )    // a5c561934e089..
  .custom /*0C000058:0A00001A*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*0100004C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A00001A */ = ( 01 00 2E 53 79 73 74 65 6D 2E 57 69 6E 64 6F 77   // ...System.Window
                                                                                                                                                                                                          73 2E 4D 61 72 6B 75 70 2E 53 74 61 74 69 63 45   // s.Markup.StaticE
                                                                                                                                                                                                          78 74 65 6E 73 69 6F 6E 43 6F 6E 76 65 72 74 65   // xtensionConverte
                                                                                                                                                                                                          72 00 00 )                                        // r..
  .custom /*0C000059:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 58 50 72 65 73 65 6E 74 61 74 69 6F 6E 46   // ..XPresentationF
                                                                                                                                                                             72 61 6D 65 77 6F 72 6B 2C 20 56 65 72 73 69 6F   // ramework, Versio
                                                                                                                                                                             6E 3D 33 2E 35 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=3.5.0.0, Cultu
                                                                                                                                                                             72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                                                             69 63 4B 65 79 54 6F 6B 65 6E 3D 33 31 62 66 33   // icKeyToken=31bf3
                                                                                                                                                                             38 35 36 61 64 33 36 34 65 33 35 00 00 )          // 856ad364e35..
  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method StaticExtension::.ctor

  .method /*06000085*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string member) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method StaticExtension::.ctor

  .method /*06000086*/ public hidebysig virtual 
          instance object  ProvideValue(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 01 1C 12 39
  {
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method StaticExtension::ProvideValue

  .method /*06000087*/ public hidebysig specialname 
          instance string  get_Member() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20c6
    // Code size       0 (0x0)
  } // end of method StaticExtension::get_Member

  .method /*06000088*/ public hidebysig specialname 
          instance void  set_Member(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20c7
    // Code size       0 (0x0)
  } // end of method StaticExtension::set_Member

  .method /*06000089*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_MemberType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method StaticExtension::get_MemberType

  .method /*0600008A*/ public hidebysig specialname 
          instance void  set_MemberType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20c9
    // Code size       0 (0x0)
  } // end of method StaticExtension::set_MemberType

  .property /*1700002C*/ instance callconv(8) string
          Member()
  {
    .custom /*0C000061:0600001F*/ instance void System.Windows.Markup.ConstructorArgumentAttribute/*02000008*/::.ctor(string) /* 0600001F */ = ( 01 00 06 6D 65 6D 62 65 72 00 00 )                // ...member..
    .get instance string System.Windows.Markup.StaticExtension/*02000028*/::get_Member() /* 06000087 */
    .set instance void System.Windows.Markup.StaticExtension/*02000028*/::set_Member(string) /* 06000088 */
  } // end of property StaticExtension::Member
  .property /*1700002D*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          MemberType()
  {
    .custom /*0C000064:0A00001B*/ instance void [System/*23000002*/]System.ComponentModel.DefaultValueAttribute/*0100004D*/::.ctor(string) /* 0A00001B */ = ( 01 00 FF 00 00 ) 
    .set instance void System.Windows.Markup.StaticExtension/*02000028*/::set_MemberType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600008A */
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.StaticExtension/*02000028*/::get_MemberType() /* 06000089 */
  } // end of property StaticExtension::MemberType
} // end of class System.Windows.Markup.StaticExtension

.class /*02000029*/ public auto ansi beforefieldinit System.Windows.Markup.TypeExtension
       extends System.Windows.Markup.MarkupExtension/*02000023*/
{
  .custom /*0C00005A:0A00001A*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*0100004C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A00001A */ = ( 01 00 2C 53 79 73 74 65 6D 2E 57 69 6E 64 6F 77   // ..,System.Window
                                                                                                                                                                                                          73 2E 4D 61 72 6B 75 70 2E 54 79 70 65 45 78 74   // s.Markup.TypeExt
                                                                                                                                                                                                          65 6E 73 69 6F 6E 43 6F 6E 76 65 72 74 65 72 00   // ensionConverter.
                                                                                                                                                                                                          00 ) 
  .custom /*0C00005B:0600002C*/ instance void System.Windows.Markup.MarkupExtensionReturnTypeAttribute/*0200000C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600002C */ = ( 01 00 58 53 79 73 74 65 6D 2E 54 79 70 65 2C 20   // ..XSystem.Type, 
                                                                                                                                                                                                  6D 73 63 6F 72 6C 69 62 2C 20 56 65 72 73 69 6F   // mscorlib, Versio
                                                                                                                                                                                                  6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=4.0.0.0, Cultu
                                                                                                                                                                                                  72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                                                                                  69 63 4B 65 79 54 6F 6B 65 6E 3D 62 37 37 61 35   // icKeyToken=b77a5
                                                                                                                                                                                                  63 35 36 31 39 33 34 65 30 38 39 00 00 )          // c561934e089..
  .custom /*0C00005C:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 58 50 72 65 73 65 6E 74 61 74 69 6F 6E 46   // ..XPresentationF
                                                                                                                                                                             72 61 6D 65 77 6F 72 6B 2C 20 56 65 72 73 69 6F   // ramework, Versio
                                                                                                                                                                             6E 3D 33 2E 35 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=3.5.0.0, Cultu
                                                                                                                                                                             72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                                                             69 63 4B 65 79 54 6F 6B 65 6E 3D 33 31 62 66 33   // icKeyToken=31bf3
                                                                                                                                                                             38 35 36 61 64 33 36 34 65 33 35 00 00 )          // 856ad364e35..
  .method /*0600008B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ca
    // Code size       0 (0x0)
  } // end of method TypeExtension::.ctor

  .method /*0600008C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20cb
    // Code size       0 (0x0)
  } // end of method TypeExtension::.ctor

  .method /*0600008D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20cc
    // Code size       0 (0x0)
  } // end of method TypeExtension::.ctor

  .method /*0600008E*/ public hidebysig virtual 
          instance object  ProvideValue(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 01 1C 12 39
  {
    // Method begins at RVA 0x20cd
    // Code size       0 (0x0)
  } // end of method TypeExtension::ProvideValue

  .method /*0600008F*/ public hidebysig specialname 
          instance string  get_TypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20ce
    // Code size       0 (0x0)
  } // end of method TypeExtension::get_TypeName

  .method /*06000090*/ public hidebysig specialname 
          instance void  set_TypeName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20cf
    // Code size       0 (0x0)
  } // end of method TypeExtension::set_TypeName

  .method /*06000091*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_Type() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x20d0
    // Code size       0 (0x0)
  } // end of method TypeExtension::get_Type

  .method /*06000092*/ public hidebysig specialname 
          instance void  set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20d1
    // Code size       0 (0x0)
  } // end of method TypeExtension::set_Type

  .property /*1700002E*/ instance callconv(8) string
          TypeName()
  {
    .custom /*0C000065:0A00001E*/ instance void [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibilityAttribute/*01000051*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibility/*01000029*/) /* 0A00001E */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance string System.Windows.Markup.TypeExtension/*02000029*/::get_TypeName() /* 0600008F */
    .set instance void System.Windows.Markup.TypeExtension/*02000029*/::set_TypeName(string) /* 06000090 */
  } // end of property TypeExtension::TypeName
  .property /*1700002F*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          Type()
  {
    .custom /*0C000066:0A00001B*/ instance void [System/*23000002*/]System.ComponentModel.DefaultValueAttribute/*0100004D*/::.ctor(string) /* 0A00001B */ = ( 01 00 FF 00 00 ) 
    .custom /*0C000067:0600001F*/ instance void System.Windows.Markup.ConstructorArgumentAttribute/*02000008*/::.ctor(string) /* 0600001F */ = ( 01 00 04 74 79 70 65 00 00 )                      // ...type..
    .set instance void System.Windows.Markup.TypeExtension/*02000029*/::set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000092 */
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.TypeExtension/*02000029*/::get_Type() /* 06000091 */
  } // end of property TypeExtension::Type
} // end of class System.Windows.Markup.TypeExtension

.class /*0200002A*/ private auto ansi beforefieldinit System.Windows.Markup.StaticExtensionConverter
       extends [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/
{
  .method /*06000093*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 02 02 12 35 12 0D
  {
    // Method begins at RVA 0x20d2
    // Code size       0 (0x0)
  } // end of method StaticExtensionConverter::CanConvertTo

  .method /*06000094*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 04 1C 12 35 12 45 1C 12 0D
  {
    .custom /*0C000081:0A00001F*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*01000052*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    .custom /*0C000082:0A000020*/ instance void [mscorlib/*23000001*/]System.Security.SecurityTreatAsSafeAttribute/*01000053*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       0 (0x0)
  } // end of method StaticExtensionConverter::ConvertTo

  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method StaticExtensionConverter::.ctor

} // end of class System.Windows.Markup.StaticExtensionConverter

.class /*0200002B*/ private auto ansi beforefieldinit System.Windows.Markup.TypeExtensionConverter
       extends [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/
{
  .method /*06000096*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 02 02 12 35 12 0D
  {
    // Method begins at RVA 0x20d5
    // Code size       0 (0x0)
  } // end of method TypeExtensionConverter::CanConvertTo

  .method /*06000097*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 04 1C 12 35 12 45 1C 12 0D
  {
    .custom /*0C000083:0A00001F*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*01000052*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    .custom /*0C000084:0A000020*/ instance void [mscorlib/*23000001*/]System.Security.SecurityTreatAsSafeAttribute/*01000053*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       0 (0x0)
  } // end of method TypeExtensionConverter::ConvertTo

  .method /*06000098*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d7
    // Code size       0 (0x0)
  } // end of method TypeExtensionConverter::.ctor

} // end of class System.Windows.Markup.TypeExtensionConverter

.class /*0200002C*/ public auto ansi sealed beforefieldinit System.Windows.Markup.RuntimeNamePropertyAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00005F:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 04 00 00 00 00 00 ) 
  .custom /*0C000060:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.TypeForwardedFromAttribute/*0100004A*/::.ctor(string) /* 0A000018 */ = ( 01 00 4E 57 69 6E 64 6F 77 73 42 61 73 65 2C 20   // ..NWindowsBase, 
                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                             3D 33 31 62 66 33 38 35 36 61 64 33 36 34 65 33   // =31bf3856ad364e3
                                                                                                                                                                             35 00 00 )                                        // 5..
  .method /*06000099*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20d8
    // Code size       0 (0x0)
  } // end of method RuntimeNamePropertyAttribute::.ctor

  .method /*0600009A*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d9
    // Code size       0 (0x0)
  } // end of method RuntimeNamePropertyAttribute::get_Name

  .property /*17000030*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Windows.Markup.RuntimeNamePropertyAttribute/*0200002C*/::get_Name() /* 0600009A */
  } // end of property RuntimeNamePropertyAttribute::Name
} // end of class System.Windows.Markup.RuntimeNamePropertyAttribute

.class /*0200002D*/ public auto ansi sealed System.Xaml.Schema.AllowedMemberLocations
       extends [mscorlib/*23000001*/]System.Enum/*01000014*/
{
  .custom /*0C000063:0A000021*/ instance void [mscorlib/*23000001*/]System.FlagsAttribute/*01000054*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ None = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ Attribute = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ MemberElement = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ Any = int32(0x00000003)
} // end of class System.Xaml.Schema.AllowedMemberLocations

.class /*0200002E*/ public auto ansi beforefieldinit System.Xaml.AttachableMemberIdentifier
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class [mscorlib/*23000001*/]System.IEquatable`1/*01000015*/<class System.Xaml.AttachableMemberIdentifier/*0200002E*/>/*1B000004*/
{
  .method /*0600009B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ declaringType,
                               string memberName) cil managed
  // SIG: 20 02 01 12 0D 0E
  {
    // Method begins at RVA 0x20da
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::.ctor

  .method /*0600009C*/ public hidebysig specialname 
          instance string  get_MemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20db
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::get_MemberName

  .method /*0600009D*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x20dc
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::get_DeclaringType

  .method /*0600009E*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xaml.AttachableMemberIdentifier/*0200002E*/ left,
                              class System.Xaml.AttachableMemberIdentifier/*0200002E*/ right) cil managed
  // SIG: 00 02 02 12 80 B8 12 80 B8
  {
    // Method begins at RVA 0x20dd
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::op_Inequality

  .method /*0600009F*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xaml.AttachableMemberIdentifier/*0200002E*/ left,
                            class System.Xaml.AttachableMemberIdentifier/*0200002E*/ right) cil managed
  // SIG: 00 02 02 12 80 B8 12 80 B8
  {
    // Method begins at RVA 0x20de
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::op_Equality

  .method /*060000A0*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x20df
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::Equals

  .method /*060000A1*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Xaml.AttachableMemberIdentifier/*0200002E*/ other) cil managed
  // SIG: 20 01 02 12 80 B8
  {
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::Equals

  .method /*060000A2*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::GetHashCode

  .method /*060000A3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method AttachableMemberIdentifier::ToString

  .property /*17000031*/ instance callconv(8) string
          MemberName()
  {
    .get instance string System.Xaml.AttachableMemberIdentifier/*0200002E*/::get_MemberName() /* 0600009C */
  } // end of property AttachableMemberIdentifier::MemberName
  .property /*17000032*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          DeclaringType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Xaml.AttachableMemberIdentifier/*0200002E*/::get_DeclaringType() /* 0600009D */
  } // end of property AttachableMemberIdentifier::DeclaringType
} // end of class System.Xaml.AttachableMemberIdentifier

.class /*0200002F*/ public abstract auto ansi sealed beforefieldinit System.Xaml.AttachablePropertyServices
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*060000A4*/ public hidebysig static 
          int32  GetAttachedPropertyCount(object 'instance') cil managed
  // SIG: 00 01 08 1C
  {
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::GetAttachedPropertyCount

  .method /*060000A5*/ public hidebysig static 
          void  CopyPropertiesTo(object 'instance',
                                 valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100000A*/<class System.Xaml.AttachableMemberIdentifier/*0200002E*/,object>[] 'array',
                                 int32 index) cil managed
  // SIG: 00 03 01 1C 1D 15 11 29 02 12 80 B8 1C 08
  {
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::CopyPropertiesTo

  .method /*060000A6*/ public hidebysig static 
          bool  RemoveProperty(object 'instance',
                               class System.Xaml.AttachableMemberIdentifier/*0200002E*/ name) cil managed
  // SIG: 00 02 02 1C 12 80 B8
  {
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::RemoveProperty

  .method /*060000A7*/ public hidebysig static 
          void  SetProperty(object 'instance',
                            class System.Xaml.AttachableMemberIdentifier/*0200002E*/ name,
                            object 'value') cil managed
  // SIG: 00 03 01 1C 12 80 B8 1C
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::SetProperty

  .method /*060000A8*/ public hidebysig static 
          bool  TryGetProperty(object 'instance',
                               class System.Xaml.AttachableMemberIdentifier/*0200002E*/ name,
                               [out] object& 'value') cil managed
  // SIG: 00 03 02 1C 12 80 B8 10 1C
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::TryGetProperty

  .method /*060000A9*/ public hidebysig static 
          bool  TryGetProperty<T>(object 'instance',
                                  class System.Xaml.AttachableMemberIdentifier/*0200002E*/ name,
                                  [out] !!T& 'value') cil managed
  // SIG: 10 01 03 02 1C 12 80 B8 10 1E 00
  {
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::TryGetProperty

  .method /*060000AA*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method AttachablePropertyServices::.cctor

} // end of class System.Xaml.AttachablePropertyServices

.class /*02000030*/ interface public abstract auto ansi System.Xaml.IXamlNameResolver
{
  .method /*060000AB*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsFixupTokenAvailable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::get_IsFixupTokenAvailable

  .method /*060000AC*/ public hidebysig newslot abstract virtual 
          instance object  Resolve(string name) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::Resolve

  .method /*060000AD*/ public hidebysig newslot abstract virtual 
          instance object  Resolve(string name,
                                   [out] bool& isFullyInitialized) cil managed
  // SIG: 20 02 1C 0E 10 02
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::Resolve

  .method /*060000AE*/ public hidebysig newslot abstract virtual 
          instance object  GetFixupToken(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<string> names) cil managed
  // SIG: 20 01 1C 15 12 09 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::GetFixupToken

  .method /*060000AF*/ public hidebysig newslot abstract virtual 
          instance object  GetFixupToken(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<string> names,
                                         bool canAssignDirectly) cil managed
  // SIG: 20 02 1C 15 12 09 01 0E 02
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::GetFixupToken

  .method /*060000B0*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100000A*/<string,object>> 
          GetAllNamesAndValuesInScope() cil managed
  // SIG: 20 00 15 12 09 01 15 11 29 02 0E 1C
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::GetAllNamesAndValuesInScope

  .method /*060000B1*/ public hidebysig newslot specialname abstract virtual 
          instance void  add_OnNameScopeInitializationComplete(class [mscorlib/*23000001*/]System.EventHandler/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::add_OnNameScopeInitializationComplete

  .method /*060000B2*/ public hidebysig newslot specialname abstract virtual 
          instance void  remove_OnNameScopeInitializationComplete(class [mscorlib/*23000001*/]System.EventHandler/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameResolver::remove_OnNameScopeInitializationComplete

  .event /*14000001*/ [mscorlib/*23000001*/]System.EventHandler/*01000016*/ OnNameScopeInitializationComplete
  {
    .removeon instance void System.Xaml.IXamlNameResolver/*02000030*/::remove_OnNameScopeInitializationComplete(class [mscorlib/*23000001*/]System.EventHandler/*01000016*/) /* 060000B2 */
    .addon instance void System.Xaml.IXamlNameResolver/*02000030*/::add_OnNameScopeInitializationComplete(class [mscorlib/*23000001*/]System.EventHandler/*01000016*/) /* 060000B1 */
  } // end of event IXamlNameResolver::OnNameScopeInitializationComplete
  .property /*17000033*/ instance callconv(8) bool
          IsFixupTokenAvailable()
  {
    .get instance bool System.Xaml.IXamlNameResolver/*02000030*/::get_IsFixupTokenAvailable() /* 060000AB */
  } // end of property IXamlNameResolver::IsFixupTokenAvailable
} // end of class System.Xaml.IXamlNameResolver

.class /*02000031*/ interface public abstract auto ansi System.Xaml.IAmbientProvider
{
  .method /*060000B3*/ public hidebysig newslot abstract virtual 
          instance class System.Xaml.AmbientPropertyValue/*02000039*/ 
          GetFirstAmbientValue(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.XamlType/*02000065*/> ceilingTypes,
                               class System.Xaml.XamlMember/*02000054*/[] properties) cil managed
  // SIG: 20 02 12 80 E4 15 12 09 01 12 81 94 1D 12 81 50
  {
    .param [2]/*08000097*/ 
    .custom /*0C000085:0A000022*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000055*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAmbientProvider::GetFirstAmbientValue

  .method /*060000B4*/ public hidebysig newslot abstract virtual 
          instance object  GetFirstAmbientValue(class System.Xaml.XamlType/*02000065*/[] types) cil managed
  // SIG: 20 01 1C 1D 12 81 94
  {
    .param [1]/*08000098*/ 
    .custom /*0C000086:0A000022*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000055*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAmbientProvider::GetFirstAmbientValue

  .method /*060000B5*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.AmbientPropertyValue/*02000039*/> 
          GetAllAmbientValues(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.XamlType/*02000065*/> ceilingTypes,
                              class System.Xaml.XamlMember/*02000054*/[] properties) cil managed
  // SIG: 20 02 15 12 09 01 12 80 E4 15 12 09 01 12 81 94 1D 12 81 50
  {
    .param [2]/*0800009A*/ 
    .custom /*0C000087:0A000022*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000055*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAmbientProvider::GetAllAmbientValues

  .method /*060000B6*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<object> 
          GetAllAmbientValues(class System.Xaml.XamlType/*02000065*/[] types) cil managed
  // SIG: 20 01 15 12 09 01 1C 1D 12 81 94
  {
    .param [1]/*0800009B*/ 
    .custom /*0C000088:0A000022*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000055*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAmbientProvider::GetAllAmbientValues

  .method /*060000B7*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.AmbientPropertyValue/*02000039*/> 
          GetAllAmbientValues(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.XamlType/*02000065*/> ceilingTypes,
                              bool searchLiveStackOnly,
                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.XamlType/*02000065*/> types,
                              class System.Xaml.XamlMember/*02000054*/[] properties) cil managed
  // SIG: 20 04 15 12 09 01 12 80 E4 15 12 09 01 12 81 94 02 15 12 09 01 12 81 94 1D 12 81 50
  {
    .param [4]/*0800009F*/ 
    .custom /*0C000089:0A000022*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000055*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAmbientProvider::GetAllAmbientValues

} // end of class System.Xaml.IAmbientProvider

.class /*02000032*/ interface public abstract auto ansi System.Xaml.IXamlSchemaContextProvider
{
  .method /*060000B8*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x0
  } // end of method IXamlSchemaContextProvider::get_SchemaContext

  .property /*17000034*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.IXamlSchemaContextProvider/*02000032*/::get_SchemaContext() /* 060000B8 */
  } // end of property IXamlSchemaContextProvider::SchemaContext
} // end of class System.Xaml.IXamlSchemaContextProvider

.class /*02000033*/ interface public abstract auto ansi System.Xaml.IRootObjectProvider
{
  .method /*060000B9*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_RootObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method IRootObjectProvider::get_RootObject

  .property /*17000035*/ instance callconv(8) object
          RootObject()
  {
    .get instance object System.Xaml.IRootObjectProvider/*02000033*/::get_RootObject() /* 060000B9 */
  } // end of property IRootObjectProvider::RootObject
} // end of class System.Xaml.IRootObjectProvider

.class /*02000034*/ interface public abstract auto ansi System.Xaml.IXamlNamespaceResolver
{
  .method /*060000BA*/ public hidebysig newslot abstract virtual 
          instance string  GetNamespace(string prefix) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNamespaceResolver::GetNamespace

  .method /*060000BB*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.NamespaceDeclaration/*02000048*/> 
          GetNamespacePrefixes() cil managed
  // SIG: 20 00 15 12 09 01 12 81 20
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNamespaceResolver::GetNamespacePrefixes

} // end of class System.Xaml.IXamlNamespaceResolver

.class /*02000035*/ interface public abstract auto ansi System.Xaml.IDestinationTypeProvider
{
  .method /*060000BC*/ public hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          GetDestinationType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x0
  } // end of method IDestinationTypeProvider::GetDestinationType

} // end of class System.Xaml.IDestinationTypeProvider

.class /*02000036*/ interface public abstract auto ansi System.Xaml.IXamlLineInfo
{
  .method /*060000BD*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HasLineInfo() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IXamlLineInfo::get_HasLineInfo

  .method /*060000BE*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlLineInfo::get_LineNumber

  .method /*060000BF*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlLineInfo::get_LinePosition

  .property /*17000036*/ instance callconv(8) bool
          HasLineInfo()
  {
    .get instance bool System.Xaml.IXamlLineInfo/*02000036*/::get_HasLineInfo() /* 060000BD */
  } // end of property IXamlLineInfo::HasLineInfo
  .property /*17000037*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xaml.IXamlLineInfo/*02000036*/::get_LineNumber() /* 060000BE */
  } // end of property IXamlLineInfo::LineNumber
  .property /*17000038*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xaml.IXamlLineInfo/*02000036*/::get_LinePosition() /* 060000BF */
  } // end of property IXamlLineInfo::LinePosition
} // end of class System.Xaml.IXamlLineInfo

.class /*02000037*/ interface public abstract auto ansi System.Xaml.IXamlObjectWriterFactory
{
  .method /*060000C0*/ public hidebysig newslot abstract virtual 
          instance class System.Xaml.XamlObjectWriterSettings/*02000042*/ 
          GetParentSettings() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlObjectWriterFactory::GetParentSettings

  .method /*060000C1*/ public hidebysig newslot abstract virtual 
          instance class System.Xaml.XamlObjectWriter/*02000040*/ 
          GetXamlObjectWriter(class System.Xaml.XamlObjectWriterSettings/*02000042*/ settings) cil managed
  // SIG: 20 01 12 81 00 12 81 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlObjectWriterFactory::GetXamlObjectWriter

} // end of class System.Xaml.IXamlObjectWriterFactory

.class /*02000038*/ public auto ansi beforefieldinit System.Xaml.XamlObjectEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000F*/
{
  .method /*060000C2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'instance') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method XamlObjectEventArgs::.ctor

  .method /*060000C3*/ public hidebysig specialname 
          instance object  get_Instance() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00008A:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method XamlObjectEventArgs::get_Instance

  .method /*060000C4*/ private hidebysig specialname 
          instance void  set_Instance(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00008B:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method XamlObjectEventArgs::set_Instance

  .property /*17000039*/ instance callconv(8) object
          Instance()
  {
    .get instance object System.Xaml.XamlObjectEventArgs/*02000038*/::get_Instance() /* 060000C3 */
    .set instance void System.Xaml.XamlObjectEventArgs/*02000038*/::set_Instance(object) /* 060000C4 */
  } // end of property XamlObjectEventArgs::Instance
} // end of class System.Xaml.XamlObjectEventArgs

.class /*02000039*/ public auto ansi beforefieldinit System.Xaml.AmbientPropertyValue
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*060000C5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlMember/*02000054*/ 'property',
                               object 'value') cil managed
  // SIG: 20 02 01 12 81 50 1C
  {
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method AmbientPropertyValue::.ctor

  .method /*060000C6*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method AmbientPropertyValue::get_Value

  .method /*060000C7*/ public hidebysig specialname 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_RetrievedProperty() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method AmbientPropertyValue::get_RetrievedProperty

  .property /*1700003A*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Xaml.AmbientPropertyValue/*02000039*/::get_Value() /* 060000C6 */
  } // end of property AmbientPropertyValue::Value
  .property /*1700003B*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          RetrievedProperty()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.AmbientPropertyValue/*02000039*/::get_RetrievedProperty() /* 060000C7 */
  } // end of property AmbientPropertyValue::RetrievedProperty
} // end of class System.Xaml.AmbientPropertyValue

.class /*0200003A*/ interface public abstract auto ansi System.Xaml.IAttachedPropertyStore
{
  .method /*060000C8*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_PropertyCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IAttachedPropertyStore::get_PropertyCount

  .method /*060000C9*/ public hidebysig newslot abstract virtual 
          instance void  CopyPropertiesTo(valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100000A*/<class System.Xaml.AttachableMemberIdentifier/*0200002E*/,object>[] 'array',
                                          int32 index) cil managed
  // SIG: 20 02 01 1D 15 11 29 02 12 80 B8 1C 08
  {
    // Method begins at RVA 0x0
  } // end of method IAttachedPropertyStore::CopyPropertiesTo

  .method /*060000CA*/ public hidebysig newslot abstract virtual 
          instance bool  RemoveProperty(class System.Xaml.AttachableMemberIdentifier/*0200002E*/ attachableMemberIdentifier) cil managed
  // SIG: 20 01 02 12 80 B8
  {
    // Method begins at RVA 0x0
  } // end of method IAttachedPropertyStore::RemoveProperty

  .method /*060000CB*/ public hidebysig newslot abstract virtual 
          instance void  SetProperty(class System.Xaml.AttachableMemberIdentifier/*0200002E*/ attachableMemberIdentifier,
                                     object 'value') cil managed
  // SIG: 20 02 01 12 80 B8 1C
  {
    // Method begins at RVA 0x0
  } // end of method IAttachedPropertyStore::SetProperty

  .method /*060000CC*/ public hidebysig newslot abstract virtual 
          instance bool  TryGetProperty(class System.Xaml.AttachableMemberIdentifier/*0200002E*/ attachableMemberIdentifier,
                                        [out] object& 'value') cil managed
  // SIG: 20 02 02 12 80 B8 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method IAttachedPropertyStore::TryGetProperty

  .property /*1700003C*/ instance callconv(8) int32
          PropertyCount()
  {
    .get instance int32 System.Xaml.IAttachedPropertyStore/*0200003A*/::get_PropertyCount() /* 060000C8 */
  } // end of property IAttachedPropertyStore::PropertyCount
} // end of class System.Xaml.IAttachedPropertyStore

.class /*0200003B*/ interface public abstract auto ansi System.Xaml.INamespacePrefixLookup
{
  .method /*060000CD*/ public hidebysig newslot abstract virtual 
          instance string  LookupPrefix(string ns) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x0
  } // end of method INamespacePrefixLookup::LookupPrefix

} // end of class System.Xaml.INamespacePrefixLookup

.class /*0200003C*/ public abstract auto ansi beforefieldinit System.Xaml.XamlWriter
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements [mscorlib/*23000001*/]System.IDisposable/*01000017*/
{
  .method /*060000CE*/ public hidebysig newslot abstract virtual 
          instance void  WriteGetObject() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteGetObject

  .method /*060000CF*/ public hidebysig newslot abstract virtual 
          instance void  WriteStartObject(class System.Xaml.XamlType/*02000065*/ 'type') cil managed
  // SIG: 20 01 01 12 81 94
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteStartObject

  .method /*060000D0*/ public hidebysig newslot abstract virtual 
          instance void  WriteEndObject() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteEndObject

  .method /*060000D1*/ public hidebysig newslot abstract virtual 
          instance void  WriteStartMember(class System.Xaml.XamlMember/*02000054*/ xamlMember) cil managed
  // SIG: 20 01 01 12 81 50
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteStartMember

  .method /*060000D2*/ public hidebysig newslot abstract virtual 
          instance void  WriteEndMember() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteEndMember

  .method /*060000D3*/ public hidebysig newslot abstract virtual 
          instance void  WriteValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteValue

  .method /*060000D4*/ public hidebysig newslot abstract virtual 
          instance void  WriteNamespace(class System.Xaml.NamespaceDeclaration/*02000048*/ namespaceDeclaration) cil managed
  // SIG: 20 01 01 12 81 20
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::WriteNamespace

  .method /*060000D5*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x0
  } // end of method XamlWriter::get_SchemaContext

  .method /*060000D6*/ public hidebysig instance void 
          WriteNode(class System.Xaml.XamlReader/*02000043*/ reader) cil managed
  // SIG: 20 01 01 12 81 0C
  {
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method XamlWriter::WriteNode

  .method /*060000D7*/ family hidebysig specialname 
          instance bool  get_IsDisposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00008C:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method XamlWriter::get_IsDisposed

  .method /*060000D8*/ private hidebysig specialname 
          instance void  set_IsDisposed(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00008D:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method XamlWriter::set_IsDisposed

  .method /*060000D9*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [mscorlib/*23000001*/]System.IDisposable/*01000017*/::Dispose /*01000017::0A000001*/ 
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method XamlWriter::System.IDisposable.Dispose

  .method /*060000DA*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20f4
    // Code size       0 (0x0)
  } // end of method XamlWriter::Dispose

  .method /*060000DB*/ public hidebysig instance void 
          Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f5
    // Code size       0 (0x0)
  } // end of method XamlWriter::Close

  .method /*060000DC*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f6
    // Code size       0 (0x0)
  } // end of method XamlWriter::.ctor

  .property /*1700003D*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlWriter/*0200003C*/::get_SchemaContext() /* 060000D5 */
  } // end of property XamlWriter::SchemaContext
  .property /*1700003E*/ instance callconv(8) bool
          IsDisposed()
  {
    .set instance void System.Xaml.XamlWriter/*0200003C*/::set_IsDisposed(bool) /* 060000D8 */
    .get instance bool System.Xaml.XamlWriter/*0200003C*/::get_IsDisposed() /* 060000D7 */
  } // end of property XamlWriter::IsDisposed
} // end of class System.Xaml.XamlWriter

.class /*0200003D*/ interface public abstract auto ansi System.Xaml.IXamlLineInfoConsumer
{
  .method /*060000DD*/ public hidebysig newslot abstract virtual 
          instance void  SetLineInfo(int32 lineNumber,
                                     int32 linePosition) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlLineInfoConsumer::SetLineInfo

  .method /*060000DE*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_ShouldProvideLineInfo() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IXamlLineInfoConsumer::get_ShouldProvideLineInfo

  .property /*1700003F*/ instance callconv(8) bool
          ShouldProvideLineInfo()
  {
    .get instance bool System.Xaml.IXamlLineInfoConsumer/*0200003D*/::get_ShouldProvideLineInfo() /* 060000DE */
  } // end of property IXamlLineInfoConsumer::ShouldProvideLineInfo
} // end of class System.Xaml.IXamlLineInfoConsumer

.class /*0200003E*/ public auto ansi sealed System.Xaml.XamlNodeType
       extends [mscorlib/*23000001*/]System.Enum/*01000014*/
{
  .field /*04000006*/ public specialname rtspecialname uint8 value__
  .field /*04000007*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ None = uint8(0x00)
  .field /*04000008*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ StartObject = uint8(0x01)
  .field /*04000009*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ GetObject = uint8(0x02)
  .field /*0400000A*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ EndObject = uint8(0x03)
  .field /*0400000B*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ StartMember = uint8(0x04)
  .field /*0400000C*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ EndMember = uint8(0x05)
  .field /*0400000D*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ Value = uint8(0x06)
  .field /*0400000E*/ public static literal valuetype System.Xaml.XamlNodeType/*0200003E*/ NamespaceDeclaration = uint8(0x07)
} // end of class System.Xaml.XamlNodeType

.class /*0200003F*/ public auto ansi beforefieldinit System.Xaml.XamlReaderSettings
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*060000DF*/ public hidebysig specialname 
          instance bool  get_AllowProtectedMembersOnRoot() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00008E:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::get_AllowProtectedMembersOnRoot

  .method /*060000E0*/ public hidebysig specialname 
          instance void  set_AllowProtectedMembersOnRoot(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00008F:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::set_AllowProtectedMembersOnRoot

  .method /*060000E1*/ public hidebysig specialname 
          instance bool  get_ProvideLineInfo() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000090:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f9
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::get_ProvideLineInfo

  .method /*060000E2*/ public hidebysig specialname 
          instance void  set_ProvideLineInfo(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000091:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::set_ProvideLineInfo

  .method /*060000E3*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Uri/*0100000C*/ 
          get_BaseUri() cil managed
  // SIG: 20 00 12 31
  {
    .custom /*0C000092:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::get_BaseUri

  .method /*060000E4*/ public hidebysig specialname 
          instance void  set_BaseUri(class [System/*23000002*/]System.Uri/*0100000C*/ 'value') cil managed
  // SIG: 20 01 01 12 31
  {
    .custom /*0C000093:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::set_BaseUri

  .method /*060000E5*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/ 
          get_LocalAssembly() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000094:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::get_LocalAssembly

  .method /*060000E6*/ public hidebysig specialname 
          instance void  set_LocalAssembly(class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/ 'value') cil managed
  // SIG: 20 01 01 12 61
  {
    .custom /*0C000095:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::set_LocalAssembly

  .method /*060000E7*/ public hidebysig specialname 
          instance bool  get_IgnoreUidsOnPropertyElements() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000096:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::get_IgnoreUidsOnPropertyElements

  .method /*060000E8*/ public hidebysig specialname 
          instance void  set_IgnoreUidsOnPropertyElements(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000097:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::set_IgnoreUidsOnPropertyElements

  .method /*060000E9*/ public hidebysig specialname 
          instance bool  get_ValuesMustBeString() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000098:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::get_ValuesMustBeString

  .method /*060000EA*/ public hidebysig specialname 
          instance void  set_ValuesMustBeString(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000099:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::set_ValuesMustBeString

  .method /*060000EB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2103
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::.ctor

  .method /*060000EC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlReaderSettings/*0200003F*/ settings) cil managed
  // SIG: 20 01 01 12 80 FC
  {
    // Method begins at RVA 0x2104
    // Code size       0 (0x0)
  } // end of method XamlReaderSettings::.ctor

  .property /*17000040*/ instance callconv(8) bool
          AllowProtectedMembersOnRoot()
  {
    .get instance bool System.Xaml.XamlReaderSettings/*0200003F*/::get_AllowProtectedMembersOnRoot() /* 060000DF */
    .set instance void System.Xaml.XamlReaderSettings/*0200003F*/::set_AllowProtectedMembersOnRoot(bool) /* 060000E0 */
  } // end of property XamlReaderSettings::AllowProtectedMembersOnRoot
  .property /*17000041*/ instance callconv(8) bool
          ProvideLineInfo()
  {
    .get instance bool System.Xaml.XamlReaderSettings/*0200003F*/::get_ProvideLineInfo() /* 060000E1 */
    .set instance void System.Xaml.XamlReaderSettings/*0200003F*/::set_ProvideLineInfo(bool) /* 060000E2 */
  } // end of property XamlReaderSettings::ProvideLineInfo
  .property /*17000042*/ instance callconv(8) class [System/*23000002*/]System.Uri/*0100000C*/
          BaseUri()
  {
    .set instance void System.Xaml.XamlReaderSettings/*0200003F*/::set_BaseUri(class [System/*23000002*/]System.Uri/*0100000C*/) /* 060000E4 */
    .get instance class [System/*23000002*/]System.Uri/*0100000C*/ System.Xaml.XamlReaderSettings/*0200003F*/::get_BaseUri() /* 060000E3 */
  } // end of property XamlReaderSettings::BaseUri
  .property /*17000043*/ instance callconv(8) class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/
          LocalAssembly()
  {
    .set instance void System.Xaml.XamlReaderSettings/*0200003F*/::set_LocalAssembly(class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/) /* 060000E6 */
    .get instance class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/ System.Xaml.XamlReaderSettings/*0200003F*/::get_LocalAssembly() /* 060000E5 */
  } // end of property XamlReaderSettings::LocalAssembly
  .property /*17000044*/ instance callconv(8) bool
          IgnoreUidsOnPropertyElements()
  {
    .get instance bool System.Xaml.XamlReaderSettings/*0200003F*/::get_IgnoreUidsOnPropertyElements() /* 060000E7 */
    .set instance void System.Xaml.XamlReaderSettings/*0200003F*/::set_IgnoreUidsOnPropertyElements(bool) /* 060000E8 */
  } // end of property XamlReaderSettings::IgnoreUidsOnPropertyElements
  .property /*17000045*/ instance callconv(8) bool
          ValuesMustBeString()
  {
    .get instance bool System.Xaml.XamlReaderSettings/*0200003F*/::get_ValuesMustBeString() /* 060000E9 */
    .set instance void System.Xaml.XamlReaderSettings/*0200003F*/::set_ValuesMustBeString(bool) /* 060000EA */
  } // end of property XamlReaderSettings::ValuesMustBeString
} // end of class System.Xaml.XamlReaderSettings

.class /*02000040*/ public auto ansi beforefieldinit System.Xaml.XamlObjectWriter
       extends System.Xaml.XamlWriter/*0200003C*/
       implements System.Xaml.IXamlLineInfoConsumer/*0200003D*/
{
  .method /*060000ED*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 01 01 12 81 88
  {
    // Method begins at RVA 0x2105
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::.ctor

  .method /*060000EE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlObjectWriterSettings/*02000042*/ settings) cil managed
  // SIG: 20 02 01 12 81 88 12 81 08
  {
    // Method begins at RVA 0x2106
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::.ctor

  .method /*060000EF*/ family hidebysig newslot virtual 
          instance void  OnAfterBeginInit(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2107
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::OnAfterBeginInit

  .method /*060000F0*/ family hidebysig newslot virtual 
          instance void  OnBeforeProperties(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2108
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::OnBeforeProperties

  .method /*060000F1*/ family hidebysig newslot virtual 
          instance void  OnAfterProperties(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2109
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::OnAfterProperties

  .method /*060000F2*/ family hidebysig newslot virtual 
          instance void  OnAfterEndInit(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x210a
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::OnAfterEndInit

  .method /*060000F3*/ family hidebysig newslot virtual 
          instance bool  OnSetValue(object eventSender,
                                    class System.Xaml.XamlMember/*02000054*/ member,
                                    object 'value') cil managed
  // SIG: 20 03 02 1C 12 81 50 1C
  {
    // Method begins at RVA 0x210b
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::OnSetValue

  .method /*060000F4*/ public hidebysig virtual 
          instance void  WriteGetObject() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210c
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteGetObject

  .method /*060000F5*/ public hidebysig virtual 
          instance void  WriteStartObject(class System.Xaml.XamlType/*02000065*/ xamlType) cil managed
  // SIG: 20 01 01 12 81 94
  {
    // Method begins at RVA 0x210d
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteStartObject

  .method /*060000F6*/ public hidebysig virtual 
          instance void  WriteEndObject() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210e
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteEndObject

  .method /*060000F7*/ public hidebysig virtual 
          instance void  WriteStartMember(class System.Xaml.XamlMember/*02000054*/ 'property') cil managed
  // SIG: 20 01 01 12 81 50
  {
    // Method begins at RVA 0x210f
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteStartMember

  .method /*060000F8*/ public hidebysig virtual 
          instance void  WriteEndMember() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2110
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteEndMember

  .method /*060000F9*/ public hidebysig virtual 
          instance void  WriteValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2111
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteValue

  .method /*060000FA*/ public hidebysig virtual 
          instance void  WriteNamespace(class System.Xaml.NamespaceDeclaration/*02000048*/ namespaceDeclaration) cil managed
  // SIG: 20 01 01 12 81 20
  {
    // Method begins at RVA 0x2112
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::WriteNamespace

  .method /*060000FB*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2113
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::Clear

  .method /*060000FC*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2114
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::Dispose

  .method /*060000FD*/ public hidebysig specialname 
          instance class System.Windows.Markup.INameScope/*02000018*/ 
          get_RootNameScope() cil managed
  // SIG: 20 00 12 60
  {
    // Method begins at RVA 0x2115
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::get_RootNameScope

  .method /*060000FE*/ public hidebysig newslot specialname virtual 
          instance object  get_Result() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2116
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::get_Result

  .method /*060000FF*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x2117
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::get_SchemaContext

  .method /*06000100*/ public hidebysig newslot virtual final 
          instance void  SetLineInfo(int32 lineNumber,
                                     int32 linePosition) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x2118
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::SetLineInfo

  .method /*06000101*/ public hidebysig newslot specialname virtual final 
          instance bool  get_ShouldProvideLineInfo() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2119
    // Code size       0 (0x0)
  } // end of method XamlObjectWriter::get_ShouldProvideLineInfo

  .property /*17000046*/ instance callconv(8) class System.Windows.Markup.INameScope/*02000018*/
          RootNameScope()
  {
    .get instance class System.Windows.Markup.INameScope/*02000018*/ System.Xaml.XamlObjectWriter/*02000040*/::get_RootNameScope() /* 060000FD */
  } // end of property XamlObjectWriter::RootNameScope
  .property /*17000047*/ instance callconv(8) object
          Result()
  {
    .get instance object System.Xaml.XamlObjectWriter/*02000040*/::get_Result() /* 060000FE */
  } // end of property XamlObjectWriter::Result
  .property /*17000048*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlObjectWriter/*02000040*/::get_SchemaContext() /* 060000FF */
  } // end of property XamlObjectWriter::SchemaContext
  .property /*17000049*/ instance callconv(8) bool
          ShouldProvideLineInfo()
  {
    .get instance bool System.Xaml.XamlObjectWriter/*02000040*/::get_ShouldProvideLineInfo() /* 06000101 */
  } // end of property XamlObjectWriter::ShouldProvideLineInfo
} // end of class System.Xaml.XamlObjectWriter

.class /*02000041*/ public auto ansi beforefieldinit System.Xaml.XamlWriterSettings
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000102*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211a
    // Code size       0 (0x0)
  } // end of method XamlWriterSettings::.ctor

  .method /*06000103*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlWriterSettings/*02000041*/ settings) cil managed
  // SIG: 20 01 01 12 81 04
  {
    // Method begins at RVA 0x211b
    // Code size       0 (0x0)
  } // end of method XamlWriterSettings::.ctor

} // end of class System.Xaml.XamlWriterSettings

.class /*02000042*/ public auto ansi beforefieldinit System.Xaml.XamlObjectWriterSettings
       extends System.Xaml.XamlWriterSettings/*02000041*/
{
  .method /*06000104*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211c
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::.ctor

  .method /*06000105*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlObjectWriterSettings/*02000042*/ settings) cil managed
  // SIG: 20 01 01 12 81 08
  {
    // Method begins at RVA 0x211d
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::.ctor

  .method /*06000106*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 
          get_AfterBeginInitHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 E0
  {
    .custom /*0C00009A:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211e
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_AfterBeginInitHandler

  .method /*06000107*/ public hidebysig specialname 
          instance void  set_AfterBeginInitHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 80 E0
  {
    .custom /*0C00009B:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_AfterBeginInitHandler

  .method /*06000108*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 
          get_BeforePropertiesHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 E0
  {
    .custom /*0C00009C:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2120
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_BeforePropertiesHandler

  .method /*06000109*/ public hidebysig specialname 
          instance void  set_BeforePropertiesHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 80 E0
  {
    .custom /*0C00009D:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2121
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_BeforePropertiesHandler

  .method /*0600010A*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 
          get_AfterPropertiesHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 E0
  {
    .custom /*0C00009E:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_AfterPropertiesHandler

  .method /*0600010B*/ public hidebysig specialname 
          instance void  set_AfterPropertiesHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 80 E0
  {
    .custom /*0C00009F:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2123
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_AfterPropertiesHandler

  .method /*0600010C*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 
          get_AfterEndInitHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 E0
  {
    .custom /*0C0000A0:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2124
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_AfterEndInitHandler

  .method /*0600010D*/ public hidebysig specialname 
          instance void  set_AfterEndInitHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 80 E0
  {
    .custom /*0C0000A1:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_AfterEndInitHandler

  .method /*0600010E*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/> 
          get_XamlSetValueHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 7C
  {
    .custom /*0C0000A2:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2126
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_XamlSetValueHandler

  .method /*0600010F*/ public hidebysig specialname 
          instance void  set_XamlSetValueHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 7C
  {
    .custom /*0C0000A3:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2127
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_XamlSetValueHandler

  .method /*06000110*/ public hidebysig specialname 
          instance object  get_RootObjectInstance() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0000A4:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_RootObjectInstance

  .method /*06000111*/ public hidebysig specialname 
          instance void  set_RootObjectInstance(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000A5:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2129
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_RootObjectInstance

  .method /*06000112*/ public hidebysig specialname 
          instance bool  get_IgnoreCanConvert() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000A6:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212a
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_IgnoreCanConvert

  .method /*06000113*/ public hidebysig specialname 
          instance void  set_IgnoreCanConvert(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000A7:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_IgnoreCanConvert

  .method /*06000114*/ public hidebysig specialname 
          instance class System.Windows.Markup.INameScope/*02000018*/ 
          get_ExternalNameScope() cil managed
  // SIG: 20 00 12 60
  {
    .custom /*0C0000A8:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212c
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_ExternalNameScope

  .method /*06000115*/ public hidebysig specialname 
          instance void  set_ExternalNameScope(class System.Windows.Markup.INameScope/*02000018*/ 'value') cil managed
  // SIG: 20 01 01 12 60
  {
    .custom /*0C0000A9:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212d
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_ExternalNameScope

  .method /*06000116*/ public hidebysig specialname 
          instance bool  get_SkipDuplicatePropertyCheck() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000AA:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_SkipDuplicatePropertyCheck

  .method /*06000117*/ public hidebysig specialname 
          instance void  set_SkipDuplicatePropertyCheck(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000AB:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212f
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_SkipDuplicatePropertyCheck

  .method /*06000118*/ public hidebysig specialname 
          instance bool  get_RegisterNamesOnExternalNamescope() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000AC:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2130
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_RegisterNamesOnExternalNamescope

  .method /*06000119*/ public hidebysig specialname 
          instance void  set_RegisterNamesOnExternalNamescope(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000AD:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_RegisterNamesOnExternalNamescope

  .method /*0600011A*/ public hidebysig specialname 
          instance bool  get_SkipProvideValueOnRoot() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000AE:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2132
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_SkipProvideValueOnRoot

  .method /*0600011B*/ public hidebysig specialname 
          instance void  set_SkipProvideValueOnRoot(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000AF:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2133
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_SkipProvideValueOnRoot

  .method /*0600011C*/ public hidebysig specialname 
          instance bool  get_PreferUnconvertedDictionaryKeys() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000B0:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_PreferUnconvertedDictionaryKeys

  .method /*0600011D*/ public hidebysig specialname 
          instance void  set_PreferUnconvertedDictionaryKeys(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000B1:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2135
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_PreferUnconvertedDictionaryKeys

  .method /*0600011E*/ public hidebysig specialname 
          instance class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ 
          get_AccessLevel() cil managed
  // SIG: 20 00 12 81 2C
  {
    .custom /*0C0000B2:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2136
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::get_AccessLevel

  .method /*0600011F*/ public hidebysig specialname 
          instance void  set_AccessLevel(class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ 'value') cil managed
  // SIG: 20 01 01 12 81 2C
  {
    .custom /*0C0000B3:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterSettings::set_AccessLevel

  .property /*1700004A*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>
          AfterBeginInitHandler()
  {
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_AfterBeginInitHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>) /* 06000107 */
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> System.Xaml.XamlObjectWriterSettings/*02000042*/::get_AfterBeginInitHandler() /* 06000106 */
  } // end of property XamlObjectWriterSettings::AfterBeginInitHandler
  .property /*1700004B*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>
          BeforePropertiesHandler()
  {
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_BeforePropertiesHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>) /* 06000109 */
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> System.Xaml.XamlObjectWriterSettings/*02000042*/::get_BeforePropertiesHandler() /* 06000108 */
  } // end of property XamlObjectWriterSettings::BeforePropertiesHandler
  .property /*1700004C*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>
          AfterPropertiesHandler()
  {
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> System.Xaml.XamlObjectWriterSettings/*02000042*/::get_AfterPropertiesHandler() /* 0600010A */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_AfterPropertiesHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>) /* 0600010B */
  } // end of property XamlObjectWriterSettings::AfterPropertiesHandler
  .property /*1700004D*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>
          AfterEndInitHandler()
  {
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/> System.Xaml.XamlObjectWriterSettings/*02000042*/::get_AfterEndInitHandler() /* 0600010C */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_AfterEndInitHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Xaml.XamlObjectEventArgs/*02000038*/>) /* 0600010D */
  } // end of property XamlObjectWriterSettings::AfterEndInitHandler
  .property /*1700004E*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/>
          XamlSetValueHandler()
  {
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/> System.Xaml.XamlObjectWriterSettings/*02000042*/::get_XamlSetValueHandler() /* 0600010E */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_XamlSetValueHandler(class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetValueEventArgs/*0200001F*/>) /* 0600010F */
  } // end of property XamlObjectWriterSettings::XamlSetValueHandler
  .property /*1700004F*/ instance callconv(8) object
          RootObjectInstance()
  {
    .get instance object System.Xaml.XamlObjectWriterSettings/*02000042*/::get_RootObjectInstance() /* 06000110 */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_RootObjectInstance(object) /* 06000111 */
  } // end of property XamlObjectWriterSettings::RootObjectInstance
  .property /*17000050*/ instance callconv(8) bool
          IgnoreCanConvert()
  {
    .get instance bool System.Xaml.XamlObjectWriterSettings/*02000042*/::get_IgnoreCanConvert() /* 06000112 */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_IgnoreCanConvert(bool) /* 06000113 */
  } // end of property XamlObjectWriterSettings::IgnoreCanConvert
  .property /*17000051*/ instance callconv(8) class System.Windows.Markup.INameScope/*02000018*/
          ExternalNameScope()
  {
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_ExternalNameScope(class System.Windows.Markup.INameScope/*02000018*/) /* 06000115 */
    .get instance class System.Windows.Markup.INameScope/*02000018*/ System.Xaml.XamlObjectWriterSettings/*02000042*/::get_ExternalNameScope() /* 06000114 */
  } // end of property XamlObjectWriterSettings::ExternalNameScope
  .property /*17000052*/ instance callconv(8) bool
          SkipDuplicatePropertyCheck()
  {
    .get instance bool System.Xaml.XamlObjectWriterSettings/*02000042*/::get_SkipDuplicatePropertyCheck() /* 06000116 */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_SkipDuplicatePropertyCheck(bool) /* 06000117 */
  } // end of property XamlObjectWriterSettings::SkipDuplicatePropertyCheck
  .property /*17000053*/ instance callconv(8) bool
          RegisterNamesOnExternalNamescope()
  {
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_RegisterNamesOnExternalNamescope(bool) /* 06000119 */
    .get instance bool System.Xaml.XamlObjectWriterSettings/*02000042*/::get_RegisterNamesOnExternalNamescope() /* 06000118 */
  } // end of property XamlObjectWriterSettings::RegisterNamesOnExternalNamescope
  .property /*17000054*/ instance callconv(8) bool
          SkipProvideValueOnRoot()
  {
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_SkipProvideValueOnRoot(bool) /* 0600011B */
    .get instance bool System.Xaml.XamlObjectWriterSettings/*02000042*/::get_SkipProvideValueOnRoot() /* 0600011A */
  } // end of property XamlObjectWriterSettings::SkipProvideValueOnRoot
  .property /*17000055*/ instance callconv(8) bool
          PreferUnconvertedDictionaryKeys()
  {
    .get instance bool System.Xaml.XamlObjectWriterSettings/*02000042*/::get_PreferUnconvertedDictionaryKeys() /* 0600011C */
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_PreferUnconvertedDictionaryKeys(bool) /* 0600011D */
  } // end of property XamlObjectWriterSettings::PreferUnconvertedDictionaryKeys
  .property /*17000056*/ instance callconv(8) class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/
          AccessLevel()
  {
    .set instance void System.Xaml.XamlObjectWriterSettings/*02000042*/::set_AccessLevel(class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/) /* 0600011F */
    .get instance class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ System.Xaml.XamlObjectWriterSettings/*02000042*/::get_AccessLevel() /* 0600011E */
  } // end of property XamlObjectWriterSettings::AccessLevel
} // end of class System.Xaml.XamlObjectWriterSettings

.class /*02000043*/ public abstract auto ansi beforefieldinit System.Xaml.XamlReader
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements [mscorlib/*23000001*/]System.IDisposable/*01000017*/
{
  .method /*06000120*/ public hidebysig newslot abstract virtual 
          instance bool  Read() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::Read

  .method /*06000121*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype System.Xaml.XamlNodeType/*0200003E*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 F8
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_NodeType

  .method /*06000122*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsEof() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_IsEof

  .method /*06000123*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Xaml.NamespaceDeclaration/*02000048*/ 
          get_Namespace() cil managed
  // SIG: 20 00 12 81 20
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_Namespace

  .method /*06000124*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_Type

  .method /*06000125*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_Value

  .method /*06000126*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_Member() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_Member

  .method /*06000127*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x0
  } // end of method XamlReader::get_SchemaContext

  .method /*06000128*/ public hidebysig newslot virtual 
          instance void  Skip() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2138
    // Code size       0 (0x0)
  } // end of method XamlReader::Skip

  .method /*06000129*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [mscorlib/*23000001*/]System.IDisposable/*01000017*/::Dispose /*01000017::0A000001*/ 
    // Method begins at RVA 0x2139
    // Code size       0 (0x0)
  } // end of method XamlReader::System.IDisposable.Dispose

  .method /*0600012A*/ family hidebysig specialname 
          instance bool  get_IsDisposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000B4:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       0 (0x0)
  } // end of method XamlReader::get_IsDisposed

  .method /*0600012B*/ private hidebysig specialname 
          instance void  set_IsDisposed(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000B5:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213b
    // Code size       0 (0x0)
  } // end of method XamlReader::set_IsDisposed

  .method /*0600012C*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x213c
    // Code size       0 (0x0)
  } // end of method XamlReader::Dispose

  .method /*0600012D*/ public hidebysig instance void 
          Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213d
    // Code size       0 (0x0)
  } // end of method XamlReader::Close

  .method /*0600012E*/ public hidebysig newslot virtual 
          instance class System.Xaml.XamlReader/*02000043*/ 
          ReadSubtree() cil managed
  // SIG: 20 00 12 81 0C
  {
    // Method begins at RVA 0x213e
    // Code size       0 (0x0)
  } // end of method XamlReader::ReadSubtree

  .method /*0600012F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213f
    // Code size       0 (0x0)
  } // end of method XamlReader::.ctor

  .property /*17000057*/ instance callconv(8) valuetype System.Xaml.XamlNodeType/*0200003E*/
          NodeType()
  {
    .get instance valuetype System.Xaml.XamlNodeType/*0200003E*/ System.Xaml.XamlReader/*02000043*/::get_NodeType() /* 06000121 */
  } // end of property XamlReader::NodeType
  .property /*17000058*/ instance callconv(8) bool
          IsEof()
  {
    .get instance bool System.Xaml.XamlReader/*02000043*/::get_IsEof() /* 06000122 */
  } // end of property XamlReader::IsEof
  .property /*17000059*/ instance callconv(8) class System.Xaml.NamespaceDeclaration/*02000048*/
          Namespace()
  {
    .get instance class System.Xaml.NamespaceDeclaration/*02000048*/ System.Xaml.XamlReader/*02000043*/::get_Namespace() /* 06000123 */
  } // end of property XamlReader::Namespace
  .property /*1700005A*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlReader/*02000043*/::get_Type() /* 06000124 */
  } // end of property XamlReader::Type
  .property /*1700005B*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Xaml.XamlReader/*02000043*/::get_Value() /* 06000125 */
  } // end of property XamlReader::Value
  .property /*1700005C*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          Member()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.XamlReader/*02000043*/::get_Member() /* 06000126 */
  } // end of property XamlReader::Member
  .property /*1700005D*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlReader/*02000043*/::get_SchemaContext() /* 06000127 */
  } // end of property XamlReader::SchemaContext
  .property /*1700005E*/ instance callconv(8) bool
          IsDisposed()
  {
    .set instance void System.Xaml.XamlReader/*02000043*/::set_IsDisposed(bool) /* 0600012B */
    .get instance bool System.Xaml.XamlReader/*02000043*/::get_IsDisposed() /* 0600012A */
  } // end of property XamlReader::IsDisposed
} // end of class System.Xaml.XamlReader

.class /*02000044*/ public auto ansi beforefieldinit System.Xaml.XamlXmlReader
       extends System.Xaml.XamlReader/*02000043*/
       implements System.Xaml.IXamlLineInfo/*02000036*/
{
  .method /*06000130*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100001A*/ xmlReader) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2140
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100001A*/ xmlReader,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 02 01 12 69 12 81 14
  {
    // Method begins at RVA 0x2141
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000132*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100001A*/ xmlReader,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 69 12 81 88
  {
    // Method begins at RVA 0x2142
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000133*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100001A*/ xmlReader,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 03 01 12 69 12 81 88 12 81 14
  {
    // Method begins at RVA 0x2143
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000134*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2144
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000135*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 02 01 0E 12 81 14
  {
    // Method begins at RVA 0x2145
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000136*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 0E 12 81 88
  {
    // Method begins at RVA 0x2146
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000137*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 03 01 0E 12 81 88 12 81 14
  {
    // Method begins at RVA 0x2147
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000138*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2148
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000139*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 02 01 12 6D 12 81 14
  {
    // Method begins at RVA 0x2149
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*0600013A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 6D 12 81 88
  {
    // Method begins at RVA 0x214a
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*0600013B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 03 01 12 6D 12 81 88 12 81 14
  {
    // Method begins at RVA 0x214b
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*0600013C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextReader/*0100001C*/ textReader) cil managed
  // SIG: 20 01 01 12 71
  {
    // Method begins at RVA 0x214c
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*0600013D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextReader/*0100001C*/ textReader,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 02 01 12 71 12 81 14
  {
    // Method begins at RVA 0x214d
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*0600013E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextReader/*0100001C*/ textReader,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 71 12 81 88
  {
    // Method begins at RVA 0x214e
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*0600013F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextReader/*0100001C*/ textReader,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 03 01 12 71 12 81 88 12 81 14
  {
    // Method begins at RVA 0x214f
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::.ctor

  .method /*06000140*/ public hidebysig virtual 
          instance bool  Read() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2150
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::Read

  .method /*06000141*/ public hidebysig specialname virtual 
          instance valuetype System.Xaml.XamlNodeType/*0200003E*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 F8
  {
    // Method begins at RVA 0x2151
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_NodeType

  .method /*06000142*/ public hidebysig specialname virtual 
          instance bool  get_IsEof() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2152
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_IsEof

  .method /*06000143*/ public hidebysig specialname virtual 
          instance class System.Xaml.NamespaceDeclaration/*02000048*/ 
          get_Namespace() cil managed
  // SIG: 20 00 12 81 20
  {
    // Method begins at RVA 0x2153
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_Namespace

  .method /*06000144*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x2154
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_Type

  .method /*06000145*/ public hidebysig specialname virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2155
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_Value

  .method /*06000146*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_Member() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x2156
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_Member

  .method /*06000147*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x2157
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_SchemaContext

  .method /*06000148*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HasLineInfo() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2158
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_HasLineInfo

  .method /*06000149*/ public hidebysig newslot specialname virtual final 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2159
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_LineNumber

  .method /*0600014A*/ public hidebysig newslot specialname virtual final 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x215a
    // Code size       0 (0x0)
  } // end of method XamlXmlReader::get_LinePosition

  .property /*1700005F*/ instance callconv(8) valuetype System.Xaml.XamlNodeType/*0200003E*/
          NodeType()
  {
    .get instance valuetype System.Xaml.XamlNodeType/*0200003E*/ System.Xaml.XamlXmlReader/*02000044*/::get_NodeType() /* 06000141 */
  } // end of property XamlXmlReader::NodeType
  .property /*17000060*/ instance callconv(8) bool
          IsEof()
  {
    .get instance bool System.Xaml.XamlXmlReader/*02000044*/::get_IsEof() /* 06000142 */
  } // end of property XamlXmlReader::IsEof
  .property /*17000061*/ instance callconv(8) class System.Xaml.NamespaceDeclaration/*02000048*/
          Namespace()
  {
    .get instance class System.Xaml.NamespaceDeclaration/*02000048*/ System.Xaml.XamlXmlReader/*02000044*/::get_Namespace() /* 06000143 */
  } // end of property XamlXmlReader::Namespace
  .property /*17000062*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlXmlReader/*02000044*/::get_Type() /* 06000144 */
  } // end of property XamlXmlReader::Type
  .property /*17000063*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Xaml.XamlXmlReader/*02000044*/::get_Value() /* 06000145 */
  } // end of property XamlXmlReader::Value
  .property /*17000064*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          Member()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.XamlXmlReader/*02000044*/::get_Member() /* 06000146 */
  } // end of property XamlXmlReader::Member
  .property /*17000065*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlXmlReader/*02000044*/::get_SchemaContext() /* 06000147 */
  } // end of property XamlXmlReader::SchemaContext
  .property /*17000066*/ instance callconv(8) bool
          HasLineInfo()
  {
    .get instance bool System.Xaml.XamlXmlReader/*02000044*/::get_HasLineInfo() /* 06000148 */
  } // end of property XamlXmlReader::HasLineInfo
  .property /*17000067*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xaml.XamlXmlReader/*02000044*/::get_LineNumber() /* 06000149 */
  } // end of property XamlXmlReader::LineNumber
  .property /*17000068*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xaml.XamlXmlReader/*02000044*/::get_LinePosition() /* 0600014A */
  } // end of property XamlXmlReader::LinePosition
} // end of class System.Xaml.XamlXmlReader

.class /*02000045*/ public auto ansi beforefieldinit System.Xaml.XamlXmlReaderSettings
       extends System.Xaml.XamlReaderSettings/*0200003F*/
{
  .method /*0600014B*/ public hidebysig specialname 
          instance string  get_XmlLang() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B6:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215b
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::get_XmlLang

  .method /*0600014C*/ public hidebysig specialname 
          instance void  set_XmlLang(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000B7:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::set_XmlLang

  .method /*0600014D*/ public hidebysig specialname 
          instance bool  get_XmlSpacePreserve() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000B8:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215d
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::get_XmlSpacePreserve

  .method /*0600014E*/ public hidebysig specialname 
          instance void  set_XmlSpacePreserve(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000B9:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215e
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::set_XmlSpacePreserve

  .method /*0600014F*/ public hidebysig specialname 
          instance bool  get_SkipXmlCompatibilityProcessing() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000BA:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::get_SkipXmlCompatibilityProcessing

  .method /*06000150*/ public hidebysig specialname 
          instance void  set_SkipXmlCompatibilityProcessing(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000BB:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2160
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::set_SkipXmlCompatibilityProcessing

  .method /*06000151*/ public hidebysig specialname 
          instance bool  get_CloseInput() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000BC:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2161
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::get_CloseInput

  .method /*06000152*/ public hidebysig specialname 
          instance void  set_CloseInput(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000BD:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::set_CloseInput

  .method /*06000153*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2163
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::.ctor

  .method /*06000154*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlXmlReaderSettings/*02000045*/ settings) cil managed
  // SIG: 20 01 01 12 81 14
  {
    // Method begins at RVA 0x2164
    // Code size       0 (0x0)
  } // end of method XamlXmlReaderSettings::.ctor

  .property /*17000069*/ instance callconv(8) string
          XmlLang()
  {
    .get instance string System.Xaml.XamlXmlReaderSettings/*02000045*/::get_XmlLang() /* 0600014B */
    .set instance void System.Xaml.XamlXmlReaderSettings/*02000045*/::set_XmlLang(string) /* 0600014C */
  } // end of property XamlXmlReaderSettings::XmlLang
  .property /*1700006A*/ instance callconv(8) bool
          XmlSpacePreserve()
  {
    .get instance bool System.Xaml.XamlXmlReaderSettings/*02000045*/::get_XmlSpacePreserve() /* 0600014D */
    .set instance void System.Xaml.XamlXmlReaderSettings/*02000045*/::set_XmlSpacePreserve(bool) /* 0600014E */
  } // end of property XamlXmlReaderSettings::XmlSpacePreserve
  .property /*1700006B*/ instance callconv(8) bool
          SkipXmlCompatibilityProcessing()
  {
    .set instance void System.Xaml.XamlXmlReaderSettings/*02000045*/::set_SkipXmlCompatibilityProcessing(bool) /* 06000150 */
    .get instance bool System.Xaml.XamlXmlReaderSettings/*02000045*/::get_SkipXmlCompatibilityProcessing() /* 0600014F */
  } // end of property XamlXmlReaderSettings::SkipXmlCompatibilityProcessing
  .property /*1700006C*/ instance callconv(8) bool
          CloseInput()
  {
    .set instance void System.Xaml.XamlXmlReaderSettings/*02000045*/::set_CloseInput(bool) /* 06000152 */
    .get instance bool System.Xaml.XamlXmlReaderSettings/*02000045*/::get_CloseInput() /* 06000151 */
  } // end of property XamlXmlReaderSettings::CloseInput
} // end of class System.Xaml.XamlXmlReaderSettings

.class /*02000046*/ interface public abstract auto ansi System.Xaml.IXamlNameProvider
{
  .method /*06000155*/ public hidebysig newslot abstract virtual 
          instance string  GetName(object 'value') cil managed
  // SIG: 20 01 0E 1C
  {
    // Method begins at RVA 0x0
  } // end of method IXamlNameProvider::GetName

} // end of class System.Xaml.IXamlNameProvider

.class /*02000047*/ public auto ansi beforefieldinit System.Windows.Markup.NameReferenceConverter
       extends [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/
{
  .method /*06000156*/ public hidebysig virtual 
          instance bool  CanConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                        class [mscorlib/*23000001*/]System.Type/*01000003*/ sourceType) cil managed
  // SIG: 20 02 02 12 35 12 0D
  {
    // Method begins at RVA 0x2165
    // Code size       0 (0x0)
  } // end of method NameReferenceConverter::CanConvertFrom

  .method /*06000157*/ public hidebysig virtual 
          instance object  ConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ culture,
                                       object 'value') cil managed
  // SIG: 20 03 1C 12 35 12 45 1C
  {
    // Method begins at RVA 0x2166
    // Code size       0 (0x0)
  } // end of method NameReferenceConverter::ConvertFrom

  .method /*06000158*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 02 02 12 35 12 0D
  {
    // Method begins at RVA 0x2167
    // Code size       0 (0x0)
  } // end of method NameReferenceConverter::CanConvertTo

  .method /*06000159*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 04 1C 12 35 12 45 1C 12 0D
  {
    // Method begins at RVA 0x2168
    // Code size       0 (0x0)
  } // end of method NameReferenceConverter::ConvertTo

  .method /*0600015A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2169
    // Code size       0 (0x0)
  } // end of method NameReferenceConverter::.ctor

} // end of class System.Windows.Markup.NameReferenceConverter

.class /*02000048*/ public auto ansi beforefieldinit System.Xaml.NamespaceDeclaration
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00006A:0A000023*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerDisplayAttribute/*01000056*/::.ctor(string) /* 0A000023 */ = ( 01 00 25 50 72 65 66 69 78 3D 7B 50 72 65 66 69   // ..%Prefix={Prefi
                                                                                                                                                              78 7D 20 4E 61 6D 65 73 70 61 63 65 3D 7B 4E 61   // x} Namespace={Na
                                                                                                                                                              6D 65 73 70 61 63 65 7D 00 00 )                   // mespace}..
  .method /*0600015B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string ns,
                               string prefix) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x216a
    // Code size       0 (0x0)
  } // end of method NamespaceDeclaration::.ctor

  .method /*0600015C*/ public hidebysig specialname 
          instance string  get_Prefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x216b
    // Code size       0 (0x0)
  } // end of method NamespaceDeclaration::get_Prefix

  .method /*0600015D*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x216c
    // Code size       0 (0x0)
  } // end of method NamespaceDeclaration::get_Namespace

  .property /*1700006D*/ instance callconv(8) string
          Prefix()
  {
    .get instance string System.Xaml.NamespaceDeclaration/*02000048*/::get_Prefix() /* 0600015C */
  } // end of property NamespaceDeclaration::Prefix
  .property /*1700006E*/ instance callconv(8) string
          Namespace()
  {
    .get instance string System.Xaml.NamespaceDeclaration/*02000048*/::get_Namespace() /* 0600015D */
  } // end of property NamespaceDeclaration::Namespace
} // end of class System.Xaml.NamespaceDeclaration

.class /*02000049*/ public auto ansi serializable beforefieldinit System.Xaml.XamlException
       extends [mscorlib/*23000001*/]System.Exception/*01000005*/
{
  .method /*0600015E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException,
                               int32 lineNumber,
                               int32 linePosition) cil managed
  // SIG: 20 04 01 0E 12 15 08 08
  {
    // Method begins at RVA 0x216d
    // Code size       0 (0x0)
  } // end of method XamlException::.ctor

  .method /*0600015F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x216e
    // Code size       0 (0x0)
  } // end of method XamlException::.ctor

  .method /*06000160*/ public hidebysig specialname virtual 
          instance string  get_Message() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x216f
    // Code size       0 (0x0)
  } // end of method XamlException::get_Message

  .method /*06000161*/ public hidebysig specialname 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000BE:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       0 (0x0)
  } // end of method XamlException::get_LineNumber

  .method /*06000162*/ family hidebysig specialname 
          instance void  set_LineNumber(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000BF:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       0 (0x0)
  } // end of method XamlException::set_LineNumber

  .method /*06000163*/ public hidebysig specialname 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C0:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2172
    // Code size       0 (0x0)
  } // end of method XamlException::get_LinePosition

  .method /*06000164*/ family hidebysig specialname 
          instance void  set_LinePosition(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000C1:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       0 (0x0)
  } // end of method XamlException::set_LinePosition

  .method /*06000165*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2174
    // Code size       0 (0x0)
  } // end of method XamlException::.ctor

  .method /*06000166*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2175
    // Code size       0 (0x0)
  } // end of method XamlException::.ctor

  .method /*06000167*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x2176
    // Code size       0 (0x0)
  } // end of method XamlException::.ctor

  .method /*06000168*/ public hidebysig virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    .custom /*0C0000C2:0A00001F*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*01000052*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       0 (0x0)
  } // end of method XamlException::GetObjectData

  .property /*1700006F*/ instance callconv(8) string
          Message()
  {
    .get instance string System.Xaml.XamlException/*02000049*/::get_Message() /* 06000160 */
  } // end of property XamlException::Message
  .property /*17000070*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xaml.XamlException/*02000049*/::get_LineNumber() /* 06000161 */
    .set instance void System.Xaml.XamlException/*02000049*/::set_LineNumber(int32) /* 06000162 */
  } // end of property XamlException::LineNumber
  .property /*17000071*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xaml.XamlException/*02000049*/::get_LinePosition() /* 06000163 */
    .set instance void System.Xaml.XamlException/*02000049*/::set_LinePosition(int32) /* 06000164 */
  } // end of property XamlException::LinePosition
} // end of class System.Xaml.XamlException

.class /*0200004A*/ public auto ansi serializable beforefieldinit System.Xaml.XamlParseException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*06000169*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2178
    // Code size       0 (0x0)
  } // end of method XamlParseException::.ctor

  .method /*0600016A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2179
    // Code size       0 (0x0)
  } // end of method XamlParseException::.ctor

  .method /*0600016B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x217a
    // Code size       0 (0x0)
  } // end of method XamlParseException::.ctor

  .method /*0600016C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x217b
    // Code size       0 (0x0)
  } // end of method XamlParseException::.ctor

} // end of class System.Xaml.XamlParseException

.class /*0200004B*/ public auto ansi serializable beforefieldinit System.Xaml.Permissions.XamlAccessLevel
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600016D*/ public hidebysig static 
          class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ 
          AssemblyAccessTo(class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/ 'assembly') cil managed
  // SIG: 00 01 12 81 2C 12 61
  {
    // Method begins at RVA 0x217c
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::AssemblyAccessTo

  .method /*0600016E*/ public hidebysig static 
          class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ 
          AssemblyAccessTo(class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*0100001F*/ assemblyName) cil managed
  // SIG: 00 01 12 81 2C 12 7D
  {
    // Method begins at RVA 0x217d
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::AssemblyAccessTo

  .method /*0600016F*/ public hidebysig static 
          class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ 
          PrivateAccessTo(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 00 01 12 81 2C 12 0D
  {
    // Method begins at RVA 0x217e
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::PrivateAccessTo

  .method /*06000170*/ public hidebysig static 
          class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ 
          PrivateAccessTo(string assemblyQualifiedTypeName) cil managed
  // SIG: 00 01 12 81 2C 0E
  {
    // Method begins at RVA 0x217f
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::PrivateAccessTo

  .method /*06000171*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*0100001F*/ 
          get_AssemblyAccessToAssemblyName() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x2180
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::get_AssemblyAccessToAssemblyName

  .method /*06000172*/ public hidebysig specialname 
          instance string  get_PrivateAccessToTypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C3:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2181
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::get_PrivateAccessToTypeName

  .method /*06000173*/ private hidebysig specialname 
          instance void  set_PrivateAccessToTypeName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C4:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::set_PrivateAccessToTypeName

  .method /*06000174*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2183
    // Code size       0 (0x0)
  } // end of method XamlAccessLevel::.ctor

  .property /*17000072*/ instance callconv(8) class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*0100001F*/
          AssemblyAccessToAssemblyName()
  {
    .get instance class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*0100001F*/ System.Xaml.Permissions.XamlAccessLevel/*0200004B*/::get_AssemblyAccessToAssemblyName() /* 06000171 */
  } // end of property XamlAccessLevel::AssemblyAccessToAssemblyName
  .property /*17000073*/ instance callconv(8) string
          PrivateAccessToTypeName()
  {
    .set instance void System.Xaml.Permissions.XamlAccessLevel/*0200004B*/::set_PrivateAccessToTypeName(string) /* 06000173 */
    .get instance string System.Xaml.Permissions.XamlAccessLevel/*0200004B*/::get_PrivateAccessToTypeName() /* 06000172 */
  } // end of property XamlAccessLevel::PrivateAccessToTypeName
} // end of class System.Xaml.Permissions.XamlAccessLevel

.class /*0200004C*/ public auto ansi serializable sealed beforefieldinit System.Xaml.Permissions.XamlLoadPermission
       extends [mscorlib/*23000001*/]System.Security.CodeAccessPermission/*01000020*/
       implements [mscorlib/*23000001*/]System.Security.Permissions.IUnrestrictedPermission/*01000021*/
{
  .method /*06000175*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib/*23000001*/]System.Security.Permissions.PermissionState/*01000022*/ state) cil managed
  // SIG: 20 01 01 11 80 89
  {
    // Method begins at RVA 0x2184
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::.ctor

  .method /*06000176*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ allowedAccess) cil managed
  // SIG: 20 01 01 12 81 2C
  {
    // Method begins at RVA 0x2185
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::.ctor

  .method /*06000177*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/> allowedAccess) cil managed
  // SIG: 20 01 01 15 12 09 01 12 81 2C
  {
    // Method begins at RVA 0x2186
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::.ctor

  .method /*06000178*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/> 
          get_AllowedAccess() cil managed
  // SIG: 20 00 15 12 19 01 12 81 2C
  {
    .custom /*0C0000C5:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2187
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::get_AllowedAccess

  .method /*06000179*/ private hidebysig specialname 
          instance void  set_AllowedAccess(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 19 01 12 81 2C
  {
    .custom /*0C0000C6:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2188
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::set_AllowedAccess

  .method /*0600017A*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Security.IPermission/*01000023*/ 
          Copy() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x2189
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::Copy

  .method /*0600017B*/ public hidebysig virtual 
          instance void  FromXml(class [mscorlib/*23000001*/]System.Security.SecurityElement/*01000024*/ elem) cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x218a
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::FromXml

  .method /*0600017C*/ public hidebysig instance bool 
          Includes(class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/ requestedAccess) cil managed
  // SIG: 20 01 02 12 81 2C
  {
    // Method begins at RVA 0x218b
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::Includes

  .method /*0600017D*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Security.IPermission/*01000023*/ 
          Intersect(class [mscorlib/*23000001*/]System.Security.IPermission/*01000023*/ target) cil managed
  // SIG: 20 01 12 80 8D 12 80 8D
  {
    // Method begins at RVA 0x218c
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::Intersect

  .method /*0600017E*/ public hidebysig virtual 
          instance bool  IsSubsetOf(class [mscorlib/*23000001*/]System.Security.IPermission/*01000023*/ target) cil managed
  // SIG: 20 01 02 12 80 8D
  {
    // Method begins at RVA 0x218d
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::IsSubsetOf

  .method /*0600017F*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Security.SecurityElement/*01000024*/ 
          ToXml() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x218e
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::ToXml

  .method /*06000180*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Security.IPermission/*01000023*/ 
          Union(class [mscorlib/*23000001*/]System.Security.IPermission/*01000023*/ other) cil managed
  // SIG: 20 01 12 80 8D 12 80 8D
  {
    // Method begins at RVA 0x218f
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::Union

  .method /*06000181*/ public hidebysig newslot virtual final 
          instance bool  IsUnrestricted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2190
    // Code size       0 (0x0)
  } // end of method XamlLoadPermission::IsUnrestricted

  .property /*17000074*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/>
          AllowedAccess()
  {
    .set instance void System.Xaml.Permissions.XamlLoadPermission/*0200004C*/::set_AllowedAccess(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/>) /* 06000179 */
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Permissions.XamlAccessLevel/*0200004B*/> System.Xaml.Permissions.XamlLoadPermission/*0200004C*/::get_AllowedAccess() /* 06000178 */
  } // end of property XamlLoadPermission::AllowedAccess
} // end of class System.Xaml.Permissions.XamlLoadPermission

.class /*0200004D*/ interface public abstract auto ansi System.Xaml.IXamlIndexingReader
{
  .method /*06000182*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlIndexingReader::get_Count

  .method /*06000183*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_CurrentIndex() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlIndexingReader::get_CurrentIndex

  .method /*06000184*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_CurrentIndex(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IXamlIndexingReader::set_CurrentIndex

  .property /*17000075*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Xaml.IXamlIndexingReader/*0200004D*/::get_Count() /* 06000182 */
  } // end of property IXamlIndexingReader::Count
  .property /*17000076*/ instance callconv(8) int32
          CurrentIndex()
  {
    .set instance void System.Xaml.IXamlIndexingReader/*0200004D*/::set_CurrentIndex(int32) /* 06000184 */
    .get instance int32 System.Xaml.IXamlIndexingReader/*0200004D*/::get_CurrentIndex() /* 06000183 */
  } // end of property IXamlIndexingReader::CurrentIndex
} // end of class System.Xaml.IXamlIndexingReader

.class /*0200004E*/ public auto ansi beforefieldinit System.Xaml.XamlBackgroundReader
       extends System.Xaml.XamlReader/*02000043*/
       implements System.Xaml.IXamlLineInfo/*02000036*/
{
  .method /*06000185*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlReader/*02000043*/ wrappedReader) cil managed
  // SIG: 20 01 01 12 81 0C
  {
    // Method begins at RVA 0x2191
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::.ctor

  .method /*06000186*/ public hidebysig instance void 
          StartThread() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2192
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::StartThread

  .method /*06000187*/ public hidebysig instance void 
          StartThread(string threadName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2193
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::StartThread

  .method /*06000188*/ public hidebysig virtual 
          instance bool  Read() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2194
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::Read

  .method /*06000189*/ public hidebysig specialname virtual 
          instance valuetype System.Xaml.XamlNodeType/*0200003E*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 F8
  {
    // Method begins at RVA 0x2195
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_NodeType

  .method /*0600018A*/ public hidebysig specialname virtual 
          instance bool  get_IsEof() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2196
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_IsEof

  .method /*0600018B*/ public hidebysig specialname virtual 
          instance class System.Xaml.NamespaceDeclaration/*02000048*/ 
          get_Namespace() cil managed
  // SIG: 20 00 12 81 20
  {
    // Method begins at RVA 0x2197
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_Namespace

  .method /*0600018C*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x2198
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_Type

  .method /*0600018D*/ public hidebysig specialname virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2199
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_Value

  .method /*0600018E*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_Member() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x219a
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_Member

  .method /*0600018F*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x219b
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_SchemaContext

  .method /*06000190*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HasLineInfo() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x219c
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_HasLineInfo

  .method /*06000191*/ public hidebysig newslot specialname virtual final 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x219d
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_LineNumber

  .method /*06000192*/ public hidebysig newslot specialname virtual final 
          instance int32  get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x219e
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::get_LinePosition

  .method /*06000193*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x219f
    // Code size       0 (0x0)
  } // end of method XamlBackgroundReader::Dispose

  .property /*17000077*/ instance callconv(8) valuetype System.Xaml.XamlNodeType/*0200003E*/
          NodeType()
  {
    .get instance valuetype System.Xaml.XamlNodeType/*0200003E*/ System.Xaml.XamlBackgroundReader/*0200004E*/::get_NodeType() /* 06000189 */
  } // end of property XamlBackgroundReader::NodeType
  .property /*17000078*/ instance callconv(8) bool
          IsEof()
  {
    .get instance bool System.Xaml.XamlBackgroundReader/*0200004E*/::get_IsEof() /* 0600018A */
  } // end of property XamlBackgroundReader::IsEof
  .property /*17000079*/ instance callconv(8) class System.Xaml.NamespaceDeclaration/*02000048*/
          Namespace()
  {
    .get instance class System.Xaml.NamespaceDeclaration/*02000048*/ System.Xaml.XamlBackgroundReader/*0200004E*/::get_Namespace() /* 0600018B */
  } // end of property XamlBackgroundReader::Namespace
  .property /*1700007A*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlBackgroundReader/*0200004E*/::get_Type() /* 0600018C */
  } // end of property XamlBackgroundReader::Type
  .property /*1700007B*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Xaml.XamlBackgroundReader/*0200004E*/::get_Value() /* 0600018D */
  } // end of property XamlBackgroundReader::Value
  .property /*1700007C*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          Member()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.XamlBackgroundReader/*0200004E*/::get_Member() /* 0600018E */
  } // end of property XamlBackgroundReader::Member
  .property /*1700007D*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlBackgroundReader/*0200004E*/::get_SchemaContext() /* 0600018F */
  } // end of property XamlBackgroundReader::SchemaContext
  .property /*1700007E*/ instance callconv(8) bool
          HasLineInfo()
  {
    .get instance bool System.Xaml.XamlBackgroundReader/*0200004E*/::get_HasLineInfo() /* 06000190 */
  } // end of property XamlBackgroundReader::HasLineInfo
  .property /*1700007F*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 System.Xaml.XamlBackgroundReader/*0200004E*/::get_LineNumber() /* 06000191 */
  } // end of property XamlBackgroundReader::LineNumber
  .property /*17000080*/ instance callconv(8) int32
          LinePosition()
  {
    .get instance int32 System.Xaml.XamlBackgroundReader/*0200004E*/::get_LinePosition() /* 06000192 */
  } // end of property XamlBackgroundReader::LinePosition
} // end of class System.Xaml.XamlBackgroundReader

.class /*0200004F*/ public auto ansi beforefieldinit System.Xaml.XamlNodeList
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000194*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 01 01 12 81 88
  {
    // Method begins at RVA 0x21a0
    // Code size       0 (0x0)
  } // end of method XamlNodeList::.ctor

  .method /*06000195*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               int32 size) cil managed
  // SIG: 20 02 01 12 81 88 08
  {
    // Method begins at RVA 0x21a1
    // Code size       0 (0x0)
  } // end of method XamlNodeList::.ctor

  .method /*06000196*/ public hidebysig specialname 
          instance class System.Xaml.XamlWriter/*0200003C*/ 
          get_Writer() cil managed
  // SIG: 20 00 12 80 F0
  {
    // Method begins at RVA 0x21a2
    // Code size       0 (0x0)
  } // end of method XamlNodeList::get_Writer

  .method /*06000197*/ public hidebysig instance class System.Xaml.XamlReader/*02000043*/ 
          GetReader() cil managed
  // SIG: 20 00 12 81 0C
  {
    // Method begins at RVA 0x21a3
    // Code size       0 (0x0)
  } // end of method XamlNodeList::GetReader

  .method /*06000198*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a4
    // Code size       0 (0x0)
  } // end of method XamlNodeList::Clear

  .method /*06000199*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21a5
    // Code size       0 (0x0)
  } // end of method XamlNodeList::get_Count

  .property /*17000081*/ instance callconv(8) class System.Xaml.XamlWriter/*0200003C*/
          Writer()
  {
    .get instance class System.Xaml.XamlWriter/*0200003C*/ System.Xaml.XamlNodeList/*0200004F*/::get_Writer() /* 06000196 */
  } // end of property XamlNodeList::Writer
  .property /*17000082*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Xaml.XamlNodeList/*0200004F*/::get_Count() /* 06000199 */
  } // end of property XamlNodeList::Count
} // end of class System.Xaml.XamlNodeList

.class /*02000050*/ public auto ansi beforefieldinit System.Xaml.XamlNodeQueue
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600019A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 01 01 12 81 88
  {
    // Method begins at RVA 0x21a6
    // Code size       0 (0x0)
  } // end of method XamlNodeQueue::.ctor

  .method /*0600019B*/ public hidebysig specialname 
          instance class System.Xaml.XamlReader/*02000043*/ 
          get_Reader() cil managed
  // SIG: 20 00 12 81 0C
  {
    // Method begins at RVA 0x21a7
    // Code size       0 (0x0)
  } // end of method XamlNodeQueue::get_Reader

  .method /*0600019C*/ public hidebysig specialname 
          instance class System.Xaml.XamlWriter/*0200003C*/ 
          get_Writer() cil managed
  // SIG: 20 00 12 80 F0
  {
    // Method begins at RVA 0x21a8
    // Code size       0 (0x0)
  } // end of method XamlNodeQueue::get_Writer

  .method /*0600019D*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a9
    // Code size       0 (0x0)
  } // end of method XamlNodeQueue::get_IsEmpty

  .method /*0600019E*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21aa
    // Code size       0 (0x0)
  } // end of method XamlNodeQueue::get_Count

  .property /*17000083*/ instance callconv(8) class System.Xaml.XamlReader/*02000043*/
          Reader()
  {
    .get instance class System.Xaml.XamlReader/*02000043*/ System.Xaml.XamlNodeQueue/*02000050*/::get_Reader() /* 0600019B */
  } // end of property XamlNodeQueue::Reader
  .property /*17000084*/ instance callconv(8) class System.Xaml.XamlWriter/*0200003C*/
          Writer()
  {
    .get instance class System.Xaml.XamlWriter/*0200003C*/ System.Xaml.XamlNodeQueue/*02000050*/::get_Writer() /* 0600019C */
  } // end of property XamlNodeQueue::Writer
  .property /*17000085*/ instance callconv(8) bool
          IsEmpty()
  {
    .get instance bool System.Xaml.XamlNodeQueue/*02000050*/::get_IsEmpty() /* 0600019D */
  } // end of property XamlNodeQueue::IsEmpty
  .property /*17000086*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Xaml.XamlNodeQueue/*02000050*/::get_Count() /* 0600019E */
  } // end of property XamlNodeQueue::Count
} // end of class System.Xaml.XamlNodeQueue

.class /*02000051*/ public auto ansi beforefieldinit System.Windows.Markup.Reference
       extends System.Windows.Markup.MarkupExtension/*02000023*/
{
  .custom /*0C00006B:06000022*/ instance void System.Windows.Markup.ContentPropertyAttribute/*02000009*/::.ctor(string) /* 06000022 */ = ( 01 00 04 4E 61 6D 65 00 00 )                      // ...Name..
  .method /*0600019F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ab
    // Code size       0 (0x0)
  } // end of method Reference::.ctor

  .method /*060001A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21ac
    // Code size       0 (0x0)
  } // end of method Reference::.ctor

  .method /*060001A1*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C7:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       0 (0x0)
  } // end of method Reference::get_Name

  .method /*060001A2*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C8:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ae
    // Code size       0 (0x0)
  } // end of method Reference::set_Name

  .method /*060001A3*/ public hidebysig virtual 
          instance object  ProvideValue(class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 01 1C 12 39
  {
    // Method begins at RVA 0x21af
    // Code size       0 (0x0)
  } // end of method Reference::ProvideValue

  .property /*17000087*/ instance callconv(8) string
          Name()
  {
    .custom /*0C000080:0600001F*/ instance void System.Windows.Markup.ConstructorArgumentAttribute/*02000008*/::.ctor(string) /* 0600001F */ = ( 01 00 04 6E 61 6D 65 00 00 )                      // ...name..
    .set instance void System.Windows.Markup.Reference/*02000051*/::set_Name(string) /* 060001A2 */
    .get instance string System.Windows.Markup.Reference/*02000051*/::get_Name() /* 060001A1 */
  } // end of property Reference::Name
} // end of class System.Windows.Markup.Reference

.class /*02000052*/ public auto ansi beforefieldinit System.Xaml.Schema.XamlValueConverter`1<class TConverterBase>
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class [mscorlib/*23000001*/]System.IEquatable`1/*01000015*/<class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!TConverterBase>>/*1B000005*/
{
  .method /*060001A4*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C9:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::get_Name

  .method /*060001A5*/ private hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000CA:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b1
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::set_Name

  .method /*060001A6*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ConverterType() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C0000CB:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b2
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::get_ConverterType

  .method /*060001A7*/ private hidebysig specialname 
          instance void  set_ConverterType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    .custom /*0C0000CC:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::set_ConverterType

  .method /*060001A8*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_TargetType() cil managed
  // SIG: 20 00 12 81 94
  {
    .custom /*0C0000CD:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b4
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::get_TargetType

  .method /*060001A9*/ private hidebysig specialname 
          instance void  set_TargetType(class System.Xaml.XamlType/*02000065*/ 'value') cil managed
  // SIG: 20 01 01 12 81 94
  {
    .custom /*0C0000CE:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::set_TargetType

  .method /*060001AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ converterType,
                               class System.Xaml.XamlType/*02000065*/ targetType) cil managed
  // SIG: 20 02 01 12 0D 12 81 94
  {
    // Method begins at RVA 0x21b6
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::.ctor

  .method /*060001AB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ converterType,
                               class System.Xaml.XamlType/*02000065*/ targetType,
                               string name) cil managed
  // SIG: 20 03 01 12 0D 12 81 94 0E
  {
    // Method begins at RVA 0x21b7
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::.ctor

  .method /*060001AC*/ public hidebysig specialname 
          instance !TConverterBase  get_ConverterInstance() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x21b8
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::get_ConverterInstance

  .method /*060001AD*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21b9
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::ToString

  .method /*060001AE*/ family hidebysig newslot virtual 
          instance !TConverterBase  CreateInstance() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x21ba
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::CreateInstance

  .method /*060001AF*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x21bb
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::Equals

  .method /*060001B0*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21bc
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::GetHashCode

  .method /*060001B1*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!TConverterBase> other) cil managed
  // SIG: 20 01 02 15 12 81 48 01 13 00
  {
    // Method begins at RVA 0x21bd
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::Equals

  .method /*060001B2*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!TConverterBase> converter1,
                            class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!TConverterBase> converter2) cil managed
  // SIG: 00 02 02 15 12 81 48 01 13 00 15 12 81 48 01 13 00
  {
    // Method begins at RVA 0x21be
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::op_Equality

  .method /*060001B3*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!TConverterBase> converter1,
                              class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!TConverterBase> converter2) cil managed
  // SIG: 00 02 02 15 12 81 48 01 13 00 15 12 81 48 01 13 00
  {
    // Method begins at RVA 0x21bf
    // Code size       0 (0x0)
  } // end of method XamlValueConverter`1::op_Inequality

  .property /*17000088*/ instance callconv(8) string
          Name()
  {
    .set instance void System.Xaml.Schema.XamlValueConverter`1/*02000052*/::set_Name(string) /* 060001A5 */
    .get instance string System.Xaml.Schema.XamlValueConverter`1/*02000052*/::get_Name() /* 060001A4 */
  } // end of property XamlValueConverter`1::Name
  .property /*17000089*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ConverterType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Xaml.Schema.XamlValueConverter`1/*02000052*/::get_ConverterType() /* 060001A6 */
    .set instance void System.Xaml.Schema.XamlValueConverter`1/*02000052*/::set_ConverterType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060001A7 */
  } // end of property XamlValueConverter`1::ConverterType
  .property /*1700008A*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          TargetType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.Schema.XamlValueConverter`1/*02000052*/::get_TargetType() /* 060001A8 */
    .set instance void System.Xaml.Schema.XamlValueConverter`1/*02000052*/::set_TargetType(class System.Xaml.XamlType/*02000065*/) /* 060001A9 */
  } // end of property XamlValueConverter`1::TargetType
  .property /*1700008B*/ instance callconv(8) !TConverterBase
          ConverterInstance()
  {
    .get instance !TConverterBase System.Xaml.Schema.XamlValueConverter`1/*02000052*/::get_ConverterInstance() /* 060001AC */
  } // end of property XamlValueConverter`1::ConverterInstance
} // end of class System.Xaml.Schema.XamlValueConverter`1

.class /*02000053*/ public auto ansi sealed System.Xaml.Schema.XamlCollectionKind
       extends [mscorlib/*23000001*/]System.Enum/*01000014*/
{
  .field /*0400000F*/ public specialname rtspecialname uint8 value__
  .field /*04000010*/ public static literal valuetype System.Xaml.Schema.XamlCollectionKind/*02000053*/ None = uint8(0x00)
  .field /*04000011*/ public static literal valuetype System.Xaml.Schema.XamlCollectionKind/*02000053*/ Collection = uint8(0x01)
  .field /*04000012*/ public static literal valuetype System.Xaml.Schema.XamlCollectionKind/*02000053*/ Dictionary = uint8(0x02)
  .field /*04000013*/ public static literal valuetype System.Xaml.Schema.XamlCollectionKind/*02000053*/ Array = uint8(0x03)
} // end of class System.Xaml.Schema.XamlCollectionKind

.class /*02000054*/ public auto ansi beforefieldinit System.Xaml.XamlMember
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class [mscorlib/*23000001*/]System.IEquatable`1/*01000015*/<class System.Xaml.XamlMember/*02000054*/>/*1B000006*/
{
  .method /*060001B4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class System.Xaml.XamlType/*02000065*/ declaringType,
                               bool isAttachable) cil managed
  // SIG: 20 03 01 0E 12 81 94 02
  {
    // Method begins at RVA 0x21c0
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001B5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000025*/ propertyInfo,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 80 95 12 81 88
  {
    // Method begins at RVA 0x21c1
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001B6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000025*/ propertyInfo,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ invoker) cil managed
  // SIG: 20 03 01 12 80 95 12 81 88 12 81 58
  {
    // Method begins at RVA 0x21c2
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001B7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Reflection.EventInfo/*01000026*/ eventInfo,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 80 99 12 81 88
  {
    // Method begins at RVA 0x21c3
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Reflection.EventInfo/*01000026*/ eventInfo,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ invoker) cil managed
  // SIG: 20 03 01 12 80 99 12 81 88 12 81 58
  {
    // Method begins at RVA 0x21c4
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001B9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string attachablePropertyName,
                               class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ getter,
                               class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ setter,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 04 01 0E 12 80 9D 12 80 9D 12 81 88
  {
    // Method begins at RVA 0x21c5
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001BA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string attachablePropertyName,
                               class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ getter,
                               class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ setter,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ invoker) cil managed
  // SIG: 20 05 01 0E 12 80 9D 12 80 9D 12 81 88 12 81 58
  {
    // Method begins at RVA 0x21c8
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001BB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string attachableEventName,
                               class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ adder,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 03 01 0E 12 80 9D 12 81 88
  {
    // Method begins at RVA 0x21d4
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001BC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string attachableEventName,
                               class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ adder,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ invoker) cil managed
  // SIG: 20 04 01 0E 12 80 9D 12 81 88 12 81 58
  {
    // Method begins at RVA 0x21d5
    // Code size       0 (0x0)
  } // end of method XamlMember::.ctor

  .method /*060001BD*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x21d6
    // Code size       0 (0x0)
  } // end of method XamlMember::get_DeclaringType

  .method /*060001BE*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ 
          get_Invoker() cil managed
  // SIG: 20 00 12 81 58
  {
    // Method begins at RVA 0x21d7
    // Code size       0 (0x0)
  } // end of method XamlMember::get_Invoker

  .method /*060001BF*/ public hidebysig specialname 
          instance bool  get_IsUnknown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21d8
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsUnknown

  .method /*060001C0*/ public hidebysig specialname 
          instance bool  get_IsReadPublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21d9
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsReadPublic

  .method /*060001C1*/ public hidebysig specialname 
          instance bool  get_IsWritePublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21da
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsWritePublic

  .method /*060001C2*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21db
    // Code size       0 (0x0)
  } // end of method XamlMember::get_Name

  .method /*060001C3*/ public hidebysig specialname 
          instance bool  get_IsNameValid() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21dc
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsNameValid

  .method /*060001C4*/ public hidebysig specialname 
          instance string  get_PreferredXamlNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21dd
    // Code size       0 (0x0)
  } // end of method XamlMember::get_PreferredXamlNamespace

  .method /*060001C5*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_TargetType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x21de
    // Code size       0 (0x0)
  } // end of method XamlMember::get_TargetType

  .method /*060001C6*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x21df
    // Code size       0 (0x0)
  } // end of method XamlMember::get_Type

  .method /*060001C7*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> 
          get_TypeConverter() cil managed
  // SIG: 20 00 15 12 81 48 01 12 41
  {
    // Method begins at RVA 0x21e0
    // Code size       0 (0x0)
  } // end of method XamlMember::get_TypeConverter

  .method /*060001C8*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/> 
          get_ValueSerializer() cil managed
  // SIG: 20 00 15 12 81 48 01 12 08
  {
    // Method begins at RVA 0x21e1
    // Code size       0 (0x0)
  } // end of method XamlMember::get_ValueSerializer

  .method /*060001C9*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> 
          get_DeferringLoader() cil managed
  // SIG: 20 00 15 12 81 48 01 12 81 68
  {
    // Method begins at RVA 0x21e2
    // Code size       0 (0x0)
  } // end of method XamlMember::get_DeferringLoader

  .method /*060001CA*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*01000028*/ 
          get_UnderlyingMember() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C0000D0:0A000024*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*01000057*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21e3
    // Code size       0 (0x0)
  } // end of method XamlMember::get_UnderlyingMember

  .method /*060001CB*/ public hidebysig specialname 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e4
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsReadOnly

  .method /*060001CC*/ public hidebysig specialname 
          instance bool  get_IsWriteOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e5
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsWriteOnly

  .method /*060001CD*/ public hidebysig specialname 
          instance bool  get_IsAttachable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e6
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsAttachable

  .method /*060001CE*/ public hidebysig specialname 
          instance bool  get_IsEvent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e7
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsEvent

  .method /*060001CF*/ public hidebysig specialname 
          instance bool  get_IsDirective() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e8
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsDirective

  .method /*060001D0*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> 
          GetXamlNamespaces() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x21e9
    // Code size       0 (0x0)
  } // end of method XamlMember::GetXamlNamespaces

  .method /*060001D1*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21ea
    // Code size       0 (0x0)
  } // end of method XamlMember::ToString

  .method /*060001D2*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlMember/*02000054*/> 
          get_DependsOn() cil managed
  // SIG: 20 00 15 12 19 01 12 81 50
  {
    // Method begins at RVA 0x21eb
    // Code size       0 (0x0)
  } // end of method XamlMember::get_DependsOn

  .method /*060001D3*/ public hidebysig specialname 
          instance bool  get_IsAmbient() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ec
    // Code size       0 (0x0)
  } // end of method XamlMember::get_IsAmbient

  .method /*060001D4*/ public hidebysig specialname 
          instance valuetype [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibility/*01000029*/ 
          get_SerializationVisibility() cil managed
  // SIG: 20 00 11 80 A5
  {
    // Method begins at RVA 0x21ed
    // Code size       0 (0x0)
  } // end of method XamlMember::get_SerializationVisibility

  .method /*060001D5*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ 
          LookupInvoker() cil managed
  // SIG: 20 00 12 81 58
  {
    // Method begins at RVA 0x21ee
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupInvoker

  .method /*060001D6*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.ICustomAttributeProvider/*0100002A*/ 
          LookupCustomAttributeProvider() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x21ef
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupCustomAttributeProvider

  .method /*060001D7*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> 
          LookupDeferringLoader() cil managed
  // SIG: 20 00 15 12 81 48 01 12 81 68
  {
    // Method begins at RVA 0x21f0
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupDeferringLoader

  .method /*060001D8*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlMember/*02000054*/> 
          LookupDependsOn() cil managed
  // SIG: 20 00 15 12 19 01 12 81 50
  {
    // Method begins at RVA 0x21f1
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupDependsOn

  .method /*060001D9*/ family hidebysig newslot virtual 
          instance bool  LookupIsAmbient() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f2
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsAmbient

  .method /*060001DA*/ family hidebysig newslot virtual 
          instance bool  LookupIsEvent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f3
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsEvent

  .method /*060001DB*/ family hidebysig newslot virtual 
          instance bool  LookupIsReadPublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f4
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsReadPublic

  .method /*060001DC*/ family hidebysig newslot virtual 
          instance bool  LookupIsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f5
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsReadOnly

  .method /*060001DD*/ family hidebysig newslot virtual 
          instance bool  LookupIsUnknown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f6
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsUnknown

  .method /*060001DE*/ family hidebysig newslot virtual 
          instance bool  LookupIsWriteOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f7
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsWriteOnly

  .method /*060001DF*/ family hidebysig newslot virtual 
          instance bool  LookupIsWritePublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f8
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupIsWritePublic

  .method /*060001E0*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupTargetType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x21f9
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupTargetType

  .method /*060001E1*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> 
          LookupTypeConverter() cil managed
  // SIG: 20 00 15 12 81 48 01 12 41
  {
    // Method begins at RVA 0x21fa
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupTypeConverter

  .method /*060001E2*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/> 
          LookupValueSerializer() cil managed
  // SIG: 20 00 15 12 81 48 01 12 08
  {
    // Method begins at RVA 0x21fb
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupValueSerializer

  .method /*060001E3*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x21fc
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupType

  .method /*060001E4*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          LookupUnderlyingGetter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x21fd
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupUnderlyingGetter

  .method /*060001E5*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          LookupUnderlyingSetter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x21fe
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupUnderlyingSetter

  .method /*060001E6*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*01000028*/ 
          LookupUnderlyingMember() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x21ff
    // Code size       0 (0x0)
  } // end of method XamlMember::LookupUnderlyingMember

  .method /*060001E7*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2200
    // Code size       0 (0x0)
  } // end of method XamlMember::Equals

  .method /*060001E8*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2201
    // Code size       0 (0x0)
  } // end of method XamlMember::GetHashCode

  .method /*060001E9*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Xaml.XamlMember/*02000054*/ other) cil managed
  // SIG: 20 01 02 12 81 50
  {
    // Method begins at RVA 0x2202
    // Code size       0 (0x0)
  } // end of method XamlMember::Equals

  .method /*060001EA*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xaml.XamlMember/*02000054*/ xamlMember1,
                            class System.Xaml.XamlMember/*02000054*/ xamlMember2) cil managed
  // SIG: 00 02 02 12 81 50 12 81 50
  {
    // Method begins at RVA 0x2203
    // Code size       0 (0x0)
  } // end of method XamlMember::op_Equality

  .method /*060001EB*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xaml.XamlMember/*02000054*/ xamlMember1,
                              class System.Xaml.XamlMember/*02000054*/ xamlMember2) cil managed
  // SIG: 00 02 02 12 81 50 12 81 50
  {
    // Method begins at RVA 0x2204
    // Code size       0 (0x0)
  } // end of method XamlMember::op_Inequality

  .property /*1700008C*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          DeclaringType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlMember/*02000054*/::get_DeclaringType() /* 060001BD */
  } // end of property XamlMember::DeclaringType
  .property /*1700008D*/ instance callconv(8) class System.Xaml.Schema.XamlMemberInvoker/*02000056*/
          Invoker()
  {
    .get instance class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ System.Xaml.XamlMember/*02000054*/::get_Invoker() /* 060001BE */
  } // end of property XamlMember::Invoker
  .property /*1700008E*/ instance callconv(8) bool
          IsUnknown()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsUnknown() /* 060001BF */
  } // end of property XamlMember::IsUnknown
  .property /*1700008F*/ instance callconv(8) bool
          IsReadPublic()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsReadPublic() /* 060001C0 */
  } // end of property XamlMember::IsReadPublic
  .property /*17000090*/ instance callconv(8) bool
          IsWritePublic()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsWritePublic() /* 060001C1 */
  } // end of property XamlMember::IsWritePublic
  .property /*17000091*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Xaml.XamlMember/*02000054*/::get_Name() /* 060001C2 */
  } // end of property XamlMember::Name
  .property /*17000092*/ instance callconv(8) bool
          IsNameValid()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsNameValid() /* 060001C3 */
  } // end of property XamlMember::IsNameValid
  .property /*17000093*/ instance callconv(8) string
          PreferredXamlNamespace()
  {
    .get instance string System.Xaml.XamlMember/*02000054*/::get_PreferredXamlNamespace() /* 060001C4 */
  } // end of property XamlMember::PreferredXamlNamespace
  .property /*17000094*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          TargetType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlMember/*02000054*/::get_TargetType() /* 060001C5 */
  } // end of property XamlMember::TargetType
  .property /*17000095*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlMember/*02000054*/::get_Type() /* 060001C6 */
  } // end of property XamlMember::Type
  .property /*17000096*/ instance callconv(8) class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/>
          TypeConverter()
  {
    .get instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> System.Xaml.XamlMember/*02000054*/::get_TypeConverter() /* 060001C7 */
  } // end of property XamlMember::TypeConverter
  .property /*17000097*/ instance callconv(8) class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/>
          ValueSerializer()
  {
    .get instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/> System.Xaml.XamlMember/*02000054*/::get_ValueSerializer() /* 060001C8 */
  } // end of property XamlMember::ValueSerializer
  .property /*17000098*/ instance callconv(8) class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/>
          DeferringLoader()
  {
    .get instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> System.Xaml.XamlMember/*02000054*/::get_DeferringLoader() /* 060001C9 */
  } // end of property XamlMember::DeferringLoader
  .property /*17000099*/ instance callconv(8) class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*01000028*/
          UnderlyingMember()
  {
    .get instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*01000028*/ System.Xaml.XamlMember/*02000054*/::get_UnderlyingMember() /* 060001CA */
  } // end of property XamlMember::UnderlyingMember
  .property /*1700009A*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsReadOnly() /* 060001CB */
  } // end of property XamlMember::IsReadOnly
  .property /*1700009B*/ instance callconv(8) bool
          IsWriteOnly()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsWriteOnly() /* 060001CC */
  } // end of property XamlMember::IsWriteOnly
  .property /*1700009C*/ instance callconv(8) bool
          IsAttachable()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsAttachable() /* 060001CD */
  } // end of property XamlMember::IsAttachable
  .property /*1700009D*/ instance callconv(8) bool
          IsEvent()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsEvent() /* 060001CE */
  } // end of property XamlMember::IsEvent
  .property /*1700009E*/ instance callconv(8) bool
          IsDirective()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsDirective() /* 060001CF */
  } // end of property XamlMember::IsDirective
  .property /*1700009F*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlMember/*02000054*/>
          DependsOn()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlMember/*02000054*/> System.Xaml.XamlMember/*02000054*/::get_DependsOn() /* 060001D2 */
  } // end of property XamlMember::DependsOn
  .property /*170000A0*/ instance callconv(8) bool
          IsAmbient()
  {
    .get instance bool System.Xaml.XamlMember/*02000054*/::get_IsAmbient() /* 060001D3 */
  } // end of property XamlMember::IsAmbient
  .property /*170000A1*/ instance callconv(8) valuetype [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibility/*01000029*/
          SerializationVisibility()
  {
    .get instance valuetype [System/*23000002*/]System.ComponentModel.DesignerSerializationVisibility/*01000029*/ System.Xaml.XamlMember/*02000054*/::get_SerializationVisibility() /* 060001D4 */
  } // end of property XamlMember::SerializationVisibility
} // end of class System.Xaml.XamlMember

.class /*02000055*/ public auto ansi beforefieldinit System.Xaml.XamlDirective
       extends System.Xaml.XamlMember/*02000054*/
{
  .method /*060001EC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<string> xamlNamespaces,
                               string name,
                               class System.Xaml.XamlType/*02000065*/ xamlType,
                               class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> typeConverter,
                               valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ allowedLocation) cil managed
  // SIG: 20 05 01 15 12 09 01 0E 0E 12 81 94 15 12 81 48 01 12 41 11 80 B4
  {
    // Method begins at RVA 0x2205
    // Code size       0 (0x0)
  } // end of method XamlDirective::.ctor

  .method /*060001ED*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xamlNamespace,
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2206
    // Code size       0 (0x0)
  } // end of method XamlDirective::.ctor

  .method /*060001EE*/ public hidebysig specialname 
          instance valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ 
          get_AllowedLocation() cil managed
  // SIG: 20 00 11 80 B4
  {
    // Method begins at RVA 0x2207
    // Code size       0 (0x0)
  } // end of method XamlDirective::get_AllowedLocation

  .method /*060001EF*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2208
    // Code size       0 (0x0)
  } // end of method XamlDirective::GetHashCode

  .method /*060001F0*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2209
    // Code size       0 (0x0)
  } // end of method XamlDirective::ToString

  .method /*060001F1*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> 
          GetXamlNamespaces() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x220a
    // Code size       0 (0x0)
  } // end of method XamlDirective::GetXamlNamespaces

  .method /*060001F2*/ family hidebysig virtual final 
          instance class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ 
          LookupInvoker() cil managed
  // SIG: 20 00 12 81 58
  {
    // Method begins at RVA 0x220b
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupInvoker

  .method /*060001F3*/ family hidebysig virtual final 
          instance class [mscorlib/*23000001*/]System.Reflection.ICustomAttributeProvider/*0100002A*/ 
          LookupCustomAttributeProvider() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x220c
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupCustomAttributeProvider

  .method /*060001F4*/ family hidebysig virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlMember/*02000054*/> 
          LookupDependsOn() cil managed
  // SIG: 20 00 15 12 19 01 12 81 50
  {
    // Method begins at RVA 0x220d
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupDependsOn

  .method /*060001F5*/ family hidebysig virtual final 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> 
          LookupDeferringLoader() cil managed
  // SIG: 20 00 15 12 81 48 01 12 81 68
  {
    // Method begins at RVA 0x220e
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupDeferringLoader

  .method /*060001F6*/ family hidebysig virtual final 
          instance bool  LookupIsAmbient() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x220f
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsAmbient

  .method /*060001F7*/ family hidebysig virtual final 
          instance bool  LookupIsEvent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2210
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsEvent

  .method /*060001F8*/ family hidebysig virtual final 
          instance bool  LookupIsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2211
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsReadOnly

  .method /*060001F9*/ family hidebysig virtual final 
          instance bool  LookupIsReadPublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2212
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsReadPublic

  .method /*060001FA*/ family hidebysig virtual final 
          instance bool  LookupIsUnknown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2213
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsUnknown

  .method /*060001FB*/ family hidebysig virtual final 
          instance bool  LookupIsWriteOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2214
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsWriteOnly

  .method /*060001FC*/ family hidebysig virtual final 
          instance bool  LookupIsWritePublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2215
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupIsWritePublic

  .method /*060001FD*/ family hidebysig virtual final 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupTargetType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x2216
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupTargetType

  .method /*060001FE*/ family hidebysig virtual final 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> 
          LookupTypeConverter() cil managed
  // SIG: 20 00 15 12 81 48 01 12 41
  {
    // Method begins at RVA 0x2217
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupTypeConverter

  .method /*060001FF*/ family hidebysig virtual final 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x2218
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupType

  .method /*06000200*/ family hidebysig virtual final 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          LookupUnderlyingGetter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x2219
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupUnderlyingGetter

  .method /*06000201*/ family hidebysig virtual final 
          instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*01000028*/ 
          LookupUnderlyingMember() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x221a
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupUnderlyingMember

  .method /*06000202*/ family hidebysig virtual final 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          LookupUnderlyingSetter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x221b
    // Code size       0 (0x0)
  } // end of method XamlDirective::LookupUnderlyingSetter

  .property /*170000A2*/ instance callconv(8) valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/
          AllowedLocation()
  {
    .get instance valuetype System.Xaml.Schema.AllowedMemberLocations/*0200002D*/ System.Xaml.XamlDirective/*02000055*/::get_AllowedLocation() /* 060001EE */
  } // end of property XamlDirective::AllowedLocation
} // end of class System.Xaml.XamlDirective

.class /*02000056*/ public auto ansi beforefieldinit System.Xaml.Schema.XamlMemberInvoker
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000203*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x221c
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::.ctor

  .method /*06000204*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlMember/*02000054*/ member) cil managed
  // SIG: 20 01 01 12 81 50
  {
    // Method begins at RVA 0x221d
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::.ctor

  .method /*06000205*/ public hidebysig specialname static 
          class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ 
          get_UnknownInvoker() cil managed
  // SIG: 00 00 12 81 58
  {
    // Method begins at RVA 0x221e
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::get_UnknownInvoker

  .method /*06000206*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          get_UnderlyingGetter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x221f
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::get_UnderlyingGetter

  .method /*06000207*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          get_UnderlyingSetter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x2220
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::get_UnderlyingSetter

  .method /*06000208*/ public hidebysig newslot virtual 
          instance object  GetValue(object 'instance') cil managed
  // SIG: 20 01 1C 1C
  {
    // Method begins at RVA 0x2221
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::GetValue

  .method /*06000209*/ public hidebysig newslot virtual 
          instance void  SetValue(object 'instance',
                                  object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    // Method begins at RVA 0x2222
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::SetValue

  .method /*0600020A*/ public hidebysig newslot virtual 
          instance valuetype System.Xaml.Schema.ShouldSerializeResult/*02000057*/ 
          ShouldSerializeValue(object 'instance') cil managed
  // SIG: 20 01 11 81 5C 1C
  {
    // Method begins at RVA 0x2223
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::ShouldSerializeValue

  .method /*0600020B*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2224
    // Code size       0 (0x0)
  } // end of method XamlMemberInvoker::.cctor

  .property /*170000A3*/ callconv(8) class System.Xaml.Schema.XamlMemberInvoker/*02000056*/
          UnknownInvoker()
  {
    .get class System.Xaml.Schema.XamlMemberInvoker/*02000056*/ System.Xaml.Schema.XamlMemberInvoker/*02000056*/::get_UnknownInvoker() /* 06000205 */
  } // end of property XamlMemberInvoker::UnknownInvoker
  .property /*170000A4*/ instance callconv(8) class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/
          UnderlyingGetter()
  {
    .get instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ System.Xaml.Schema.XamlMemberInvoker/*02000056*/::get_UnderlyingGetter() /* 06000206 */
  } // end of property XamlMemberInvoker::UnderlyingGetter
  .property /*170000A5*/ instance callconv(8) class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/
          UnderlyingSetter()
  {
    .get instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ System.Xaml.Schema.XamlMemberInvoker/*02000056*/::get_UnderlyingSetter() /* 06000207 */
  } // end of property XamlMemberInvoker::UnderlyingSetter
} // end of class System.Xaml.Schema.XamlMemberInvoker

.class /*02000057*/ public auto ansi sealed System.Xaml.Schema.ShouldSerializeResult
       extends [mscorlib/*23000001*/]System.Enum/*01000014*/
{
  .field /*04000014*/ public specialname rtspecialname int32 value__
  .field /*04000015*/ public static literal valuetype System.Xaml.Schema.ShouldSerializeResult/*02000057*/ Default = int32(0x00000000)
  .field /*04000016*/ public static literal valuetype System.Xaml.Schema.ShouldSerializeResult/*02000057*/ True = int32(0x00000001)
  .field /*04000017*/ public static literal valuetype System.Xaml.Schema.ShouldSerializeResult/*02000057*/ False = int32(0x00000002)
} // end of class System.Xaml.Schema.ShouldSerializeResult

.class /*02000058*/ public auto ansi beforefieldinit System.Xaml.Schema.XamlTypeInvoker
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600020C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2225
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::.ctor

  .method /*0600020D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlType/*02000065*/ 'type') cil managed
  // SIG: 20 01 01 12 81 94
  {
    // Method begins at RVA 0x2226
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::.ctor

  .method /*0600020E*/ public hidebysig specialname static 
          class System.Xaml.Schema.XamlTypeInvoker/*02000058*/ 
          get_UnknownInvoker() cil managed
  // SIG: 00 00 12 81 60
  {
    // Method begins at RVA 0x2227
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::get_UnknownInvoker

  .method /*0600020F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/> 
          get_SetMarkupExtensionHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 80
  {
    // Method begins at RVA 0x2228
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::get_SetMarkupExtensionHandler

  .method /*06000210*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/> 
          get_SetTypeConverterHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 88
  {
    // Method begins at RVA 0x2229
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::get_SetTypeConverterHandler

  .method /*06000211*/ public hidebysig newslot virtual 
          instance void  AddToCollection(object 'instance',
                                         object item) cil managed
  // SIG: 20 02 01 1C 1C
  {
    // Method begins at RVA 0x222a
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::AddToCollection

  .method /*06000212*/ public hidebysig newslot virtual 
          instance void  AddToDictionary(object 'instance',
                                         object key,
                                         object item) cil managed
  // SIG: 20 03 01 1C 1C 1C
  {
    // Method begins at RVA 0x222b
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::AddToDictionary

  .method /*06000213*/ public hidebysig newslot virtual 
          instance object  CreateInstance(object[] arguments) cil managed
  // SIG: 20 01 1C 1D 1C
  {
    // Method begins at RVA 0x222c
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::CreateInstance

  .method /*06000214*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          GetAddMethod(class System.Xaml.XamlType/*02000065*/ contentType) cil managed
  // SIG: 20 01 12 80 9D 12 81 94
  {
    // Method begins at RVA 0x222d
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::GetAddMethod

  .method /*06000215*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000027*/ 
          GetEnumeratorMethod() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x222e
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::GetEnumeratorMethod

  .method /*06000216*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100002B*/ 
          GetItems(object 'instance') cil managed
  // SIG: 20 01 12 80 AD 1C
  {
    // Method begins at RVA 0x222f
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::GetItems

  .method /*06000217*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2230
    // Code size       0 (0x0)
  } // end of method XamlTypeInvoker::.cctor

  .property /*170000A6*/ callconv(8) class System.Xaml.Schema.XamlTypeInvoker/*02000058*/
          UnknownInvoker()
  {
    .get class System.Xaml.Schema.XamlTypeInvoker/*02000058*/ System.Xaml.Schema.XamlTypeInvoker/*02000058*/::get_UnknownInvoker() /* 0600020E */
  } // end of property XamlTypeInvoker::UnknownInvoker
  .property /*170000A7*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/>
          SetMarkupExtensionHandler()
  {
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/> System.Xaml.Schema.XamlTypeInvoker/*02000058*/::get_SetMarkupExtensionHandler() /* 0600020F */
  } // end of property XamlTypeInvoker::SetMarkupExtensionHandler
  .property /*170000A8*/ instance callconv(8) class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/>
          SetTypeConverterHandler()
  {
    .get instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/> System.Xaml.Schema.XamlTypeInvoker/*02000058*/::get_SetTypeConverterHandler() /* 06000210 */
  } // end of property XamlTypeInvoker::SetTypeConverterHandler
} // end of class System.Xaml.Schema.XamlTypeInvoker

.class /*02000059*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XamlDeferLoadAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000007*/
{
  .custom /*0C00006C:0A00001C*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100004E*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100004F*/) /* 0A00001C */ = ( 01 00 84 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 01 )                               // rited.
  .method /*06000218*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ loaderType,
                               class [mscorlib/*23000001*/]System.Type/*01000003*/ contentType) cil managed
  // SIG: 20 02 01 12 0D 12 0D
  {
    // Method begins at RVA 0x2231
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::.ctor

  .method /*06000219*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string loaderType,
                               string contentType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2232
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::.ctor

  .method /*0600021A*/ public hidebysig specialname 
          instance string  get_LoaderTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2233
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::get_LoaderTypeName

  .method /*0600021B*/ public hidebysig specialname 
          instance string  get_ContentTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2234
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::get_ContentTypeName

  .method /*0600021C*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_LoaderType() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C0000D1:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2235
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::get_LoaderType

  .method /*0600021D*/ private hidebysig specialname 
          instance void  set_LoaderType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    .custom /*0C0000D2:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2236
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::set_LoaderType

  .method /*0600021E*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ContentType() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C0000D3:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2237
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::get_ContentType

  .method /*0600021F*/ private hidebysig specialname 
          instance void  set_ContentType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    .custom /*0C0000D4:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2238
    // Code size       0 (0x0)
  } // end of method XamlDeferLoadAttribute::set_ContentType

  .property /*170000A9*/ instance callconv(8) string
          LoaderTypeName()
  {
    .get instance string System.Windows.Markup.XamlDeferLoadAttribute/*02000059*/::get_LoaderTypeName() /* 0600021A */
  } // end of property XamlDeferLoadAttribute::LoaderTypeName
  .property /*170000AA*/ instance callconv(8) string
          ContentTypeName()
  {
    .get instance string System.Windows.Markup.XamlDeferLoadAttribute/*02000059*/::get_ContentTypeName() /* 0600021B */
  } // end of property XamlDeferLoadAttribute::ContentTypeName
  .property /*170000AB*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          LoaderType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.XamlDeferLoadAttribute/*02000059*/::get_LoaderType() /* 0600021C */
    .set instance void System.Windows.Markup.XamlDeferLoadAttribute/*02000059*/::set_LoaderType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600021D */
  } // end of property XamlDeferLoadAttribute::LoaderType
  .property /*170000AC*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ContentType()
  {
    .set instance void System.Windows.Markup.XamlDeferLoadAttribute/*02000059*/::set_ContentType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600021F */
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Windows.Markup.XamlDeferLoadAttribute/*02000059*/::get_ContentType() /* 0600021E */
  } // end of property XamlDeferLoadAttribute::ContentType
} // end of class System.Windows.Markup.XamlDeferLoadAttribute

.class /*0200005A*/ public abstract auto ansi beforefieldinit System.Xaml.XamlDeferringLoader
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000220*/ public hidebysig newslot abstract virtual 
          instance object  Load(class System.Xaml.XamlReader/*02000043*/ xamlReader,
                                class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 02 1C 12 81 0C 12 39
  {
    // Method begins at RVA 0x0
  } // end of method XamlDeferringLoader::Load

  .method /*06000221*/ public hidebysig newslot abstract virtual 
          instance class System.Xaml.XamlReader/*02000043*/ 
          Save(object 'value',
               class [mscorlib/*23000001*/]System.IServiceProvider/*0100000E*/ serviceProvider) cil managed
  // SIG: 20 02 12 81 0C 1C 12 39
  {
    // Method begins at RVA 0x0
  } // end of method XamlDeferringLoader::Save

  .method /*06000222*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2239
    // Code size       0 (0x0)
  } // end of method XamlDeferringLoader::.ctor

} // end of class System.Xaml.XamlDeferringLoader

.class /*0200005B*/ public auto ansi serializable beforefieldinit System.Xaml.XamlObjectWriterException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*06000223*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x223a
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterException::.ctor

  .method /*06000224*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x223b
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterException::.ctor

  .method /*06000225*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x223c
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterException::.ctor

  .method /*06000226*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x223d
    // Code size       0 (0x0)
  } // end of method XamlObjectWriterException::.ctor

} // end of class System.Xaml.XamlObjectWriterException

.class /*0200005C*/ public auto ansi serializable beforefieldinit System.Xaml.XamlDuplicateMemberException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*06000227*/ public hidebysig specialname 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_DuplicateMember() cil managed
  // SIG: 20 00 12 81 50
  {
    .custom /*0C0000D5:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x223e
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::get_DuplicateMember

  .method /*06000228*/ public hidebysig specialname 
          instance void  set_DuplicateMember(class System.Xaml.XamlMember/*02000054*/ 'value') cil managed
  // SIG: 20 01 01 12 81 50
  {
    .custom /*0C0000D6:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::set_DuplicateMember

  .method /*06000229*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_ParentType() cil managed
  // SIG: 20 00 12 81 94
  {
    .custom /*0C0000D7:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2240
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::get_ParentType

  .method /*0600022A*/ public hidebysig specialname 
          instance void  set_ParentType(class System.Xaml.XamlType/*02000065*/ 'value') cil managed
  // SIG: 20 01 01 12 81 94
  {
    .custom /*0C0000D8:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2241
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::set_ParentType

  .method /*0600022B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2242
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::.ctor

  .method /*0600022C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlMember/*02000054*/ member,
                               class System.Xaml.XamlType/*02000065*/ 'type') cil managed
  // SIG: 20 02 01 12 81 50 12 81 94
  {
    // Method begins at RVA 0x2243
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::.ctor

  .method /*0600022D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2244
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::.ctor

  .method /*0600022E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x2245
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::.ctor

  .method /*0600022F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x2246
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::.ctor

  .method /*06000230*/ public hidebysig virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    .custom /*0C0000D9:0A00001F*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*01000052*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2247
    // Code size       0 (0x0)
  } // end of method XamlDuplicateMemberException::GetObjectData

  .property /*170000AD*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          DuplicateMember()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.XamlDuplicateMemberException/*0200005C*/::get_DuplicateMember() /* 06000227 */
    .set instance void System.Xaml.XamlDuplicateMemberException/*0200005C*/::set_DuplicateMember(class System.Xaml.XamlMember/*02000054*/) /* 06000228 */
  } // end of property XamlDuplicateMemberException::DuplicateMember
  .property /*170000AE*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          ParentType()
  {
    .set instance void System.Xaml.XamlDuplicateMemberException/*0200005C*/::set_ParentType(class System.Xaml.XamlType/*02000065*/) /* 0600022A */
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlDuplicateMemberException/*0200005C*/::get_ParentType() /* 06000229 */
  } // end of property XamlDuplicateMemberException::ParentType
} // end of class System.Xaml.XamlDuplicateMemberException

.class /*0200005D*/ public auto ansi serializable beforefieldinit System.Xaml.XamlInternalException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*06000231*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2248
    // Code size       0 (0x0)
  } // end of method XamlInternalException::.ctor

  .method /*06000232*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2249
    // Code size       0 (0x0)
  } // end of method XamlInternalException::.ctor

  .method /*06000233*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x224a
    // Code size       0 (0x0)
  } // end of method XamlInternalException::.ctor

  .method /*06000234*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x224b
    // Code size       0 (0x0)
  } // end of method XamlInternalException::.ctor

} // end of class System.Xaml.XamlInternalException

.class /*0200005E*/ public auto ansi serializable beforefieldinit System.Xaml.XamlSchemaException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*06000235*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x224c
    // Code size       0 (0x0)
  } // end of method XamlSchemaException::.ctor

  .method /*06000236*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x224d
    // Code size       0 (0x0)
  } // end of method XamlSchemaException::.ctor

  .method /*06000237*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x224e
    // Code size       0 (0x0)
  } // end of method XamlSchemaException::.ctor

  .method /*06000238*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x224f
    // Code size       0 (0x0)
  } // end of method XamlSchemaException::.ctor

} // end of class System.Xaml.XamlSchemaException

.class /*0200005F*/ public auto ansi serializable beforefieldinit System.Xaml.XamlObjectReaderException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*06000239*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2250
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderException::.ctor

  .method /*0600023A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2251
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderException::.ctor

  .method /*0600023B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x2252
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderException::.ctor

  .method /*0600023C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x2253
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderException::.ctor

} // end of class System.Xaml.XamlObjectReaderException

.class /*02000060*/ public auto ansi serializable beforefieldinit System.Xaml.XamlXmlWriterException
       extends System.Xaml.XamlException/*02000049*/
{
  .method /*0600023D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2254
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterException::.ctor

  .method /*0600023E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2255
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterException::.ctor

  .method /*0600023F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000005*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 15
  {
    // Method begins at RVA 0x2256
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterException::.ctor

  .method /*06000240*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x2257
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterException::.ctor

} // end of class System.Xaml.XamlXmlWriterException

.class /*02000061*/ public abstract auto ansi sealed beforefieldinit System.Xaml.XamlLanguage
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000018*/ public static literal string Xaml2006Namespace = "http://schemas.microsoft.com/winfx/2006/xaml"
  .field /*04000019*/ public static literal string Xml1998Namespace = "http://www.w3.org/XML/1998/namespace"
  .method /*06000241*/ public hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> 
          get_XamlNamespaces() cil managed
  // SIG: 00 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x2258
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_XamlNamespaces

  .method /*06000242*/ public hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> 
          get_XmlNamespaces() cil managed
  // SIG: 00 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x2259
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_XmlNamespaces

  .method /*06000243*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Array() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x225a
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Array

  .method /*06000244*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Member() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x225b
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Member

  .method /*06000245*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Null() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x225c
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Null

  .method /*06000246*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Property() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x225d
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Property

  .method /*06000247*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Reference() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x225e
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Reference

  .method /*06000248*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Static() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x225f
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Static

  .method /*06000249*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2260
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Type

  .method /*0600024A*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_String() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2261
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_String

  .method /*0600024B*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Double() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2262
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Double

  .method /*0600024C*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Int32() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2263
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Int32

  .method /*0600024D*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Boolean() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2264
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Boolean

  .method /*0600024E*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_XData() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2265
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_XData

  .method /*0600024F*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Object() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2266
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Object

  .method /*06000250*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Char() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2267
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Char

  .method /*06000251*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Single() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2268
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Single

  .method /*06000252*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Byte() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x2269
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Byte

  .method /*06000253*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Int16() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x226a
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Int16

  .method /*06000254*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Int64() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x226b
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Int64

  .method /*06000255*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Decimal() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x226c
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Decimal

  .method /*06000256*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_Uri() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x226d
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Uri

  .method /*06000257*/ public hidebysig specialname static 
          class System.Xaml.XamlType/*02000065*/ 
          get_TimeSpan() cil managed
  // SIG: 00 00 12 81 94
  {
    // Method begins at RVA 0x226e
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_TimeSpan

  .method /*06000258*/ public hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002C*/<class System.Xaml.XamlType/*02000065*/> 
          get_AllTypes() cil managed
  // SIG: 00 00 15 12 80 B1 01 12 81 94
  {
    // Method begins at RVA 0x226f
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_AllTypes

  .method /*06000259*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Arguments() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2270
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Arguments

  .method /*0600025A*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_AsyncRecords() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2271
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_AsyncRecords

  .method /*0600025B*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Class() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2272
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Class

  .method /*0600025C*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_ClassModifier() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2273
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_ClassModifier

  .method /*0600025D*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Code() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2274
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Code

  .method /*0600025E*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_ConnectionId() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2275
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_ConnectionId

  .method /*0600025F*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_FactoryMethod() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2276
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_FactoryMethod

  .method /*06000260*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_FieldModifier() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2277
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_FieldModifier

  .method /*06000261*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Items() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2278
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Items

  .method /*06000262*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Initialization() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2279
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Initialization

  .method /*06000263*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Key() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x227a
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Key

  .method /*06000264*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Members() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x227b
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Members

  .method /*06000265*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_ClassAttributes() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x227c
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_ClassAttributes

  .method /*06000266*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Name() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x227d
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Name

  .method /*06000267*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_PositionalParameters() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x227e
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_PositionalParameters

  .method /*06000268*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Shared() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x227f
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Shared

  .method /*06000269*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Subclass() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2280
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Subclass

  .method /*0600026A*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_SynchronousMode() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2281
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_SynchronousMode

  .method /*0600026B*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_TypeArguments() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2282
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_TypeArguments

  .method /*0600026C*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Uid() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2283
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Uid

  .method /*0600026D*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_UnknownContent() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2284
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_UnknownContent

  .method /*0600026E*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Base() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2285
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Base

  .method /*0600026F*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Lang() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2286
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Lang

  .method /*06000270*/ public hidebysig specialname static 
          class System.Xaml.XamlDirective/*02000055*/ 
          get_Space() cil managed
  // SIG: 00 00 12 81 54
  {
    // Method begins at RVA 0x2287
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_Space

  .method /*06000271*/ public hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002C*/<class System.Xaml.XamlDirective/*02000055*/> 
          get_AllDirectives() cil managed
  // SIG: 00 00 15 12 80 B1 01 12 81 54
  {
    // Method begins at RVA 0x2288
    // Code size       0 (0x0)
  } // end of method XamlLanguage::get_AllDirectives

  .method /*06000272*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2289
    // Code size       0 (0x0)
  } // end of method XamlLanguage::.cctor

  .property /*170000AF*/ callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string>
          XamlNamespaces()
  {
    .get class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> System.Xaml.XamlLanguage/*02000061*/::get_XamlNamespaces() /* 06000241 */
  } // end of property XamlLanguage::XamlNamespaces
  .property /*170000B0*/ callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string>
          XmlNamespaces()
  {
    .get class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> System.Xaml.XamlLanguage/*02000061*/::get_XmlNamespaces() /* 06000242 */
  } // end of property XamlLanguage::XmlNamespaces
  .property /*170000B1*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Array()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Array() /* 06000243 */
  } // end of property XamlLanguage::Array
  .property /*170000B2*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Member()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Member() /* 06000244 */
  } // end of property XamlLanguage::Member
  .property /*170000B3*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Null()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Null() /* 06000245 */
  } // end of property XamlLanguage::Null
  .property /*170000B4*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Property()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Property() /* 06000246 */
  } // end of property XamlLanguage::Property
  .property /*170000B5*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Reference()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Reference() /* 06000247 */
  } // end of property XamlLanguage::Reference
  .property /*170000B6*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Static()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Static() /* 06000248 */
  } // end of property XamlLanguage::Static
  .property /*170000B7*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Type() /* 06000249 */
  } // end of property XamlLanguage::Type
  .property /*170000B8*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          String()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_String() /* 0600024A */
  } // end of property XamlLanguage::String
  .property /*170000B9*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Double()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Double() /* 0600024B */
  } // end of property XamlLanguage::Double
  .property /*170000BA*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Int32()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Int32() /* 0600024C */
  } // end of property XamlLanguage::Int32
  .property /*170000BB*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Boolean()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Boolean() /* 0600024D */
  } // end of property XamlLanguage::Boolean
  .property /*170000BC*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          XData()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_XData() /* 0600024E */
  } // end of property XamlLanguage::XData
  .property /*170000BD*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Object()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Object() /* 0600024F */
  } // end of property XamlLanguage::Object
  .property /*170000BE*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Char()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Char() /* 06000250 */
  } // end of property XamlLanguage::Char
  .property /*170000BF*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Single()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Single() /* 06000251 */
  } // end of property XamlLanguage::Single
  .property /*170000C0*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Byte()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Byte() /* 06000252 */
  } // end of property XamlLanguage::Byte
  .property /*170000C1*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Int16()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Int16() /* 06000253 */
  } // end of property XamlLanguage::Int16
  .property /*170000C2*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Int64()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Int64() /* 06000254 */
  } // end of property XamlLanguage::Int64
  .property /*170000C3*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Decimal()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Decimal() /* 06000255 */
  } // end of property XamlLanguage::Decimal
  .property /*170000C4*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          Uri()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_Uri() /* 06000256 */
  } // end of property XamlLanguage::Uri
  .property /*170000C5*/ callconv(8) class System.Xaml.XamlType/*02000065*/
          TimeSpan()
  {
    .get class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlLanguage/*02000061*/::get_TimeSpan() /* 06000257 */
  } // end of property XamlLanguage::TimeSpan
  .property /*170000C6*/ callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002C*/<class System.Xaml.XamlType/*02000065*/>
          AllTypes()
  {
    .get class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002C*/<class System.Xaml.XamlType/*02000065*/> System.Xaml.XamlLanguage/*02000061*/::get_AllTypes() /* 06000258 */
  } // end of property XamlLanguage::AllTypes
  .property /*170000C7*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Arguments()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Arguments() /* 06000259 */
  } // end of property XamlLanguage::Arguments
  .property /*170000C8*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          AsyncRecords()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_AsyncRecords() /* 0600025A */
  } // end of property XamlLanguage::AsyncRecords
  .property /*170000C9*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Class()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Class() /* 0600025B */
  } // end of property XamlLanguage::Class
  .property /*170000CA*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          ClassModifier()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_ClassModifier() /* 0600025C */
  } // end of property XamlLanguage::ClassModifier
  .property /*170000CB*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Code()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Code() /* 0600025D */
  } // end of property XamlLanguage::Code
  .property /*170000CC*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          ConnectionId()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_ConnectionId() /* 0600025E */
  } // end of property XamlLanguage::ConnectionId
  .property /*170000CD*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          FactoryMethod()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_FactoryMethod() /* 0600025F */
  } // end of property XamlLanguage::FactoryMethod
  .property /*170000CE*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          FieldModifier()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_FieldModifier() /* 06000260 */
  } // end of property XamlLanguage::FieldModifier
  .property /*170000CF*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Items()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Items() /* 06000261 */
  } // end of property XamlLanguage::Items
  .property /*170000D0*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Initialization()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Initialization() /* 06000262 */
  } // end of property XamlLanguage::Initialization
  .property /*170000D1*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Key()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Key() /* 06000263 */
  } // end of property XamlLanguage::Key
  .property /*170000D2*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Members()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Members() /* 06000264 */
  } // end of property XamlLanguage::Members
  .property /*170000D3*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          ClassAttributes()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_ClassAttributes() /* 06000265 */
  } // end of property XamlLanguage::ClassAttributes
  .property /*170000D4*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Name()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Name() /* 06000266 */
  } // end of property XamlLanguage::Name
  .property /*170000D5*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          PositionalParameters()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_PositionalParameters() /* 06000267 */
  } // end of property XamlLanguage::PositionalParameters
  .property /*170000D6*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Shared()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Shared() /* 06000268 */
  } // end of property XamlLanguage::Shared
  .property /*170000D7*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Subclass()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Subclass() /* 06000269 */
  } // end of property XamlLanguage::Subclass
  .property /*170000D8*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          SynchronousMode()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_SynchronousMode() /* 0600026A */
  } // end of property XamlLanguage::SynchronousMode
  .property /*170000D9*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          TypeArguments()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_TypeArguments() /* 0600026B */
  } // end of property XamlLanguage::TypeArguments
  .property /*170000DA*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Uid()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Uid() /* 0600026C */
  } // end of property XamlLanguage::Uid
  .property /*170000DB*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          UnknownContent()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_UnknownContent() /* 0600026D */
  } // end of property XamlLanguage::UnknownContent
  .property /*170000DC*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Base()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Base() /* 0600026E */
  } // end of property XamlLanguage::Base
  .property /*170000DD*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Lang()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Lang() /* 0600026F */
  } // end of property XamlLanguage::Lang
  .property /*170000DE*/ callconv(8) class System.Xaml.XamlDirective/*02000055*/
          Space()
  {
    .get class System.Xaml.XamlDirective/*02000055*/ System.Xaml.XamlLanguage/*02000061*/::get_Space() /* 06000270 */
  } // end of property XamlLanguage::Space
  .property /*170000DF*/ callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002C*/<class System.Xaml.XamlDirective/*02000055*/>
          AllDirectives()
  {
    .get class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100002C*/<class System.Xaml.XamlDirective/*02000055*/> System.Xaml.XamlLanguage/*02000061*/::get_AllDirectives() /* 06000271 */
  } // end of property XamlLanguage::AllDirectives
} // end of class System.Xaml.XamlLanguage

.class /*02000062*/ public auto ansi beforefieldinit System.Xaml.XamlSchemaContext
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000273*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228a
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::.ctor

  .method /*06000274*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContextSettings/*02000063*/ settings) cil managed
  // SIG: 20 01 01 12 81 8C
  {
    // Method begins at RVA 0x228b
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::.ctor

  .method /*06000275*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/> referenceAssemblies) cil managed
  // SIG: 20 01 01 15 12 09 01 12 61
  {
    // Method begins at RVA 0x228c
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::.ctor

  .method /*06000276*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/> referenceAssemblies,
                               class System.Xaml.XamlSchemaContextSettings/*02000063*/ settings) cil managed
  // SIG: 20 02 01 15 12 09 01 12 61 12 81 8C
  {
    // Method begins at RVA 0x228d
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::.ctor

  .method /*06000277*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228e
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::Finalize

  .method /*06000278*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.ICollection`1/*01000009*/<class System.Xaml.XamlType/*02000065*/> 
          GetAllXamlTypes(string xamlNamespace) cil managed
  // SIG: 20 01 15 12 25 01 12 81 94 0E
  {
    // Method begins at RVA 0x228f
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetAllXamlTypes

  .method /*06000279*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<string> 
          GetAllXamlNamespaces() cil managed
  // SIG: 20 00 15 12 09 01 0E
  {
    // Method begins at RVA 0x2290
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetAllXamlNamespaces

  .method /*0600027A*/ public hidebysig newslot virtual 
          instance string  GetPreferredPrefix(string xmlns) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2291
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetPreferredPrefix

  .method /*0600027B*/ public hidebysig newslot virtual 
          instance class System.Xaml.XamlDirective/*02000055*/ 
          GetXamlDirective(string xamlNamespace,
                           string name) cil managed
  // SIG: 20 02 12 81 54 0E 0E
  {
    // Method begins at RVA 0x2292
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetXamlDirective

  .method /*0600027C*/ public hidebysig instance class System.Xaml.XamlType/*02000065*/ 
          GetXamlType(class System.Xaml.Schema.XamlTypeName/*02000066*/ xamlTypeName) cil managed
  // SIG: 20 01 12 81 94 12 81 98
  {
    // Method begins at RVA 0x2293
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetXamlType

  .method /*0600027D*/ famorassem hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          GetXamlType(string xamlNamespace,
                      string name,
                      class System.Xaml.XamlType/*02000065*/[] typeArguments) cil managed
  // SIG: 20 03 12 81 94 0E 0E 1D 12 81 94
  {
    .param [3]/*080001BD*/ 
    .custom /*0C0000CF:0A000022*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000055*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2294
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetXamlType

  .method /*0600027E*/ public hidebysig newslot virtual 
          instance bool  TryGetCompatibleXamlNamespace(string xamlNamespace,
                                                       [out] string& compatibleNamespace) cil managed
  // SIG: 20 02 02 0E 10 0E
  {
    // Method begins at RVA 0x2295
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::TryGetCompatibleXamlNamespace

  .method /*0600027F*/ public hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          GetXamlType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 20 01 12 81 94 12 0D
  {
    // Method begins at RVA 0x2296
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetXamlType

  .method /*06000280*/ famorassem hidebysig 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<!!TConverterBase> 
          GetValueConverter<class TConverterBase>(class [mscorlib/*23000001*/]System.Type/*01000003*/ converterType,
                                                  class System.Xaml.XamlType/*02000065*/ targetType) cil managed
  // SIG: 30 01 02 15 12 81 48 01 1E 00 12 0D 12 81 94
  {
    // Method begins at RVA 0x2297
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::GetValueConverter

  .method /*06000281*/ public hidebysig specialname 
          instance bool  get_SupportMarkupExtensionsWithDuplicateArity() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2298
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::get_SupportMarkupExtensionsWithDuplicateArity

  .method /*06000282*/ public hidebysig specialname 
          instance bool  get_FullyQualifyAssemblyNamesInClrNamespaces() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2299
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::get_FullyQualifyAssemblyNamesInClrNamespaces

  .method /*06000283*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/> 
          get_ReferenceAssemblies() cil managed
  // SIG: 20 00 15 12 19 01 12 61
  {
    // Method begins at RVA 0x229a
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::get_ReferenceAssemblies

  .method /*06000284*/ famorassem hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/ 
          OnAssemblyResolve(string assemblyName) cil managed
  // SIG: 20 01 12 61 0E
  {
    // Method begins at RVA 0x229b
    // Code size       0 (0x0)
  } // end of method XamlSchemaContext::OnAssemblyResolve

  .property /*170000E0*/ instance callconv(8) bool
          SupportMarkupExtensionsWithDuplicateArity()
  {
    .get instance bool System.Xaml.XamlSchemaContext/*02000062*/::get_SupportMarkupExtensionsWithDuplicateArity() /* 06000281 */
  } // end of property XamlSchemaContext::SupportMarkupExtensionsWithDuplicateArity
  .property /*170000E1*/ instance callconv(8) bool
          FullyQualifyAssemblyNamesInClrNamespaces()
  {
    .get instance bool System.Xaml.XamlSchemaContext/*02000062*/::get_FullyQualifyAssemblyNamesInClrNamespaces() /* 06000282 */
  } // end of property XamlSchemaContext::FullyQualifyAssemblyNamesInClrNamespaces
  .property /*170000E2*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/>
          ReferenceAssemblies()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000018*/> System.Xaml.XamlSchemaContext/*02000062*/::get_ReferenceAssemblies() /* 06000283 */
  } // end of property XamlSchemaContext::ReferenceAssemblies
} // end of class System.Xaml.XamlSchemaContext

.class /*02000063*/ public auto ansi beforefieldinit System.Xaml.XamlSchemaContextSettings
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000285*/ public hidebysig specialname 
          instance bool  get_SupportMarkupExtensionsWithDuplicateArity() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000DA:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229c
    // Code size       0 (0x0)
  } // end of method XamlSchemaContextSettings::get_SupportMarkupExtensionsWithDuplicateArity

  .method /*06000286*/ public hidebysig specialname 
          instance void  set_SupportMarkupExtensionsWithDuplicateArity(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000DB:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229d
    // Code size       0 (0x0)
  } // end of method XamlSchemaContextSettings::set_SupportMarkupExtensionsWithDuplicateArity

  .method /*06000287*/ public hidebysig specialname 
          instance bool  get_FullyQualifyAssemblyNamesInClrNamespaces() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000DC:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229e
    // Code size       0 (0x0)
  } // end of method XamlSchemaContextSettings::get_FullyQualifyAssemblyNamesInClrNamespaces

  .method /*06000288*/ public hidebysig specialname 
          instance void  set_FullyQualifyAssemblyNamesInClrNamespaces(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000DD:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229f
    // Code size       0 (0x0)
  } // end of method XamlSchemaContextSettings::set_FullyQualifyAssemblyNamesInClrNamespaces

  .method /*06000289*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22a0
    // Code size       0 (0x0)
  } // end of method XamlSchemaContextSettings::.ctor

  .method /*0600028A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlSchemaContextSettings/*02000063*/ settings) cil managed
  // SIG: 20 01 01 12 81 8C
  {
    // Method begins at RVA 0x22a1
    // Code size       0 (0x0)
  } // end of method XamlSchemaContextSettings::.ctor

  .property /*170000E3*/ instance callconv(8) bool
          SupportMarkupExtensionsWithDuplicateArity()
  {
    .get instance bool System.Xaml.XamlSchemaContextSettings/*02000063*/::get_SupportMarkupExtensionsWithDuplicateArity() /* 06000285 */
    .set instance void System.Xaml.XamlSchemaContextSettings/*02000063*/::set_SupportMarkupExtensionsWithDuplicateArity(bool) /* 06000286 */
  } // end of property XamlSchemaContextSettings::SupportMarkupExtensionsWithDuplicateArity
  .property /*170000E4*/ instance callconv(8) bool
          FullyQualifyAssemblyNamesInClrNamespaces()
  {
    .get instance bool System.Xaml.XamlSchemaContextSettings/*02000063*/::get_FullyQualifyAssemblyNamesInClrNamespaces() /* 06000287 */
    .set instance void System.Xaml.XamlSchemaContextSettings/*02000063*/::set_FullyQualifyAssemblyNamesInClrNamespaces(bool) /* 06000288 */
  } // end of property XamlSchemaContextSettings::FullyQualifyAssemblyNamesInClrNamespaces
} // end of class System.Xaml.XamlSchemaContextSettings

.class /*02000064*/ public abstract auto ansi sealed beforefieldinit System.Xaml.XamlServices
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600028B*/ public hidebysig static 
          object  Parse(string xaml) cil managed
  // SIG: 00 01 1C 0E
  {
    // Method begins at RVA 0x22a2
    // Code size       0 (0x0)
  } // end of method XamlServices::Parse

  .method /*0600028C*/ public hidebysig static 
          object  Load(string fileName) cil managed
  // SIG: 00 01 1C 0E
  {
    // Method begins at RVA 0x22a3
    // Code size       0 (0x0)
  } // end of method XamlServices::Load

  .method /*0600028D*/ public hidebysig static 
          object  Load(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream') cil managed
  // SIG: 00 01 1C 12 6D
  {
    // Method begins at RVA 0x22a4
    // Code size       0 (0x0)
  } // end of method XamlServices::Load

  .method /*0600028E*/ public hidebysig static 
          object  Load(class [mscorlib/*23000001*/]System.IO.TextReader/*0100001C*/ textReader) cil managed
  // SIG: 00 01 1C 12 71
  {
    // Method begins at RVA 0x22a5
    // Code size       0 (0x0)
  } // end of method XamlServices::Load

  .method /*0600028F*/ public hidebysig static 
          object  Load(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100001A*/ xmlReader) cil managed
  // SIG: 00 01 1C 12 69
  {
    // Method begins at RVA 0x22a6
    // Code size       0 (0x0)
  } // end of method XamlServices::Load

  .method /*06000290*/ public hidebysig static 
          object  Load(class System.Xaml.XamlReader/*02000043*/ xamlReader) cil managed
  // SIG: 00 01 1C 12 81 0C
  {
    // Method begins at RVA 0x22a7
    // Code size       0 (0x0)
  } // end of method XamlServices::Load

  .method /*06000291*/ public hidebysig static 
          void  Transform(class System.Xaml.XamlReader/*02000043*/ xamlReader,
                          class System.Xaml.XamlWriter/*0200003C*/ xamlWriter) cil managed
  // SIG: 00 02 01 12 81 0C 12 80 F0
  {
    // Method begins at RVA 0x22a8
    // Code size       0 (0x0)
  } // end of method XamlServices::Transform

  .method /*06000292*/ public hidebysig static 
          void  Transform(class System.Xaml.XamlReader/*02000043*/ xamlReader,
                          class System.Xaml.XamlWriter/*0200003C*/ xamlWriter,
                          bool closeWriter) cil managed
  // SIG: 00 03 01 12 81 0C 12 80 F0 02
  {
    // Method begins at RVA 0x22a9
    // Code size       0 (0x0)
  } // end of method XamlServices::Transform

  .method /*06000293*/ public hidebysig static 
          string  Save(object 'instance') cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22aa
    // Code size       0 (0x0)
  } // end of method XamlServices::Save

  .method /*06000294*/ public hidebysig static 
          void  Save(string fileName,
                     object 'instance') cil managed
  // SIG: 00 02 01 0E 1C
  {
    // Method begins at RVA 0x22ab
    // Code size       0 (0x0)
  } // end of method XamlServices::Save

  .method /*06000295*/ public hidebysig static 
          void  Save(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                     object 'instance') cil managed
  // SIG: 00 02 01 12 6D 1C
  {
    // Method begins at RVA 0x22ac
    // Code size       0 (0x0)
  } // end of method XamlServices::Save

  .method /*06000296*/ public hidebysig static 
          void  Save(class [mscorlib/*23000001*/]System.IO.TextWriter/*0100002D*/ writer,
                     object 'instance') cil managed
  // SIG: 00 02 01 12 80 B5 1C
  {
    // Method begins at RVA 0x22ad
    // Code size       0 (0x0)
  } // end of method XamlServices::Save

  .method /*06000297*/ public hidebysig static 
          void  Save(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100002E*/ writer,
                     object 'instance') cil managed
  // SIG: 00 02 01 12 80 B9 1C
  {
    // Method begins at RVA 0x22ae
    // Code size       0 (0x0)
  } // end of method XamlServices::Save

  .method /*06000298*/ public hidebysig static 
          void  Save(class System.Xaml.XamlWriter/*0200003C*/ writer,
                     object 'instance') cil managed
  // SIG: 00 02 01 12 80 F0 1C
  {
    // Method begins at RVA 0x22af
    // Code size       0 (0x0)
  } // end of method XamlServices::Save

} // end of class System.Xaml.XamlServices

.class /*02000065*/ public auto ansi beforefieldinit System.Xaml.XamlType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class [mscorlib/*23000001*/]System.IEquatable`1/*01000015*/<class System.Xaml.XamlType/*02000065*/>/*1B000007*/
{
  .method /*06000299*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> typeArguments,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 03 01 0E 15 12 19 01 12 81 94 12 81 88
  {
    // Method begins at RVA 0x22b0
    // Code size       0 (0x0)
  } // end of method XamlType::.ctor

  .method /*0600029A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string unknownTypeNamespace,
                               string unknownTypeName,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> typeArguments,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 04 01 0E 0E 15 12 19 01 12 81 94 12 81 88
  {
    // Method begins at RVA 0x22b1
    // Code size       0 (0x0)
  } // end of method XamlType::.ctor

  .method /*0600029B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ underlyingType,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 0D 12 81 88
  {
    // Method begins at RVA 0x22b2
    // Code size       0 (0x0)
  } // end of method XamlType::.ctor

  .method /*0600029C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/ underlyingType,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.Schema.XamlTypeInvoker/*02000058*/ invoker) cil managed
  // SIG: 20 03 01 12 0D 12 81 88 12 81 60
  {
    // Method begins at RVA 0x22b3
    // Code size       0 (0x0)
  } // end of method XamlType::.ctor

  .method /*0600029D*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22b4
    // Code size       0 (0x0)
  } // end of method XamlType::get_BaseType

  .method /*0600029E*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlTypeInvoker/*02000058*/ 
          get_Invoker() cil managed
  // SIG: 20 00 12 81 60
  {
    // Method begins at RVA 0x22b5
    // Code size       0 (0x0)
  } // end of method XamlType::get_Invoker

  .method /*0600029F*/ public hidebysig specialname 
          instance bool  get_IsNameValid() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b6
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsNameValid

  .method /*060002A0*/ public hidebysig specialname 
          instance bool  get_IsUnknown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b7
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsUnknown

  .method /*060002A1*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22b8
    // Code size       0 (0x0)
  } // end of method XamlType::get_Name

  .method /*060002A2*/ public hidebysig specialname 
          instance string  get_PreferredXamlNamespace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22b9
    // Code size       0 (0x0)
  } // end of method XamlType::get_PreferredXamlNamespace

  .method /*060002A3*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          get_TypeArguments() cil managed
  // SIG: 20 00 15 12 19 01 12 81 94
  {
    // Method begins at RVA 0x22ba
    // Code size       0 (0x0)
  } // end of method XamlType::get_TypeArguments

  .method /*060002A4*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_UnderlyingType() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C0000DE:0A000024*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*01000057*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22bb
    // Code size       0 (0x0)
  } // end of method XamlType::get_UnderlyingType

  .method /*060002A5*/ public hidebysig specialname 
          instance bool  get_ConstructionRequiresArguments() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22bc
    // Code size       0 (0x0)
  } // end of method XamlType::get_ConstructionRequiresArguments

  .method /*060002A6*/ public hidebysig specialname 
          instance bool  get_IsArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22bd
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsArray

  .method /*060002A7*/ public hidebysig specialname 
          instance bool  get_IsCollection() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22be
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsCollection

  .method /*060002A8*/ public hidebysig specialname 
          instance bool  get_IsConstructible() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22bf
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsConstructible

  .method /*060002A9*/ public hidebysig specialname 
          instance bool  get_IsDictionary() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c0
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsDictionary

  .method /*060002AA*/ public hidebysig specialname 
          instance bool  get_IsGeneric() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c1
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsGeneric

  .method /*060002AB*/ public hidebysig specialname 
          instance bool  get_IsMarkupExtension() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c2
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsMarkupExtension

  .method /*060002AC*/ public hidebysig specialname 
          instance bool  get_IsNameScope() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c3
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsNameScope

  .method /*060002AD*/ public hidebysig specialname 
          instance bool  get_IsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c4
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsNullable

  .method /*060002AE*/ public hidebysig specialname 
          instance bool  get_IsPublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c5
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsPublic

  .method /*060002AF*/ public hidebysig specialname 
          instance bool  get_IsUsableDuringInitialization() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c6
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsUsableDuringInitialization

  .method /*060002B0*/ public hidebysig specialname 
          instance bool  get_IsWhitespaceSignificantCollection() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c7
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsWhitespaceSignificantCollection

  .method /*060002B1*/ public hidebysig specialname 
          instance bool  get_IsXData() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c8
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsXData

  .method /*060002B2*/ public hidebysig specialname 
          instance bool  get_TrimSurroundingWhitespace() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c9
    // Code size       0 (0x0)
  } // end of method XamlType::get_TrimSurroundingWhitespace

  .method /*060002B3*/ public hidebysig specialname 
          instance bool  get_IsAmbient() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ca
    // Code size       0 (0x0)
  } // end of method XamlType::get_IsAmbient

  .method /*060002B4*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_KeyType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22cb
    // Code size       0 (0x0)
  } // end of method XamlType::get_KeyType

  .method /*060002B5*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_ItemType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22cc
    // Code size       0 (0x0)
  } // end of method XamlType::get_ItemType

  .method /*060002B6*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          get_AllowedContentTypes() cil managed
  // SIG: 20 00 15 12 19 01 12 81 94
  {
    // Method begins at RVA 0x22cd
    // Code size       0 (0x0)
  } // end of method XamlType::get_AllowedContentTypes

  .method /*060002B7*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          get_ContentWrappers() cil managed
  // SIG: 20 00 15 12 19 01 12 81 94
  {
    // Method begins at RVA 0x22ce
    // Code size       0 (0x0)
  } // end of method XamlType::get_ContentWrappers

  .method /*060002B8*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> 
          get_TypeConverter() cil managed
  // SIG: 20 00 15 12 81 48 01 12 41
  {
    // Method begins at RVA 0x22cf
    // Code size       0 (0x0)
  } // end of method XamlType::get_TypeConverter

  .method /*060002B9*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/> 
          get_ValueSerializer() cil managed
  // SIG: 20 00 15 12 81 48 01 12 08
  {
    // Method begins at RVA 0x22d0
    // Code size       0 (0x0)
  } // end of method XamlType::get_ValueSerializer

  .method /*060002BA*/ public hidebysig specialname 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_ContentProperty() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x22d1
    // Code size       0 (0x0)
  } // end of method XamlType::get_ContentProperty

  .method /*060002BB*/ public hidebysig specialname 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> 
          get_DeferringLoader() cil managed
  // SIG: 20 00 15 12 81 48 01 12 81 68
  {
    // Method begins at RVA 0x22d2
    // Code size       0 (0x0)
  } // end of method XamlType::get_DeferringLoader

  .method /*060002BC*/ public hidebysig specialname 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_MarkupExtensionReturnType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22d3
    // Code size       0 (0x0)
  } // end of method XamlType::get_MarkupExtensionReturnType

  .method /*060002BD*/ public hidebysig specialname 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x22d4
    // Code size       0 (0x0)
  } // end of method XamlType::get_SchemaContext

  .method /*060002BE*/ public hidebysig instance class System.Xaml.XamlMember/*02000054*/ 
          GetMember(string name) cil managed
  // SIG: 20 01 12 81 50 0E
  {
    // Method begins at RVA 0x22d5
    // Code size       0 (0x0)
  } // end of method XamlType::GetMember

  .method /*060002BF*/ public hidebysig instance class [mscorlib/*23000001*/]System.Collections.Generic.ICollection`1/*01000009*/<class System.Xaml.XamlMember/*02000054*/> 
          GetAllMembers() cil managed
  // SIG: 20 00 15 12 25 01 12 81 50
  {
    // Method begins at RVA 0x22d6
    // Code size       0 (0x0)
  } // end of method XamlType::GetAllMembers

  .method /*060002C0*/ public hidebysig instance class System.Xaml.XamlMember/*02000054*/ 
          GetAliasedProperty(class System.Xaml.XamlDirective/*02000055*/ directive) cil managed
  // SIG: 20 01 12 81 50 12 81 54
  {
    // Method begins at RVA 0x22d7
    // Code size       0 (0x0)
  } // end of method XamlType::GetAliasedProperty

  .method /*060002C1*/ public hidebysig instance class System.Xaml.XamlMember/*02000054*/ 
          GetAttachableMember(string name) cil managed
  // SIG: 20 01 12 81 50 0E
  {
    // Method begins at RVA 0x22d8
    // Code size       0 (0x0)
  } // end of method XamlType::GetAttachableMember

  .method /*060002C2*/ public hidebysig instance class [mscorlib/*23000001*/]System.Collections.Generic.ICollection`1/*01000009*/<class System.Xaml.XamlMember/*02000054*/> 
          GetAllAttachableMembers() cil managed
  // SIG: 20 00 15 12 25 01 12 81 50
  {
    // Method begins at RVA 0x22d9
    // Code size       0 (0x0)
  } // end of method XamlType::GetAllAttachableMembers

  .method /*060002C3*/ public hidebysig newslot virtual 
          instance bool  CanAssignTo(class System.Xaml.XamlType/*02000065*/ xamlType) cil managed
  // SIG: 20 01 02 12 81 94
  {
    // Method begins at RVA 0x22da
    // Code size       0 (0x0)
  } // end of method XamlType::CanAssignTo

  .method /*060002C4*/ public hidebysig instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          GetPositionalParameters(int32 parameterCount) cil managed
  // SIG: 20 01 15 12 19 01 12 81 94 08
  {
    // Method begins at RVA 0x22db
    // Code size       0 (0x0)
  } // end of method XamlType::GetPositionalParameters

  .method /*060002C5*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<string> 
          GetXamlNamespaces() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x22dc
    // Code size       0 (0x0)
  } // end of method XamlType::GetXamlNamespaces

  .method /*060002C6*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22dd
    // Code size       0 (0x0)
  } // end of method XamlType::ToString

  .method /*060002C7*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          LookupAliasedProperty(class System.Xaml.XamlDirective/*02000055*/ directive) cil managed
  // SIG: 20 01 12 81 50 12 81 54
  {
    // Method begins at RVA 0x22de
    // Code size       0 (0x0)
  } // end of method XamlType::LookupAliasedProperty

  .method /*060002C8*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          LookupAllowedContentTypes() cil managed
  // SIG: 20 00 15 12 19 01 12 81 94
  {
    // Method begins at RVA 0x22df
    // Code size       0 (0x0)
  } // end of method XamlType::LookupAllowedContentTypes

  .method /*060002C9*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupBaseType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22e0
    // Code size       0 (0x0)
  } // end of method XamlType::LookupBaseType

  .method /*060002CA*/ family hidebysig newslot virtual 
          instance valuetype System.Xaml.Schema.XamlCollectionKind/*02000053*/ 
          LookupCollectionKind() cil managed
  // SIG: 20 00 11 81 4C
  {
    // Method begins at RVA 0x22e1
    // Code size       0 (0x0)
  } // end of method XamlType::LookupCollectionKind

  .method /*060002CB*/ family hidebysig newslot virtual 
          instance bool  LookupConstructionRequiresArguments() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22e2
    // Code size       0 (0x0)
  } // end of method XamlType::LookupConstructionRequiresArguments

  .method /*060002CC*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          LookupContentProperty() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x22e3
    // Code size       0 (0x0)
  } // end of method XamlType::LookupContentProperty

  .method /*060002CD*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          LookupContentWrappers() cil managed
  // SIG: 20 00 15 12 19 01 12 81 94
  {
    // Method begins at RVA 0x22e4
    // Code size       0 (0x0)
  } // end of method XamlType::LookupContentWrappers

  .method /*060002CE*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Reflection.ICustomAttributeProvider/*0100002A*/ 
          LookupCustomAttributeProvider() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x22e5
    // Code size       0 (0x0)
  } // end of method XamlType::LookupCustomAttributeProvider

  .method /*060002CF*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> 
          LookupDeferringLoader() cil managed
  // SIG: 20 00 15 12 81 48 01 12 81 68
  {
    // Method begins at RVA 0x22e6
    // Code size       0 (0x0)
  } // end of method XamlType::LookupDeferringLoader

  .method /*060002D0*/ family hidebysig newslot virtual 
          instance bool  LookupIsConstructible() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22e7
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsConstructible

  .method /*060002D1*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlTypeInvoker/*02000058*/ 
          LookupInvoker() cil managed
  // SIG: 20 00 12 81 60
  {
    // Method begins at RVA 0x22e8
    // Code size       0 (0x0)
  } // end of method XamlType::LookupInvoker

  .method /*060002D2*/ family hidebysig newslot virtual 
          instance bool  LookupIsMarkupExtension() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22e9
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsMarkupExtension

  .method /*060002D3*/ family hidebysig newslot virtual 
          instance bool  LookupIsNameScope() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ea
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsNameScope

  .method /*060002D4*/ family hidebysig newslot virtual 
          instance bool  LookupIsNullable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22eb
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsNullable

  .method /*060002D5*/ family hidebysig newslot virtual 
          instance bool  LookupIsUnknown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ec
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsUnknown

  .method /*060002D6*/ family hidebysig newslot virtual 
          instance bool  LookupIsWhitespaceSignificantCollection() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ed
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsWhitespaceSignificantCollection

  .method /*060002D7*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupKeyType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22ee
    // Code size       0 (0x0)
  } // end of method XamlType::LookupKeyType

  .method /*060002D8*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupItemType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22ef
    // Code size       0 (0x0)
  } // end of method XamlType::LookupItemType

  .method /*060002D9*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          LookupMarkupExtensionReturnType() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x22f0
    // Code size       0 (0x0)
  } // end of method XamlType::LookupMarkupExtensionReturnType

  .method /*060002DA*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.XamlMember/*02000054*/> 
          LookupAllAttachableMembers() cil managed
  // SIG: 20 00 15 12 09 01 12 81 50
  {
    // Method begins at RVA 0x22f1
    // Code size       0 (0x0)
  } // end of method XamlType::LookupAllAttachableMembers

  .method /*060002DB*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.XamlMember/*02000054*/> 
          LookupAllMembers() cil managed
  // SIG: 20 00 15 12 09 01 12 81 50
  {
    // Method begins at RVA 0x22f2
    // Code size       0 (0x0)
  } // end of method XamlType::LookupAllMembers

  .method /*060002DC*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          LookupMember(string name,
                       bool skipReadOnlyCheck) cil managed
  // SIG: 20 02 12 81 50 0E 02
  {
    // Method begins at RVA 0x22f3
    // Code size       0 (0x0)
  } // end of method XamlType::LookupMember

  .method /*060002DD*/ family hidebysig newslot virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          LookupAttachableMember(string name) cil managed
  // SIG: 20 01 12 81 50 0E
  {
    // Method begins at RVA 0x22f4
    // Code size       0 (0x0)
  } // end of method XamlType::LookupAttachableMember

  .method /*060002DE*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> 
          LookupPositionalParameters(int32 parameterCount) cil managed
  // SIG: 20 01 15 12 19 01 12 81 94 08
  {
    // Method begins at RVA 0x22f5
    // Code size       0 (0x0)
  } // end of method XamlType::LookupPositionalParameters

  .method /*060002DF*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          LookupUnderlyingType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x22f6
    // Code size       0 (0x0)
  } // end of method XamlType::LookupUnderlyingType

  .method /*060002E0*/ family hidebysig newslot virtual 
          instance bool  LookupIsPublic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22f7
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsPublic

  .method /*060002E1*/ family hidebysig newslot virtual 
          instance bool  LookupIsXData() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22f8
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsXData

  .method /*060002E2*/ family hidebysig newslot virtual 
          instance bool  LookupIsAmbient() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22f9
    // Code size       0 (0x0)
  } // end of method XamlType::LookupIsAmbient

  .method /*060002E3*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> 
          LookupTypeConverter() cil managed
  // SIG: 20 00 15 12 81 48 01 12 41
  {
    // Method begins at RVA 0x22fa
    // Code size       0 (0x0)
  } // end of method XamlType::LookupTypeConverter

  .method /*060002E4*/ family hidebysig newslot virtual 
          instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/> 
          LookupValueSerializer() cil managed
  // SIG: 20 00 15 12 81 48 01 12 08
  {
    // Method begins at RVA 0x22fb
    // Code size       0 (0x0)
  } // end of method XamlType::LookupValueSerializer

  .method /*060002E5*/ family hidebysig newslot virtual 
          instance bool  LookupTrimSurroundingWhitespace() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22fc
    // Code size       0 (0x0)
  } // end of method XamlType::LookupTrimSurroundingWhitespace

  .method /*060002E6*/ family hidebysig newslot virtual 
          instance bool  LookupUsableDuringInitialization() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22fd
    // Code size       0 (0x0)
  } // end of method XamlType::LookupUsableDuringInitialization

  .method /*060002E7*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetMarkupExtensionEventArgs/*02000020*/> 
          LookupSetMarkupExtensionHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 80
  {
    // Method begins at RVA 0x22fe
    // Code size       0 (0x0)
  } // end of method XamlType::LookupSetMarkupExtensionHandler

  .method /*060002E8*/ family hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.EventHandler`1/*01000019*/<class System.Windows.Markup.XamlSetTypeConverterEventArgs/*02000022*/> 
          LookupSetTypeConverterHandler() cil managed
  // SIG: 20 00 15 12 65 01 12 80 88
  {
    // Method begins at RVA 0x22ff
    // Code size       0 (0x0)
  } // end of method XamlType::LookupSetTypeConverterHandler

  .method /*060002E9*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2300
    // Code size       0 (0x0)
  } // end of method XamlType::Equals

  .method /*060002EA*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2301
    // Code size       0 (0x0)
  } // end of method XamlType::GetHashCode

  .method /*060002EB*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Xaml.XamlType/*02000065*/ other) cil managed
  // SIG: 20 01 02 12 81 94
  {
    // Method begins at RVA 0x2302
    // Code size       0 (0x0)
  } // end of method XamlType::Equals

  .method /*060002EC*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xaml.XamlType/*02000065*/ xamlType1,
                            class System.Xaml.XamlType/*02000065*/ xamlType2) cil managed
  // SIG: 00 02 02 12 81 94 12 81 94
  {
    // Method begins at RVA 0x2303
    // Code size       0 (0x0)
  } // end of method XamlType::op_Equality

  .method /*060002ED*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xaml.XamlType/*02000065*/ xamlType1,
                              class System.Xaml.XamlType/*02000065*/ xamlType2) cil managed
  // SIG: 00 02 02 12 81 94 12 81 94
  {
    // Method begins at RVA 0x2304
    // Code size       0 (0x0)
  } // end of method XamlType::op_Inequality

  .property /*170000E5*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          BaseType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlType/*02000065*/::get_BaseType() /* 0600029D */
  } // end of property XamlType::BaseType
  .property /*170000E6*/ instance callconv(8) class System.Xaml.Schema.XamlTypeInvoker/*02000058*/
          Invoker()
  {
    .get instance class System.Xaml.Schema.XamlTypeInvoker/*02000058*/ System.Xaml.XamlType/*02000065*/::get_Invoker() /* 0600029E */
  } // end of property XamlType::Invoker
  .property /*170000E7*/ instance callconv(8) bool
          IsNameValid()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsNameValid() /* 0600029F */
  } // end of property XamlType::IsNameValid
  .property /*170000E8*/ instance callconv(8) bool
          IsUnknown()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsUnknown() /* 060002A0 */
  } // end of property XamlType::IsUnknown
  .property /*170000E9*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Xaml.XamlType/*02000065*/::get_Name() /* 060002A1 */
  } // end of property XamlType::Name
  .property /*170000EA*/ instance callconv(8) string
          PreferredXamlNamespace()
  {
    .get instance string System.Xaml.XamlType/*02000065*/::get_PreferredXamlNamespace() /* 060002A2 */
  } // end of property XamlType::PreferredXamlNamespace
  .property /*170000EB*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/>
          TypeArguments()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> System.Xaml.XamlType/*02000065*/::get_TypeArguments() /* 060002A3 */
  } // end of property XamlType::TypeArguments
  .property /*170000EC*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          UnderlyingType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Xaml.XamlType/*02000065*/::get_UnderlyingType() /* 060002A4 */
  } // end of property XamlType::UnderlyingType
  .property /*170000ED*/ instance callconv(8) bool
          ConstructionRequiresArguments()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_ConstructionRequiresArguments() /* 060002A5 */
  } // end of property XamlType::ConstructionRequiresArguments
  .property /*170000EE*/ instance callconv(8) bool
          IsArray()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsArray() /* 060002A6 */
  } // end of property XamlType::IsArray
  .property /*170000EF*/ instance callconv(8) bool
          IsCollection()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsCollection() /* 060002A7 */
  } // end of property XamlType::IsCollection
  .property /*170000F0*/ instance callconv(8) bool
          IsConstructible()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsConstructible() /* 060002A8 */
  } // end of property XamlType::IsConstructible
  .property /*170000F1*/ instance callconv(8) bool
          IsDictionary()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsDictionary() /* 060002A9 */
  } // end of property XamlType::IsDictionary
  .property /*170000F2*/ instance callconv(8) bool
          IsGeneric()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsGeneric() /* 060002AA */
  } // end of property XamlType::IsGeneric
  .property /*170000F3*/ instance callconv(8) bool
          IsMarkupExtension()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsMarkupExtension() /* 060002AB */
  } // end of property XamlType::IsMarkupExtension
  .property /*170000F4*/ instance callconv(8) bool
          IsNameScope()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsNameScope() /* 060002AC */
  } // end of property XamlType::IsNameScope
  .property /*170000F5*/ instance callconv(8) bool
          IsNullable()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsNullable() /* 060002AD */
  } // end of property XamlType::IsNullable
  .property /*170000F6*/ instance callconv(8) bool
          IsPublic()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsPublic() /* 060002AE */
  } // end of property XamlType::IsPublic
  .property /*170000F7*/ instance callconv(8) bool
          IsUsableDuringInitialization()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsUsableDuringInitialization() /* 060002AF */
  } // end of property XamlType::IsUsableDuringInitialization
  .property /*170000F8*/ instance callconv(8) bool
          IsWhitespaceSignificantCollection()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsWhitespaceSignificantCollection() /* 060002B0 */
  } // end of property XamlType::IsWhitespaceSignificantCollection
  .property /*170000F9*/ instance callconv(8) bool
          IsXData()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsXData() /* 060002B1 */
  } // end of property XamlType::IsXData
  .property /*170000FA*/ instance callconv(8) bool
          TrimSurroundingWhitespace()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_TrimSurroundingWhitespace() /* 060002B2 */
  } // end of property XamlType::TrimSurroundingWhitespace
  .property /*170000FB*/ instance callconv(8) bool
          IsAmbient()
  {
    .get instance bool System.Xaml.XamlType/*02000065*/::get_IsAmbient() /* 060002B3 */
  } // end of property XamlType::IsAmbient
  .property /*170000FC*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          KeyType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlType/*02000065*/::get_KeyType() /* 060002B4 */
  } // end of property XamlType::KeyType
  .property /*170000FD*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          ItemType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlType/*02000065*/::get_ItemType() /* 060002B5 */
  } // end of property XamlType::ItemType
  .property /*170000FE*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/>
          AllowedContentTypes()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> System.Xaml.XamlType/*02000065*/::get_AllowedContentTypes() /* 060002B6 */
  } // end of property XamlType::AllowedContentTypes
  .property /*170000FF*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/>
          ContentWrappers()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.XamlType/*02000065*/> System.Xaml.XamlType/*02000065*/::get_ContentWrappers() /* 060002B7 */
  } // end of property XamlType::ContentWrappers
  .property /*17000100*/ instance callconv(8) class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/>
          TypeConverter()
  {
    .get instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/> System.Xaml.XamlType/*02000065*/::get_TypeConverter() /* 060002B8 */
  } // end of property XamlType::TypeConverter
  .property /*17000101*/ instance callconv(8) class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/>
          ValueSerializer()
  {
    .get instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Windows.Markup.ValueSerializer/*02000002*/> System.Xaml.XamlType/*02000065*/::get_ValueSerializer() /* 060002B9 */
  } // end of property XamlType::ValueSerializer
  .property /*17000102*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          ContentProperty()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.XamlType/*02000065*/::get_ContentProperty() /* 060002BA */
  } // end of property XamlType::ContentProperty
  .property /*17000103*/ instance callconv(8) class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/>
          DeferringLoader()
  {
    .get instance class System.Xaml.Schema.XamlValueConverter`1/*02000052*/<class System.Xaml.XamlDeferringLoader/*0200005A*/> System.Xaml.XamlType/*02000065*/::get_DeferringLoader() /* 060002BB */
  } // end of property XamlType::DeferringLoader
  .property /*17000104*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          MarkupExtensionReturnType()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlType/*02000065*/::get_MarkupExtensionReturnType() /* 060002BC */
  } // end of property XamlType::MarkupExtensionReturnType
  .property /*17000105*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlType/*02000065*/::get_SchemaContext() /* 060002BD */
  } // end of property XamlType::SchemaContext
} // end of class System.Xaml.XamlType

.class /*02000066*/ public auto ansi beforefieldinit System.Xaml.Schema.XamlTypeName
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000075:0A000023*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerDisplayAttribute/*01000056*/::.ctor(string) /* 0A000023 */ = ( 01 00 2F 7B 7B 7B 4E 61 6D 65 73 70 61 63 65 7D   // ../{{{Namespace}
                                                                                                                                                              7D 7D 7B 4E 61 6D 65 7D 7B 54 79 70 65 41 72 67   // }}{Name}{TypeArg
                                                                                                                                                              53 74 72 69 6E 67 46 6F 72 44 65 62 75 67 67 65   // StringForDebugge
                                                                                                                                                              72 7D 00 00 )                                     // r}..
  .method /*060002EE*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000DF:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       0 (0x0)
  } // end of method XamlTypeName::get_Name

  .method /*060002EF*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000E0:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2306
    // Code size       0 (0x0)
  } // end of method XamlTypeName::set_Name

  .method /*060002F0*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E1:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2307
    // Code size       0 (0x0)
  } // end of method XamlTypeName::get_Namespace

  .method /*060002F1*/ public hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000E2:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       0 (0x0)
  } // end of method XamlTypeName::set_Namespace

  .method /*060002F2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2309
    // Code size       0 (0x0)
  } // end of method XamlTypeName::.ctor

  .method /*060002F3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xamlNamespace,
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x230a
    // Code size       0 (0x0)
  } // end of method XamlTypeName::.ctor

  .method /*060002F4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string xamlNamespace,
                               string name,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000002*/<class System.Xaml.Schema.XamlTypeName/*02000066*/> typeArguments) cil managed
  // SIG: 20 03 01 0E 0E 15 12 09 01 12 81 98
  {
    // Method begins at RVA 0x230b
    // Code size       0 (0x0)
  } // end of method XamlTypeName::.ctor

  .method /*060002F5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xaml.XamlType/*02000065*/ xamlType) cil managed
  // SIG: 20 01 01 12 81 94
  {
    // Method begins at RVA 0x230c
    // Code size       0 (0x0)
  } // end of method XamlTypeName::.ctor

  .method /*060002F6*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Schema.XamlTypeName/*02000066*/> 
          get_TypeArguments() cil managed
  // SIG: 20 00 15 12 19 01 12 81 98
  {
    // Method begins at RVA 0x230d
    // Code size       0 (0x0)
  } // end of method XamlTypeName::get_TypeArguments

  .method /*060002F7*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x230e
    // Code size       0 (0x0)
  } // end of method XamlTypeName::ToString

  .method /*060002F8*/ public hidebysig instance string 
          ToString(class System.Xaml.INamespacePrefixLookup/*0200003B*/ prefixLookup) cil managed
  // SIG: 20 01 0E 12 80 EC
  {
    // Method begins at RVA 0x230f
    // Code size       0 (0x0)
  } // end of method XamlTypeName::ToString

  .method /*060002F9*/ public hidebysig static 
          string  ToString(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Schema.XamlTypeName/*02000066*/> typeNameList,
                           class System.Xaml.INamespacePrefixLookup/*0200003B*/ prefixLookup) cil managed
  // SIG: 00 02 0E 15 12 19 01 12 81 98 12 80 EC
  {
    // Method begins at RVA 0x2310
    // Code size       0 (0x0)
  } // end of method XamlTypeName::ToString

  .method /*060002FA*/ public hidebysig static 
          class System.Xaml.Schema.XamlTypeName/*02000066*/ 
          Parse(string typeName,
                class System.Xaml.IXamlNamespaceResolver/*02000034*/ namespaceResolver) cil managed
  // SIG: 00 02 12 81 98 0E 12 80 D0
  {
    // Method begins at RVA 0x2311
    // Code size       0 (0x0)
  } // end of method XamlTypeName::Parse

  .method /*060002FB*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Schema.XamlTypeName/*02000066*/> 
          ParseList(string typeNameList,
                    class System.Xaml.IXamlNamespaceResolver/*02000034*/ namespaceResolver) cil managed
  // SIG: 00 02 15 12 19 01 12 81 98 0E 12 80 D0
  {
    // Method begins at RVA 0x2312
    // Code size       0 (0x0)
  } // end of method XamlTypeName::ParseList

  .method /*060002FC*/ public hidebysig static 
          bool  TryParse(string typeName,
                         class System.Xaml.IXamlNamespaceResolver/*02000034*/ namespaceResolver,
                         [out] class System.Xaml.Schema.XamlTypeName/*02000066*/& result) cil managed
  // SIG: 00 03 02 0E 12 80 D0 10 12 81 98
  {
    // Method begins at RVA 0x2313
    // Code size       0 (0x0)
  } // end of method XamlTypeName::TryParse

  .method /*060002FD*/ public hidebysig static 
          bool  TryParseList(string typeNameList,
                             class System.Xaml.IXamlNamespaceResolver/*02000034*/ namespaceResolver,
                             [out] class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Schema.XamlTypeName/*02000066*/>& result) cil managed
  // SIG: 00 03 02 0E 12 80 D0 10 15 12 19 01 12 81 98
  {
    // Method begins at RVA 0x2314
    // Code size       0 (0x0)
  } // end of method XamlTypeName::TryParseList

  .property /*17000106*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Xaml.Schema.XamlTypeName/*02000066*/::get_Name() /* 060002EE */
    .set instance void System.Xaml.Schema.XamlTypeName/*02000066*/::set_Name(string) /* 060002EF */
  } // end of property XamlTypeName::Name
  .property /*17000107*/ instance callconv(8) string
          Namespace()
  {
    .set instance void System.Xaml.Schema.XamlTypeName/*02000066*/::set_Namespace(string) /* 060002F1 */
    .get instance string System.Xaml.Schema.XamlTypeName/*02000066*/::get_Namespace() /* 060002F0 */
  } // end of property XamlTypeName::Namespace
  .property /*17000108*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Schema.XamlTypeName/*02000066*/>
          TypeArguments()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000006*/<class System.Xaml.Schema.XamlTypeName/*02000066*/> System.Xaml.Schema.XamlTypeName/*02000066*/::get_TypeArguments() /* 060002F6 */
  } // end of property XamlTypeName::TypeArguments
} // end of class System.Xaml.Schema.XamlTypeName

.class /*02000067*/ public auto ansi beforefieldinit System.Xaml.Schema.XamlTypeTypeConverter
       extends [System/*23000002*/]System.ComponentModel.TypeConverter/*01000010*/
{
  .method /*060002FE*/ public hidebysig virtual 
          instance bool  CanConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                        class [mscorlib/*23000001*/]System.Type/*01000003*/ sourceType) cil managed
  // SIG: 20 02 02 12 35 12 0D
  {
    // Method begins at RVA 0x2315
    // Code size       0 (0x0)
  } // end of method XamlTypeTypeConverter::CanConvertFrom

  .method /*060002FF*/ public hidebysig virtual 
          instance object  ConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ culture,
                                       object 'value') cil managed
  // SIG: 20 03 1C 12 35 12 45 1C
  {
    // Method begins at RVA 0x2316
    // Code size       0 (0x0)
  } // end of method XamlTypeTypeConverter::ConvertFrom

  .method /*06000300*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 02 02 12 35 12 0D
  {
    // Method begins at RVA 0x2317
    // Code size       0 (0x0)
  } // end of method XamlTypeTypeConverter::CanConvertTo

  .method /*06000301*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100000D*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000011*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*01000003*/ destinationType) cil managed
  // SIG: 20 04 1C 12 35 12 45 1C 12 0D
  {
    // Method begins at RVA 0x2318
    // Code size       0 (0x0)
  } // end of method XamlTypeTypeConverter::ConvertTo

  .method /*06000302*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2319
    // Code size       0 (0x0)
  } // end of method XamlTypeTypeConverter::.ctor

} // end of class System.Xaml.Schema.XamlTypeTypeConverter

.class /*02000068*/ public auto ansi beforefieldinit System.Xaml.XamlObjectReader
       extends System.Xaml.XamlReader/*02000043*/
{
  .method /*06000303*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'instance') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x231a
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::.ctor

  .method /*06000304*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'instance',
                               class System.Xaml.XamlObjectReaderSettings/*02000069*/ settings) cil managed
  // SIG: 20 02 01 1C 12 81 A4
  {
    // Method begins at RVA 0x231b
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::.ctor

  .method /*06000305*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'instance',
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 1C 12 81 88
  {
    // Method begins at RVA 0x231c
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::.ctor

  .method /*06000306*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'instance',
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlObjectReaderSettings/*02000069*/ settings) cil managed
  // SIG: 20 03 01 1C 12 81 88 12 81 A4
  {
    // Method begins at RVA 0x231d
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::.ctor

  .method /*06000307*/ public hidebysig virtual 
          instance bool  Read() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231e
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::Read

  .method /*06000308*/ public hidebysig specialname virtual 
          instance valuetype System.Xaml.XamlNodeType/*0200003E*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 F8
  {
    // Method begins at RVA 0x231f
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_NodeType

  .method /*06000309*/ public hidebysig specialname virtual 
          instance class System.Xaml.NamespaceDeclaration/*02000048*/ 
          get_Namespace() cil managed
  // SIG: 20 00 12 81 20
  {
    // Method begins at RVA 0x2320
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_Namespace

  .method /*0600030A*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlType/*02000065*/ 
          get_Type() cil managed
  // SIG: 20 00 12 81 94
  {
    // Method begins at RVA 0x2321
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_Type

  .method /*0600030B*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlMember/*02000054*/ 
          get_Member() cil managed
  // SIG: 20 00 12 81 50
  {
    // Method begins at RVA 0x2322
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_Member

  .method /*0600030C*/ public hidebysig specialname virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2323
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_Value

  .method /*0600030D*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x2324
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_SchemaContext

  .method /*0600030E*/ public hidebysig specialname virtual 
          instance bool  get_IsEof() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2325
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_IsEof

  .method /*0600030F*/ public hidebysig newslot specialname virtual 
          instance object  get_Instance() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2326
    // Code size       0 (0x0)
  } // end of method XamlObjectReader::get_Instance

  .property /*17000109*/ instance callconv(8) valuetype System.Xaml.XamlNodeType/*0200003E*/
          NodeType()
  {
    .get instance valuetype System.Xaml.XamlNodeType/*0200003E*/ System.Xaml.XamlObjectReader/*02000068*/::get_NodeType() /* 06000308 */
  } // end of property XamlObjectReader::NodeType
  .property /*1700010A*/ instance callconv(8) class System.Xaml.NamespaceDeclaration/*02000048*/
          Namespace()
  {
    .get instance class System.Xaml.NamespaceDeclaration/*02000048*/ System.Xaml.XamlObjectReader/*02000068*/::get_Namespace() /* 06000309 */
  } // end of property XamlObjectReader::Namespace
  .property /*1700010B*/ instance callconv(8) class System.Xaml.XamlType/*02000065*/
          Type()
  {
    .get instance class System.Xaml.XamlType/*02000065*/ System.Xaml.XamlObjectReader/*02000068*/::get_Type() /* 0600030A */
  } // end of property XamlObjectReader::Type
  .property /*1700010C*/ instance callconv(8) class System.Xaml.XamlMember/*02000054*/
          Member()
  {
    .get instance class System.Xaml.XamlMember/*02000054*/ System.Xaml.XamlObjectReader/*02000068*/::get_Member() /* 0600030B */
  } // end of property XamlObjectReader::Member
  .property /*1700010D*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Xaml.XamlObjectReader/*02000068*/::get_Value() /* 0600030C */
  } // end of property XamlObjectReader::Value
  .property /*1700010E*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlObjectReader/*02000068*/::get_SchemaContext() /* 0600030D */
  } // end of property XamlObjectReader::SchemaContext
  .property /*1700010F*/ instance callconv(8) bool
          IsEof()
  {
    .get instance bool System.Xaml.XamlObjectReader/*02000068*/::get_IsEof() /* 0600030E */
  } // end of property XamlObjectReader::IsEof
  .property /*17000110*/ instance callconv(8) object
          Instance()
  {
    .get instance object System.Xaml.XamlObjectReader/*02000068*/::get_Instance() /* 0600030F */
  } // end of property XamlObjectReader::Instance
} // end of class System.Xaml.XamlObjectReader

.class /*02000069*/ public auto ansi beforefieldinit System.Xaml.XamlObjectReaderSettings
       extends System.Xaml.XamlReaderSettings/*0200003F*/
{
  .method /*06000310*/ public hidebysig specialname 
          instance bool  get_RequireExplicitContentVisibility() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000E3:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2327
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderSettings::get_RequireExplicitContentVisibility

  .method /*06000311*/ public hidebysig specialname 
          instance void  set_RequireExplicitContentVisibility(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000E4:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2328
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderSettings::set_RequireExplicitContentVisibility

  .method /*06000312*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2329
    // Code size       0 (0x0)
  } // end of method XamlObjectReaderSettings::.ctor

  .property /*17000111*/ instance callconv(8) bool
          RequireExplicitContentVisibility()
  {
    .get instance bool System.Xaml.XamlObjectReaderSettings/*02000069*/::get_RequireExplicitContentVisibility() /* 06000310 */
    .set instance void System.Xaml.XamlObjectReaderSettings/*02000069*/::set_RequireExplicitContentVisibility(bool) /* 06000311 */
  } // end of property XamlObjectReaderSettings::RequireExplicitContentVisibility
} // end of class System.Xaml.XamlObjectReaderSettings

.class /*0200006A*/ public auto ansi beforefieldinit System.Xaml.XamlXmlWriter
       extends System.Xaml.XamlWriter/*0200003C*/
{
  .method /*06000313*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 6D 12 81 88
  {
    // Method begins at RVA 0x232a
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::.ctor

  .method /*06000314*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlWriterSettings/*0200006B*/ settings) cil managed
  // SIG: 20 03 01 12 6D 12 81 88 12 81 AC
  {
    // Method begins at RVA 0x232b
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::.ctor

  .method /*06000315*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextWriter/*0100002D*/ textWriter,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 80 B5 12 81 88
  {
    // Method begins at RVA 0x232c
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::.ctor

  .method /*06000316*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextWriter/*0100002D*/ textWriter,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlWriterSettings/*0200006B*/ settings) cil managed
  // SIG: 20 03 01 12 80 B5 12 81 88 12 81 AC
  {
    // Method begins at RVA 0x232d
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::.ctor

  .method /*06000317*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100002E*/ xmlWriter,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext) cil managed
  // SIG: 20 02 01 12 80 B9 12 81 88
  {
    // Method begins at RVA 0x232e
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::.ctor

  .method /*06000318*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100002E*/ xmlWriter,
                               class System.Xaml.XamlSchemaContext/*02000062*/ schemaContext,
                               class System.Xaml.XamlXmlWriterSettings/*0200006B*/ settings) cil managed
  // SIG: 20 03 01 12 80 B9 12 81 88 12 81 AC
  {
    // Method begins at RVA 0x232f
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::.ctor

  .method /*06000319*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2330
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::Dispose

  .method /*0600031A*/ public hidebysig instance void 
          Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2331
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::Flush

  .method /*0600031B*/ public hidebysig virtual 
          instance void  WriteGetObject() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2332
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteGetObject

  .method /*0600031C*/ public hidebysig virtual 
          instance void  WriteStartObject(class System.Xaml.XamlType/*02000065*/ 'type') cil managed
  // SIG: 20 01 01 12 81 94
  {
    // Method begins at RVA 0x2333
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteStartObject

  .method /*0600031D*/ public hidebysig virtual 
          instance void  WriteEndObject() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2334
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteEndObject

  .method /*0600031E*/ public hidebysig virtual 
          instance void  WriteStartMember(class System.Xaml.XamlMember/*02000054*/ 'property') cil managed
  // SIG: 20 01 01 12 81 50
  {
    // Method begins at RVA 0x2335
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteStartMember

  .method /*0600031F*/ public hidebysig virtual 
          instance void  WriteEndMember() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2336
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteEndMember

  .method /*06000320*/ public hidebysig virtual 
          instance void  WriteValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2337
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteValue

  .method /*06000321*/ public hidebysig virtual 
          instance void  WriteNamespace(class System.Xaml.NamespaceDeclaration/*02000048*/ namespaceDeclaration) cil managed
  // SIG: 20 01 01 12 81 20
  {
    // Method begins at RVA 0x2338
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::WriteNamespace

  .method /*06000322*/ public hidebysig specialname 
          instance class System.Xaml.XamlXmlWriterSettings/*0200006B*/ 
          get_Settings() cil managed
  // SIG: 20 00 12 81 AC
  {
    // Method begins at RVA 0x2339
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::get_Settings

  .method /*06000323*/ public hidebysig specialname virtual 
          instance class System.Xaml.XamlSchemaContext/*02000062*/ 
          get_SchemaContext() cil managed
  // SIG: 20 00 12 81 88
  {
    // Method begins at RVA 0x233a
    // Code size       0 (0x0)
  } // end of method XamlXmlWriter::get_SchemaContext

  .property /*17000112*/ instance callconv(8) class System.Xaml.XamlXmlWriterSettings/*0200006B*/
          Settings()
  {
    .get instance class System.Xaml.XamlXmlWriterSettings/*0200006B*/ System.Xaml.XamlXmlWriter/*0200006A*/::get_Settings() /* 06000322 */
  } // end of property XamlXmlWriter::Settings
  .property /*17000113*/ instance callconv(8) class System.Xaml.XamlSchemaContext/*02000062*/
          SchemaContext()
  {
    .get instance class System.Xaml.XamlSchemaContext/*02000062*/ System.Xaml.XamlXmlWriter/*0200006A*/::get_SchemaContext() /* 06000323 */
  } // end of property XamlXmlWriter::SchemaContext
} // end of class System.Xaml.XamlXmlWriter

.class /*0200006B*/ public auto ansi beforefieldinit System.Xaml.XamlXmlWriterSettings
       extends System.Xaml.XamlWriterSettings/*02000041*/
{
  .method /*06000324*/ public hidebysig specialname 
          instance bool  get_AssumeValidInput() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000E5:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x233b
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterSettings::get_AssumeValidInput

  .method /*06000325*/ public hidebysig specialname 
          instance void  set_AssumeValidInput(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000E6:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x233c
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterSettings::set_AssumeValidInput

  .method /*06000326*/ public hidebysig specialname 
          instance bool  get_CloseOutput() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000E7:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x233d
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterSettings::get_CloseOutput

  .method /*06000327*/ public hidebysig specialname 
          instance void  set_CloseOutput(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000E8:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x233e
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterSettings::set_CloseOutput

  .method /*06000328*/ public hidebysig instance class System.Xaml.XamlXmlWriterSettings/*0200006B*/ 
          Copy() cil managed
  // SIG: 20 00 12 81 AC
  {
    // Method begins at RVA 0x233f
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterSettings::Copy

  .method /*06000329*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2340
    // Code size       0 (0x0)
  } // end of method XamlXmlWriterSettings::.ctor

  .property /*17000114*/ instance callconv(8) bool
          AssumeValidInput()
  {
    .get instance bool System.Xaml.XamlXmlWriterSettings/*0200006B*/::get_AssumeValidInput() /* 06000324 */
    .set instance void System.Xaml.XamlXmlWriterSettings/*0200006B*/::set_AssumeValidInput(bool) /* 06000325 */
  } // end of property XamlXmlWriterSettings::AssumeValidInput
  .property /*17000115*/ instance callconv(8) bool
          CloseOutput()
  {
    .get instance bool System.Xaml.XamlXmlWriterSettings/*0200006B*/::get_CloseOutput() /* 06000326 */
    .set instance void System.Xaml.XamlXmlWriterSettings/*0200006B*/::set_CloseOutput(bool) /* 06000327 */
  } // end of property XamlXmlWriterSettings::CloseOutput
} // end of class System.Xaml.XamlXmlWriterSettings

.class /*0200006C*/ public auto ansi sealed beforefieldinit System.Windows.Markup.XData
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000079:06000022*/ instance void System.Windows.Markup.ContentPropertyAttribute/*02000009*/::.ctor(string) /* 06000022 */ = ( 01 00 04 54 65 78 74 00 00 )                      // ...Text..
  .method /*0600032A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2341
    // Code size       0 (0x0)
  } // end of method XData::.ctor

  .method /*0600032B*/ public hidebysig specialname 
          instance string  get_Text() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2342
    // Code size       0 (0x0)
  } // end of method XData::get_Text

  .method /*0600032C*/ public hidebysig specialname 
          instance void  set_Text(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2343
    // Code size       0 (0x0)
  } // end of method XData::set_Text

  .method /*0600032D*/ public hidebysig specialname 
          instance object  get_XmlReader() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2344
    // Code size       0 (0x0)
  } // end of method XData::get_XmlReader

  .method /*0600032E*/ public hidebysig specialname 
          instance void  set_XmlReader(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2345
    // Code size       0 (0x0)
  } // end of method XData::set_XmlReader

  .property /*17000116*/ instance callconv(8) string
          Text()
  {
    .set instance void System.Windows.Markup.XData/*0200006C*/::set_Text(string) /* 0600032C */
    .get instance string System.Windows.Markup.XData/*0200006C*/::get_Text() /* 0600032B */
  } // end of property XData::Text
  .property /*17000117*/ instance callconv(8) object
          XmlReader()
  {
    .get instance object System.Windows.Markup.XData/*0200006C*/::get_XmlReader() /* 0600032D */
    .set instance void System.Windows.Markup.XData/*0200006C*/::set_XmlReader(object) /* 0600032E */
  } // end of property XData::XmlReader
} // end of class System.Windows.Markup.XData

.class /*0200006D*/ private auto ansi '<PrivateImplementationDetails>{55D021A1-CF45-4D2E-812A-0BA1C23EF42C}'
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007B:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100004B*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .class /*0200006E*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=10'
         extends [mscorlib/*23000001*/]System.ValueType/*01000030*/
  {
    .pack 1
    .size 10
  } // end of class '__StaticArrayInitTypeSize=10'

  .class /*0200006F*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=16'
         extends [mscorlib/*23000001*/]System.ValueType/*01000030*/
  {
    .pack 1
    .size 16
  } // end of class '__StaticArrayInitTypeSize=16'

  .field /*0400001A*/ static assembly valuetype '<PrivateImplementationDetails>{55D021A1-CF45-4D2E-812A-0BA1C23EF42C}'/*0200006D*//'__StaticArrayInitTypeSize=10'/*0200006E*/ '$$method0x6000d0b-1' at I_0001820C
  .field /*0400001B*/ static assembly class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100002F*/<string,int32> '$$method0x60008d3-1'
  .field /*0400001C*/ static assembly class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100002F*/<string,int32> '$$method0x60008d4-1'
  .field /*0400001D*/ static assembly valuetype '<PrivateImplementationDetails>{55D021A1-CF45-4D2E-812A-0BA1C23EF42C}'/*0200006D*//'__StaticArrayInitTypeSize=16'/*0200006F*/ '$$method0x6000e34-1' at I_0001821C
  .field /*0400001E*/ static assembly int64 '$$method0x6000e7a-1' at I_0001822C
} // end of class '<PrivateImplementationDetails>{55D021A1-CF45-4D2E-812A-0BA1C23EF42C}'


// =============================================================

.data cil I_0001820C = bytearray (
                 20 00 09 00 0A 00 0D 00 0C 00)                   //  .........
.data cil I_00018216 = int8[2]
.data cil I_0001821C = bytearray (
                 27 00 22 00 2C 00 3D 00 7B 00 7D 00 5C 00 20 00) // '.".,.=.{.}.\. .
.data cil I_0001822C = bytearray (
                 31 BF 38 56 AD 36 4E 35)                         // 1.8V.6N5
// *********** DISASSEMBLY COMPLETE ***********************
