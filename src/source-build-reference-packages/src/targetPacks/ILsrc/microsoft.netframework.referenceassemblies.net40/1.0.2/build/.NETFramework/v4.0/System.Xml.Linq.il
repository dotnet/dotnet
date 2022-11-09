
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
// Time-date stamp:            0x4ba1e3a2
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x08
// Minor linker version:           0x00
// Size of code:                   0x00009a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000b8ca
// Base of code:                   0x00002000
// Base of data:                   0x0000c000
// Image base:                     0x69e50000
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
// Checksum:                       0x0000bdd3
// Subsystem:                      0x0003
// DLL characteristics:            0x8540
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000b878 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x0000043c] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000a400 [0x00001760] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
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
//              0x00009900 Virtual Size
//              0x00002000 Virtual Address
//              0x00009a00 Size of Raw Data
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
//              0x0000043c Virtual Size
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00009c00 Pointer to Raw Data
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
//              0x0000a200 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000008cc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000b8ba Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 E5 69 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000021b0 [0x00008578] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x0000a728 [0x000010d0] address [size] of Resources Directory:       
// 0x0000b7f8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002e78 Size
//              '#~' Name
//    Stream 2:
//              0x00002ee4 Offset
//              0x000018cc Size
//              '#Strings' Name
//    Stream 3:
//              0x000047b0 Offset
//              0x00000bb4 Size
//              '#US' Name
//    Stream 4:
//              0x00005364 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00005374 Offset
//              0x00003204 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000017092bb4df57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 47968
// PE header size       : 512 (496 used)    ( 1.07%)
// PE additional info   : 7239              (15.09%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.15%)
// CLR meta-data size  : 34168              (71.23%)
// CLR additional info : 4432               ( 9.24%)
// CLR method headers  : 350                ( 0.73%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 4.27%)
// Unaccounted          : -853              (-1.78%)

// Num.of PE sections   : 3
//   .text    - 39424
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 34168
//   Module        -    1 (10 bytes)
//   TypeDef       -   33 (462 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   71 (426 bytes)
//   MethodDef     -  352 (4928 bytes)    2 abstract, 0 native, 350 bodies
//   FieldDef      -   96 (576 bytes)     4 constant
//   MemberRef     -   41 (246 bytes)
//   ParamDef      -  335 (2010 bytes)
//   MethodImpl    -   13 (78 bytes)
//   Constant      -   86 (516 bytes)
//   CustomAttribute-  211 (1266 bytes)
//   ClassLayout   -    1 (8 bytes)
//   InterfaceImpl -   10 (40 bytes)
//   PropertyMap   -   16 (64 bytes)
//   Property      -   57 (342 bytes)
//   MethodSemantic-   77 (462 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    4 (8 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   ManifestResource-    1 (12 bytes)
//   NestedClass   -    1 (4 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    2 (12 bytes)
//   FieldRVA      -    2 (12 bytes)
//   GenericParam  -   14 (112 bytes)
//   GenericParamConstraint-   10 (40 bytes)
//   Strings       -  6345 bytes
//   Blobs         - 12804 bytes
//   UserStrings   -  2996 bytes
//   Guids         -    16 bytes
//   Uncategorized -   241 bytes

// CLR additional info : 4432
//   Resources         - 4304

// CLR method headers : 350
//   Num.of method bodies  - 350
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 350

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class XName                          (public) (auto) (ansi) (sealed) 
// Class XNamespace                     (public) (auto) (ansi) (sealed) 
// Class XObject                        (public) (abstract) (auto) (ansi) 
// Class XObjectChange                  (public) (auto) (ansi) (sealed) 
// Class XObjectChangeEventArgs         (public) (auto) (ansi) 
// Class XNode                          (public) (abstract) (auto) (ansi) 
// Class XNodeDocumentOrderComparer     (public) (auto) (ansi) (sealed) 
// Class XNodeEqualityComparer          (public) (auto) (ansi) (sealed) 
// Class XText                          (public) (auto) (ansi) 
// Class XCData                         (public) (auto) (ansi) 
// Class XContainer                     (public) (abstract) (auto) (ansi) 
// Class XElement                       (public) (auto) (ansi) 
// Class LoadOptions                    (public) (auto) (ansi) (sealed) 
// Class SaveOptions                    (public) (auto) (ansi) (sealed) 
// Class ReaderOptions                  (public) (auto) (ansi) (sealed) 
// Class XDocument                      (public) (auto) (ansi) 
// Class XComment                       (public) (auto) (ansi) 
// Class XProcessingInstruction         (public) (auto) (ansi) 
// Class XDeclaration                   (public) (auto) (ansi) 
// Class XDocumentType                  (public) (auto) (ansi) 
// Class XAttribute                     (public) (auto) (ansi) 
// Class XStreamingElement              (public) (auto) (ansi) 
// Class Extensions                     (public) (abstract) (auto) (ansi) (sealed) 
// Class Extensions                     (public) (abstract) (auto) (ansi) (sealed) 
// Class Extensions                     (public) (abstract) (auto) (ansi) (sealed) 
// Class XTypeDescriptionProvider`1     (auto) (ansi) 
// Class NameSerializer                 (auto) (ansi) (sealed) 
// Class FXAssembly                     (abstract) (auto) (ansi) (sealed) 
// Class ThisAssembly                   (abstract) (auto) (ansi) (sealed) 
// Class AssemblyRef                    (abstract) (auto) (ansi) (sealed) 
// Class <PrivateImplementationDetails>{DCA98F07-D191-4730-A70A-A34D421FD7EE} (auto) (ansi) 
// Class __StaticArrayInitTypeSize=40   (explicit) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000003*/ System.Xml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ System.Core
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000005*/ System.Runtime.Serialization
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ noplatform System.Xml.Linq
{
  .custom /*0C000002:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A00000E*/ instance void [mscorlib/*23000002*/]System.Security.AllowPartiallyTrustedCallersAttribute/*0100002A*/::.ctor() /* 0A00000E */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A00000F*/ instance void [mscorlib/*23000002*/]System.Security.SecurityRulesAttribute/*0100002B*/::.ctor(valuetype [mscorlib/*23000002*/]System.Security.SecurityRuleSet/*0100002C*/) /* 0A00000F */ = ( 01 00 01 01 00 54 02 1B 53 6B 69 70 56 65 72 69   // .....T..SkipVeri
                                                                                                                                                                                                                              66 69 63 61 74 69 6F 6E 49 6E 46 75 6C 6C 54 72   // ficationInFullTr
                                                                                                                                                                                                                              75 73 74 01 )                                     // ust.
  .custom /*0C000005:0A000010*/ instance void [mscorlib/*23000002*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*0100002D*/::.ctor(int32) /* 0A000010 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000011*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyDefaultAliasAttribute/*0100002E*/::.ctor(string) /* 0A000011 */ = ( 01 00 13 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69   // ...System.Xml.Li
                                                                                                                                                                  6E 71 2E 64 6C 6C 00 00 )                         // nq.dll..

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000012*/ instance void [mscorlib/*23000002*/]System.Diagnostics.DebuggableAttribute/*0100002F*/::.ctor(valuetype [mscorlib/*23000002*/]System.Diagnostics.DebuggableAttribute/*0100002F*//DebuggingModes/*01000030*/) /* 0A000012 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000013*/ instance void [mscorlib/*23000002*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000031*/::.ctor(bool) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000009:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000A:0A000015*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyCopyrightAttribute/*01000033*/::.ctor(string) /* 0A000015 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C00000B:0A000016*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyFileVersionAttribute/*01000034*/::.ctor(string) /* 0A000016 */ = ( 01 00 0B 34 2E 30 2E 33 30 33 31 39 2E 31 00 00 ) // ...4.0.30319.1..
  .custom /*0C00000C:0A000017*/ instance void [mscorlib/*23000002*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000035*/::.ctor() /* 0A000017 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C00000D:0A000018*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyDescriptionAttribute/*01000036*/::.ctor(string) /* 0A000018 */ = ( 01 00 13 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69   // ...System.Xml.Li
                                                                                                                                                                 6E 71 2E 64 6C 6C 00 00 )                         // nq.dll..
  .custom /*0C00000E:0A000019*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyCompanyAttribute/*01000037*/::.ctor(string) /* 0A000019 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00001A*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyProductAttribute/*01000038*/::.ctor(string) /* 0A00001A */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C000010:0A00001B*/ instance void [mscorlib/*23000002*/]System.Resources.SatelliteContractVersionAttribute/*01000039*/::.ctor(string) /* 0A00001B */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000011:0A00001C*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyKeyFileAttribute/*0100003A*/::.ctor(string) /* 0A00001C */ = ( 01 00 24 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..$f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 45 63 6D 61 50 75 62 6C 69 63   // evdiv\EcmaPublic
                                                                                                                                                             4B 65 79 2E 73 6E 6B 00 00 )                      // Key.snk..
  .custom /*0C000012:0A00001D*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100003B*/::.ctor(string) /* 0A00001D */ = ( 01 00 0B 34 2E 30 2E 33 30 33 31 39 2E 31 00 00 ) // ...4.0.30319.1..
  .custom /*0C000013:0A00001E*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyTitleAttribute/*0100003C*/::.ctor(string) /* 0A00001E */ = ( 01 00 13 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69   // ...System.Xml.Li
                                                                                                                                                           6E 71 2E 64 6C 6C 00 00 )                         // nq.dll..
  .custom /*0C000014:0A00001F*/ instance void [mscorlib/*23000002*/]System.Resources.NeutralResourcesLanguageAttribute/*0100003D*/::.ctor(string) /* 0A00001F */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C000015:0A000020*/ instance void [mscorlib/*23000002*/]System.Reflection.AssemblyDelaySignAttribute/*0100003E*/::.ctor(bool) /* 0A000020 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000016:0A000021*/ instance void [mscorlib/*23000002*/]System.Runtime.AssemblyTargetedPatchBandAttribute/*0100003F*/::.ctor(string) /* 0A000021 */ = ( 01 00 08 31 2E 30 2E 32 31 2D 30 00 00 )          // ...1.0.21-0..
  .custom /*0C000017:0A000022*/ instance void [mscorlib/*23000002*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000040*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.module System.Xml.Linq.dll
// MVID: {b1acf4e4-d5a6-452c-9276-e601578405f8}
.imagebase 0x69e50000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FC5E0714000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi serializable sealed beforefieldinit System.Xml.Linq.XName
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
       implements class [mscorlib/*23000002*/]System.IEquatable`1/*01000005*/<class System.Xml.Linq.XName/*02000002*/>/*1B000001*/,
                  [mscorlib/*23000002*/]System.Runtime.Serialization.ISerializable/*01000002*/
{
  .custom /*0C000019:0A000023*/ instance void [System.Runtime.Serialization/*23000005*/]System.Runtime.Serialization.KnownTypeAttribute/*01000041*/::.ctor(class [mscorlib/*23000002*/]System.Type/*01000009*/) /* 0A000023 */ = ( 01 00 1E 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69   // ...System.Xml.Li
                                                                                                                                                                                                                                   6E 71 2E 4E 61 6D 65 53 65 72 69 61 6C 69 7A 65   // nq.NameSerialize
                                                                                                                                                                                                                                   72 00 00 )                                        // r..
  .method /*06000001*/ public hidebysig specialname 
          instance string  get_LocalName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000001:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method XName::get_LocalName

  .method /*06000002*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNamespace/*02000003*/ 
          get_Namespace() cil managed
  // SIG: 20 00 12 0C
  {
    .custom /*0C000018:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method XName::get_Namespace

  .method /*06000003*/ public hidebysig specialname 
          instance string  get_NamespaceName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method XName::get_NamespaceName

  .method /*06000004*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method XName::ToString

  .method /*06000005*/ public hidebysig static 
          class System.Xml.Linq.XName/*02000002*/ 
          Get(string expandedName) cil managed
  // SIG: 00 01 12 08 0E
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method XName::Get

  .method /*06000006*/ public hidebysig static 
          class System.Xml.Linq.XName/*02000002*/ 
          Get(string localName,
              string namespaceName) cil managed
  // SIG: 00 02 12 08 0E 0E
  {
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method XName::Get

  .method /*06000007*/ public hidebysig specialname static 
          class System.Xml.Linq.XName/*02000002*/ 
          op_Implicit(string expandedName) cil managed
  // SIG: 00 01 12 08 0E
  {
    .custom /*0C00001A:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method XName::op_Implicit

  .method /*06000008*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method XName::Equals

  .method /*06000009*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method XName::GetHashCode

  .method /*0600000A*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xml.Linq.XName/*02000002*/ left,
                            class System.Xml.Linq.XName/*02000002*/ right) cil managed
  // SIG: 00 02 02 12 08 12 08
  {
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method XName::op_Equality

  .method /*0600000B*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xml.Linq.XName/*02000002*/ left,
                              class System.Xml.Linq.XName/*02000002*/ right) cil managed
  // SIG: 00 02 02 12 08 12 08
  {
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method XName::op_Inequality

  .method /*0600000C*/ private hidebysig newslot virtual final 
          instance bool  'System.IEquatable<System.Xml.Linq.XName>.Equals'(class System.Xml.Linq.XName/*02000002*/ other) cil managed
  // SIG: 20 01 02 12 08
  {
    .override  method instance bool class [mscorlib/*23000002*/]System.IEquatable`1/*01000005*/<class System.Xml.Linq.XName/*02000002*/>/*1B000001*/::Equals(!0) /* 0A000002 */
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method XName::'System.IEquatable<System.Xml.Linq.XName>.Equals'

  .method /*0600000D*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [mscorlib/*23000002*/]System.Runtime.Serialization.SerializationInfo/*01000003*/ info,
                                                                                  valuetype [mscorlib/*23000002*/]System.Runtime.Serialization.StreamingContext/*01000004*/ context) cil managed
  // SIG: 20 02 01 12 0D 11 11
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(128)}}
    .override [mscorlib/*23000002*/]System.Runtime.Serialization.ISerializable/*01000002*/::GetObjectData /*01000002::0A000001*/ 
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method XName::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000001*/ instance callconv(8) string
          LocalName()
  {
    .get instance string System.Xml.Linq.XName/*02000002*/::get_LocalName() /* 06000001 */
  } // end of property XName::LocalName
  .property /*17000002*/ instance callconv(8) class System.Xml.Linq.XNamespace/*02000003*/
          Namespace()
  {
    .get instance class System.Xml.Linq.XNamespace/*02000003*/ System.Xml.Linq.XName/*02000002*/::get_Namespace() /* 06000002 */
  } // end of property XName::Namespace
  .property /*17000003*/ instance callconv(8) string
          NamespaceName()
  {
    .get instance string System.Xml.Linq.XName/*02000002*/::get_NamespaceName() /* 06000003 */
  } // end of property XName::NamespaceName
} // end of class System.Xml.Linq.XName

