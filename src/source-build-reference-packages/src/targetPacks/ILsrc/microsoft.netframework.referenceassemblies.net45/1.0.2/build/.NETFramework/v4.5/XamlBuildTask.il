
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
// Time-date stamp:            0x4ffa807f
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x0b
// Minor linker version:           0x00
// Size of code:                   0x00004e00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006d5e
// Base of code:                   0x00002000
// Base of data:                   0x00008000
// Image base:                     0x53e60000
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
// Checksum:                       0x0001172a
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00006d0c [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000450] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005800 [0x00003dc0] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
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
//              0x00004d64 Virtual Size
//              0x00002000 Virtual Address
//              0x00004e00 Size of Raw Data
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
//              0x00000450 Virtual Size
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00005000 Pointer to Raw Data
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
//              0x00005600 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000d60
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00006d4e Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 E6 53 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000020f4 [0x00002d88] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00004e7c [0x00001d90] address [size] of Resources Directory:       
// 0x00006c0c [0x00000100] address [size] of Strong Name Signature:     
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
//              0x00001770 Size
//              '#~' Name
//    Stream 2:
//              0x000017dc Offset
//              0x00000ec0 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000269c Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x000026a4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000026b4 Offset
//              0x000006d4 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000010901a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 38336
// PE header size       : 512 (496 used)    ( 1.34%)
// PE additional info   : 17083             (44.56%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.19%)
// CLR meta-data size  : 11656              (30.40%)
// CLR additional info : 7824               (20.41%)
// CLR method headers  : 164                ( 0.43%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 5.34%)
// Unaccounted          : -1023             (-2.67%)

// Num.of PE sections   : 3
//   .text    - 19968
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 11656
//   Module        -    1 (10 bytes)
//   TypeDef       -   16 (224 bytes)     2 interfaces, 0 explicit layout
//   TypeRef       -   38 (228 bytes)
//   MethodDef     -  166 (2324 bytes)    2 abstract, 0 native, 164 bodies
//   FieldDef      -    8 (48 bytes)      0 constant
//   MemberRef     -   24 (144 bytes)
//   ParamDef      -   76 (456 bytes)
//   Constant      -    6 (36 bytes)
//   CustomAttribute-  143 (858 bytes)
//   PropertyMap   -   11 (44 bytes)
//   Property      -   78 (468 bytes)
//   MethodSemantic-  143 (858 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    9 (180 bytes)
//   ManifestResource-    1 (12 bytes)
//   Strings       -  3775 bytes
//   Blobs         -  1748 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   197 bytes

// CLR additional info : 7824
//   Resources         - 7568

// CLR method headers : 164
//   Num.of method bodies  - 164
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 164

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AttributeData                  (public) (auto) (ansi) (sealed) 
// Class AttributeParameterData         (public) (auto) (ansi) (sealed) 
// Class BuildExtensionContext          (public) (auto) (ansi) 
// Class ClassData                      (public) (auto) (ansi) 
// Class CompilationPass2Task           (public) (auto) (ansi) 
// Class GenerateTemporaryAssemblyTask  (public) (auto) (ansi) 
// Interface IXamlBuildTypeGenerationExtension (public) (abstract) (auto) (ansi) 
// Interface IXamlBuildTypeInspectionExtension (public) (abstract) (auto) (ansi) 
// Class MemberVisibility               (public) (auto) (ansi) (sealed) 
// Class NamedObject                    (public) (auto) (ansi) 
// Class PartialClassGenerationTask     (public) (auto) (ansi) 
// Class PropertyData                   (public) (auto) (ansi) (sealed) 
// Class XamlBuildTypeGenerationExtensionContext (public) (auto) (ansi) (sealed) 
// Class XamlBuildTypeInspectionExtensionContext (public) (auto) (ansi) (sealed) 
// Class <PrivateImplementationDetails>{F5EA9ED2-04FA-4CA1-B2E8-75B8A22937A0} (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ 'Microsoft.Build.Utilities.v4.0'
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000003*/ System.Xaml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Build.Framework
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000005*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000006*/ System.Xml.Linq
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000007*/ System.Core
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000008*/ System.ServiceModel.Internals
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*23000009*/ System.Xml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ noplatform XamlBuildTask
{
  .custom /*0C000002:0A000001*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A000001 */ = ( 01 00 0F 34 2E 30 2E 33 30 33 31 39 2E 31 37 39   // ...4.0.30319.179
                                                                                                                                                                 32 39 00 00 )                                     // 29..
  .custom /*0C000003:0A000002*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*0100000F*/::.ctor(bool) /* 0A000002 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000004:0A000003*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*01000010*/::.ctor(string) /* 0A000003 */ = ( 01 00 28 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..(f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 33 35 4D 53 53 68 61 72 65 64   // evdiv\35MSShared
                                                                                                                                                             4C 69 62 31 30 32 34 2E 73 6E 6B 00 00 )          // Lib1024.snk..
  .custom /*0C000005:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000011*/::.ctor(string) /* 0A000004 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C000006:0A000005*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000012*/::.ctor(string) /* 0A000005 */ = ( 01 00 0F 34 2E 30 2E 33 30 33 31 39 2E 31 37 39   // ...4.0.30319.179
                                                                                                                                                                          32 39 00 00 )                                     // 29..
  .custom /*0C000007:0A000006*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*01000013*/::.ctor(string) /* 0A000006 */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000008:0A000007*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000014*/::.ctor(string) /* 0A000007 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C000009:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000015*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00000A:0A000009*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000016*/::.ctor(string) /* 0A000009 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C00000B:0A00000A*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000017*/::.ctor() /* 0A00000A */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C00000C:0A00000B*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000018*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000018*//DebuggingModes/*01000019*/) /* 0A00000B */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C00000D:0A00000C*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*0100001A*/::.ctor(int32) /* 0A00000C */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C00000E:0A00000D*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100001B*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
  .custom /*0C00000F:0A00000E*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblySignatureKeyAttribute/*0100001C*/::.ctor(string,
                                                                                                                                       string) /* 0A00000E */ = ( 01 00 82 40 30 30 32 34 30 30 30 30 30 63 38 30   // ...@002400000c80
                                                                                                                                                                  30 30 30 30 31 34 30 31 30 30 30 30 30 36 30 32   // 0000140100000602
                                                                                                                                                                  30 30 30 30 30 30 32 34 30 30 30 30 35 32 35 33   // 0000002400005253
                                                                                                                                                                  34 31 33 31 30 30 30 38 30 30 30 30 30 31 30 30   // 4131000800000100
                                                                                                                                                                  30 31 30 30 38 35 61 61 64 30 62 65 66 30 36 38   // 010085aad0bef068
                                                                                                                                                                  38 64 31 62 39 39 34 61 30 64 37 38 65 31 66 64   // 8d1b994a0d78e1fd
                                                                                                                                                                  32 39 66 63 32 34 61 63 33 34 65 64 33 64 33 61   // 29fc24ac34ed3d3a
                                                                                                                                                                  63 33 66 62 39 62 33 64 30 63 34 38 33 38 36 62   // c3fb9b3d0c48386b
                                                                                                                                                                  61 38 33 34 61 61 38 38 30 30 33 35 30 36 30 61   // a834aa880035060a
                                                                                                                                                                  38 38 34 38 62 32 64 38 61 64 66 35 38 65 36 37   // 8848b2d8adf58e67
                                                                                                                                                                  30 65 64 32 30 39 31 34 62 65 33 36 38 31 61 38   // 0ed20914be3681a8
                                                                                                                                                                  39 31 63 39 63 38 63 30 31 65 65 66 32 61 62 32   // 91c9c8c01eef2ab2
                                                                                                                                                                  32 38 37 32 35 34 37 63 33 39 62 65 30 30 61 66   // 2872547c39be00af
                                                                                                                                                                  30 65 36 63 37 32 34 38 35 64 37 63 66 64 31 61   // 0e6c72485d7cfd1a
                                                                                                                                                                  35 31 64 66 38 39 34 37 64 33 36 63 65 62 61 39   // 51df8947d36ceba9
                                                                                                                                                                  39 38 39 31 30 36 62 35 38 61 62 65 37 39 65 36   // 989106b58abe79e6
                                                                                                                                                                  61 33 65 37 31 61 30 31 65 64 36 62 64 63 38 36   // a3e71a01ed6bdc86
                                                                                                                                                                  37 30 31 32 38 38 33 65 30 62 31 61 34 64 33 35   // 7012883e0b1a4d35
                                                                                                                                                                  62 31 62 35 65 65 65 64 36 64 66 32 31 65 34 30   // b1b5eeed6df21e40
                                                                                                                                                                  31 62 62 30 63 32 32 66 32 32 34 36 63 63 62 36   // 1bb0c22f2246ccb6
                                                                                                                                                                  39 39 37 39 64 63 39 65 36 31 65 65 66 32 36 32   // 9979dc9e61eef262
                                                                                                                                                                  38 33 32 65 64 30 66 32 30 36 34 38 35 33 37 32   // 832ed0f206485372
                                                                                                                                                                  35 61 37 35 34 38 35 66 61 38 61 33 65 66 62 37   // 5a75485fa8a3efb7
                                                                                                                                                                  65 30 32 37 33 31 39 63 38 36 64 65 63 30 33 64   // e027319c86dec03d
                                                                                                                                                                  63 33 62 31 62 63 61 32 62 35 30 38 31 62 61 62   // c3b1bca2b5081bab
                                                                                                                                                                  35 32 61 36 32 37 62 39 39 31 37 34 35 30 64 66   // 52a627b9917450df
                                                                                                                                                                  61 64 35 33 34 37 39 39 65 31 63 37 61 66 35 38   // ad534799e1c7af58
                                                                                                                                                                  36 38 33 62 64 66 61 31 33 35 66 31 35 31 38 66   // 683bdfa135f1518f
                                                                                                                                                                  66 31 65 61 36 30 65 39 30 64 37 62 39 39 33 61   // f1ea60e90d7b993a
                                                                                                                                                                  36 63 38 37 66 64 33 64 64 39 33 34 30 38 65 33   // 6c87fd3dd93408e3
                                                                                                                                                                  35 64 31 32 39 36 66 39 61 37 66 39 61 39 37 63   // 5d1296f9a7f9a97c
                                                                                                                                                                  35 64 62 35 36 63 30 66 33 63 63 32 35 61 64 31   // 5db56c0f3cc25ad1
                                                                                                                                                                  31 65 39 37 37 37 66 39 34 64 31 33 38 62 33 63   // 1e9777f94d138b3c
                                                                                                                                                                  65 61 35 33 62 39 61 38 33 33 31 63 32 65 36 64   // ea53b9a8331c2e6d
                                                                                                                                                                  63 62 38 64 32 65 61 39 34 65 31 38 62 66 31 31   // cb8d2ea94e18bf11
                                                                                                                                                                  36 33 66 66 31 31 32 61 32 32 64 62 64 39 32 64   // 63ff112a22dbd92d
                                                                                                                                                                  34 32 39 61 81 00 38 39 31 33 65 66 38 36 39 36   // 429a..8913ef8696
                                                                                                                                                                  34 36 64 31 34 39 37 31 64 66 32 32 32 63 32 31   // 46d14971df222c21
                                                                                                                                                                  30 30 31 38 61 62 33 39 34 63 66 65 36 33 66 38   // 0018ab394cfe63f8
                                                                                                                                                                  65 62 39 62 34 64 38 39 34 63 30 64 64 61 37 33   // eb9b4d894c0dda73
                                                                                                                                                                  36 38 63 66 62 36 39 64 66 31 35 64 30 34 39 62   // 68cfb69df15d049b
                                                                                                                                                                  33 34 37 66 38 61 38 62 39 32 30 35 63 64 63 63   // 347f8a8b9205cdcc
                                                                                                                                                                  33 64 36 62 64 36 36 39 30 66 30 62 64 32 34 62   // 3d6bd6690f0bd24b
                                                                                                                                                                  33 64 61 33 31 37 39 66 65 62 38 63 30 33 66 34   // 3da3179feb8c03f4
                                                                                                                                                                  31 30 37 30 33 30 32 37 63 37 38 34 34 66 66 36   // 10703027c7844ff6
                                                                                                                                                                  35 34 39 39 37 65 33 38 30 31 35 64 66 63 39 37   // 54997e38015dfc97
                                                                                                                                                                  32 32 32 63 31 35 30 36 31 61 66 32 61 31 64 33   // 222c15061af2a1d3
                                                                                                                                                                  66 65 39 31 37 37 35 62 39 64 64 34 62 38 65 64   // fe91775b9dd4b8ed
                                                                                                                                                                  65 36 34 64 33 64 35 39 38 31 36 32 37 30 61 39   // e64d3d59816270a9
                                                                                                                                                                  35 32 30 65 33 39 33 63 38 65 36 30 36 31 39 62   // 520e393c8e60619b
                                                                                                                                                                  38 30 64 34 30 66 63 31 64 63 34 66 31 32 62 35   // 80d40fc1dc4f12b5
                                                                                                                                                                  61 61 30 63 32 64 66 32 30 64 30 32 65 61 33 32   // aa0c2df20d02ea32
                                                                                                                                                                  39 36 30 38 35 31 00 00 )                         // 960851..
  .custom /*0C000010:0A00000F*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*0100001D*/::.ctor(bool) /* 0A00000F */ = ( 01 00 01 00 00 ) 
  .custom /*0C000011:0A000010*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100001E*/::.ctor(string) /* 0A000010 */ = ( 01 00 11 58 61 6D 6C 42 75 69 6C 64 54 61 73 6B   // ...XamlBuildTask
                                                                                                                                                           2E 64 6C 6C 00 00 )                               // .dll..
  .custom /*0C000012:0A000011*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100001F*/::.ctor(string) /* 0A000011 */ = ( 01 00 11 58 61 6D 6C 42 75 69 6C 64 54 61 73 6B   // ...XamlBuildTask
                                                                                                                                                                 2E 64 6C 6C 00 00 )                               // .dll..
  .custom /*0C000013:0A000012*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000020*/::.ctor(string) /* 0A000012 */ = ( 01 00 11 58 61 6D 6C 42 75 69 6C 64 54 61 73 6B   // ...XamlBuildTask
                                                                                                                                                                  2E 64 6C 6C 00 00 )                               // .dll..
  .custom /*0C000014:0A000013*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000021*/::.ctor(string) /* 0A000013 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000015:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000022*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                B5 FC 90 E7 02 7F 67 87 1E 77 3A 8F DE 89 38 C8   // ......g..w:...8.
                1D D4 02 BA 65 B9 20 1D 60 59 3E 96 C4 92 65 1E   // ....e. .`Y>...e.
                88 9C C1 3F 14 15 EB B5 3F AC 11 31 AE 0B D3 33   // ...?....?..1...3
                C5 EE 60 21 67 2D 97 18 EA 31 A8 AE BD 0D A0 07   // ..`!g-...1......
                2F 25 D8 7D BA 6F C9 0F FD 59 8E D4 DA 35 E4 4C   // /%.}.o...Y...5.L
                39 8C 45 43 07 E8 E3 3B 84 26 14 3D AE C9 F5 96   // 9.EC...;.&.=....
                83 6F 97 C8 F7 47 50 E5 97 5C 64 E2 18 9F 45 DE   // .o...GP..\d...E.
                F4 6B 2A 2B 12 47 AD C3 65 2B F5 C3 08 05 5D A9 ) // .k*+.G..e+....].
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.module XamlBuildTask.dll
// MVID: {56c0b1f8-35cf-4bb6-a4e1-3196ae61df15}
.imagebase 0x53e60000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F9642DCB000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit Microsoft.Build.Tasks.Xaml.AttributeData
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000001*/ public hidebysig specialname 
          instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 
          get_Type() cil managed
  // SIG: 20 00 12 09
  {
    .custom /*0C000001:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method AttributeData::get_Type

  .method /*06000002*/ public hidebysig specialname 
          instance void  set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'value') cil managed
  // SIG: 20 01 01 12 09
  {
    .custom /*0C000016:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method AttributeData::set_Type

  .method /*06000003*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 0D 01 12 0C
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method AttributeData::get_Parameters

  .method /*06000004*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000004*/<string,class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> 
          get_Properties() cil managed
  // SIG: 20 00 15 12 11 02 0E 12 0C
  {
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method AttributeData::get_Properties

  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method AttributeData::.ctor

  .method /*06000006*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method AttributeData::.cctor

  .property /*17000001*/ instance callconv(8) class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/
          Type()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/::set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/) /* 06000002 */
    .get instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/::get_Type() /* 06000001 */
  } // end of property AttributeData::Type
  .property /*17000002*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/>
          Parameters()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/::get_Parameters() /* 06000003 */
  } // end of property AttributeData::Parameters
  .property /*17000003*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000004*/<string,class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/>
          Properties()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000004*/<string,class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/::get_Properties() /* 06000004 */
  } // end of property AttributeData::Properties
} // end of class Microsoft.Build.Tasks.Xaml.AttributeData