.class /*02000003*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XNamespace
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .method /*0600000E*/ public hidebysig specialname 
          instance string  get_NamespaceName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00001D:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method XNamespace::get_NamespaceName

  .method /*0600000F*/ public hidebysig instance class System.Xml.Linq.XName/*02000002*/ 
          GetName(string localName) cil managed
  // SIG: 20 01 12 08 0E
  {
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method XNamespace::GetName

  .method /*06000010*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method XNamespace::ToString

  .method /*06000011*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000003*/ 
          get_None() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method XNamespace::get_None

  .method /*06000012*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000003*/ 
          get_Xml() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method XNamespace::get_Xml

  .method /*06000013*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000003*/ 
          get_Xmlns() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method XNamespace::get_Xmlns

  .method /*06000014*/ public hidebysig static 
          class System.Xml.Linq.XNamespace/*02000003*/ 
          Get(string namespaceName) cil managed
  // SIG: 00 01 12 0C 0E
  {
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method XNamespace::Get

  .method /*06000015*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000003*/ 
          op_Implicit(string namespaceName) cil managed
  // SIG: 00 01 12 0C 0E
  {
    .custom /*0C000021:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method XNamespace::op_Implicit

  .method /*06000016*/ public hidebysig specialname static 
          class System.Xml.Linq.XName/*02000002*/ 
          op_Addition(class System.Xml.Linq.XNamespace/*02000003*/ ns,
                      string localName) cil managed
  // SIG: 00 02 12 08 12 0C 0E
  {
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method XNamespace::op_Addition

  .method /*06000017*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method XNamespace::Equals

  .method /*06000018*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method XNamespace::GetHashCode

  .method /*06000019*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xml.Linq.XNamespace/*02000003*/ left,
                            class System.Xml.Linq.XNamespace/*02000003*/ right) cil managed
  // SIG: 00 02 02 12 0C 12 0C
  {
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method XNamespace::op_Equality

  .method /*0600001A*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xml.Linq.XNamespace/*02000003*/ left,
                              class System.Xml.Linq.XNamespace/*02000003*/ right) cil managed
  // SIG: 00 02 02 12 0C 12 0C
  {
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method XNamespace::op_Inequality

  .property /*17000004*/ instance callconv(8) string
          NamespaceName()
  {
    .get instance string System.Xml.Linq.XNamespace/*02000003*/::get_NamespaceName() /* 0600000E */
  } // end of property XNamespace::NamespaceName
  .property /*17000005*/ callconv(8) class System.Xml.Linq.XNamespace/*02000003*/
          None()
  {
    .get class System.Xml.Linq.XNamespace/*02000003*/ System.Xml.Linq.XNamespace/*02000003*/::get_None() /* 06000011 */
  } // end of property XNamespace::None
  .property /*17000006*/ callconv(8) class System.Xml.Linq.XNamespace/*02000003*/
          Xml()
  {
    .get class System.Xml.Linq.XNamespace/*02000003*/ System.Xml.Linq.XNamespace/*02000003*/::get_Xml() /* 06000012 */
  } // end of property XNamespace::Xml
  .property /*17000007*/ callconv(8) class System.Xml.Linq.XNamespace/*02000003*/
          Xmlns()
  {
    .get class System.Xml.Linq.XNamespace/*02000003*/ System.Xml.Linq.XNamespace/*02000003*/::get_Xmlns() /* 06000013 */
  } // end of property XNamespace::Xmlns
} // end of class System.Xml.Linq.XNamespace

.class /*02000004*/ public abstract auto ansi beforefieldinit System.Xml.Linq.XObject
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
       implements [System.Xml/*23000003*/]System.Xml.IXmlLineInfo/*01000006*/
{
  .method /*0600001B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000026:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method XObject::.ctor

  .method /*0600001C*/ public hidebysig specialname 
          instance string  get_BaseUri() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method XObject::get_BaseUri

  .method /*0600001D*/ public hidebysig specialname 
          instance class System.Xml.Linq.XDocument/*02000011*/ 
          get_Document() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method XObject::get_Document

  .method /*0600001E*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x0
  } // end of method XObject::get_NodeType

  .method /*0600001F*/ public hidebysig specialname 
          instance class System.Xml.Linq.XElement/*0200000D*/ 
          get_Parent() cil managed
  // SIG: 20 00 12 34
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method XObject::get_Parent

  .method /*06000020*/ public hidebysig instance void 
          AddAnnotation(object annotation) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method XObject::AddAnnotation

  .method /*06000021*/ public hidebysig instance object 
          Annotation(class [mscorlib/*23000002*/]System.Type/*01000009*/ 'type') cil managed
  // SIG: 20 01 1C 12 25
  {
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method XObject::Annotation

  .method /*06000022*/ public hidebysig instance !!T 
          Annotation<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method XObject::Annotation

  .method /*06000023*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<object> 
          Annotations(class [mscorlib/*23000002*/]System.Type/*01000009*/ 'type') cil managed
  // SIG: 20 01 15 12 29 01 1C 12 25
  {
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method XObject::Annotations

  .method /*06000024*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> 
          Annotations<class T>() cil managed
  // SIG: 30 01 00 15 12 29 01 1E 00
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method XObject::Annotations

  .method /*06000025*/ public hidebysig instance void 
          RemoveAnnotations(class [mscorlib/*23000002*/]System.Type/*01000009*/ 'type') cil managed
  // SIG: 20 01 01 12 25
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method XObject::RemoveAnnotations

  .method /*06000026*/ public hidebysig instance void 
          RemoveAnnotations<class T>() cil managed
  // SIG: 30 01 00 01
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method XObject::RemoveAnnotations

  .method /*06000027*/ public hidebysig specialname 
          instance void  add_Changed(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/> 'value') cil managed
  // SIG: 20 01 01 15 12 1D 01 12 18
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method XObject::add_Changed

  .method /*06000028*/ public hidebysig specialname 
          instance void  remove_Changed(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/> 'value') cil managed
  // SIG: 20 01 01 15 12 1D 01 12 18
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method XObject::remove_Changed

  .method /*06000029*/ public hidebysig specialname 
          instance void  add_Changing(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/> 'value') cil managed
  // SIG: 20 01 01 15 12 1D 01 12 18
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method XObject::add_Changing

  .method /*0600002A*/ public hidebysig specialname 
          instance void  remove_Changing(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/> 'value') cil managed
  // SIG: 20 01 01 15 12 1D 01 12 18
  {
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method XObject::remove_Changing

  .method /*0600002B*/ private hidebysig newslot virtual final 
          instance bool  System.Xml.IXmlLineInfo.HasLineInfo() cil managed
  // SIG: 20 00 02
  {
    .override [System.Xml/*23000003*/]System.Xml.IXmlLineInfo/*01000006*/::HasLineInfo /*01000006::0A000004*/ 
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method XObject::System.Xml.IXmlLineInfo.HasLineInfo

  .method /*0600002C*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Xml.IXmlLineInfo.get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    .override [System.Xml/*23000003*/]System.Xml.IXmlLineInfo/*01000006*/::get_LineNumber /*01000006::0A000003*/ 
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method XObject::System.Xml.IXmlLineInfo.get_LineNumber

  .method /*0600002D*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Xml.IXmlLineInfo.get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    .override [System.Xml/*23000003*/]System.Xml.IXmlLineInfo/*01000006*/::get_LinePosition /*01000006::0A000005*/ 
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method XObject::System.Xml.IXmlLineInfo.get_LinePosition

  .event /*14000001*/ class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/>/*1B000002*/ Changed
  {
    .removeon instance void System.Xml.Linq.XObject/*02000004*/::remove_Changed(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/>) /* 06000028 */
    .addon instance void System.Xml.Linq.XObject/*02000004*/::add_Changed(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/>) /* 06000027 */
  } // end of event XObject::Changed
  .event /*14000002*/ class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/>/*1B000002*/ Changing
  {
    .addon instance void System.Xml.Linq.XObject/*02000004*/::add_Changing(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/>) /* 06000029 */
    .removeon instance void System.Xml.Linq.XObject/*02000004*/::remove_Changing(class [mscorlib/*23000002*/]System.EventHandler`1/*01000007*/<class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/>) /* 0600002A */
  } // end of event XObject::Changing
  .property /*17000008*/ instance callconv(8) string
          BaseUri()
  {
    .get instance string System.Xml.Linq.XObject/*02000004*/::get_BaseUri() /* 0600001C */
  } // end of property XObject::BaseUri
  .property /*17000009*/ instance callconv(8) class System.Xml.Linq.XDocument/*02000011*/
          Document()
  {
    .get instance class System.Xml.Linq.XDocument/*02000011*/ System.Xml.Linq.XObject/*02000004*/::get_Document() /* 0600001D */
  } // end of property XObject::Document
  .property /*1700000A*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XObject/*02000004*/::get_NodeType() /* 0600001E */
  } // end of property XObject::NodeType
  .property /*1700000B*/ instance callconv(8) class System.Xml.Linq.XElement/*0200000D*/
          Parent()
  {
    .get instance class System.Xml.Linq.XElement/*0200000D*/ System.Xml.Linq.XObject/*02000004*/::get_Parent() /* 0600001F */
  } // end of property XObject::Parent
  .property /*1700000C*/ instance callconv(8) int32
          System.Xml.IXmlLineInfo.LineNumber()
  {
    .get instance int32 System.Xml.Linq.XObject/*02000004*/::System.Xml.IXmlLineInfo.get_LineNumber() /* 0600002C */
  } // end of property XObject::System.Xml.IXmlLineInfo.LineNumber
  .property /*1700000D*/ instance callconv(8) int32
          System.Xml.IXmlLineInfo.LinePosition()
  {
    .get instance int32 System.Xml.Linq.XObject/*02000004*/::System.Xml.IXmlLineInfo.get_LinePosition() /* 0600002D */
  } // end of property XObject::System.Xml.IXmlLineInfo.LinePosition
} // end of class System.Xml.Linq.XObject

.class /*02000005*/ public auto ansi sealed System.Xml.Linq.XObjectChange
       extends [mscorlib/*23000002*/]System.Enum/*0100000B*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Xml.Linq.XObjectChange/*02000005*/ Add = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Xml.Linq.XObjectChange/*02000005*/ Remove = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Xml.Linq.XObjectChange/*02000005*/ Name = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.Xml.Linq.XObjectChange/*02000005*/ Value = int32(0x00000003)
} // end of class System.Xml.Linq.XObjectChange

.class /*02000006*/ public auto ansi beforefieldinit System.Xml.Linq.XObjectChangeEventArgs
       extends [mscorlib/*23000002*/]System.EventArgs/*0100000C*/
{
  .field /*04000006*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/ Add
  .field /*04000007*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/ Remove
  .field /*04000008*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/ Name
  .field /*04000009*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*02000006*/ Value
  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Xml.Linq.XObjectChange/*02000005*/ objectChange) cil managed
  // SIG: 20 01 01 11 14
  {
    .custom /*0C00002A:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method XObjectChangeEventArgs::.ctor

  .method /*0600002F*/ public hidebysig specialname 
          instance valuetype System.Xml.Linq.XObjectChange/*02000005*/ 
          get_ObjectChange() cil managed
  // SIG: 20 00 11 14
  {
    .custom /*0C00002C:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method XObjectChangeEventArgs::get_ObjectChange

  .method /*06000030*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method XObjectChangeEventArgs::.cctor

  .property /*1700000E*/ instance callconv(8) valuetype System.Xml.Linq.XObjectChange/*02000005*/
          ObjectChange()
  {
    .get instance valuetype System.Xml.Linq.XObjectChange/*02000005*/ System.Xml.Linq.XObjectChangeEventArgs/*02000006*/::get_ObjectChange() /* 0600002F */
  } // end of property XObjectChangeEventArgs::ObjectChange
} // end of class System.Xml.Linq.XObjectChangeEventArgs

.class /*02000007*/ public abstract auto ansi beforefieldinit System.Xml.Linq.XNode
       extends System.Xml.Linq.XObject/*02000004*/
{
  .method /*06000031*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00002D:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method XNode::.ctor

  .method /*06000032*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000007*/ 
          get_NextNode() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method XNode::get_NextNode

  .method /*06000033*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000007*/ 
          get_PreviousNode() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method XNode::get_PreviousNode

  .method /*06000034*/ public hidebysig specialname static 
          class System.Xml.Linq.XNodeDocumentOrderComparer/*02000008*/ 
          get_DocumentOrderComparer() cil managed
  // SIG: 00 00 12 20
  {
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method XNode::get_DocumentOrderComparer

  .method /*06000035*/ public hidebysig specialname static 
          class System.Xml.Linq.XNodeEqualityComparer/*02000009*/ 
          get_EqualityComparer() cil managed
  // SIG: 00 00 12 24
  {
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method XNode::get_EqualityComparer

  .method /*06000036*/ public hidebysig instance void 
          AddAfterSelf(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method XNode::AddAfterSelf

  .method /*06000037*/ public hidebysig instance void 
          AddAfterSelf(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C00002E:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*08000021*/ 
    .custom /*0C000028:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method XNode::AddAfterSelf

  .method /*06000038*/ public hidebysig instance void 
          AddBeforeSelf(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method XNode::AddBeforeSelf

  .method /*06000039*/ public hidebysig instance void 
          AddBeforeSelf(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C00002F:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*08000023*/ 
    .custom /*0C000029:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method XNode::AddBeforeSelf

  .method /*0600003A*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Ancestors() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    .custom /*0C000030:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method XNode::Ancestors

  .method /*0600003B*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Ancestors(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method XNode::Ancestors

  .method /*0600003C*/ public hidebysig static 
          int32  CompareDocumentOrder(class System.Xml.Linq.XNode/*02000007*/ n1,
                                      class System.Xml.Linq.XNode/*02000007*/ n2) cil managed
  // SIG: 00 02 08 12 1C 12 1C
  {
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method XNode::CompareDocumentOrder

  .method /*0600003D*/ public hidebysig instance class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ 
          CreateReader() cil managed
  // SIG: 20 00 12 35
  {
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method XNode::CreateReader

  .method /*0600003E*/ public hidebysig instance class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ 
          CreateReader(valuetype System.Xml.Linq.ReaderOptions/*02000010*/ readerOptions) cil managed
  // SIG: 20 01 12 35 11 40
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method XNode::CreateReader

  .method /*0600003F*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          NodesAfterSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 1C
  {
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method XNode::NodesAfterSelf

  .method /*06000040*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          NodesBeforeSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 1C
  {
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method XNode::NodesBeforeSelf

  .method /*06000041*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          ElementsAfterSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    .custom /*0C000031:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method XNode::ElementsAfterSelf

  .method /*06000042*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          ElementsAfterSelf(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method XNode::ElementsAfterSelf

  .method /*06000043*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          ElementsBeforeSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    .custom /*0C000032:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method XNode::ElementsBeforeSelf

  .method /*06000044*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          ElementsBeforeSelf(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method XNode::ElementsBeforeSelf

  .method /*06000045*/ public hidebysig instance bool 
          IsAfter(class System.Xml.Linq.XNode/*02000007*/ node) cil managed
  // SIG: 20 01 02 12 1C
  {
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method XNode::IsAfter

  .method /*06000046*/ public hidebysig instance bool 
          IsBefore(class System.Xml.Linq.XNode/*02000007*/ node) cil managed
  // SIG: 20 01 02 12 1C
  {
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method XNode::IsBefore

  .method /*06000047*/ public hidebysig static 
          class System.Xml.Linq.XNode/*02000007*/ 
          ReadFrom(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ reader) cil managed
  // SIG: 00 01 12 1C 12 35
  {
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method XNode::ReadFrom

  .method /*06000048*/ public hidebysig instance void 
          Remove() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method XNode::Remove

  .method /*06000049*/ public hidebysig instance void 
          ReplaceWith(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method XNode::ReplaceWith

  .method /*0600004A*/ public hidebysig instance void 
          ReplaceWith(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C000035:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*0800002E*/ 
    .custom /*0C00002B:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method XNode::ReplaceWith

  .method /*0600004B*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method XNode::ToString

  .method /*0600004C*/ public hidebysig instance string 
          ToString(valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 01 0E 11 3C
  {
    .custom /*0C000036:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method XNode::ToString

  .method /*0600004D*/ public hidebysig static 
          bool  DeepEquals(class System.Xml.Linq.XNode/*02000007*/ n1,
                           class System.Xml.Linq.XNode/*02000007*/ n2) cil managed
  // SIG: 00 02 02 12 1C 12 1C
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method XNode::DeepEquals

  .method /*0600004E*/ public hidebysig newslot abstract virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x0
  } // end of method XNode::WriteTo

  .property /*1700000F*/ instance callconv(8) class System.Xml.Linq.XNode/*02000007*/
          NextNode()
  {
    .get instance class System.Xml.Linq.XNode/*02000007*/ System.Xml.Linq.XNode/*02000007*/::get_NextNode() /* 06000032 */
  } // end of property XNode::NextNode
  .property /*17000010*/ instance callconv(8) class System.Xml.Linq.XNode/*02000007*/
          PreviousNode()
  {
    .get instance class System.Xml.Linq.XNode/*02000007*/ System.Xml.Linq.XNode/*02000007*/::get_PreviousNode() /* 06000033 */
  } // end of property XNode::PreviousNode
  .property /*17000011*/ callconv(8) class System.Xml.Linq.XNodeDocumentOrderComparer/*02000008*/
          DocumentOrderComparer()
  {
    .get class System.Xml.Linq.XNodeDocumentOrderComparer/*02000008*/ System.Xml.Linq.XNode/*02000007*/::get_DocumentOrderComparer() /* 06000034 */
  } // end of property XNode::DocumentOrderComparer
  .property /*17000012*/ callconv(8) class System.Xml.Linq.XNodeEqualityComparer/*02000009*/
          EqualityComparer()
  {
    .get class System.Xml.Linq.XNodeEqualityComparer/*02000009*/ System.Xml.Linq.XNode/*02000007*/::get_EqualityComparer() /* 06000035 */
  } // end of property XNode::EqualityComparer
} // end of class System.Xml.Linq.XNode

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XNodeDocumentOrderComparer
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
       implements [mscorlib/*23000002*/]System.Collections.IComparer/*0100000F*/,
                  class [mscorlib/*23000002*/]System.Collections.Generic.IComparer`1/*01000010*/<class System.Xml.Linq.XNode/*02000007*/>/*1B000003*/
{
  .method /*0600004F*/ public hidebysig newslot virtual final 
          instance int32  Compare(class System.Xml.Linq.XNode/*02000007*/ x,
                                  class System.Xml.Linq.XNode/*02000007*/ y) cil managed
  // SIG: 20 02 08 12 1C 12 1C
  {
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method XNodeDocumentOrderComparer::Compare

  .method /*06000050*/ private hidebysig newslot virtual final 
          instance int32  System.Collections.IComparer.Compare(object x,
                                                               object y) cil managed
  // SIG: 20 02 08 1C 1C
  {
    .override [mscorlib/*23000002*/]System.Collections.IComparer/*0100000F*/::Compare /*0100000F::0A000006*/ 
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method XNodeDocumentOrderComparer::System.Collections.IComparer.Compare

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000038:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method XNodeDocumentOrderComparer::.ctor

} // end of class System.Xml.Linq.XNodeDocumentOrderComparer

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XNodeEqualityComparer
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
       implements [mscorlib/*23000002*/]System.Collections.IEqualityComparer/*01000011*/,
                  class [mscorlib/*23000002*/]System.Collections.Generic.IEqualityComparer`1/*01000012*/<class System.Xml.Linq.XNode/*02000007*/>/*1B000004*/
{
  .method /*06000052*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Xml.Linq.XNode/*02000007*/ x,
                                class System.Xml.Linq.XNode/*02000007*/ y) cil managed
  // SIG: 20 02 02 12 1C 12 1C
  {
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method XNodeEqualityComparer::Equals

  .method /*06000053*/ public hidebysig newslot virtual final 
          instance int32  GetHashCode(class System.Xml.Linq.XNode/*02000007*/ obj) cil managed
  // SIG: 20 01 08 12 1C
  {
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method XNodeEqualityComparer::GetHashCode

  .method /*06000054*/ private hidebysig newslot virtual final 
          instance bool  System.Collections.IEqualityComparer.Equals(object x,
                                                                     object y) cil managed
  // SIG: 20 02 02 1C 1C
  {
    .override [mscorlib/*23000002*/]System.Collections.IEqualityComparer/*01000011*/::Equals /*01000011::0A000007*/ 
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method XNodeEqualityComparer::System.Collections.IEqualityComparer.Equals

  .method /*06000055*/ private hidebysig newslot virtual final 
          instance int32  System.Collections.IEqualityComparer.GetHashCode(object obj) cil managed
  // SIG: 20 01 08 1C
  {
    .override [mscorlib/*23000002*/]System.Collections.IEqualityComparer/*01000011*/::GetHashCode /*01000011::0A000008*/ 
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method XNodeEqualityComparer::System.Collections.IEqualityComparer.GetHashCode

  .method /*06000056*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00003A:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method XNodeEqualityComparer::.ctor

} // end of class System.Xml.Linq.XNodeEqualityComparer

.class /*0200000A*/ public auto ansi beforefieldinit System.Xml.Linq.XText
       extends System.Xml.Linq.XNode/*02000007*/
{
  .method /*06000057*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method XText::.ctor

  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XText/*0200000A*/ other) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method XText::.ctor

  .method /*06000059*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method XText::get_NodeType

  .method /*0600005A*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00003B:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method XText::get_Value

  .method /*0600005B*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method XText::set_Value

  .method /*0600005C*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method XText::WriteTo

  .property /*17000013*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XText/*0200000A*/::get_NodeType() /* 06000059 */
  } // end of property XText::NodeType
  .property /*17000014*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XText/*0200000A*/::get_Value() /* 0600005A */
    .set instance void System.Xml.Linq.XText/*0200000A*/::set_Value(string) /* 0600005B */
  } // end of property XText::Value
} // end of class System.Xml.Linq.XText

.class /*0200000B*/ public auto ansi beforefieldinit System.Xml.Linq.XCData
       extends System.Xml.Linq.XText/*0200000A*/
{
  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00003C:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method XCData::.ctor

  .method /*0600005E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XCData/*0200000B*/ other) cil managed
  // SIG: 20 01 01 12 2C
  {
    .custom /*0C00003D:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method XCData::.ctor

  .method /*0600005F*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method XCData::get_NodeType

  .method /*06000060*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method XCData::WriteTo

  .property /*17000015*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XCData/*0200000B*/::get_NodeType() /* 0600005F */
  } // end of property XCData::NodeType
} // end of class System.Xml.Linq.XCData

.class /*0200000C*/ public abstract auto ansi beforefieldinit System.Xml.Linq.XContainer
       extends System.Xml.Linq.XNode/*02000007*/
{
  .method /*06000061*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00003E:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method XContainer::.ctor

  .method /*06000062*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000007*/ 
          get_FirstNode() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method XContainer::get_FirstNode

  .method /*06000063*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000007*/ 
          get_LastNode() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method XContainer::get_LastNode

  .method /*06000064*/ public hidebysig instance void 
          Add(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method XContainer::Add

  .method /*06000065*/ public hidebysig instance void 
          Add(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C00003F:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*08000045*/ 
    .custom /*0C000033:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method XContainer::Add

  .method /*06000066*/ public hidebysig instance void 
          AddFirst(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method XContainer::AddFirst

  .method /*06000067*/ public hidebysig instance void 
          AddFirst(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C000040:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*08000047*/ 
    .custom /*0C000034:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method XContainer::AddFirst

  .method /*06000068*/ public hidebysig instance class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ 
          CreateWriter() cil managed
  // SIG: 20 00 12 39
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method XContainer::CreateWriter

  .method /*06000069*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          DescendantNodes() cil managed
  // SIG: 20 00 15 12 29 01 12 1C
  {
    .custom /*0C000041:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method XContainer::DescendantNodes

  .method /*0600006A*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Descendants() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    .custom /*0C000042:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method XContainer::Descendants

  .method /*0600006B*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Descendants(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x20b8
    // Code size       0 (0x0)
  } // end of method XContainer::Descendants

  .method /*0600006C*/ public hidebysig instance class System.Xml.Linq.XElement/*0200000D*/ 
          Element(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 12 34 12 08
  {
    // Method begins at RVA 0x20b9
    // Code size       0 (0x0)
  } // end of method XContainer::Element

  .method /*0600006D*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Elements() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    .custom /*0C000044:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20ba
    // Code size       0 (0x0)
  } // end of method XContainer::Elements

  .method /*0600006E*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Elements(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x20bb
    // Code size       0 (0x0)
  } // end of method XContainer::Elements

  .method /*0600006F*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          Nodes() cil managed
  // SIG: 20 00 15 12 29 01 12 1C
  {
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method XContainer::Nodes

  .method /*06000070*/ public hidebysig instance void 
          RemoveNodes() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method XContainer::RemoveNodes

  .method /*06000071*/ public hidebysig instance void 
          ReplaceNodes(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method XContainer::ReplaceNodes

  .method /*06000072*/ public hidebysig instance void 
          ReplaceNodes(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C000046:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*0800004C*/ 
    .custom /*0C000037:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method XContainer::ReplaceNodes

  .property /*17000016*/ instance callconv(8) class System.Xml.Linq.XNode/*02000007*/
          FirstNode()
  {
    .get instance class System.Xml.Linq.XNode/*02000007*/ System.Xml.Linq.XContainer/*0200000C*/::get_FirstNode() /* 06000062 */
  } // end of property XContainer::FirstNode
  .property /*17000017*/ instance callconv(8) class System.Xml.Linq.XNode/*02000007*/
          LastNode()
  {
    .get instance class System.Xml.Linq.XNode/*02000007*/ System.Xml.Linq.XContainer/*0200000C*/::get_LastNode() /* 06000063 */
  } // end of property XContainer::LastNode
} // end of class System.Xml.Linq.XContainer

.class /*0200000D*/ public auto ansi beforefieldinit System.Xml.Linq.XElement
       extends System.Xml.Linq.XContainer/*0200000C*/
       implements [System.Xml/*23000003*/]System.Xml.Serialization.IXmlSerializable/*01000013*/
{
  .custom /*0C00001B:0A000026*/ instance void [System/*23000001*/]System.ComponentModel.TypeDescriptionProviderAttribute/*01000044*/::.ctor(class [mscorlib/*23000002*/]System.Type/*01000009*/) /* 0A000026 */ = ( 01 00 58 4D 53 2E 49 6E 74 65 72 6E 61 6C 2E 58   // ..XMS.Internal.X
                                                                                                                                                                                                                    6D 6C 2E 4C 69 6E 71 2E 43 6F 6D 70 6F 6E 65 6E   // ml.Linq.Componen
                                                                                                                                                                                                                    74 4D 6F 64 65 6C 2E 58 54 79 70 65 44 65 73 63   // tModel.XTypeDesc
                                                                                                                                                                                                                    72 69 70 74 69 6F 6E 50 72 6F 76 69 64 65 72 60   // riptionProvider`
                                                                                                                                                                                                                    31 5B 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69 6E   // 1[System.Xml.Lin
                                                                                                                                                                                                                    71 2E 58 45 6C 65 6D 65 6E 74 5D 00 00 )          // q.XElement]..
  .custom /*0C00001C:0A000027*/ instance void [System.Xml/*23000003*/]System.Xml.Serialization.XmlSchemaProviderAttribute/*01000045*/::.ctor(string) /* 0A000027 */ = ( 01 00 FF 01 00 54 02 05 49 73 41 6E 79 01 )       // .....T..IsAny.
  .method /*06000073*/ public hidebysig specialname static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          get_EmptySequence() cil managed
  // SIG: 00 00 15 12 29 01 12 34
  {
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method XElement::get_EmptySequence

  .method /*06000074*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 01 12 08
  {
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method XElement::.ctor

  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name,
                               object content) cil managed
  // SIG: 20 02 01 12 08 1C
  {
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method XElement::.ctor

  .method /*06000076*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name,
                               object[] content) cil managed
  // SIG: 20 02 01 12 08 1D 1C
  {
    .custom /*0C000047:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [2]/*08000051*/ 
    .custom /*0C000039:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method XElement::.ctor

  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XElement/*0200000D*/ other) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method XElement::.ctor

  .method /*06000078*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XStreamingElement/*02000017*/ other) cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method XElement::.ctor

  .method /*06000079*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*02000016*/ 
          get_FirstAttribute() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x20c6
    // Code size       0 (0x0)
  } // end of method XElement::get_FirstAttribute

  .method /*0600007A*/ public hidebysig specialname 
          instance bool  get_HasAttributes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20c7
    // Code size       0 (0x0)
  } // end of method XElement::get_HasAttributes

  .method /*0600007B*/ public hidebysig specialname 
          instance bool  get_HasElements() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method XElement::get_HasElements

  .method /*0600007C*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20c9
    // Code size       0 (0x0)
  } // end of method XElement::get_IsEmpty

  .method /*0600007D*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*02000016*/ 
          get_LastAttribute() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C000048:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20ca
    // Code size       0 (0x0)
  } // end of method XElement::get_LastAttribute

  .method /*0600007E*/ public hidebysig specialname 
          instance class System.Xml.Linq.XName/*02000002*/ 
          get_Name() cil managed
  // SIG: 20 00 12 08
  {
    .custom /*0C000049:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20cb
    // Code size       0 (0x0)
  } // end of method XElement::get_Name

  .method /*0600007F*/ public hidebysig specialname 
          instance void  set_Name(class System.Xml.Linq.XName/*02000002*/ 'value') cil managed
  // SIG: 20 01 01 12 08
  {
    // Method begins at RVA 0x20cc
    // Code size       0 (0x0)
  } // end of method XElement::set_Name

  .method /*06000080*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x20cd
    // Code size       0 (0x0)
  } // end of method XElement::get_NodeType

  .method /*06000081*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20ce
    // Code size       0 (0x0)
  } // end of method XElement::get_Value

  .method /*06000082*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20cf
    // Code size       0 (0x0)
  } // end of method XElement::set_Value

  .method /*06000083*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          AncestorsAndSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    // Method begins at RVA 0x20d0
    // Code size       0 (0x0)
  } // end of method XElement::AncestorsAndSelf

  .method /*06000084*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          AncestorsAndSelf(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x20d1
    // Code size       0 (0x0)
  } // end of method XElement::AncestorsAndSelf

  .method /*06000085*/ public hidebysig instance class System.Xml.Linq.XAttribute/*02000016*/ 
          Attribute(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 12 58 12 08
  {
    // Method begins at RVA 0x20d2
    // Code size       0 (0x0)
  } // end of method XElement::Attribute

  .method /*06000086*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> 
          Attributes() cil managed
  // SIG: 20 00 15 12 29 01 12 58
  {
    .custom /*0C00004A:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20d3
    // Code size       0 (0x0)
  } // end of method XElement::Attributes

  .method /*06000087*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> 
          Attributes(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 58 12 08
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method XElement::Attributes

  .method /*06000088*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          DescendantNodesAndSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 1C
  {
    // Method begins at RVA 0x20d5
    // Code size       0 (0x0)
  } // end of method XElement::DescendantNodesAndSelf

  .method /*06000089*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          DescendantsAndSelf() cil managed
  // SIG: 20 00 15 12 29 01 12 34
  {
    // Method begins at RVA 0x20d6
    // Code size       0 (0x0)
  } // end of method XElement::DescendantsAndSelf

  .method /*0600008A*/ public hidebysig instance class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          DescendantsAndSelf(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 15 12 29 01 12 34 12 08
  {
    // Method begins at RVA 0x20d7
    // Code size       0 (0x0)
  } // end of method XElement::DescendantsAndSelf

  .method /*0600008B*/ public hidebysig instance class System.Xml.Linq.XNamespace/*02000003*/ 
          GetDefaultNamespace() cil managed
  // SIG: 20 00 12 0C
  {
    // Method begins at RVA 0x20d8
    // Code size       0 (0x0)
  } // end of method XElement::GetDefaultNamespace

  .method /*0600008C*/ public hidebysig instance class System.Xml.Linq.XNamespace/*02000003*/ 
          GetNamespaceOfPrefix(string prefix) cil managed
  // SIG: 20 01 12 0C 0E
  {
    // Method begins at RVA 0x20d9
    // Code size       0 (0x0)
  } // end of method XElement::GetNamespaceOfPrefix

  .method /*0600008D*/ public hidebysig instance string 
          GetPrefixOfNamespace(class System.Xml.Linq.XNamespace/*02000003*/ ns) cil managed
  // SIG: 20 01 0E 12 0C
  {
    // Method begins at RVA 0x20da
    // Code size       0 (0x0)
  } // end of method XElement::GetPrefixOfNamespace

  .method /*0600008E*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(string uri) cil managed
  // SIG: 00 01 12 34 0E
  {
    .custom /*0C00004B:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20db
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*0600008F*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(string uri,
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 34 0E 11 38
  {
    // Method begins at RVA 0x20dc
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000090*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 00 01 12 34 12 55
  {
    .custom /*0C00004C:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20dd
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000091*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream',
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 34 12 55 11 38
  {
    // Method begins at RVA 0x20de
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000092*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(class [mscorlib/*23000002*/]System.IO.TextReader/*01000016*/ textReader) cil managed
  // SIG: 00 01 12 34 12 59
  {
    .custom /*0C00004D:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20df
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000093*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(class [mscorlib/*23000002*/]System.IO.TextReader/*01000016*/ textReader,
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 34 12 59 11 38
  {
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000094*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ reader) cil managed
  // SIG: 00 01 12 34 12 35
  {
    .custom /*0C00004E:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000095*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Load(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ reader,
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 34 12 35 11 38
  {
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method XElement::Load

  .method /*06000096*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Parse(string text) cil managed
  // SIG: 00 01 12 34 0E
  {
    .custom /*0C00004F:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method XElement::Parse

  .method /*06000097*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          Parse(string text,
                valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 34 0E 11 38
  {
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method XElement::Parse

  .method /*06000098*/ public hidebysig instance void 
          RemoveAll() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method XElement::RemoveAll

  .method /*06000099*/ public hidebysig instance void 
          RemoveAttributes() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method XElement::RemoveAttributes

  .method /*0600009A*/ public hidebysig instance void 
          ReplaceAll(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method XElement::ReplaceAll

  .method /*0600009B*/ public hidebysig instance void 
          ReplaceAll(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C000051:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*0800006C*/ 
    .custom /*0C000043:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method XElement::ReplaceAll

  .method /*0600009C*/ public hidebysig instance void 
          ReplaceAttributes(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method XElement::ReplaceAttributes

  .method /*0600009D*/ public hidebysig instance void 
          ReplaceAttributes(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C000053:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*0800006E*/ 
    .custom /*0C000045:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method XElement::ReplaceAttributes

  .method /*0600009E*/ public hidebysig instance void 
          Save(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*0600009F*/ public hidebysig instance void 
          Save(string fileName,
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 0E 11 3C
  {
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*060000A0*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*060000A1*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream',
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 12 55 11 3C
  {
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*060000A2*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.TextWriter/*01000017*/ textWriter) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*060000A3*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.TextWriter/*01000017*/ textWriter,
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 12 5D 11 3C
  {
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*060000A4*/ public hidebysig instance void 
          Save(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method XElement::Save

  .method /*060000A5*/ public hidebysig instance void 
          SetAttributeValue(class System.Xml.Linq.XName/*02000002*/ name,
                            object 'value') cil managed
  // SIG: 20 02 01 12 08 1C
  {
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method XElement::SetAttributeValue

  .method /*060000A6*/ public hidebysig instance void 
          SetElementValue(class System.Xml.Linq.XName/*02000002*/ name,
                          object 'value') cil managed
  // SIG: 20 02 01 12 08 1C
  {
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method XElement::SetElementValue

  .method /*060000A7*/ public hidebysig instance void 
          SetValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20f4
    // Code size       0 (0x0)
  } // end of method XElement::SetValue

  .method /*060000A8*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x20f5
    // Code size       0 (0x0)
  } // end of method XElement::WriteTo

  .method /*060000A9*/ public hidebysig specialname static 
          string  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 0E 12 34
  {
    .custom /*0C000054:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f6
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000AA*/ public hidebysig specialname static 
          bool  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 02 12 34
  {
    .custom /*0C000055:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000AB*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<bool> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 02 12 34
  {
    .custom /*0C000056:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000AC*/ public hidebysig specialname static 
          int32  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 08 12 34
  {
    .custom /*0C000057:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f9
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000AD*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<int32> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 08 12 34
  {
    .custom /*0C000058:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000AE*/ public hidebysig specialname static 
          uint32  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 09 12 34
  {
    .custom /*0C000059:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000AF*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<uint32> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 09 12 34
  {
    .custom /*0C00005A:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B0*/ public hidebysig specialname static 
          int64  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 0A 12 34
  {
    .custom /*0C00005B:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B1*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<int64> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 0A 12 34
  {
    .custom /*0C00005C:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B2*/ public hidebysig specialname static 
          uint64  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 0B 12 34
  {
    .custom /*0C00005D:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B3*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<uint64> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 0B 12 34
  {
    .custom /*0C00005E:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B4*/ public hidebysig specialname static 
          float32  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 0C 12 34
  {
    .custom /*0C00005F:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B5*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<float32> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 0C 12 34
  {
    .custom /*0C000060:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B6*/ public hidebysig specialname static 
          float64  op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 0D 12 34
  {
    .custom /*0C000061:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B7*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<float64> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 0D 12 34
  {
    .custom /*0C000062:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B8*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Decimal/*01000019*/ 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 11 65 12 34
  {
    .custom /*0C000063:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000B9*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.Decimal/*01000019*/> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 11 65 12 34
  {
    .custom /*0C000064:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2106
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000BA*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.DateTime/*0100001A*/ 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 11 69 12 34
  {
    .custom /*0C000065:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2107
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000BB*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.DateTime/*0100001A*/> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 11 69 12 34
  {
    .custom /*0C000066:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2108
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000BC*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.DateTimeOffset/*0100001B*/ 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 11 6D 12 34
  {
    .custom /*0C000067:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2109
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000BD*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.DateTimeOffset/*0100001B*/> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 11 6D 12 34
  {
    .custom /*0C000068:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x210a
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000BE*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.TimeSpan/*0100001C*/ 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 11 71 12 34
  {
    .custom /*0C000069:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000BF*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.TimeSpan/*0100001C*/> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 11 71 12 34
  {
    .custom /*0C00006A:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x210c
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000C0*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Guid/*0100001D*/ 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 11 75 12 34
  {
    .custom /*0C00006B:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x210d
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000C1*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.Guid/*0100001D*/> 
          op_Explicit(class System.Xml.Linq.XElement/*0200000D*/ element) cil managed
  // SIG: 00 01 15 11 61 01 11 75 12 34
  {
    .custom /*0C00006C:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x210e
    // Code size       0 (0x0)
  } // end of method XElement::op_Explicit

  .method /*060000C2*/ private hidebysig newslot virtual final 
          instance class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchema/*01000014*/ 
          System.Xml.Serialization.IXmlSerializable.GetSchema() cil managed
  // SIG: 20 00 12 51
  {
    .override [System.Xml/*23000003*/]System.Xml.Serialization.IXmlSerializable/*01000013*/::GetSchema /*01000013::0A000009*/ 
    // Method begins at RVA 0x210f
    // Code size       0 (0x0)
  } // end of method XElement::System.Xml.Serialization.IXmlSerializable.GetSchema

  .method /*060000C3*/ private hidebysig newslot virtual final 
          instance void  System.Xml.Serialization.IXmlSerializable.ReadXml(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ reader) cil managed
  // SIG: 20 01 01 12 35
  {
    .override [System.Xml/*23000003*/]System.Xml.Serialization.IXmlSerializable/*01000013*/::ReadXml /*01000013::0A00000A*/ 
    // Method begins at RVA 0x2110
    // Code size       0 (0x0)
  } // end of method XElement::System.Xml.Serialization.IXmlSerializable.ReadXml

  .method /*060000C4*/ private hidebysig newslot virtual final 
          instance void  System.Xml.Serialization.IXmlSerializable.WriteXml(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    .override [System.Xml/*23000003*/]System.Xml.Serialization.IXmlSerializable/*01000013*/::WriteXml /*01000013::0A00000B*/ 
    // Method begins at RVA 0x2111
    // Code size       0 (0x0)
  } // end of method XElement::System.Xml.Serialization.IXmlSerializable.WriteXml

  .method /*060000C5*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2112
    // Code size       0 (0x0)
  } // end of method XElement::.ctor

  .property /*17000018*/ callconv(8) class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/>
          EmptySequence()
  {
    .get class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> System.Xml.Linq.XElement/*0200000D*/::get_EmptySequence() /* 06000073 */
  } // end of property XElement::EmptySequence
  .property /*17000019*/ instance callconv(8) class System.Xml.Linq.XAttribute/*02000016*/
          FirstAttribute()
  {
    .get instance class System.Xml.Linq.XAttribute/*02000016*/ System.Xml.Linq.XElement/*0200000D*/::get_FirstAttribute() /* 06000079 */
  } // end of property XElement::FirstAttribute
  .property /*1700001A*/ instance callconv(8) bool
          HasAttributes()
  {
    .get instance bool System.Xml.Linq.XElement/*0200000D*/::get_HasAttributes() /* 0600007A */
  } // end of property XElement::HasAttributes
  .property /*1700001B*/ instance callconv(8) bool
          HasElements()
  {
    .get instance bool System.Xml.Linq.XElement/*0200000D*/::get_HasElements() /* 0600007B */
  } // end of property XElement::HasElements
  .property /*1700001C*/ instance callconv(8) bool
          IsEmpty()
  {
    .get instance bool System.Xml.Linq.XElement/*0200000D*/::get_IsEmpty() /* 0600007C */
  } // end of property XElement::IsEmpty
  .property /*1700001D*/ instance callconv(8) class System.Xml.Linq.XAttribute/*02000016*/
          LastAttribute()
  {
    .get instance class System.Xml.Linq.XAttribute/*02000016*/ System.Xml.Linq.XElement/*0200000D*/::get_LastAttribute() /* 0600007D */
  } // end of property XElement::LastAttribute
  .property /*1700001E*/ instance callconv(8) class System.Xml.Linq.XName/*02000002*/
          Name()
  {
    .get instance class System.Xml.Linq.XName/*02000002*/ System.Xml.Linq.XElement/*0200000D*/::get_Name() /* 0600007E */
    .set instance void System.Xml.Linq.XElement/*0200000D*/::set_Name(class System.Xml.Linq.XName/*02000002*/) /* 0600007F */
  } // end of property XElement::Name
  .property /*1700001F*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XElement/*0200000D*/::get_NodeType() /* 06000080 */
  } // end of property XElement::NodeType
  .property /*17000020*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XElement/*0200000D*/::get_Value() /* 06000081 */
    .set instance void System.Xml.Linq.XElement/*0200000D*/::set_Value(string) /* 06000082 */
  } // end of property XElement::Value
} // end of class System.Xml.Linq.XElement

.class /*0200000E*/ public auto ansi sealed System.Xml.Linq.LoadOptions
       extends [mscorlib/*23000002*/]System.Enum/*0100000B*/
{
  .custom /*0C00001E:0A000028*/ instance void [mscorlib/*23000002*/]System.FlagsAttribute/*01000046*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
  .field /*0400000A*/ public specialname rtspecialname int32 value__
  .field /*0400000B*/ public static literal valuetype System.Xml.Linq.LoadOptions/*0200000E*/ None = int32(0x00000000)
  .field /*0400000C*/ public static literal valuetype System.Xml.Linq.LoadOptions/*0200000E*/ PreserveWhitespace = int32(0x00000001)
  .field /*0400000D*/ public static literal valuetype System.Xml.Linq.LoadOptions/*0200000E*/ SetBaseUri = int32(0x00000002)
  .field /*0400000E*/ public static literal valuetype System.Xml.Linq.LoadOptions/*0200000E*/ SetLineInfo = int32(0x00000004)
} // end of class System.Xml.Linq.LoadOptions

.class /*0200000F*/ public auto ansi sealed System.Xml.Linq.SaveOptions
       extends [mscorlib/*23000002*/]System.Enum/*0100000B*/
{
  .custom /*0C00001F:0A000028*/ instance void [mscorlib/*23000002*/]System.FlagsAttribute/*01000046*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
  .field /*0400000F*/ public specialname rtspecialname int32 value__
  .field /*04000010*/ public static literal valuetype System.Xml.Linq.SaveOptions/*0200000F*/ None = int32(0x00000000)
  .field /*04000011*/ public static literal valuetype System.Xml.Linq.SaveOptions/*0200000F*/ DisableFormatting = int32(0x00000001)
  .field /*04000012*/ public static literal valuetype System.Xml.Linq.SaveOptions/*0200000F*/ OmitDuplicateNamespaces = int32(0x00000002)
} // end of class System.Xml.Linq.SaveOptions

.class /*02000010*/ public auto ansi sealed System.Xml.Linq.ReaderOptions
       extends [mscorlib/*23000002*/]System.Enum/*0100000B*/
{
  .custom /*0C000020:0A000028*/ instance void [mscorlib/*23000002*/]System.FlagsAttribute/*01000046*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
  .field /*04000013*/ public specialname rtspecialname int32 value__
  .field /*04000014*/ public static literal valuetype System.Xml.Linq.ReaderOptions/*02000010*/ None = int32(0x00000000)
  .field /*04000015*/ public static literal valuetype System.Xml.Linq.ReaderOptions/*02000010*/ OmitDuplicateNamespaces = int32(0x00000001)
} // end of class System.Xml.Linq.ReaderOptions

.class /*02000011*/ public auto ansi beforefieldinit System.Xml.Linq.XDocument
       extends System.Xml.Linq.XContainer/*0200000C*/
{
  .method /*060000C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00006D:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2113
    // Code size       0 (0x0)
  } // end of method XDocument::.ctor

  .method /*060000C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800009A*/ 
    .custom /*0C000050:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2114
    // Code size       0 (0x0)
  } // end of method XDocument::.ctor

  .method /*060000C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDeclaration/*02000014*/ declaration,
                               object[] content) cil managed
  // SIG: 20 02 01 12 50 1D 1C
  {
    .custom /*0C00006E:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [2]/*0800009C*/ 
    .custom /*0C000052:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2115
    // Code size       0 (0x0)
  } // end of method XDocument::.ctor

  .method /*060000C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDocument/*02000011*/ other) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x2116
    // Code size       0 (0x0)
  } // end of method XDocument::.ctor

  .method /*060000CA*/ public hidebysig specialname 
          instance class System.Xml.Linq.XDeclaration/*02000014*/ 
          get_Declaration() cil managed
  // SIG: 20 00 12 50
  {
    .custom /*0C00006F:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2117
    // Code size       0 (0x0)
  } // end of method XDocument::get_Declaration

  .method /*060000CB*/ public hidebysig specialname 
          instance void  set_Declaration(class System.Xml.Linq.XDeclaration/*02000014*/ 'value') cil managed
  // SIG: 20 01 01 12 50
  {
    .custom /*0C000070:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2118
    // Code size       0 (0x0)
  } // end of method XDocument::set_Declaration

  .method /*060000CC*/ public hidebysig specialname 
          instance class System.Xml.Linq.XDocumentType/*02000015*/ 
          get_DocumentType() cil managed
  // SIG: 20 00 12 54
  {
    .custom /*0C000071:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2119
    // Code size       0 (0x0)
  } // end of method XDocument::get_DocumentType

  .method /*060000CD*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x211a
    // Code size       0 (0x0)
  } // end of method XDocument::get_NodeType

  .method /*060000CE*/ public hidebysig specialname 
          instance class System.Xml.Linq.XElement/*0200000D*/ 
          get_Root() cil managed
  // SIG: 20 00 12 34
  {
    .custom /*0C000072:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x211b
    // Code size       0 (0x0)
  } // end of method XDocument::get_Root

  .method /*060000CF*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(string uri) cil managed
  // SIG: 00 01 12 44 0E
  {
    .custom /*0C000073:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x211c
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D0*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(string uri,
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 44 0E 11 38
  {
    // Method begins at RVA 0x211d
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D1*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 00 01 12 44 12 55
  {
    .custom /*0C000074:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x211e
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D2*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream',
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 44 12 55 11 38
  {
    // Method begins at RVA 0x211f
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D3*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [mscorlib/*23000002*/]System.IO.TextReader/*01000016*/ textReader) cil managed
  // SIG: 00 01 12 44 12 59
  {
    .custom /*0C000075:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2120
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D4*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [mscorlib/*23000002*/]System.IO.TextReader/*01000016*/ textReader,
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 44 12 59 11 38
  {
    // Method begins at RVA 0x2121
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D5*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ reader) cil managed
  // SIG: 00 01 12 44 12 35
  {
    .custom /*0C000076:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2122
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D6*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Xml/*23000003*/]System.Xml.XmlReader/*0100000D*/ reader,
               valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 44 12 35 11 38
  {
    // Method begins at RVA 0x2123
    // Code size       0 (0x0)
  } // end of method XDocument::Load

  .method /*060000D7*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Parse(string text) cil managed
  // SIG: 00 01 12 44 0E
  {
    .custom /*0C000077:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2124
    // Code size       0 (0x0)
  } // end of method XDocument::Parse

  .method /*060000D8*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Parse(string text,
                valuetype System.Xml.Linq.LoadOptions/*0200000E*/ options) cil managed
  // SIG: 00 02 12 44 0E 11 38
  {
    // Method begins at RVA 0x2125
    // Code size       0 (0x0)
  } // end of method XDocument::Parse

  .method /*060000D9*/ public hidebysig instance void 
          Save(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2126
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000DA*/ public hidebysig instance void 
          Save(string fileName,
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 0E 11 3C
  {
    // Method begins at RVA 0x2127
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000DB*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x2128
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000DC*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream',
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 12 55 11 3C
  {
    // Method begins at RVA 0x2129
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000DD*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.TextWriter/*01000017*/ textWriter) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x212a
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000DE*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.TextWriter/*01000017*/ textWriter,
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 12 5D 11 3C
  {
    // Method begins at RVA 0x212b
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000DF*/ public hidebysig instance void 
          Save(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x212c
    // Code size       0 (0x0)
  } // end of method XDocument::Save

  .method /*060000E0*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x212d
    // Code size       0 (0x0)
  } // end of method XDocument::WriteTo

  .property /*17000021*/ instance callconv(8) class System.Xml.Linq.XDeclaration/*02000014*/
          Declaration()
  {
    .get instance class System.Xml.Linq.XDeclaration/*02000014*/ System.Xml.Linq.XDocument/*02000011*/::get_Declaration() /* 060000CA */
    .set instance void System.Xml.Linq.XDocument/*02000011*/::set_Declaration(class System.Xml.Linq.XDeclaration/*02000014*/) /* 060000CB */
  } // end of property XDocument::Declaration
  .property /*17000022*/ instance callconv(8) class System.Xml.Linq.XDocumentType/*02000015*/
          DocumentType()
  {
    .get instance class System.Xml.Linq.XDocumentType/*02000015*/ System.Xml.Linq.XDocument/*02000011*/::get_DocumentType() /* 060000CC */
  } // end of property XDocument::DocumentType
  .property /*17000023*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XDocument/*02000011*/::get_NodeType() /* 060000CD */
  } // end of property XDocument::NodeType
  .property /*17000024*/ instance callconv(8) class System.Xml.Linq.XElement/*0200000D*/
          Root()
  {
    .get instance class System.Xml.Linq.XElement/*0200000D*/ System.Xml.Linq.XDocument/*02000011*/::get_Root() /* 060000CE */
  } // end of property XDocument::Root
} // end of class System.Xml.Linq.XDocument

.class /*02000012*/ public auto ansi beforefieldinit System.Xml.Linq.XComment
       extends System.Xml.Linq.XNode/*02000007*/
{
  .method /*060000E1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x212e
    // Code size       0 (0x0)
  } // end of method XComment::.ctor

  .method /*060000E2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XComment/*02000012*/ other) cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x212f
    // Code size       0 (0x0)
  } // end of method XComment::.ctor

  .method /*060000E3*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x2130
    // Code size       0 (0x0)
  } // end of method XComment::get_NodeType

  .method /*060000E4*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000078:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2131
    // Code size       0 (0x0)
  } // end of method XComment::get_Value

  .method /*060000E5*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2132
    // Code size       0 (0x0)
  } // end of method XComment::set_Value

  .method /*060000E6*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x2133
    // Code size       0 (0x0)
  } // end of method XComment::WriteTo

  .property /*17000025*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XComment/*02000012*/::get_NodeType() /* 060000E3 */
  } // end of property XComment::NodeType
  .property /*17000026*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XComment/*02000012*/::get_Value() /* 060000E4 */
    .set instance void System.Xml.Linq.XComment/*02000012*/::set_Value(string) /* 060000E5 */
  } // end of property XComment::Value
} // end of class System.Xml.Linq.XComment

.class /*02000013*/ public auto ansi beforefieldinit System.Xml.Linq.XProcessingInstruction
       extends System.Xml.Linq.XNode/*02000007*/
{
  .method /*060000E7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string target,
                               string data) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2134
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::.ctor

  .method /*060000E8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XProcessingInstruction/*02000013*/ other) cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x2135
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::.ctor

  .method /*060000E9*/ public hidebysig specialname 
          instance string  get_Data() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000079:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2136
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::get_Data

  .method /*060000EA*/ public hidebysig specialname 
          instance void  set_Data(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2137
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::set_Data

  .method /*060000EB*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x2138
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::get_NodeType

  .method /*060000EC*/ public hidebysig specialname 
          instance string  get_Target() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007A:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2139
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::get_Target

  .method /*060000ED*/ public hidebysig specialname 
          instance void  set_Target(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x213a
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::set_Target

  .method /*060000EE*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x213b
    // Code size       0 (0x0)
  } // end of method XProcessingInstruction::WriteTo

  .property /*17000027*/ instance callconv(8) string
          Data()
  {
    .get instance string System.Xml.Linq.XProcessingInstruction/*02000013*/::get_Data() /* 060000E9 */
    .set instance void System.Xml.Linq.XProcessingInstruction/*02000013*/::set_Data(string) /* 060000EA */
  } // end of property XProcessingInstruction::Data
  .property /*17000028*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XProcessingInstruction/*02000013*/::get_NodeType() /* 060000EB */
  } // end of property XProcessingInstruction::NodeType
  .property /*17000029*/ instance callconv(8) string
          Target()
  {
    .get instance string System.Xml.Linq.XProcessingInstruction/*02000013*/::get_Target() /* 060000EC */
    .set instance void System.Xml.Linq.XProcessingInstruction/*02000013*/::set_Target(string) /* 060000ED */
  } // end of property XProcessingInstruction::Target
} // end of class System.Xml.Linq.XProcessingInstruction

.class /*02000014*/ public auto ansi beforefieldinit System.Xml.Linq.XDeclaration
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .method /*060000EF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string version,
                               string encoding,
                               string standalone) cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    .custom /*0C00007B:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x213c
    // Code size       0 (0x0)
  } // end of method XDeclaration::.ctor

  .method /*060000F0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDeclaration/*02000014*/ other) cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x213d
    // Code size       0 (0x0)
  } // end of method XDeclaration::.ctor

  .method /*060000F1*/ public hidebysig specialname 
          instance string  get_Encoding() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007C:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x213e
    // Code size       0 (0x0)
  } // end of method XDeclaration::get_Encoding

  .method /*060000F2*/ public hidebysig specialname 
          instance void  set_Encoding(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007D:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x213f
    // Code size       0 (0x0)
  } // end of method XDeclaration::set_Encoding

  .method /*060000F3*/ public hidebysig specialname 
          instance string  get_Standalone() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007E:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2140
    // Code size       0 (0x0)
  } // end of method XDeclaration::get_Standalone

  .method /*060000F4*/ public hidebysig specialname 
          instance void  set_Standalone(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007F:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2141
    // Code size       0 (0x0)
  } // end of method XDeclaration::set_Standalone

  .method /*060000F5*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000080:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2142
    // Code size       0 (0x0)
  } // end of method XDeclaration::get_Version

  .method /*060000F6*/ public hidebysig specialname 
          instance void  set_Version(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000081:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2143
    // Code size       0 (0x0)
  } // end of method XDeclaration::set_Version

  .method /*060000F7*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2144
    // Code size       0 (0x0)
  } // end of method XDeclaration::ToString

  .property /*1700002A*/ instance callconv(8) string
          Encoding()
  {
    .set instance void System.Xml.Linq.XDeclaration/*02000014*/::set_Encoding(string) /* 060000F2 */
    .get instance string System.Xml.Linq.XDeclaration/*02000014*/::get_Encoding() /* 060000F1 */
  } // end of property XDeclaration::Encoding
  .property /*1700002B*/ instance callconv(8) string
          Standalone()
  {
    .get instance string System.Xml.Linq.XDeclaration/*02000014*/::get_Standalone() /* 060000F3 */
    .set instance void System.Xml.Linq.XDeclaration/*02000014*/::set_Standalone(string) /* 060000F4 */
  } // end of property XDeclaration::Standalone
  .property /*1700002C*/ instance callconv(8) string
          Version()
  {
    .set instance void System.Xml.Linq.XDeclaration/*02000014*/::set_Version(string) /* 060000F6 */
    .get instance string System.Xml.Linq.XDeclaration/*02000014*/::get_Version() /* 060000F5 */
  } // end of property XDeclaration::Version
} // end of class System.Xml.Linq.XDeclaration

.class /*02000015*/ public auto ansi beforefieldinit System.Xml.Linq.XDocumentType
       extends System.Xml.Linq.XNode/*02000007*/
{
  .method /*060000F8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string publicId,
                               string systemId,
                               string internalSubset) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2145
    // Code size       0 (0x0)
  } // end of method XDocumentType::.ctor

  .method /*060000F9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDocumentType/*02000015*/ other) cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x2146
    // Code size       0 (0x0)
  } // end of method XDocumentType::.ctor

  .method /*060000FA*/ public hidebysig specialname 
          instance string  get_InternalSubset() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000084:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2147
    // Code size       0 (0x0)
  } // end of method XDocumentType::get_InternalSubset

  .method /*060000FB*/ public hidebysig specialname 
          instance void  set_InternalSubset(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2148
    // Code size       0 (0x0)
  } // end of method XDocumentType::set_InternalSubset

  .method /*060000FC*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000085:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2149
    // Code size       0 (0x0)
  } // end of method XDocumentType::get_Name

  .method /*060000FD*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x214a
    // Code size       0 (0x0)
  } // end of method XDocumentType::set_Name

  .method /*060000FE*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x214b
    // Code size       0 (0x0)
  } // end of method XDocumentType::get_NodeType

  .method /*060000FF*/ public hidebysig specialname 
          instance string  get_PublicId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000086:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x214c
    // Code size       0 (0x0)
  } // end of method XDocumentType::get_PublicId

  .method /*06000100*/ public hidebysig specialname 
          instance void  set_PublicId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x214d
    // Code size       0 (0x0)
  } // end of method XDocumentType::set_PublicId

  .method /*06000101*/ public hidebysig specialname 
          instance string  get_SystemId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000087:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x214e
    // Code size       0 (0x0)
  } // end of method XDocumentType::get_SystemId

  .method /*06000102*/ public hidebysig specialname 
          instance void  set_SystemId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x214f
    // Code size       0 (0x0)
  } // end of method XDocumentType::set_SystemId

  .method /*06000103*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x2150
    // Code size       0 (0x0)
  } // end of method XDocumentType::WriteTo

  .property /*1700002D*/ instance callconv(8) string
          InternalSubset()
  {
    .set instance void System.Xml.Linq.XDocumentType/*02000015*/::set_InternalSubset(string) /* 060000FB */
    .get instance string System.Xml.Linq.XDocumentType/*02000015*/::get_InternalSubset() /* 060000FA */
  } // end of property XDocumentType::InternalSubset
  .property /*1700002E*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Xml.Linq.XDocumentType/*02000015*/::get_Name() /* 060000FC */
    .set instance void System.Xml.Linq.XDocumentType/*02000015*/::set_Name(string) /* 060000FD */
  } // end of property XDocumentType::Name
  .property /*1700002F*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XDocumentType/*02000015*/::get_NodeType() /* 060000FE */
  } // end of property XDocumentType::NodeType
  .property /*17000030*/ instance callconv(8) string
          PublicId()
  {
    .get instance string System.Xml.Linq.XDocumentType/*02000015*/::get_PublicId() /* 060000FF */
    .set instance void System.Xml.Linq.XDocumentType/*02000015*/::set_PublicId(string) /* 06000100 */
  } // end of property XDocumentType::PublicId
  .property /*17000031*/ instance callconv(8) string
          SystemId()
  {
    .get instance string System.Xml.Linq.XDocumentType/*02000015*/::get_SystemId() /* 06000101 */
    .set instance void System.Xml.Linq.XDocumentType/*02000015*/::set_SystemId(string) /* 06000102 */
  } // end of property XDocumentType::SystemId
} // end of class System.Xml.Linq.XDocumentType

.class /*02000016*/ public auto ansi beforefieldinit System.Xml.Linq.XAttribute
       extends System.Xml.Linq.XObject/*02000004*/
{
  .custom /*0C000022:0A000026*/ instance void [System/*23000001*/]System.ComponentModel.TypeDescriptionProviderAttribute/*01000044*/::.ctor(class [mscorlib/*23000002*/]System.Type/*01000009*/) /* 0A000026 */ = ( 01 00 5A 4D 53 2E 49 6E 74 65 72 6E 61 6C 2E 58   // ..ZMS.Internal.X
                                                                                                                                                                                                                    6D 6C 2E 4C 69 6E 71 2E 43 6F 6D 70 6F 6E 65 6E   // ml.Linq.Componen
                                                                                                                                                                                                                    74 4D 6F 64 65 6C 2E 58 54 79 70 65 44 65 73 63   // tModel.XTypeDesc
                                                                                                                                                                                                                    72 69 70 74 69 6F 6E 50 72 6F 76 69 64 65 72 60   // riptionProvider`
                                                                                                                                                                                                                    31 5B 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69 6E   // 1[System.Xml.Lin
                                                                                                                                                                                                                    71 2E 58 41 74 74 72 69 62 75 74 65 5D 00 00 )    // q.XAttribute]..
  .method /*06000104*/ public hidebysig specialname static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> 
          get_EmptySequence() cil managed
  // SIG: 00 00 15 12 29 01 12 58
  {
    // Method begins at RVA 0x2151
    // Code size       0 (0x0)
  } // end of method XAttribute::get_EmptySequence

  .method /*06000105*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name,
                               object 'value') cil managed
  // SIG: 20 02 01 12 08 1C
  {
    // Method begins at RVA 0x2152
    // Code size       0 (0x0)
  } // end of method XAttribute::.ctor

  .method /*06000106*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XAttribute/*02000016*/ other) cil managed
  // SIG: 20 01 01 12 58
  {
    // Method begins at RVA 0x2153
    // Code size       0 (0x0)
  } // end of method XAttribute::.ctor

  .method /*06000107*/ public hidebysig specialname 
          instance bool  get_IsNamespaceDeclaration() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2154
    // Code size       0 (0x0)
  } // end of method XAttribute::get_IsNamespaceDeclaration

  .method /*06000108*/ public hidebysig specialname 
          instance class System.Xml.Linq.XName/*02000002*/ 
          get_Name() cil managed
  // SIG: 20 00 12 08
  {
    .custom /*0C000088:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2155
    // Code size       0 (0x0)
  } // end of method XAttribute::get_Name

  .method /*06000109*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*02000016*/ 
          get_NextAttribute() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x2156
    // Code size       0 (0x0)
  } // end of method XAttribute::get_NextAttribute

  .method /*0600010A*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 21
  {
    // Method begins at RVA 0x2157
    // Code size       0 (0x0)
  } // end of method XAttribute::get_NodeType

  .method /*0600010B*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*02000016*/ 
          get_PreviousAttribute() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x2158
    // Code size       0 (0x0)
  } // end of method XAttribute::get_PreviousAttribute

  .method /*0600010C*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000089:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2159
    // Code size       0 (0x0)
  } // end of method XAttribute::get_Value

  .method /*0600010D*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x215a
    // Code size       0 (0x0)
  } // end of method XAttribute::set_Value

  .method /*0600010E*/ public hidebysig instance void 
          Remove() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215b
    // Code size       0 (0x0)
  } // end of method XAttribute::Remove

  .method /*0600010F*/ public hidebysig instance void 
          SetValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x215c
    // Code size       0 (0x0)
  } // end of method XAttribute::SetValue

  .method /*06000110*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x215d
    // Code size       0 (0x0)
  } // end of method XAttribute::ToString

  .method /*06000111*/ public hidebysig specialname static 
          string  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 0E 12 58
  {
    .custom /*0C00008A:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x215e
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000112*/ public hidebysig specialname static 
          bool  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 02 12 58
  {
    .custom /*0C00008B:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000113*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<bool> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 02 12 58
  {
    .custom /*0C00008C:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2160
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000114*/ public hidebysig specialname static 
          int32  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 08 12 58
  {
    .custom /*0C00008D:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2161
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000115*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<int32> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 08 12 58
  {
    .custom /*0C00008E:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000116*/ public hidebysig specialname static 
          uint32  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 09 12 58
  {
    .custom /*0C00008F:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2163
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000117*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<uint32> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 09 12 58
  {
    .custom /*0C000090:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2164
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000118*/ public hidebysig specialname static 
          int64  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 0A 12 58
  {
    .custom /*0C000091:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2165
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000119*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<int64> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 0A 12 58
  {
    .custom /*0C000092:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2166
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*0600011A*/ public hidebysig specialname static 
          uint64  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 0B 12 58
  {
    .custom /*0C000093:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2167
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*0600011B*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<uint64> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 0B 12 58
  {
    .custom /*0C000094:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2168
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*0600011C*/ public hidebysig specialname static 
          float32  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 0C 12 58
  {
    .custom /*0C000095:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2169
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*0600011D*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<float32> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 0C 12 58
  {
    .custom /*0C000096:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216a
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*0600011E*/ public hidebysig specialname static 
          float64  op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 0D 12 58
  {
    .custom /*0C000097:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*0600011F*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<float64> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 0D 12 58
  {
    .custom /*0C000098:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216c
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000120*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Decimal/*01000019*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 11 65 12 58
  {
    .custom /*0C000099:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000121*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.Decimal/*01000019*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 11 65 12 58
  {
    .custom /*0C00009A:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216e
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000122*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.DateTime/*0100001A*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 11 69 12 58
  {
    .custom /*0C00009B:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216f
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000123*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.DateTime/*0100001A*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 11 69 12 58
  {
    .custom /*0C00009C:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000124*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.DateTimeOffset/*0100001B*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 11 6D 12 58
  {
    .custom /*0C00009D:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000125*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.DateTimeOffset/*0100001B*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 11 6D 12 58
  {
    .custom /*0C00009E:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2172
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000126*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.TimeSpan/*0100001C*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 11 71 12 58
  {
    .custom /*0C00009F:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000127*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.TimeSpan/*0100001C*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 11 71 12 58
  {
    .custom /*0C0000A0:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2174
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000128*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Guid/*0100001D*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 11 75 12 58
  {
    .custom /*0C0000A1:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2175
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .method /*06000129*/ public hidebysig specialname static 
          valuetype [mscorlib/*23000002*/]System.Nullable`1/*01000018*/<valuetype [mscorlib/*23000002*/]System.Guid/*0100001D*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*02000016*/ attribute) cil managed
  // SIG: 00 01 15 11 61 01 11 75 12 58
  {
    .custom /*0C0000A2:0A000014*/ instance void [mscorlib/*23000002*/]System.CLSCompliantAttribute/*01000032*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2176
    // Code size       0 (0x0)
  } // end of method XAttribute::op_Explicit

  .property /*17000032*/ callconv(8) class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/>
          EmptySequence()
  {
    .get class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> System.Xml.Linq.XAttribute/*02000016*/::get_EmptySequence() /* 06000104 */
  } // end of property XAttribute::EmptySequence
  .property /*17000033*/ instance callconv(8) bool
          IsNamespaceDeclaration()
  {
    .get instance bool System.Xml.Linq.XAttribute/*02000016*/::get_IsNamespaceDeclaration() /* 06000107 */
  } // end of property XAttribute::IsNamespaceDeclaration
  .property /*17000034*/ instance callconv(8) class System.Xml.Linq.XName/*02000002*/
          Name()
  {
    .get instance class System.Xml.Linq.XName/*02000002*/ System.Xml.Linq.XAttribute/*02000016*/::get_Name() /* 06000108 */
  } // end of property XAttribute::Name
  .property /*17000035*/ instance callconv(8) class System.Xml.Linq.XAttribute/*02000016*/
          NextAttribute()
  {
    .get instance class System.Xml.Linq.XAttribute/*02000016*/ System.Xml.Linq.XAttribute/*02000016*/::get_NextAttribute() /* 06000109 */
  } // end of property XAttribute::NextAttribute
  .property /*17000036*/ instance callconv(8) valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000003*/]System.Xml.XmlNodeType/*01000008*/ System.Xml.Linq.XAttribute/*02000016*/::get_NodeType() /* 0600010A */
  } // end of property XAttribute::NodeType
  .property /*17000037*/ instance callconv(8) class System.Xml.Linq.XAttribute/*02000016*/
          PreviousAttribute()
  {
    .get instance class System.Xml.Linq.XAttribute/*02000016*/ System.Xml.Linq.XAttribute/*02000016*/::get_PreviousAttribute() /* 0600010B */
  } // end of property XAttribute::PreviousAttribute
  .property /*17000038*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XAttribute/*02000016*/::get_Value() /* 0600010C */
    .set instance void System.Xml.Linq.XAttribute/*02000016*/::set_Value(string) /* 0600010D */
  } // end of property XAttribute::Value
} // end of class System.Xml.Linq.XAttribute

.class /*02000017*/ public auto ansi beforefieldinit System.Xml.Linq.XStreamingElement
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .method /*0600012A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 20 01 01 12 08
  {
    // Method begins at RVA 0x2177
    // Code size       0 (0x0)
  } // end of method XStreamingElement::.ctor

  .method /*0600012B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name,
                               object content) cil managed
  // SIG: 20 02 01 12 08 1C
  {
    // Method begins at RVA 0x2178
    // Code size       0 (0x0)
  } // end of method XStreamingElement::.ctor

  .method /*0600012C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000002*/ name,
                               object[] content) cil managed
  // SIG: 20 02 01 12 08 1D 1C
  {
    .custom /*0C0000A3:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [2]/*080000F6*/ 
    .custom /*0C000082:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       0 (0x0)
  } // end of method XStreamingElement::.ctor

  .method /*0600012D*/ public hidebysig specialname 
          instance class System.Xml.Linq.XName/*02000002*/ 
          get_Name() cil managed
  // SIG: 20 00 12 08
  {
    .custom /*0C0000A4:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x217a
    // Code size       0 (0x0)
  } // end of method XStreamingElement::get_Name

  .method /*0600012E*/ public hidebysig specialname 
          instance void  set_Name(class System.Xml.Linq.XName/*02000002*/ 'value') cil managed
  // SIG: 20 01 01 12 08
  {
    // Method begins at RVA 0x217b
    // Code size       0 (0x0)
  } // end of method XStreamingElement::set_Name

  .method /*0600012F*/ public hidebysig instance void 
          Add(object content) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x217c
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Add

  .method /*06000130*/ public hidebysig instance void 
          Add(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .custom /*0C0000A5:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [1]/*080000F9*/ 
    .custom /*0C000083:0A000025*/ instance void [mscorlib/*23000002*/]System.ParamArrayAttribute/*01000043*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x217d
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Add

  .method /*06000131*/ public hidebysig instance void 
          Save(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A6:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x217e
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000132*/ public hidebysig instance void 
          Save(string fileName,
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 0E 11 3C
  {
    // Method begins at RVA 0x217f
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000133*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 01 01 12 55
  {
    .custom /*0C0000A7:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2180
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000134*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.Stream/*01000015*/ 'stream',
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 12 55 11 3C
  {
    // Method begins at RVA 0x2181
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000135*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.TextWriter/*01000017*/ textWriter) cil managed
  // SIG: 20 01 01 12 5D
  {
    .custom /*0C0000A8:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2182
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000136*/ public hidebysig instance void 
          Save(class [mscorlib/*23000002*/]System.IO.TextWriter/*01000017*/ textWriter,
               valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 02 01 12 5D 11 3C
  {
    // Method begins at RVA 0x2183
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000137*/ public hidebysig instance void 
          Save(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x2184
    // Code size       0 (0x0)
  } // end of method XStreamingElement::Save

  .method /*06000138*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A9:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2185
    // Code size       0 (0x0)
  } // end of method XStreamingElement::ToString

  .method /*06000139*/ public hidebysig instance string 
          ToString(valuetype System.Xml.Linq.SaveOptions/*0200000F*/ options) cil managed
  // SIG: 20 01 0E 11 3C
  {
    .custom /*0C0000AA:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2186
    // Code size       0 (0x0)
  } // end of method XStreamingElement::ToString

  .method /*0600013A*/ public hidebysig instance void 
          WriteTo(class [System.Xml/*23000003*/]System.Xml.XmlWriter/*0100000E*/ writer) cil managed
  // SIG: 20 01 01 12 39
  {
    // Method begins at RVA 0x2187
    // Code size       0 (0x0)
  } // end of method XStreamingElement::WriteTo

  .property /*17000039*/ instance callconv(8) class System.Xml.Linq.XName/*02000002*/
          Name()
  {
    .get instance class System.Xml.Linq.XName/*02000002*/ System.Xml.Linq.XStreamingElement/*02000017*/::get_Name() /* 0600012D */
    .set instance void System.Xml.Linq.XStreamingElement/*02000017*/::set_Name(class System.Xml.Linq.XName/*02000002*/) /* 0600012E */
  } // end of property XStreamingElement::Name
} // end of class System.Xml.Linq.XStreamingElement

.class /*02000018*/ public abstract auto ansi sealed beforefieldinit System.Xml.Linq.Extensions
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .custom /*0C000023:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
  .method /*0600013B*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> 
          Attributes(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source) cil managed
  // SIG: 00 01 15 12 29 01 12 58 15 12 29 01 12 34
  {
    .custom /*0C0000AB:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2188
    // Code size       0 (0x0)
  } // end of method Extensions::Attributes

  .method /*0600013C*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> 
          Attributes(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source,
                     class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 00 02 15 12 29 01 12 58 15 12 29 01 12 34 12 08
  {
    .custom /*0C0000AC:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2189
    // Code size       0 (0x0)
  } // end of method Extensions::Attributes

  .method /*0600013D*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Ancestors<(System.Xml.Linq.XNode/*02000007*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 29 01 12 34 15 12 29 01 1E 00
  {
    .custom /*0C0000AD:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218a
    // Code size       0 (0x0)
  } // end of method Extensions::Ancestors

  .method /*0600013E*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Ancestors<(System.Xml.Linq.XNode/*02000007*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source,
                                                           class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 10 01 02 15 12 29 01 12 34 15 12 29 01 1E 00 12 08
  {
    .custom /*0C0000AE:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218b
    // Code size       0 (0x0)
  } // end of method Extensions::Ancestors

  .method /*0600013F*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          AncestorsAndSelf(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source) cil managed
  // SIG: 00 01 15 12 29 01 12 34 15 12 29 01 12 34
  {
    .custom /*0C0000AF:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218c
    // Code size       0 (0x0)
  } // end of method Extensions::AncestorsAndSelf

  .method /*06000140*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          AncestorsAndSelf(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source,
                           class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 00 02 15 12 29 01 12 34 15 12 29 01 12 34 12 08
  {
    .custom /*0C0000B0:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218d
    // Code size       0 (0x0)
  } // end of method Extensions::AncestorsAndSelf

  .method /*06000141*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          Nodes<(System.Xml.Linq.XContainer/*0200000C*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 29 01 12 1C 15 12 29 01 1E 00
  {
    .custom /*0C0000B1:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218e
    // Code size       0 (0x0)
  } // end of method Extensions::Nodes

  .method /*06000142*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          DescendantNodes<(System.Xml.Linq.XContainer/*0200000C*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 29 01 12 1C 15 12 29 01 1E 00
  {
    .custom /*0C0000B2:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       0 (0x0)
  } // end of method Extensions::DescendantNodes

  .method /*06000143*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Descendants<(System.Xml.Linq.XContainer/*0200000C*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 29 01 12 34 15 12 29 01 1E 00
  {
    .custom /*0C0000B3:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2190
    // Code size       0 (0x0)
  } // end of method Extensions::Descendants

  .method /*06000144*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Descendants<(System.Xml.Linq.XContainer/*0200000C*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source,
                                                                  class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 10 01 02 15 12 29 01 12 34 15 12 29 01 1E 00 12 08
  {
    .custom /*0C0000B4:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2191
    // Code size       0 (0x0)
  } // end of method Extensions::Descendants

  .method /*06000145*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XNode/*02000007*/> 
          DescendantNodesAndSelf(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source) cil managed
  // SIG: 00 01 15 12 29 01 12 1C 15 12 29 01 12 34
  {
    .custom /*0C0000B5:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2192
    // Code size       0 (0x0)
  } // end of method Extensions::DescendantNodesAndSelf

  .method /*06000146*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          DescendantsAndSelf(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source) cil managed
  // SIG: 00 01 15 12 29 01 12 34 15 12 29 01 12 34
  {
    .custom /*0C0000B6:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2193
    // Code size       0 (0x0)
  } // end of method Extensions::DescendantsAndSelf

  .method /*06000147*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          DescendantsAndSelf(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> source,
                             class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 00 02 15 12 29 01 12 34 15 12 29 01 12 34 12 08
  {
    .custom /*0C0000B7:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2194
    // Code size       0 (0x0)
  } // end of method Extensions::DescendantsAndSelf

  .method /*06000148*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Elements<(System.Xml.Linq.XContainer/*0200000C*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 29 01 12 34 15 12 29 01 1E 00
  {
    .custom /*0C0000B8:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2195
    // Code size       0 (0x0)
  } // end of method Extensions::Elements

  .method /*06000149*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          Elements<(System.Xml.Linq.XContainer/*0200000C*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source,
                                                               class System.Xml.Linq.XName/*02000002*/ name) cil managed
  // SIG: 10 01 02 15 12 29 01 12 34 15 12 29 01 1E 00 12 08
  {
    .custom /*0C0000B9:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2196
    // Code size       0 (0x0)
  } // end of method Extensions::Elements

  .method /*0600014A*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> 
          InDocumentOrder<(System.Xml.Linq.XNode/*02000007*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 29 01 1E 00 15 12 29 01 1E 00
  {
    .custom /*0C0000BA:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2197
    // Code size       0 (0x0)
  } // end of method Extensions::InDocumentOrder

  .method /*0600014B*/ public hidebysig static 
          void  Remove(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XAttribute/*02000016*/> source) cil managed
  // SIG: 00 01 01 15 12 29 01 12 58
  {
    .custom /*0C0000BB:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2198
    // Code size       0 (0x0)
  } // end of method Extensions::Remove

  .method /*0600014C*/ public hidebysig static 
          void  Remove<(System.Xml.Linq.XNode/*02000007*/) T>(class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<!!T> source) cil managed
  // SIG: 10 01 01 01 15 12 29 01 1E 00
  {
    .custom /*0C0000BC:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2199
    // Code size       0 (0x0)
  } // end of method Extensions::Remove

} // end of class System.Xml.Linq.Extensions

.class /*02000019*/ public abstract auto ansi sealed beforefieldinit System.Xml.XPath.Extensions
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .custom /*0C000024:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
  .method /*0600014D*/ public hidebysig static 
          class [System.Xml/*23000003*/]System.Xml.XPath.XPathNavigator/*0100001E*/ 
          CreateNavigator(class System.Xml.Linq.XNode/*02000007*/ node) cil managed
  // SIG: 00 01 12 79 12 1C
  {
    .custom /*0C0000BD:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000BE:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x219a
    // Code size       0 (0x0)
  } // end of method Extensions::CreateNavigator

  .method /*0600014E*/ public hidebysig static 
          class [System.Xml/*23000003*/]System.Xml.XPath.XPathNavigator/*0100001E*/ 
          CreateNavigator(class System.Xml.Linq.XNode/*02000007*/ node,
                          class [System.Xml/*23000003*/]System.Xml.XmlNameTable/*0100001F*/ nameTable) cil managed
  // SIG: 00 02 12 79 12 1C 12 7D
  {
    .custom /*0C0000BF:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219b
    // Code size       0 (0x0)
  } // end of method Extensions::CreateNavigator

  .method /*0600014F*/ public hidebysig static 
          object  XPathEvaluate(class System.Xml.Linq.XNode/*02000007*/ node,
                                string expression) cil managed
  // SIG: 00 02 1C 12 1C 0E
  {
    .custom /*0C0000C0:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000C1:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x219c
    // Code size       0 (0x0)
  } // end of method Extensions::XPathEvaluate

  .method /*06000150*/ public hidebysig static 
          object  XPathEvaluate(class System.Xml.Linq.XNode/*02000007*/ node,
                                string expression,
                                class [System.Xml/*23000003*/]System.Xml.IXmlNamespaceResolver/*01000020*/ resolver) cil managed
  // SIG: 00 03 1C 12 1C 0E 12 80 81
  {
    .custom /*0C0000C2:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219d
    // Code size       0 (0x0)
  } // end of method Extensions::XPathEvaluate

  .method /*06000151*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          XPathSelectElement(class System.Xml.Linq.XNode/*02000007*/ node,
                             string expression) cil managed
  // SIG: 00 02 12 34 12 1C 0E
  {
    .custom /*0C0000C3:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000C4:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x219e
    // Code size       0 (0x0)
  } // end of method Extensions::XPathSelectElement

  .method /*06000152*/ public hidebysig static 
          class System.Xml.Linq.XElement/*0200000D*/ 
          XPathSelectElement(class System.Xml.Linq.XNode/*02000007*/ node,
                             string expression,
                             class [System.Xml/*23000003*/]System.Xml.IXmlNamespaceResolver/*01000020*/ resolver) cil managed
  // SIG: 00 03 12 34 12 1C 0E 12 80 81
  {
    .custom /*0C0000C5:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219f
    // Code size       0 (0x0)
  } // end of method Extensions::XPathSelectElement

  .method /*06000153*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          XPathSelectElements(class System.Xml.Linq.XNode/*02000007*/ node,
                              string expression) cil managed
  // SIG: 00 02 15 12 29 01 12 34 12 1C 0E
  {
    .custom /*0C0000C6:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000C7:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x21a0
    // Code size       0 (0x0)
  } // end of method Extensions::XPathSelectElements

  .method /*06000154*/ public hidebysig static 
          class [mscorlib/*23000002*/]System.Collections.Generic.IEnumerable`1/*0100000A*/<class System.Xml.Linq.XElement/*0200000D*/> 
          XPathSelectElements(class System.Xml.Linq.XNode/*02000007*/ node,
                              string expression,
                              class [System.Xml/*23000003*/]System.Xml.IXmlNamespaceResolver/*01000020*/ resolver) cil managed
  // SIG: 00 03 15 12 29 01 12 34 12 1C 0E 12 80 81
  {
    .custom /*0C0000C8:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a1
    // Code size       0 (0x0)
  } // end of method Extensions::XPathSelectElements

} // end of class System.Xml.XPath.Extensions

.class /*0200001A*/ public abstract auto ansi sealed beforefieldinit System.Xml.Schema.Extensions
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .custom /*0C000025:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
  .method /*06000155*/ public hidebysig static 
          class [System.Xml/*23000003*/]System.Xml.Schema.IXmlSchemaInfo/*01000021*/ 
          GetSchemaInfo(class System.Xml.Linq.XElement/*0200000D*/ source) cil managed
  // SIG: 00 01 12 80 85 12 34
  {
    .custom /*0C0000C9:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       0 (0x0)
  } // end of method Extensions::GetSchemaInfo

  .method /*06000156*/ public hidebysig static 
          class [System.Xml/*23000003*/]System.Xml.Schema.IXmlSchemaInfo/*01000021*/ 
          GetSchemaInfo(class System.Xml.Linq.XAttribute/*02000016*/ source) cil managed
  // SIG: 00 01 12 80 85 12 58
  {
    .custom /*0C0000CA:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a3
    // Code size       0 (0x0)
  } // end of method Extensions::GetSchemaInfo

  .method /*06000157*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XDocument/*02000011*/ source,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaSet/*01000022*/ schemas,
                         class [System.Xml/*23000003*/]System.Xml.Schema.ValidationEventHandler/*01000023*/ validationEventHandler) cil managed
  // SIG: 00 03 01 12 44 12 80 89 12 80 8D
  {
    .custom /*0C0000CB:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000CC:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x21a4
    // Code size       0 (0x0)
  } // end of method Extensions::Validate

  .method /*06000158*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XDocument/*02000011*/ source,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaSet/*01000022*/ schemas,
                         class [System.Xml/*23000003*/]System.Xml.Schema.ValidationEventHandler/*01000023*/ validationEventHandler,
                         bool addSchemaInfo) cil managed
  // SIG: 00 04 01 12 44 12 80 89 12 80 8D 02
  {
    .custom /*0C0000CD:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a5
    // Code size       0 (0x0)
  } // end of method Extensions::Validate

  .method /*06000159*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XElement/*0200000D*/ source,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaObject/*01000024*/ partialValidationType,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaSet/*01000022*/ schemas,
                         class [System.Xml/*23000003*/]System.Xml.Schema.ValidationEventHandler/*01000023*/ validationEventHandler) cil managed
  // SIG: 00 04 01 12 34 12 80 91 12 80 89 12 80 8D
  {
    .custom /*0C0000CE:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000CF:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x21a6
    // Code size       0 (0x0)
  } // end of method Extensions::Validate

  .method /*0600015A*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XElement/*0200000D*/ source,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaObject/*01000024*/ partialValidationType,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaSet/*01000022*/ schemas,
                         class [System.Xml/*23000003*/]System.Xml.Schema.ValidationEventHandler/*01000023*/ validationEventHandler,
                         bool addSchemaInfo) cil managed
  // SIG: 00 05 01 12 34 12 80 91 12 80 89 12 80 8D 02
  {
    .custom /*0C0000D0:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a7
    // Code size       0 (0x0)
  } // end of method Extensions::Validate

  .method /*0600015B*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XAttribute/*02000016*/ source,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaObject/*01000024*/ partialValidationType,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaSet/*01000022*/ schemas,
                         class [System.Xml/*23000003*/]System.Xml.Schema.ValidationEventHandler/*01000023*/ validationEventHandler) cil managed
  // SIG: 00 04 01 12 58 12 80 91 12 80 89 12 80 8D
  {
    .custom /*0C0000D1:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    .custom /*0C0000D2:0A000024*/ instance void [mscorlib/*23000002*/]System.Runtime.TargetedPatchingOptOutAttribute/*01000042*/::.ctor(string) /* 0A000024 */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x21a8
    // Code size       0 (0x0)
  } // end of method Extensions::Validate

  .method /*0600015C*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XAttribute/*02000016*/ source,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaObject/*01000024*/ partialValidationType,
                         class [System.Xml/*23000003*/]System.Xml.Schema.XmlSchemaSet/*01000022*/ schemas,
                         class [System.Xml/*23000003*/]System.Xml.Schema.ValidationEventHandler/*01000023*/ validationEventHandler,
                         bool addSchemaInfo) cil managed
  // SIG: 00 05 01 12 58 12 80 91 12 80 89 12 80 8D 02
  {
    .custom /*0C0000D3:0A00000D*/ instance void [System.Core/*23000004*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000029*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a9
    // Code size       0 (0x0)
  } // end of method Extensions::Validate

} // end of class System.Xml.Schema.Extensions

.class /*0200001B*/ private auto ansi beforefieldinit MS.Internal.Xml.Linq.ComponentModel.XTypeDescriptionProvider`1<T>
       extends [System/*23000001*/]System.ComponentModel.TypeDescriptionProvider/*01000025*/
{
  .method /*0600015D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21aa
    // Code size       0 (0x0)
  } // end of method XTypeDescriptionProvider`1::.ctor

  .method /*0600015E*/ public hidebysig virtual 
          instance class [System/*23000001*/]System.ComponentModel.ICustomTypeDescriptor/*01000026*/ 
          GetTypeDescriptor(class [mscorlib/*23000002*/]System.Type/*01000009*/ 'type',
                            object 'instance') cil managed
  // SIG: 20 02 12 80 99 12 25 1C
  {
    // Method begins at RVA 0x21ab
    // Code size       0 (0x0)
  } // end of method XTypeDescriptionProvider`1::GetTypeDescriptor

} // end of class MS.Internal.Xml.Linq.ComponentModel.XTypeDescriptionProvider`1

.class /*0200001C*/ private auto ansi serializable sealed beforefieldinit System.Xml.Linq.NameSerializer
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
       implements [mscorlib/*23000002*/]System.Runtime.Serialization.IObjectReference/*01000027*/,
                  [mscorlib/*23000002*/]System.Runtime.Serialization.ISerializable/*01000002*/
{
  .method /*0600015F*/ private hidebysig newslot virtual final 
          instance object  System.Runtime.Serialization.IObjectReference.GetRealObject(valuetype [mscorlib/*23000002*/]System.Runtime.Serialization.StreamingContext/*01000004*/ context) cil managed
  // SIG: 20 01 1C 11 11
  {
    .override [mscorlib/*23000002*/]System.Runtime.Serialization.IObjectReference/*01000027*/::GetRealObject /*01000027::0A00000C*/ 
    // Method begins at RVA 0x21ac
    // Code size       0 (0x0)
  } // end of method NameSerializer::System.Runtime.Serialization.IObjectReference.GetRealObject

  .method /*06000160*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [mscorlib/*23000002*/]System.Runtime.Serialization.SerializationInfo/*01000003*/ info,
                                                                                  valuetype [mscorlib/*23000002*/]System.Runtime.Serialization.StreamingContext/*01000004*/ context) cil managed
  // SIG: 20 02 01 12 0D 11 11
  {
    .override [mscorlib/*23000002*/]System.Runtime.Serialization.ISerializable/*01000002*/::GetObjectData /*01000002::0A000001*/ 
    // Method begins at RVA 0x21ad
    // Code size       0 (0x0)
  } // end of method NameSerializer::System.Runtime.Serialization.ISerializable.GetObjectData

} // end of class System.Xml.Linq.NameSerializer

.class /*0200001D*/ private abstract auto ansi sealed beforefieldinit FXAssembly
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .field /*04000016*/ static assembly literal string Version = "4.0.0.0"
} // end of class FXAssembly

.class /*0200001E*/ private abstract auto ansi sealed beforefieldinit ThisAssembly
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .field /*04000017*/ static assembly literal string Title = "System.Xml.Linq.dll"
  .field /*04000018*/ static assembly literal string Description = "System.Xml.Linq.dll"
  .field /*04000019*/ static assembly literal string DefaultAlias = "System.Xml.Linq.dll"
  .field /*0400001A*/ static assembly literal string Copyright = bytearray (A9 00 20 00 4D 00 69 00 63 00 72 00 6F 00 73 00   // .. .M.i.c.r.o.s.
                                                                            6F 00 66 00 74 00 20 00 43 00 6F 00 72 00 70 00   // o.f.t. .C.o.r.p.
                                                                            6F 00 72 00 61 00 74 00 69 00 6F 00 6E 00 2E 00   // o.r.a.t.i.o.n...
                                                                            20 00 20 00 41 00 6C 00 6C 00 20 00 72 00 69 00   //  . .A.l.l. .r.i.
                                                                            67 00 68 00 74 00 73 00 20 00 72 00 65 00 73 00   // g.h.t.s. .r.e.s.
                                                                            65 00 72 00 76 00 65 00 64 00 2E 00 )             // e.r.v.e.d...
  .field /*0400001B*/ static assembly literal string Version = "4.0.0.0"
  .field /*0400001C*/ static assembly literal string InformationalVersion = "4.0.30319.1"
  .field /*0400001D*/ static assembly literal string DailyBuildNumberStr = "30319"
  .field /*0400001E*/ static assembly literal string BuildRevisionStr = "1"
  .field /*0400001F*/ static assembly literal int32 DailyBuildNumber = int32(0x0000766F)
} // end of class ThisAssembly

.class /*0200001F*/ private abstract auto ansi sealed beforefieldinit AssemblyRef
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .field /*04000020*/ static assembly literal string EcmaPublicKey = "b77a5c561934e089"
  .field /*04000021*/ static assembly literal string EcmaPublicKeyToken = "b77a5c561934e089"
  .field /*04000022*/ static assembly literal string EcmaPublicKeyFull = "00000000000000000400000000000000"
  .field /*04000023*/ static assembly literal string SilverlightPublicKey = "31bf3856ad364e35"
  .field /*04000024*/ static assembly literal string SilverlightPublicKeyToken = "31bf3856ad364e35"
  .field /*04000025*/ static assembly literal string SilverlightPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "04000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA"
  + "31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC365"
  + "2BF5C308055DA9"
  .field /*04000026*/ static assembly literal string SilverlightPlatformPublicKey = "7cec85d7bea7798e"
  .field /*04000027*/ static assembly literal string SilverlightPlatformPublicKeyToken = "7cec85d7bea7798e"
  .field /*04000028*/ static assembly literal string SilverlightPlatformPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "040000010001008D56C76F9E8649383049F383C44BE0EC204181822A6C31CF5EB7EF486944D032188EA1D3920763712CCB12D75FB77E9811149E6148E5D32FBAAB37611C"
  + "1878DDC19E20EF135D0CB2CFF2BFEC3D115810C3D9069638FE4BE215DBF795861920E5AB6F7DB2E2CEEF136AC23D5DD2BF031700AEC232F6C6B1C785B4305C123B37AB"
  .field /*04000029*/ static assembly literal string PlatformPublicKey = "b77a5c561934e089"
  .field /*0400002A*/ static assembly literal string PlatformPublicKeyToken = "b77a5c561934e089"
  .field /*0400002B*/ static assembly literal string PlatformPublicKeyFull = "00000000000000000400000000000000"
  .field /*0400002C*/ static assembly literal string Mscorlib = "mscorlib, Version=4.0.0.0, Culture=neutral, Public"
  + "KeyToken=b77a5c561934e089"
  .field /*0400002D*/ static assembly literal string SystemData = "System.Data, Version=4.0.0.0, Culture=neutral, Pub"
  + "licKeyToken=b77a5c561934e089"
  .field /*0400002E*/ static assembly literal string SystemDataOracleClient = "System.Data.OracleClient, Version=4.0.0.0, Culture"
  + "=neutral, PublicKeyToken=b77a5c561934e089"
  .field /*0400002F*/ static assembly literal string System = "System, Version=4.0.0.0, Culture=neutral, PublicKe"
  + "yToken=b77a5c561934e089"
  .field /*04000030*/ static assembly literal string SystemCore = "System.Core, Version=4.0.0.0, Culture=neutral, Pub"
  + "licKeyToken=b77a5c561934e089"
  .field /*04000031*/ static assembly literal string SystemNumerics = "System.Numerics, Version=4.0.0.0, Culture=neutral,"
  + " PublicKeyToken=b77a5c561934e089"
  .field /*04000032*/ static assembly literal string SystemRuntimeRemoting = "System.Runtime.Remoting, Version=4.0.0.0, Culture="
  + "neutral, PublicKeyToken=b77a5c561934e089"
  .field /*04000033*/ static assembly literal string SystemWindowsForms = "System.Windows.Forms, Version=4.0.0.0, Culture=neu"
  + "tral, PublicKeyToken=b77a5c561934e089"
  .field /*04000034*/ static assembly literal string SystemXml = "System.Xml, Version=4.0.0.0, Culture=neutral, Publ"
  + "icKeyToken=b77a5c561934e089"
  .field /*04000035*/ static assembly literal string MicrosoftPublicKey = "b03f5f7f11d50a3a"
  .field /*04000036*/ static assembly literal string MicrosoftPublicKeyToken = "b03f5f7f11d50a3a"
  .field /*04000037*/ static assembly literal string MicrosoftPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "0400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4"
  + "E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518"
  + "206DC093344D5AD293"
  .field /*04000038*/ static assembly literal string SharedLibPublicKey = "31bf3856ad364e35"
  .field /*04000039*/ static assembly literal string SharedLibPublicKeyToken = "31bf3856ad364e35"
  .field /*0400003A*/ static assembly literal string SharedLibPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "04000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718"
  + "EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247AD"
  + "C3652BF5C308055DA9"
  .field /*0400003B*/ static assembly literal string SystemComponentModelDataAnnotations = "System.ComponentModel.DataAnnotations, Version=4.0"
  + ".0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400003C*/ static assembly literal string SystemConfiguration = "System.Configuration, Version=4.0.0.0, Culture=neu"
  + "tral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400003D*/ static assembly literal string SystemConfigurationInstall = "System.Configuration.Install, Version=4.0.0.0, Cul"
  + "ture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400003E*/ static assembly literal string SystemDeployment = "System.Deployment, Version=4.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400003F*/ static assembly literal string SystemDesign = "System.Design, Version=4.0.0.0, Culture=neutral, P"
  + "ublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000040*/ static assembly literal string SystemDirectoryServices = "System.DirectoryServices, Version=4.0.0.0, Culture"
  + "=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000041*/ static assembly literal string SystemDrawingDesign = "System.Drawing.Design, Version=4.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000042*/ static assembly literal string SystemDrawing = "System.Drawing, Version=4.0.0.0, Culture=neutral, "
  + "PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000043*/ static assembly literal string SystemEnterpriseServices = "System.EnterpriseServices, Version=4.0.0.0, Cultur"
  + "e=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000044*/ static assembly literal string SystemManagement = "System.Management, Version=4.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000045*/ static assembly literal string SystemMessaging = "System.Messaging, Version=4.0.0.0, Culture=neutral"
  + ", PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000046*/ static assembly literal string SystemRuntimeSerializationFormattersSoap = "System.Runtime.Serialization.Formatters.Soap, Vers"
  + "ion=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000047*/ static assembly literal string SystemSecurity = "System.Security, Version=4.0.0.0, Culture=neutral,"
  + " PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000048*/ static assembly literal string SystemServiceProcess = "System.ServiceProcess, Version=4.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000049*/ static assembly literal string SystemWeb = "System.Web, Version=4.0.0.0, Culture=neutral, Publ"
  + "icKeyToken=b03f5f7f11d50a3a"
  .field /*0400004A*/ static assembly literal string SystemWebAbstractions = "System.Web.Abstractions, Version=4.0.0.0, Culture="
  + "neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004B*/ static assembly literal string SystemWebDynamicData = "System.Web.DynamicData, Version=4.0.0.0, Culture=n"
  + "eutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004C*/ static assembly literal string SystemWebDynamicDataDesign = "System.Web.DynamicData.Design, Version=4.0.0.0, Cu"
  + "lture=neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004D*/ static assembly literal string SystemWebEntityDesign = "System.Web.Entity.Design, Version=4.0.0.0, Culture"
  + "=neutral, PublicKeyToken=b77a5c561934e089"
  .field /*0400004E*/ static assembly literal string SystemWebExtensions = "System.Web.Extensions, Version=4.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004F*/ static assembly literal string SystemWebExtensionsDesign = "System.Web.Extensions.Design, Version=4.0.0.0, Cul"
  + "ture=neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*04000050*/ static assembly literal string SystemWebMobile = "System.Web.Mobile, Version=4.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000051*/ static assembly literal string SystemWebRegularExpressions = "System.Web.RegularExpressions, Version=4.0.0.0, Cu"
  + "lture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000052*/ static assembly literal string SystemWebRouting = "System.Web.Routing, Version=4.0.0.0, Culture=neutr"
  + "al, PublicKeyToken=31bf3856ad364e35"
  .field /*04000053*/ static assembly literal string SystemWebServices = "System.Web.Services, Version=4.0.0.0, Culture=neut"
  + "ral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000054*/ static assembly literal string WindowsBase = "WindowsBase, Version=4.0.0.0, Culture=neutral, Pub"
  + "licKeyToken=31bf3856ad364e35"
  .field /*04000055*/ static assembly literal string MicrosoftVisualStudio = "Microsoft.VisualStudio, Version=4.0.0.0, Culture=n"
  + "eutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000056*/ static assembly literal string MicrosoftVisualStudioWindowsForms = "Microsoft.VisualStudio.Windows.Forms, Version=4.0."
  + "0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000057*/ static assembly literal string VJSharpCodeProvider = "VJSharpCodeProvider, Version=4.0.0.0, Culture=neut"
  + "ral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000058*/ static assembly literal string ASPBrowserCapsPublicKey = "b7bd7678b977bd8f"
  .field /*04000059*/ static assembly literal string ASPBrowserCapsFactory = "ASP.BrowserCapsFactory, Version=4.0.0.0, Culture=n"
  + "eutral, PublicKeyToken=b7bd7678b977bd8f"
  .field /*0400005A*/ static assembly literal string MicrosoftVSDesigner = "Microsoft.VSDesigner, Version=10.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005B*/ static assembly literal string MicrosoftVisualStudioWeb = "Microsoft.VisualStudio.Web, Version=10.0.0.0, Cult"
  + "ure=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005C*/ static assembly literal string MicrosoftWebDesign = "Microsoft.Web.Design.Client, Version=10.0.0.0, Cul"
  + "ture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005D*/ static assembly literal string MicrosoftVSDesignerMobile = "Microsoft.VSDesigner.Mobile, Version=8.0.0.0, Cult"
  + "ure=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005E*/ static assembly literal string MicrosoftJScript = "Microsoft.JScript, Version=8.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
} // end of class AssemblyRef

.class /*02000020*/ private auto ansi '<PrivateImplementationDetails>{DCA98F07-D191-4730-A70A-A34D421FD7EE}'
       extends [mscorlib/*23000002*/]System.Object/*01000001*/
{
  .custom /*0C000027:0A000029*/ instance void [mscorlib/*23000002*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000047*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
  .class /*02000021*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=40'
         extends [mscorlib/*23000002*/]System.ValueType/*01000028*/
  {
    .pack 1
    .size 40
  } // end of class '__StaticArrayInitTypeSize=40'

  .field /*0400005F*/ static assembly int64 '$$method0x600025a-1' at I_0000B8D0
  .field /*04000060*/ static assembly valuetype '<PrivateImplementationDetails>{DCA98F07-D191-4730-A70A-A34D421FD7EE}'/*02000020*//'__StaticArrayInitTypeSize=40'/*02000021*/ '$$method0x6000382-1' at I_0000B8D8
} // end of class '<PrivateImplementationDetails>{DCA98F07-D191-4730-A70A-A34D421FD7EE}'


// =============================================================

.data cil I_0000B8D0 = bytearray (
                 20 00 09 00 0A 00 0D 00)                         //  .......
.data cil I_0000B8D8 = bytearray (
                 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00
                 18 00 00 00 00 00 00 00 00 00 00 00 80 00 00 00
                 00 01 00 00 9A 01 00 00) 
// *********** DISASSEMBLY COMPLETE ***********************