.class /*02000003*/ public auto ansi sealed beforefieldinit Microsoft.Build.Tasks.Xaml.AttributeParameterData
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'type',
                               string textValue) cil managed
  // SIG: 20 02 01 12 09 0E
  {
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'type',
                               object 'value') cil managed
  // SIG: 20 02 01 12 09 1C
  {
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'type',
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000005*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> arrayContents) cil managed
  // SIG: 20 02 01 12 09 15 12 15 01 12 0C
  {
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::.ctor

  .method /*0600000A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::.ctor

  .method /*0600000B*/ public hidebysig specialname 
          instance string  get_TextValue() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000017:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::get_TextValue

  .method /*0600000C*/ assembly hidebysig specialname 
          instance void  set_TextValue(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000018:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::set_TextValue

  .method /*0600000D*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000019:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::get_Value

  .method /*0600000E*/ assembly hidebysig specialname 
          instance void  set_Value(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00001A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::set_Value

  .method /*0600000F*/ public hidebysig specialname 
          instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 
          get_Type() cil managed
  // SIG: 20 00 12 09
  {
    .custom /*0C00001B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::get_Type

  .method /*06000010*/ assembly hidebysig specialname 
          instance void  set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'value') cil managed
  // SIG: 20 01 01 12 09
  {
    .custom /*0C00001C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::set_Type

  .method /*06000011*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> 
          get_ArrayContents() cil managed
  // SIG: 20 00 15 12 19 01 12 0C
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method AttributeParameterData::get_ArrayContents

  .property /*17000004*/ instance callconv(8) string
          TextValue()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::set_TextValue(string) /* 0600000C */
    .get instance string Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::get_TextValue() /* 0600000B */
  } // end of property AttributeParameterData::TextValue
  .property /*17000005*/ instance callconv(8) object
          Value()
  {
    .get instance object Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::get_Value() /* 0600000D */
    .set instance void Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::set_Value(object) /* 0600000E */
  } // end of property AttributeParameterData::Value
  .property /*17000006*/ instance callconv(8) class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/
          Type()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/) /* 06000010 */
    .get instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::get_Type() /* 0600000F */
  } // end of property AttributeParameterData::Type
  .property /*17000007*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/>
          ArrayContents()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<class Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/> Microsoft.Build.Tasks.Xaml.AttributeParameterData/*02000003*/::get_ArrayContents() /* 06000011 */
  } // end of property AttributeParameterData::ArrayContents
} // end of class Microsoft.Build.Tasks.Xaml.AttributeParameterData

.class /*02000004*/ public auto ansi beforefieldinit Microsoft.Build.Tasks.Xaml.BuildExtensionContext
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000012*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::.ctor

  .method /*06000013*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00001E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_AssemblyName

  .method /*06000014*/ assembly hidebysig specialname 
          instance void  set_AssemblyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00001F:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_AssemblyName

  .method /*06000015*/ public hidebysig specialname 
          instance bool  get_IsInProcessXamlMarkupCompile() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000020:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_IsInProcessXamlMarkupCompile

  .method /*06000016*/ assembly hidebysig specialname 
          instance void  set_IsInProcessXamlMarkupCompile(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000021:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_IsInProcessXamlMarkupCompile

  .method /*06000017*/ public hidebysig specialname 
          instance string  get_Language() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000022:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_Language

  .method /*06000018*/ assembly hidebysig specialname 
          instance void  set_Language(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000023:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_Language

  .method /*06000019*/ public hidebysig specialname 
          instance string  get_OutputPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000024:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_OutputPath

  .method /*0600001A*/ assembly hidebysig specialname 
          instance void  set_OutputPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000025:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_OutputPath

  .method /*0600001B*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> 
          get_References() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_References

  .method /*0600001C*/ public hidebysig specialname 
          instance string  get_RootNamespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000026:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_RootNamespace

  .method /*0600001D*/ assembly hidebysig specialname 
          instance void  set_RootNamespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000027:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_RootNamespace

  .method /*0600001E*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> 
          get_SourceCodeFiles() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_SourceCodeFiles

  .method /*0600001F*/ public hidebysig specialname 
          instance string  get_LocalAssembly() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000028:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_LocalAssembly

  .method /*06000020*/ assembly hidebysig specialname 
          instance void  set_LocalAssembly(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000029:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_LocalAssembly

  .method /*06000021*/ public hidebysig specialname 
          instance class ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.TaskLoggingHelper/*01000007*/ 
          get_XamlBuildLogger() cil managed
  // SIG: 20 00 12 1D
  {
    .custom /*0C00002A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_XamlBuildLogger

  .method /*06000022*/ assembly hidebysig specialname 
          instance void  set_XamlBuildLogger(class ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.TaskLoggingHelper/*01000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1D
  {
    .custom /*0C00002B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::set_XamlBuildLogger

  .method /*06000023*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> 
          get_GeneratedFiles() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_GeneratedFiles

  .method /*06000024*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> 
          get_GeneratedResourceFiles() cil managed
  // SIG: 20 00 15 12 19 01 0E
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::get_GeneratedResourceFiles

  .method /*06000025*/ public hidebysig instance void 
          AddGeneratedFile(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::AddGeneratedFile

  .method /*06000026*/ public hidebysig instance void 
          AddGeneratedResourceFile(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::AddGeneratedResourceFile

  .method /*06000027*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method BuildExtensionContext::.cctor

  .property /*17000008*/ instance callconv(8) string
          AssemblyName()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_AssemblyName(string) /* 06000014 */
    .get instance string Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_AssemblyName() /* 06000013 */
  } // end of property BuildExtensionContext::AssemblyName
  .property /*17000009*/ instance callconv(8) bool
          IsInProcessXamlMarkupCompile()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_IsInProcessXamlMarkupCompile(bool) /* 06000016 */
    .get instance bool Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_IsInProcessXamlMarkupCompile() /* 06000015 */
  } // end of property BuildExtensionContext::IsInProcessXamlMarkupCompile
  .property /*1700000A*/ instance callconv(8) string
          Language()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_Language(string) /* 06000018 */
    .get instance string Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_Language() /* 06000017 */
  } // end of property BuildExtensionContext::Language
  .property /*1700000B*/ instance callconv(8) string
          OutputPath()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_OutputPath() /* 06000019 */
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_OutputPath(string) /* 0600001A */
  } // end of property BuildExtensionContext::OutputPath
  .property /*1700000C*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string>
          References()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_References() /* 0600001B */
  } // end of property BuildExtensionContext::References
  .property /*1700000D*/ instance callconv(8) string
          RootNamespace()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_RootNamespace() /* 0600001C */
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_RootNamespace(string) /* 0600001D */
  } // end of property BuildExtensionContext::RootNamespace
  .property /*1700000E*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string>
          SourceCodeFiles()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_SourceCodeFiles() /* 0600001E */
  } // end of property BuildExtensionContext::SourceCodeFiles
  .property /*1700000F*/ instance callconv(8) string
          LocalAssembly()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_LocalAssembly(string) /* 06000020 */
    .get instance string Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_LocalAssembly() /* 0600001F */
  } // end of property BuildExtensionContext::LocalAssembly
  .property /*17000010*/ instance callconv(8) class ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.TaskLoggingHelper/*01000007*/
          XamlBuildLogger()
  {
    .get instance class ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.TaskLoggingHelper/*01000007*/ Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_XamlBuildLogger() /* 06000021 */
    .set instance void Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::set_XamlBuildLogger(class ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.TaskLoggingHelper/*01000007*/) /* 06000022 */
  } // end of property BuildExtensionContext::XamlBuildLogger
  .property /*17000011*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string>
          GeneratedFiles()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_GeneratedFiles() /* 06000023 */
  } // end of property BuildExtensionContext::GeneratedFiles
  .property /*17000012*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string>
          GeneratedResourceFiles()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000006*/<string> Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/::get_GeneratedResourceFiles() /* 06000024 */
  } // end of property BuildExtensionContext::GeneratedResourceFiles
} // end of class Microsoft.Build.Tasks.Xaml.BuildExtensionContext

.class /*02000005*/ public auto ansi beforefieldinit Microsoft.Build.Tasks.Xaml.ClassData
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method ClassData::.ctor

  .method /*06000029*/ public hidebysig specialname 
          instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 09
  {
    .custom /*0C00002C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method ClassData::get_BaseType

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_BaseType(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'value') cil managed
  // SIG: 20 01 01 12 09
  {
    .custom /*0C00002D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method ClassData::set_BaseType

  .method /*0600002B*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<string> 
          get_CodeSnippets() cil managed
  // SIG: 20 00 15 12 0D 01 0E
  {
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method ClassData::get_CodeSnippets

  .method /*0600002C*/ public hidebysig specialname 
          instance string  get_EmbeddedResourceFileName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00002E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method ClassData::get_EmbeddedResourceFileName

  .method /*0600002D*/ assembly hidebysig specialname 
          instance void  set_EmbeddedResourceFileName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000030:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method ClassData::set_EmbeddedResourceFileName

  .method /*0600002E*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.KeyedCollection`2/*01000008*/<string,class Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/> 
          get_Properties() cil managed
  // SIG: 20 00 15 12 21 02 0E 12 34
  {
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method ClassData::get_Properties

  .method /*0600002F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/> 
          get_Attributes() cil managed
  // SIG: 20 00 15 12 0D 01 12 08
  {
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method ClassData::get_Attributes

  .method /*06000030*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000034:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method ClassData::get_Name

  .method /*06000031*/ assembly hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000036:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method ClassData::set_Name

  .method /*06000032*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/> 
          get_NamedObjects() cil managed
  // SIG: 20 00 15 12 0D 01 12 2C
  {
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method ClassData::get_NamedObjects

  .method /*06000033*/ public hidebysig specialname 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000038:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method ClassData::get_Namespace

  .method /*06000034*/ assembly hidebysig specialname 
          instance void  set_Namespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000039:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method ClassData::set_Namespace

  .method /*06000035*/ public hidebysig specialname 
          instance class [System.Xaml/*23000003*/]System.Xaml.XamlNodeList/*01000009*/ 
          get_EmbeddedResourceXaml() cil managed
  // SIG: 20 00 12 25
  {
    .custom /*0C00003B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method ClassData::get_EmbeddedResourceXaml

  .method /*06000036*/ public hidebysig specialname 
          instance void  set_EmbeddedResourceXaml(class [System.Xaml/*23000003*/]System.Xaml.XamlNodeList/*01000009*/ 'value') cil managed
  // SIG: 20 01 01 12 25
  {
    .custom /*0C00003C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method ClassData::set_EmbeddedResourceXaml

  .method /*06000037*/ public hidebysig specialname 
          instance bool  get_IsPublic() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00003D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method ClassData::get_IsPublic

  .method /*06000038*/ public hidebysig specialname 
          instance void  set_IsPublic(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00003E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method ClassData::set_IsPublic

  .method /*06000039*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000040:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method ClassData::get_FileName

  .method /*0600003A*/ assembly hidebysig specialname 
          instance void  set_FileName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000041:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method ClassData::set_FileName

  .method /*0600003B*/ public hidebysig specialname 
          instance string  get_HelperClassFullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000042:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method ClassData::get_HelperClassFullName

  .method /*0600003C*/ assembly hidebysig specialname 
          instance void  set_HelperClassFullName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000044:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method ClassData::set_HelperClassFullName

  .property /*17000013*/ instance callconv(8) class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/
          BaseType()
  {
    .get instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_BaseType() /* 06000029 */
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_BaseType(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/) /* 0600002A */
  } // end of property ClassData::BaseType
  .property /*17000014*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<string>
          CodeSnippets()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<string> Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_CodeSnippets() /* 0600002B */
  } // end of property ClassData::CodeSnippets
  .property /*17000015*/ instance callconv(8) string
          EmbeddedResourceFileName()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_EmbeddedResourceFileName(string) /* 0600002D */
    .get instance string Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_EmbeddedResourceFileName() /* 0600002C */
  } // end of property ClassData::EmbeddedResourceFileName
  .property /*17000016*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.KeyedCollection`2/*01000008*/<string,class Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/>
          Properties()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.KeyedCollection`2/*01000008*/<string,class Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/> Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_Properties() /* 0600002E */
  } // end of property ClassData::Properties
  .property /*17000017*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/>
          Attributes()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/> Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_Attributes() /* 0600002F */
  } // end of property ClassData::Attributes
  .property /*17000018*/ instance callconv(8) string
          Name()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_Name(string) /* 06000031 */
    .get instance string Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_Name() /* 06000030 */
  } // end of property ClassData::Name
  .property /*17000019*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/>
          NamedObjects()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/> Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_NamedObjects() /* 06000032 */
  } // end of property ClassData::NamedObjects
  .property /*1700001A*/ instance callconv(8) string
          Namespace()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_Namespace(string) /* 06000034 */
    .get instance string Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_Namespace() /* 06000033 */
  } // end of property ClassData::Namespace
  .property /*1700001B*/ instance callconv(8) class [System.Xaml/*23000003*/]System.Xaml.XamlNodeList/*01000009*/
          EmbeddedResourceXaml()
  {
    .get instance class [System.Xaml/*23000003*/]System.Xaml.XamlNodeList/*01000009*/ Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_EmbeddedResourceXaml() /* 06000035 */
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_EmbeddedResourceXaml(class [System.Xaml/*23000003*/]System.Xaml.XamlNodeList/*01000009*/) /* 06000036 */
  } // end of property ClassData::EmbeddedResourceXaml
  .property /*1700001C*/ instance callconv(8) bool
          IsPublic()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_IsPublic(bool) /* 06000038 */
    .get instance bool Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_IsPublic() /* 06000037 */
  } // end of property ClassData::IsPublic
  .property /*1700001D*/ instance callconv(8) string
          FileName()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_FileName() /* 06000039 */
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_FileName(string) /* 0600003A */
  } // end of property ClassData::FileName
  .property /*1700001E*/ instance callconv(8) string
          HelperClassFullName()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::set_HelperClassFullName(string) /* 0600003C */
    .get instance string Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/::get_HelperClassFullName() /* 0600003B */
  } // end of property ClassData::HelperClassFullName
} // end of class Microsoft.Build.Tasks.Xaml.ClassData

.class /*02000006*/ public auto ansi beforefieldinit Microsoft.Build.Tasks.Xaml.CompilationPass2Task
       extends ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.Task/*0100000A*/
{
  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::.ctor

  .method /*0600003E*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_ApplicationMarkup() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000046:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_ApplicationMarkup

  .method /*0600003F*/ public hidebysig specialname 
          instance void  set_ApplicationMarkup(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000048:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_ApplicationMarkup

  .method /*06000040*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00004A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_AssemblyName

  .method /*06000041*/ public hidebysig specialname 
          instance void  set_AssemblyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00004B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_AssemblyName

  .method /*06000042*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_References() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C00004C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_References

  .method /*06000043*/ public hidebysig specialname 
          instance void  set_References(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C00004D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_References

  .method /*06000044*/ public hidebysig specialname 
          instance string  get_Language() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_Language

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_Language(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_Language

  .method /*06000046*/ public hidebysig specialname 
          instance string  get_OutputPath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_OutputPath

  .method /*06000047*/ public hidebysig specialname 
          instance void  set_OutputPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_OutputPath

  .method /*06000048*/ public hidebysig specialname 
          instance string  get_MSBuildProjectDirectory() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_MSBuildProjectDirectory

  .method /*06000049*/ public hidebysig specialname 
          instance void  set_MSBuildProjectDirectory(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_MSBuildProjectDirectory

  .method /*0600004A*/ public hidebysig specialname 
          instance bool  get_IsInProcessXamlMarkupCompile() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00004F:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_IsInProcessXamlMarkupCompile

  .method /*0600004B*/ public hidebysig specialname 
          instance void  set_IsInProcessXamlMarkupCompile(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000051:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_IsInProcessXamlMarkupCompile

  .method /*0600004C*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_SourceCodeFiles() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000052:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_SourceCodeFiles

  .method /*0600004D*/ public hidebysig specialname 
          instance void  set_SourceCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000053:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_SourceCodeFiles

  .method /*0600004E*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_XamlBuildTypeInspectionExtensionNames() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000054:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_XamlBuildTypeInspectionExtensionNames

  .method /*0600004F*/ public hidebysig specialname 
          instance void  set_XamlBuildTypeInspectionExtensionNames(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000055:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_XamlBuildTypeInspectionExtensionNames

  .method /*06000050*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_ApplicationMarkupWithTypeName() cil managed
  // SIG: 20 00 1D 12 2D
  {
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_ApplicationMarkupWithTypeName

  .method /*06000051*/ public hidebysig specialname 
          instance void  set_ApplicationMarkupWithTypeName(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_ApplicationMarkupWithTypeName

  .method /*06000052*/ public hidebysig specialname 
          instance string  get_LocalAssemblyReference() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000056:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_LocalAssemblyReference

  .method /*06000053*/ public hidebysig specialname 
          instance void  set_LocalAssemblyReference(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000057:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_LocalAssemblyReference

  .method /*06000054*/ public hidebysig specialname 
          instance string  get_RootNamespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000058:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_RootNamespace

  .method /*06000055*/ public hidebysig specialname 
          instance void  set_RootNamespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000059:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_RootNamespace

  .method /*06000056*/ public hidebysig specialname 
          instance string  get_BuildTaskPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_BuildTaskPath

  .method /*06000057*/ public hidebysig specialname 
          instance void  set_BuildTaskPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00005B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_BuildTaskPath

  .method /*06000058*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_ExtensionGeneratedCodeFiles() cil managed
  // SIG: 20 00 1D 12 2D
  {
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::get_ExtensionGeneratedCodeFiles

  .method /*06000059*/ public hidebysig specialname 
          instance void  set_ExtensionGeneratedCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::set_ExtensionGeneratedCodeFiles

  .method /*0600005A*/ public hidebysig virtual 
          instance bool  Execute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method CompilationPass2Task::Execute

  .property /*1700001F*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          ApplicationMarkup()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_ApplicationMarkup() /* 0600003E */
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_ApplicationMarkup(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 0600003F */
  } // end of property CompilationPass2Task::ApplicationMarkup
  .property /*17000020*/ instance callconv(8) string
          AssemblyName()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_AssemblyName() /* 06000040 */
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_AssemblyName(string) /* 06000041 */
  } // end of property CompilationPass2Task::AssemblyName
  .property /*17000021*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          References()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_References(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000043 */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_References() /* 06000042 */
  } // end of property CompilationPass2Task::References
  .property /*17000022*/ instance callconv(8) string
          Language()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_Language(string) /* 06000045 */
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_Language() /* 06000044 */
  } // end of property CompilationPass2Task::Language
  .property /*17000023*/ instance callconv(8) string
          OutputPath()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_OutputPath(string) /* 06000047 */
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_OutputPath() /* 06000046 */
  } // end of property CompilationPass2Task::OutputPath
  .property /*17000024*/ instance callconv(8) string
          MSBuildProjectDirectory()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_MSBuildProjectDirectory(string) /* 06000049 */
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_MSBuildProjectDirectory() /* 06000048 */
  } // end of property CompilationPass2Task::MSBuildProjectDirectory
  .property /*17000025*/ instance callconv(8) bool
          IsInProcessXamlMarkupCompile()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_IsInProcessXamlMarkupCompile(bool) /* 0600004B */
    .get instance bool Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_IsInProcessXamlMarkupCompile() /* 0600004A */
  } // end of property CompilationPass2Task::IsInProcessXamlMarkupCompile
  .property /*17000026*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          SourceCodeFiles()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_SourceCodeFiles() /* 0600004C */
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_SourceCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 0600004D */
  } // end of property CompilationPass2Task::SourceCodeFiles
  .property /*17000027*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          XamlBuildTypeInspectionExtensionNames()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_XamlBuildTypeInspectionExtensionNames(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 0600004F */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_XamlBuildTypeInspectionExtensionNames() /* 0600004E */
  } // end of property CompilationPass2Task::XamlBuildTypeInspectionExtensionNames
  .property /*17000028*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          ApplicationMarkupWithTypeName()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_ApplicationMarkupWithTypeName() /* 06000050 */
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_ApplicationMarkupWithTypeName(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000051 */
  } // end of property CompilationPass2Task::ApplicationMarkupWithTypeName
  .property /*17000029*/ instance callconv(8) string
          LocalAssemblyReference()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_LocalAssemblyReference() /* 06000052 */
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_LocalAssemblyReference(string) /* 06000053 */
  } // end of property CompilationPass2Task::LocalAssemblyReference
  .property /*1700002A*/ instance callconv(8) string
          RootNamespace()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_RootNamespace(string) /* 06000055 */
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_RootNamespace() /* 06000054 */
  } // end of property CompilationPass2Task::RootNamespace
  .property /*1700002B*/ instance callconv(8) string
          BuildTaskPath()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_BuildTaskPath(string) /* 06000057 */
    .get instance string Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_BuildTaskPath() /* 06000056 */
  } // end of property CompilationPass2Task::BuildTaskPath
  .property /*1700002C*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          ExtensionGeneratedCodeFiles()
  {
    .custom /*0C00002F:0A000016*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.OutputAttribute/*01000024*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::get_ExtensionGeneratedCodeFiles() /* 06000058 */
    .set instance void Microsoft.Build.Tasks.Xaml.CompilationPass2Task/*02000006*/::set_ExtensionGeneratedCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000059 */
  } // end of property CompilationPass2Task::ExtensionGeneratedCodeFiles
} // end of class Microsoft.Build.Tasks.Xaml.CompilationPass2Task

.class /*02000007*/ public auto ansi beforefieldinit Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask
       extends ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.Task/*0100000A*/
{
  .method /*0600005B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::.ctor

  .method /*0600005C*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_AssemblyName

  .method /*0600005D*/ public hidebysig specialname 
          instance void  set_AssemblyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00005D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_AssemblyName

  .method /*0600005E*/ public hidebysig specialname 
          instance string  get_OutputPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_OutputPath

  .method /*0600005F*/ public hidebysig specialname 
          instance void  set_OutputPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00005F:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_OutputPath

  .method /*06000060*/ public hidebysig specialname 
          instance string  get_CurrentProject() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000060:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_CurrentProject

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_CurrentProject(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000061:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_CurrentProject

  .method /*06000062*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_SourceCodeFiles() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000062:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_SourceCodeFiles

  .method /*06000063*/ public hidebysig specialname 
          instance void  set_SourceCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000063:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_SourceCodeFiles

  .method /*06000064*/ public hidebysig specialname 
          instance string  get_CompileTargetName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000064:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_CompileTargetName

  .method /*06000065*/ public hidebysig specialname 
          instance void  set_CompileTargetName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000065:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_CompileTargetName

  .method /*06000066*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_GeneratedResourcesFiles() cil managed
  // SIG: 20 00 1D 12 2D
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_GeneratedResourcesFiles

  .method /*06000067*/ public hidebysig specialname 
          instance void  set_GeneratedResourcesFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_GeneratedResourcesFiles

  .method /*06000068*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_ReferencePaths() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000066:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_ReferencePaths

  .method /*06000069*/ public hidebysig specialname 
          instance void  set_ReferencePaths(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000067:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b8
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_ReferencePaths

  .method /*0600006A*/ public hidebysig specialname 
          instance string  get_ApplicationMarkupTypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000068:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b9
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::get_ApplicationMarkupTypeName

  .method /*0600006B*/ public hidebysig specialname 
          instance void  set_ApplicationMarkupTypeName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000069:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ba
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::set_ApplicationMarkupTypeName

  .method /*0600006C*/ public hidebysig virtual 
          instance bool  Execute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bb
    // Code size       0 (0x0)
  } // end of method GenerateTemporaryAssemblyTask::Execute

  .property /*1700002D*/ instance callconv(8) string
          AssemblyName()
  {
    .custom /*0C000031:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_AssemblyName() /* 0600005C */
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_AssemblyName(string) /* 0600005D */
  } // end of property GenerateTemporaryAssemblyTask::AssemblyName
  .property /*1700002E*/ instance callconv(8) string
          OutputPath()
  {
    .custom /*0C000032:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_OutputPath(string) /* 0600005F */
    .get instance string Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_OutputPath() /* 0600005E */
  } // end of property GenerateTemporaryAssemblyTask::OutputPath
  .property /*1700002F*/ instance callconv(8) string
          CurrentProject()
  {
    .custom /*0C000033:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_CurrentProject() /* 06000060 */
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_CurrentProject(string) /* 06000061 */
  } // end of property GenerateTemporaryAssemblyTask::CurrentProject
  .property /*17000030*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          SourceCodeFiles()
  {
    .custom /*0C000035:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_SourceCodeFiles() /* 06000062 */
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_SourceCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000063 */
  } // end of property GenerateTemporaryAssemblyTask::SourceCodeFiles
  .property /*17000031*/ instance callconv(8) string
          CompileTargetName()
  {
    .custom /*0C000037:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_CompileTargetName(string) /* 06000065 */
    .get instance string Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_CompileTargetName() /* 06000064 */
  } // end of property GenerateTemporaryAssemblyTask::CompileTargetName
  .property /*17000032*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          GeneratedResourcesFiles()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_GeneratedResourcesFiles() /* 06000066 */
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_GeneratedResourcesFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000067 */
  } // end of property GenerateTemporaryAssemblyTask::GeneratedResourcesFiles
  .property /*17000033*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          ReferencePaths()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_ReferencePaths() /* 06000068 */
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_ReferencePaths(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000069 */
  } // end of property GenerateTemporaryAssemblyTask::ReferencePaths
  .property /*17000034*/ instance callconv(8) string
          ApplicationMarkupTypeName()
  {
    .custom /*0C00003A:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::set_ApplicationMarkupTypeName(string) /* 0600006B */
    .get instance string Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask/*02000007*/::get_ApplicationMarkupTypeName() /* 0600006A */
  } // end of property GenerateTemporaryAssemblyTask::ApplicationMarkupTypeName
} // end of class Microsoft.Build.Tasks.Xaml.GenerateTemporaryAssemblyTask

.class /*02000008*/ interface public abstract auto ansi Microsoft.Build.Tasks.Xaml.IXamlBuildTypeGenerationExtension
{
  .method /*0600006D*/ public hidebysig newslot abstract virtual 
          instance bool  Execute(class Microsoft.Build.Tasks.Xaml.ClassData/*02000005*/ classData,
                                 class Microsoft.Build.Tasks.Xaml.XamlBuildTypeGenerationExtensionContext/*0200000E*/ buildContext) cil managed
  // SIG: 20 02 02 12 14 12 38
  {
    // Method begins at RVA 0x0
  } // end of method IXamlBuildTypeGenerationExtension::Execute

} // end of class Microsoft.Build.Tasks.Xaml.IXamlBuildTypeGenerationExtension

.class /*02000009*/ interface public abstract auto ansi Microsoft.Build.Tasks.Xaml.IXamlBuildTypeInspectionExtension
{
  .method /*0600006E*/ public hidebysig newslot abstract virtual 
          instance bool  Execute(class Microsoft.Build.Tasks.Xaml.XamlBuildTypeInspectionExtensionContext/*0200000F*/ buildContext) cil managed
  // SIG: 20 01 02 12 3C
  {
    // Method begins at RVA 0x0
  } // end of method IXamlBuildTypeInspectionExtension::Execute

} // end of class Microsoft.Build.Tasks.Xaml.IXamlBuildTypeInspectionExtension

.class /*0200000A*/ public auto ansi sealed Microsoft.Build.Tasks.Xaml.MemberVisibility
       extends [mscorlib/*23000001*/]System.Enum/*0100000C*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ Public = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ Private = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ Family = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ Assembly = int32(0x00000003)
  .field /*04000006*/ public static literal valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ FamilyOrAssembly = int32(0x00000004)
  .field /*04000007*/ public static literal valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ FamilyAndAssembly = int32(0x00000005)
} // end of class Microsoft.Build.Tasks.Xaml.MemberVisibility

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.Build.Tasks.Xaml.NamedObject
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600006F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method NamedObject::get_Name

  .method /*06000070*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00006B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method NamedObject::set_Name

  .method /*06000071*/ public hidebysig specialname 
          instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 
          get_Type() cil managed
  // SIG: 20 00 12 09
  {
    .custom /*0C00006C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method NamedObject::get_Type

  .method /*06000072*/ public hidebysig specialname 
          instance void  set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'value') cil managed
  // SIG: 20 01 01 12 09
  {
    .custom /*0C00006D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method NamedObject::set_Type

  .method /*06000073*/ public hidebysig specialname 
          instance valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ 
          get_Visibility() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method NamedObject::get_Visibility

  .method /*06000074*/ public hidebysig specialname 
          instance void  set_Visibility(valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method NamedObject::set_Visibility

  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method NamedObject::.ctor

  .property /*17000035*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/::get_Name() /* 0600006F */
    .set instance void Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/::set_Name(string) /* 06000070 */
  } // end of property NamedObject::Name
  .property /*17000036*/ instance callconv(8) class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/
          Type()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/::set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/) /* 06000072 */
    .get instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/::get_Type() /* 06000071 */
  } // end of property NamedObject::Type
  .property /*17000037*/ instance callconv(8) valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/
          Visibility()
  {
    .get instance valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/::get_Visibility() /* 06000073 */
    .set instance void Microsoft.Build.Tasks.Xaml.NamedObject/*0200000B*/::set_Visibility(valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/) /* 06000074 */
  } // end of property NamedObject::Visibility
} // end of class Microsoft.Build.Tasks.Xaml.NamedObject

.class /*0200000C*/ public auto ansi beforefieldinit Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask
       extends ['Microsoft.Build.Utilities.v4.0'/*23000002*/]Microsoft.Build.Utilities.Task/*0100000A*/
{
  .method /*06000076*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::.ctor

  .method /*06000077*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_ApplicationMarkup() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C00006E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_ApplicationMarkup

  .method /*06000078*/ public hidebysig specialname 
          instance void  set_ApplicationMarkup(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C00006F:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_ApplicationMarkup

  .method /*06000079*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000070:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c6
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_AssemblyName

  .method /*0600007A*/ public hidebysig specialname 
          instance void  set_AssemblyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000071:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_AssemblyName

  .method /*0600007B*/ public hidebysig specialname 
          instance string[]  get_KnownReferencePaths() cil managed
  // SIG: 20 00 1D 0E
  {
    .custom /*0C000072:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_KnownReferencePaths

  .method /*0600007C*/ public hidebysig specialname 
          instance void  set_KnownReferencePaths(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    .custom /*0C000073:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20c9
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_KnownReferencePaths

  .method /*0600007D*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_GeneratedResources() cil managed
  // SIG: 20 00 1D 12 2D
  {
    // Method begins at RVA 0x20ca
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_GeneratedResources

  .method /*0600007E*/ public hidebysig specialname 
          instance void  set_GeneratedResources(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    // Method begins at RVA 0x20cb
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_GeneratedResources

  .method /*0600007F*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_GeneratedCodeFiles() cil managed
  // SIG: 20 00 1D 12 2D
  {
    // Method begins at RVA 0x20cc
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_GeneratedCodeFiles

  .method /*06000080*/ public hidebysig specialname 
          instance void  set_GeneratedCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    // Method begins at RVA 0x20cd
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_GeneratedCodeFiles

  .method /*06000081*/ public hidebysig specialname 
          instance string  get_GeneratedSourceExtension() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000074:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ce
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_GeneratedSourceExtension

  .method /*06000082*/ public hidebysig specialname 
          instance void  set_GeneratedSourceExtension(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000075:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20cf
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_GeneratedSourceExtension

  .method /*06000083*/ public hidebysig specialname 
          instance string  get_Language() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000076:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_Language

  .method /*06000084*/ public hidebysig specialname 
          instance void  set_Language(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000077:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_Language

  .method /*06000085*/ public hidebysig specialname 
          instance string  get_OutputPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000078:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d2
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_OutputPath

  .method /*06000086*/ public hidebysig specialname 
          instance void  set_OutputPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000079:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_OutputPath

  .method /*06000087*/ public hidebysig specialname 
          instance string  get_MSBuildProjectDirectory() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_MSBuildProjectDirectory

  .method /*06000088*/ public hidebysig specialname 
          instance void  set_MSBuildProjectDirectory(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20d5
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_MSBuildProjectDirectory

  .method /*06000089*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_References() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C00007A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_References

  .method /*0600008A*/ public hidebysig specialname 
          instance void  set_References(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C00007B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_References

  .method /*0600008B*/ public hidebysig specialname 
          instance string  get_RootNamespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d8
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_RootNamespace

  .method /*0600008C*/ public hidebysig specialname 
          instance void  set_RootNamespace(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d9
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_RootNamespace

  .method /*0600008D*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_SourceCodeFiles() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C00007E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_SourceCodeFiles

  .method /*0600008E*/ public hidebysig specialname 
          instance void  set_SourceCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C00007F:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20db
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_SourceCodeFiles

  .method /*0600008F*/ public hidebysig specialname 
          instance bool  get_RequiresCompilationPass2() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000080:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20dc
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_RequiresCompilationPass2

  .method /*06000090*/ public hidebysig specialname 
          instance void  set_RequiresCompilationPass2(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000081:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_RequiresCompilationPass2

  .method /*06000091*/ public hidebysig specialname 
          instance string  get_BuildTaskPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000082:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_BuildTaskPath

  .method /*06000092*/ public hidebysig specialname 
          instance void  set_BuildTaskPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000083:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20df
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_BuildTaskPath

  .method /*06000093*/ public hidebysig specialname 
          instance bool  get_IsInProcessXamlMarkupCompile() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000084:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_IsInProcessXamlMarkupCompile

  .method /*06000094*/ public hidebysig specialname 
          instance void  set_IsInProcessXamlMarkupCompile(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000085:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_IsInProcessXamlMarkupCompile

  .method /*06000095*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_XamlBuildTypeGenerationExtensionNames() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000086:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_XamlBuildTypeGenerationExtensionNames

  .method /*06000096*/ public hidebysig specialname 
          instance void  set_XamlBuildTypeGenerationExtensionNames(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000087:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_XamlBuildTypeGenerationExtensionNames

  .method /*06000097*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 
          get_XamlBuildTypeInspectionExtensionNames() cil managed
  // SIG: 20 00 1D 12 2D
  {
    .custom /*0C000088:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::get_XamlBuildTypeInspectionExtensionNames

  .method /*06000098*/ public hidebysig specialname 
          instance void  set_XamlBuildTypeInspectionExtensionNames(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 2D
  {
    .custom /*0C000089:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::set_XamlBuildTypeInspectionExtensionNames

  .method /*06000099*/ public hidebysig virtual 
          instance bool  Execute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method PartialClassGenerationTask::Execute

  .property /*17000038*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          ApplicationMarkup()
  {
    .custom /*0C00003F:0A000016*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.OutputAttribute/*01000024*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_ApplicationMarkup(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000078 */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_ApplicationMarkup() /* 06000077 */
  } // end of property PartialClassGenerationTask::ApplicationMarkup
  .property /*17000039*/ instance callconv(8) string
          AssemblyName()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_AssemblyName() /* 06000079 */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_AssemblyName(string) /* 0600007A */
  } // end of property PartialClassGenerationTask::AssemblyName
  .property /*1700003A*/ instance callconv(8) string[]
          KnownReferencePaths()
  {
    .get instance string[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_KnownReferencePaths() /* 0600007B */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_KnownReferencePaths(string[]) /* 0600007C */
  } // end of property PartialClassGenerationTask::KnownReferencePaths
  .property /*1700003B*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          GeneratedResources()
  {
    .custom /*0C000043:0A000016*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.OutputAttribute/*01000024*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_GeneratedResources(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 0600007E */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_GeneratedResources() /* 0600007D */
  } // end of property PartialClassGenerationTask::GeneratedResources
  .property /*1700003C*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          GeneratedCodeFiles()
  {
    .custom /*0C000045:0A000016*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.OutputAttribute/*01000024*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_GeneratedCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000080 */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_GeneratedCodeFiles() /* 0600007F */
  } // end of property PartialClassGenerationTask::GeneratedCodeFiles
  .property /*1700003D*/ instance callconv(8) string
          GeneratedSourceExtension()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_GeneratedSourceExtension() /* 06000081 */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_GeneratedSourceExtension(string) /* 06000082 */
  } // end of property PartialClassGenerationTask::GeneratedSourceExtension
  .property /*1700003E*/ instance callconv(8) string
          Language()
  {
    .custom /*0C000047:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_Language(string) /* 06000084 */
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_Language() /* 06000083 */
  } // end of property PartialClassGenerationTask::Language
  .property /*1700003F*/ instance callconv(8) string
          OutputPath()
  {
    .custom /*0C000049:0A000017*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.RequiredAttribute/*01000025*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_OutputPath(string) /* 06000086 */
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_OutputPath() /* 06000085 */
  } // end of property PartialClassGenerationTask::OutputPath
  .property /*17000040*/ instance callconv(8) string
          MSBuildProjectDirectory()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_MSBuildProjectDirectory(string) /* 06000088 */
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_MSBuildProjectDirectory() /* 06000087 */
  } // end of property PartialClassGenerationTask::MSBuildProjectDirectory
  .property /*17000041*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          References()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_References(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 0600008A */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_References() /* 06000089 */
  } // end of property PartialClassGenerationTask::References
  .property /*17000042*/ instance callconv(8) string
          RootNamespace()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_RootNamespace() /* 0600008B */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_RootNamespace(string) /* 0600008C */
  } // end of property PartialClassGenerationTask::RootNamespace
  .property /*17000043*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          SourceCodeFiles()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_SourceCodeFiles(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 0600008E */
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_SourceCodeFiles() /* 0600008D */
  } // end of property PartialClassGenerationTask::SourceCodeFiles
  .property /*17000044*/ instance callconv(8) bool
          RequiresCompilationPass2()
  {
    .custom /*0C00004E:0A000016*/ instance void [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.OutputAttribute/*01000024*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .get instance bool Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_RequiresCompilationPass2() /* 0600008F */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_RequiresCompilationPass2(bool) /* 06000090 */
  } // end of property PartialClassGenerationTask::RequiresCompilationPass2
  .property /*17000045*/ instance callconv(8) string
          BuildTaskPath()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_BuildTaskPath() /* 06000091 */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_BuildTaskPath(string) /* 06000092 */
  } // end of property PartialClassGenerationTask::BuildTaskPath
  .property /*17000046*/ instance callconv(8) bool
          IsInProcessXamlMarkupCompile()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_IsInProcessXamlMarkupCompile(bool) /* 06000094 */
    .get instance bool Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_IsInProcessXamlMarkupCompile() /* 06000093 */
  } // end of property PartialClassGenerationTask::IsInProcessXamlMarkupCompile
  .property /*17000047*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          XamlBuildTypeGenerationExtensionNames()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_XamlBuildTypeGenerationExtensionNames() /* 06000095 */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_XamlBuildTypeGenerationExtensionNames(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000096 */
  } // end of property PartialClassGenerationTask::XamlBuildTypeGenerationExtensionNames
  .property /*17000048*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]
          XamlBuildTypeInspectionExtensionNames()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[] Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::get_XamlBuildTypeInspectionExtensionNames() /* 06000097 */
    .set instance void Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask/*0200000C*/::set_XamlBuildTypeInspectionExtensionNames(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/[]) /* 06000098 */
  } // end of property PartialClassGenerationTask::XamlBuildTypeInspectionExtensionNames
} // end of class Microsoft.Build.Tasks.Xaml.PartialClassGenerationTask

.class /*0200000D*/ public auto ansi sealed beforefieldinit Microsoft.Build.Tasks.Xaml.PropertyData
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*0600009A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method PropertyData::.ctor

  .method /*0600009B*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/> 
          get_Attributes() cil managed
  // SIG: 20 00 15 12 0D 01 12 08
  {
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Attributes

  .method /*0600009C*/ public hidebysig specialname 
          instance valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ 
          get_Visibility() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Visibility

  .method /*0600009D*/ public hidebysig specialname 
          instance void  set_Visibility(valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method PropertyData::set_Visibility

  .method /*0600009E*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008A:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Name

  .method /*0600009F*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00008B:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method PropertyData::set_Name

  .method /*060000A0*/ public hidebysig specialname 
          instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 
          get_Type() cil managed
  // SIG: 20 00 12 09
  {
    .custom /*0C00008C:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Type

  .method /*060000A1*/ public hidebysig specialname 
          instance void  set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ 'value') cil managed
  // SIG: 20 01 01 12 09
  {
    .custom /*0C00008D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method PropertyData::set_Type

  .property /*17000049*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/>
          Attributes()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000003*/<class Microsoft.Build.Tasks.Xaml.AttributeData/*02000002*/> Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::get_Attributes() /* 0600009B */
  } // end of property PropertyData::Attributes
  .property /*1700004A*/ instance callconv(8) valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/
          Visibility()
  {
    .custom /*0C000050:0A000018*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000026*/::.ctor(string) /* 0A000018 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/ Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::get_Visibility() /* 0600009C */
    .set instance void Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::set_Visibility(valuetype Microsoft.Build.Tasks.Xaml.MemberVisibility/*0200000A*/) /* 0600009D */
  } // end of property PropertyData::Visibility
  .property /*1700004B*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::get_Name() /* 0600009E */
    .set instance void Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::set_Name(string) /* 0600009F */
  } // end of property PropertyData::Name
  .property /*1700004C*/ instance callconv(8) class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/
          Type()
  {
    .set instance void Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::set_Type(class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/) /* 060000A1 */
    .get instance class [System.Xaml/*23000003*/]System.Xaml.XamlType/*01000002*/ Microsoft.Build.Tasks.Xaml.PropertyData/*0200000D*/::get_Type() /* 060000A0 */
  } // end of property PropertyData::Type
} // end of class Microsoft.Build.Tasks.Xaml.PropertyData

.class /*0200000E*/ public auto ansi sealed beforefieldinit Microsoft.Build.Tasks.Xaml.XamlBuildTypeGenerationExtensionContext
       extends Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/
{
  .method /*060000A2*/ public hidebysig specialname 
          instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/ 
          get_InputTaskItem() cil managed
  // SIG: 20 00 12 2D
  {
    .custom /*0C00008E:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method XamlBuildTypeGenerationExtensionContext::get_InputTaskItem

  .method /*060000A3*/ assembly hidebysig specialname 
          instance void  set_InputTaskItem(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    .custom /*0C00008F:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method XamlBuildTypeGenerationExtensionContext::set_InputTaskItem

  .method /*060000A4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method XamlBuildTypeGenerationExtensionContext::.ctor

  .property /*1700004D*/ instance callconv(8) class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/
          InputTaskItem()
  {
    .get instance class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/ Microsoft.Build.Tasks.Xaml.XamlBuildTypeGenerationExtensionContext/*0200000E*/::get_InputTaskItem() /* 060000A2 */
    .set instance void Microsoft.Build.Tasks.Xaml.XamlBuildTypeGenerationExtensionContext/*0200000E*/::set_InputTaskItem(class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/) /* 060000A3 */
  } // end of property XamlBuildTypeGenerationExtensionContext::InputTaskItem
} // end of class Microsoft.Build.Tasks.Xaml.XamlBuildTypeGenerationExtensionContext

.class /*0200000F*/ public auto ansi sealed beforefieldinit Microsoft.Build.Tasks.Xaml.XamlBuildTypeInspectionExtensionContext
       extends Microsoft.Build.Tasks.Xaml.BuildExtensionContext/*02000004*/
{
  .method /*060000A5*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000004*/<string,class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/> 
          get_MarkupItemsByTypeName() cil managed
  // SIG: 20 00 15 12 11 02 0E 12 2D
  {
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method XamlBuildTypeInspectionExtensionContext::get_MarkupItemsByTypeName

  .method /*060000A6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method XamlBuildTypeInspectionExtensionContext::.ctor

  .property /*1700004E*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000004*/<string,class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/>
          MarkupItemsByTypeName()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000004*/<string,class [Microsoft.Build.Framework/*23000004*/]Microsoft.Build.Framework.ITaskItem/*0100000B*/> Microsoft.Build.Tasks.Xaml.XamlBuildTypeInspectionExtensionContext/*0200000F*/::get_MarkupItemsByTypeName() /* 060000A5 */
  } // end of property XamlBuildTypeInspectionExtensionContext::MarkupItemsByTypeName
} // end of class Microsoft.Build.Tasks.Xaml.XamlBuildTypeInspectionExtensionContext

.class /*02000010*/ private auto ansi '<PrivateImplementationDetails>{F5EA9ED2-04FA-4CA1-B2E8-75B8A22937A0}'
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00001D:0A000015*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000023*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ static assembly class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100000D*/<string,int32> '$$method0x6000075-1'
} // end of class '<PrivateImplementationDetails>{F5EA9ED2-04FA-4CA1-B2E8-75B8A22937A0}'


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
