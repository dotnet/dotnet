
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
// Time-date stamp:            0x4ba1dfdb
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x08
// Minor linker version:           0x00
// Size of code:                   0x0000b000
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000cfba
// Base of code:                   0x00002000
// Base of data:                   0x0000e000
// Image base:                     0x60ba0000
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
// Checksum:                       0x0000ed8a
// Subsystem:                      0x0003
// DLL characteristics:            0x8540
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000cf68 [0x0000004f] address [size] of Import Directory:          
// 0x0000e000 [0x00000434] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000ba00 [0x00001758] address [size] of Security Directory:        
// 0x00010000 [0x0000000c] address [size] of Base Relocation Table:     
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
//              0x0000afc0 Virtual Size
//              0x00002000 Virtual Address
//              0x0000b000 Size of Raw Data
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
//              0x0000e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000b200 Pointer to Raw Data
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
//              0x0000b800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000c000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000fbc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000cfaa Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 BA 60 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000020b8 [0x00007ab0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00009b68 [0x00003380] address [size] of Resources Directory:       
// 0x0000cee8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000149c Size
//              '#~' Name
//    Stream 2:
//              0x00001508 Offset
//              0x00001320 Size
//              '#Strings' Name
//    Stream 3:
//              0x00002828 Offset
//              0x00002400 Size
//              '#US' Name
//    Stream 4:
//              0x00004c28 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00004c38 Offset
//              0x00002e78 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000005090fb45f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 53592
// PE header size       : 512 (496 used)    ( 0.96%)
// PE additional info   : 7223              (13.48%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.13%)
// CLR meta-data size  : 31408              (58.61%)
// CLR additional info : 13312              (24.84%)
// CLR method headers  : 104                ( 0.19%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 3.82%)
// Unaccounted          : -1087             (-2.03%)

// Num.of PE sections   : 3
//   .text    - 45056
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 31408
//   Module        -    1 (10 bytes)
//   TypeDef       -   24 (336 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   64 (384 bytes)
//   MethodDef     -  104 (1456 bytes)    0 abstract, 0 native, 104 bodies
//   FieldDef      -   95 (570 bytes)     0 constant
//   MemberRef     -   31 (186 bytes)
//   ParamDef      -   79 (474 bytes)
//   MethodImpl    -    1 (6 bytes)
//   Constant      -   88 (528 bytes)
//   CustomAttribute-   83 (498 bytes)
//   InterfaceImpl -    5 (20 bytes)
//   PropertyMap   -    8 (32 bytes)
//   Property      -   26 (156 bytes)
//   MethodSemantic-   35 (210 bytes)
//   Security      -    7 (42 bytes)
//   TypeSpec      -    2 (4 bytes)
//   ModuleRef     -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   ManifestResource-    1 (12 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    1 (6 bytes)
//   GenericParam  -   15 (120 bytes)
//   Strings       -  4893 bytes
//   Blobs         - 11896 bytes
//   UserStrings   -  9216 bytes
//   Guids         -    16 bytes
//   Uncategorized -   229 bytes

// CLR additional info : 13312
//   Resources         - 13184

// CLR method headers : 104
//   Num.of method bodies  - 104
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 104

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AddInAttribute                 (public) (auto) (ansi) (sealed) 
// Class AddInSegmentDirectoryNotFoundException (public) (auto) (ansi) 
// Class AddInSegmentType               (public) (auto) (ansi) (sealed) 
// Class AddInController                (public) (auto) (ansi) (sealed) 
// Class AddInEnvironment               (public) (auto) (ansi) (sealed) 
// Class Platform                       (public) (auto) (ansi) (sealed) 
// Class AddInProcess                   (public) (auto) (ansi) (sealed) 
// Class QualificationDataItem          (public) (sequential) (ansi) (sealed) 
// Class AddInSecurityLevel             (public) (auto) (ansi) (sealed) 
// Class AddInStore                     (public) (abstract) (auto) (ansi) (sealed) 
// Class AddInToken                     (public) (auto) (ansi) (sealed) 
// Class InvalidPipelineStoreException  (public) (auto) (ansi) 
// Class PipelineStoreLocation          (public) (auto) (ansi) (sealed) 
// Class HostAdapterAttribute           (public) (auto) (ansi) (sealed) 
// Class AddInAdapterAttribute          (public) (auto) (ansi) (sealed) 
// Class AddInBaseAttribute             (public) (auto) (ansi) (sealed) 
// Class CollectionAdapters             (public) (abstract) (auto) (ansi) (sealed) 
// Class ContractAdapter                (public) (abstract) (auto) (ansi) (sealed) 
// Class ContractBase                   (public) (auto) (ansi) 
// Class ContractHandle                 (public) (auto) (ansi) 
// Class FXAssembly                     (abstract) (auto) (ansi) (sealed) 
// Class ThisAssembly                   (abstract) (auto) (ansi) (sealed) 
// Class AssemblyRef                    (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.module extern KERNEL32.DLL /*1A000001*/
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.Remoting
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000003*/ System.AddIn.Contract
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ noplatform System.AddIn
{
  .custom /*0C000002:0A000002*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*0100001D*/::.ctor(string) /* 0A000002 */ = ( 01 00 24 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..$f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 45 63 6D 61 50 75 62 6C 69 63   // evdiv\EcmaPublic
                                                                                                                                                             4B 65 79 2E 73 6E 6B 00 00 )                      // Key.snk..
  .custom /*0C000003:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*0100001F*/::.ctor(bool) /* 0A000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000005:0A000005*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000020*/::.ctor() /* 0A000005 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C000006:0A000006*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.StringFreezingAttribute/*01000021*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000007*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.DefaultDependencyAttribute/*01000022*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.CompilerServices.LoadHint/*01000023*/) /* 0A000007 */ = ( 01 00 01 00 00 00 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000008:0A000008*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000024*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000024*//DebuggingModes/*01000025*/) /* 0A000008 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000009:0A000009*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000026*/::.ctor(int32) /* 0A000009 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C00000A:0A00000A*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000027*/::.ctor(string) /* 0A00000A */ = ( 01 00 10 53 79 73 74 65 6D 2E 41 64 64 49 6E 2E   // ...System.AddIn.
                                                                                                                                                                  64 6C 6C 00 00 )                                  // dll..
  .custom /*0C00000B:0A00000B*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000028*/::.ctor(string) /* 0A00000B */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C00000C:0A00000C*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000029*/::.ctor(bool) /* 0A00000C */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A00000D*/ instance void [mscorlib/*23000001*/]System.Security.AllowPartiallyTrustedCallersAttribute/*0100002A*/::.ctor() /* 0A00000D */ = ( 01 00 00 00 ) 
  .custom /*0C00000E:0A00000E*/ instance void [mscorlib/*23000001*/]System.Security.SecurityRulesAttribute/*0100002B*/::.ctor(valuetype [mscorlib/*23000001*/]System.Security.SecurityRuleSet/*0100002C*/) /* 0A00000E */ = ( 01 00 01 01 00 54 02 1B 53 6B 69 70 56 65 72 69   // .....T..SkipVeri
                                                                                                                                                                                                                              66 69 63 61 74 69 6F 6E 49 6E 46 75 6C 6C 54 72   // ficationInFullTr
                                                                                                                                                                                                                              75 73 74 01 )                                     // ust.
  .custom /*0C00000F:0A00000F*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100002D*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 53 79 73 74 65 6D 2E 41 64 64 49 6E 2E   // ...System.AddIn.
                                                                                                                                                           64 6C 6C 00 00 )                                  // dll..
  .custom /*0C000010:0A000010*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100002E*/::.ctor(string) /* 0A000010 */ = ( 01 00 10 53 79 73 74 65 6D 2E 41 64 64 49 6E 2E   // ...System.AddIn.
                                                                                                                                                                 64 6C 6C 00 00 )                                  // dll..
  .custom /*0C000011:0A000011*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*0100002F*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000012:0A000012*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000030*/::.ctor(string) /* 0A000012 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000013:0A000013*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000031*/::.ctor(string) /* 0A000013 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C000014:0A000014*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000032*/::.ctor(string) /* 0A000014 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C000015:0A000015*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000033*/::.ctor(string) /* 0A000015 */ = ( 01 00 0B 34 2E 30 2E 33 30 33 31 39 2E 31 00 00 ) // ...4.0.30319.1..
  .custom /*0C000016:0A000016*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000034*/::.ctor(string) /* 0A000016 */ = ( 01 00 0B 34 2E 30 2E 33 30 33 31 39 2E 31 00 00 ) // ...4.0.30319.1..
  .custom /*0C000017:0A000017*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*01000035*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000018:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.AssemblyTargetedPatchBandAttribute/*01000036*/::.ctor(string) /* 0A000018 */ = ( 01 00 08 31 2E 30 2E 32 31 2D 30 00 00 )          // ...1.0.21-0..
  .custom /*0C000019:0A000019*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000037*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .permissionset reqmin
             = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SkipVerification' = bool(true)}}
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.module System.AddIn.dll
// MVID: {345317c2-295b-4d12-a0cb-9c0646a46e50}
.custom /*0C000001:0A00001A*/ instance void [mscorlib/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000038*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
.imagebase 0x60ba0000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FD5DA4A7000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.AddIn.AddInAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000001*/
{
  .custom /*0C00001B:0A00001B*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000039*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003A*/) /* 0A00001B */ = ( 01 00 04 00 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method AddInAttribute::.ctor

  .method /*06000002*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00001A:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method AddInAttribute::get_Name

  .method /*06000003*/ public hidebysig specialname 
          instance string  get_Publisher() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00001C:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method AddInAttribute::get_Publisher

  .method /*06000004*/ public hidebysig specialname 
          instance void  set_Publisher(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00001D:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method AddInAttribute::set_Publisher

  .method /*06000005*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00001E:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method AddInAttribute::get_Version

  .method /*06000006*/ public hidebysig specialname 
          instance void  set_Version(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00001F:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method AddInAttribute::set_Version

  .method /*06000007*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000020:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method AddInAttribute::get_Description

  .method /*06000008*/ public hidebysig specialname 
          instance void  set_Description(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000021:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method AddInAttribute::set_Description

  .property /*17000001*/ instance callconv(8) string
          Name()
  {
    .get instance string System.AddIn.AddInAttribute/*02000002*/::get_Name() /* 06000002 */
  } // end of property AddInAttribute::Name
  .property /*17000002*/ instance callconv(8) string
          Publisher()
  {
    .set instance void System.AddIn.AddInAttribute/*02000002*/::set_Publisher(string) /* 06000004 */
    .get instance string System.AddIn.AddInAttribute/*02000002*/::get_Publisher() /* 06000003 */
  } // end of property AddInAttribute::Publisher
  .property /*17000003*/ instance callconv(8) string
          Version()
  {
    .set instance void System.AddIn.AddInAttribute/*02000002*/::set_Version(string) /* 06000006 */
    .get instance string System.AddIn.AddInAttribute/*02000002*/::get_Version() /* 06000005 */
  } // end of property AddInAttribute::Version
  .property /*17000004*/ instance callconv(8) string
          Description()
  {
    .set instance void System.AddIn.AddInAttribute/*02000002*/::set_Description(string) /* 06000008 */
    .get instance string System.AddIn.AddInAttribute/*02000002*/::get_Description() /* 06000007 */
  } // end of property AddInAttribute::Description
} // end of class System.AddIn.AddInAttribute

.class /*02000003*/ public auto ansi serializable beforefieldinit System.AddIn.Hosting.AddInSegmentDirectoryNotFoundException
       extends [mscorlib/*23000001*/]System.Exception/*01000002*/
{
  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000022:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method AddInSegmentDirectoryNotFoundException::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000002*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 09
  {
    .custom /*0C000023:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method AddInSegmentDirectoryNotFoundException::.ctor

  .method /*0600000B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000003*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000004*/ context) cil managed
  // SIG: 20 02 01 12 0D 11 11
  {
    .custom /*0C000024:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method AddInSegmentDirectoryNotFoundException::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000025:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method AddInSegmentDirectoryNotFoundException::.ctor

} // end of class System.AddIn.Hosting.AddInSegmentDirectoryNotFoundException

.class /*02000004*/ public auto ansi sealed System.AddIn.Hosting.AddInSegmentType
       extends [mscorlib/*23000001*/]System.Enum/*01000005*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ HostViewOfAddIn = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ HostSideAdapter = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ Contract = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ AddInSideAdapter = int32(0x00000003)
  .field /*04000006*/ public static literal valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ AddInView = int32(0x00000004)
  .field /*04000007*/ public static literal valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ AddIn = int32(0x00000005)
} // end of class System.AddIn.Hosting.AddInSegmentType

.class /*02000005*/ public auto ansi sealed beforefieldinit System.AddIn.Hosting.AddInController
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .method /*0600000D*/ public hidebysig static 
          class System.AddIn.Hosting.AddInController/*02000005*/ 
          GetAddInController(object addIn) cil managed
  // SIG: 00 01 12 14 1C
  {
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method AddInController::GetAddInController

  .method /*0600000E*/ public hidebysig instance void 
          Shutdown() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000026:0A00001D*/ instance void [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.ReliabilityContractAttribute/*0100003C*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Consistency/*0100003D*/,
                                                                                                                                                          valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Cer/*0100003E*/) /* 0A00001D */ = ( 01 00 03 00 00 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method AddInController::Shutdown

  .method /*0600000F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.AppDomain/*01000007*/ 
          get_AppDomain() cil managed
  // SIG: 20 00 12 1D
  {
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method AddInController::get_AppDomain

  .method /*06000010*/ public hidebysig specialname 
          instance class System.AddIn.Hosting.AddInToken/*0200000C*/ 
          get_Token() cil managed
  // SIG: 20 00 12 30
  {
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method AddInController::get_Token

  .method /*06000011*/ public hidebysig specialname 
          instance class System.AddIn.Hosting.AddInEnvironment/*02000006*/ 
          get_AddInEnvironment() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method AddInController::get_AddInEnvironment

  .property /*17000005*/ instance callconv(8) class [mscorlib/*23000001*/]System.AppDomain/*01000007*/
          AppDomain()
  {
    .get instance class [mscorlib/*23000001*/]System.AppDomain/*01000007*/ System.AddIn.Hosting.AddInController/*02000005*/::get_AppDomain() /* 0600000F */
  } // end of property AddInController::AppDomain
  .property /*17000006*/ instance callconv(8) class System.AddIn.Hosting.AddInToken/*0200000C*/
          Token()
  {
    .get instance class System.AddIn.Hosting.AddInToken/*0200000C*/ System.AddIn.Hosting.AddInController/*02000005*/::get_Token() /* 06000010 */
  } // end of property AddInController::Token
  .property /*17000007*/ instance callconv(8) class System.AddIn.Hosting.AddInEnvironment/*02000006*/
          AddInEnvironment()
  {
    .get instance class System.AddIn.Hosting.AddInEnvironment/*02000006*/ System.AddIn.Hosting.AddInController/*02000005*/::get_AddInEnvironment() /* 06000011 */
  } // end of property AddInController::AddInEnvironment
} // end of class System.AddIn.Hosting.AddInController

.class /*02000006*/ public auto ansi sealed beforefieldinit System.AddIn.Hosting.AddInEnvironment
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.AppDomain/*01000007*/ appDomain) cil managed
  // SIG: 20 01 01 12 1D
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method AddInEnvironment::.ctor

  .method /*06000013*/ public hidebysig specialname 
          instance class System.AddIn.Hosting.AddInProcess/*02000008*/ 
          get_Process() cil managed
  // SIG: 20 00 12 20
  {
    .custom /*0C00002A:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method AddInEnvironment::get_Process

  .property /*17000008*/ instance callconv(8) class System.AddIn.Hosting.AddInProcess/*02000008*/
          Process()
  {
    .get instance class System.AddIn.Hosting.AddInProcess/*02000008*/ System.AddIn.Hosting.AddInEnvironment/*02000006*/::get_Process() /* 06000013 */
  } // end of property AddInEnvironment::Process
} // end of class System.AddIn.Hosting.AddInEnvironment

.class /*02000007*/ public auto ansi serializable sealed System.AddIn.Hosting.Platform
       extends [mscorlib/*23000001*/]System.Enum/*01000005*/
{
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.AddIn.Hosting.Platform/*02000007*/ Host = int32(0x00000000)
  .field /*0400000A*/ public static literal valuetype System.AddIn.Hosting.Platform/*02000007*/ AnyCpu = int32(0x00000001)
  .field /*0400000B*/ public static literal valuetype System.AddIn.Hosting.Platform/*02000007*/ X86 = int32(0x00000002)
  .field /*0400000C*/ public static literal valuetype System.AddIn.Hosting.Platform/*02000007*/ X64 = int32(0x00000003)
} // end of class System.AddIn.Hosting.Platform

.class /*02000008*/ public auto ansi sealed beforefieldinit System.AddIn.Hosting.AddInProcess
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .method /*06000014*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000A*/ 
          get_StartupTimeout() cil managed
  // SIG: 20 00 11 29
  {
    .custom /*0C00002B:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method AddInProcess::get_StartupTimeout

  .method /*06000015*/ public hidebysig specialname 
          instance void  set_StartupTimeout(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000A*/ 'value') cil managed
  // SIG: 20 01 01 11 29
  {
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method AddInProcess::set_StartupTimeout

  .method /*06000016*/ public hidebysig specialname 
          instance valuetype System.AddIn.Hosting.Platform/*02000007*/ 
          get_Platform() cil managed
  // SIG: 20 00 11 1C
  {
    .custom /*0C00002C:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method AddInProcess::get_Platform

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00002D:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C00002E:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .permissionset demand
               = {class 'System.Security.Permissions.PermissionSetAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property string 'Name' = string('FullTrust')}}
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method AddInProcess::.ctor

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.AddIn.Hosting.Platform/*02000007*/ platform) cil managed
  // SIG: 20 01 01 11 1C
  {
    .custom /*0C00002F:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .permissionset demand
               = {class 'System.Security.Permissions.PermissionSetAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property string 'Name' = string('FullTrust')}}
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method AddInProcess::.ctor

  .method /*06000019*/ public hidebysig specialname 
          instance int32  get_ProcessId() cil managed
  // SIG: 20 00 08
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.PermissionSetAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property string 'Name' = string('FullTrust')}}
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method AddInProcess::get_ProcessId

  .method /*0600001A*/ public hidebysig specialname 
          instance bool  get_IsCurrentProcess() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method AddInProcess::get_IsCurrentProcess

  .method /*0600001B*/ public hidebysig specialname 
          instance bool  get_KeepAlive() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000030:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method AddInProcess::get_KeepAlive

  .method /*0600001C*/ public hidebysig specialname 
          instance void  set_KeepAlive(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000031:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method AddInProcess::set_KeepAlive

  .method /*0600001D*/ public hidebysig specialname 
          instance void  add_ShuttingDown(class [mscorlib/*23000001*/]System.EventHandler`1/*01000008*/<class [System/*23000004*/]System.ComponentModel.CancelEventArgs/*01000009*/> 'value') cil managed
  // SIG: 20 01 01 15 12 21 01 12 25
  {
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method AddInProcess::add_ShuttingDown

  .method /*0600001E*/ public hidebysig specialname 
          instance void  remove_ShuttingDown(class [mscorlib/*23000001*/]System.EventHandler`1/*01000008*/<class [System/*23000004*/]System.ComponentModel.CancelEventArgs/*01000009*/> 'value') cil managed
  // SIG: 20 01 01 15 12 21 01 12 25
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method AddInProcess::remove_ShuttingDown

  .method /*0600001F*/ public hidebysig instance bool 
          Start() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000032:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method AddInProcess::Start

  .method /*06000020*/ public hidebysig instance bool 
          Shutdown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method AddInProcess::Shutdown

  .method /*06000021*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method AddInProcess::.cctor

  .event /*14000001*/ class [mscorlib/*23000001*/]System.EventHandler`1/*01000008*/<class [System/*23000004*/]System.ComponentModel.CancelEventArgs/*01000009*/>/*1B000001*/ ShuttingDown
  {
    .addon instance void System.AddIn.Hosting.AddInProcess/*02000008*/::add_ShuttingDown(class [mscorlib/*23000001*/]System.EventHandler`1/*01000008*/<class [System/*23000004*/]System.ComponentModel.CancelEventArgs/*01000009*/>) /* 0600001D */
    .removeon instance void System.AddIn.Hosting.AddInProcess/*02000008*/::remove_ShuttingDown(class [mscorlib/*23000001*/]System.EventHandler`1/*01000008*/<class [System/*23000004*/]System.ComponentModel.CancelEventArgs/*01000009*/>) /* 0600001E */
  } // end of event AddInProcess::ShuttingDown
  .property /*17000009*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000A*/
          StartupTimeout()
  {
    .set instance void System.AddIn.Hosting.AddInProcess/*02000008*/::set_StartupTimeout(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000A*/) /* 06000015 */
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000A*/ System.AddIn.Hosting.AddInProcess/*02000008*/::get_StartupTimeout() /* 06000014 */
  } // end of property AddInProcess::StartupTimeout
  .property /*1700000A*/ instance callconv(8) valuetype System.AddIn.Hosting.Platform/*02000007*/
          Platform()
  {
    .get instance valuetype System.AddIn.Hosting.Platform/*02000007*/ System.AddIn.Hosting.AddInProcess/*02000008*/::get_Platform() /* 06000016 */
  } // end of property AddInProcess::Platform
  .property /*1700000B*/ instance callconv(8) int32
          ProcessId()
  {
    .get instance int32 System.AddIn.Hosting.AddInProcess/*02000008*/::get_ProcessId() /* 06000019 */
  } // end of property AddInProcess::ProcessId
  .property /*1700000C*/ instance callconv(8) bool
          IsCurrentProcess()
  {
    .get instance bool System.AddIn.Hosting.AddInProcess/*02000008*/::get_IsCurrentProcess() /* 0600001A */
  } // end of property AddInProcess::IsCurrentProcess
  .property /*1700000D*/ instance callconv(8) bool
          KeepAlive()
  {
    .get instance bool System.AddIn.Hosting.AddInProcess/*02000008*/::get_KeepAlive() /* 0600001B */
    .set instance void System.AddIn.Hosting.AddInProcess/*02000008*/::set_KeepAlive(bool) /* 0600001C */
  } // end of property AddInProcess::KeepAlive
} // end of class System.AddIn.Hosting.AddInProcess

.class /*02000009*/ public sequential ansi serializable sealed beforefieldinit System.AddIn.Hosting.QualificationDataItem
       extends [mscorlib/*23000001*/]System.ValueType/*0100000B*/
{
  .field /*0400000D*/ private valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ m_type
  .field /*0400000E*/ private string m_key
  .field /*0400000F*/ private string m_value
  .method /*06000022*/ public hidebysig specialname 
          instance valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ 
          get_Segment() cil managed
  // SIG: 20 00 11 10
  {
    .custom /*0C000034:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::get_Segment

  .method /*06000023*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000035:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::get_Name

  .method /*06000024*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000037:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::get_Value

  .method /*06000025*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::Equals

  .method /*06000026*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::GetHashCode

  .method /*06000027*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.AddIn.Hosting.QualificationDataItem/*02000009*/ item1,
                            valuetype System.AddIn.Hosting.QualificationDataItem/*02000009*/ item2) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::op_Equality

  .method /*06000028*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.AddIn.Hosting.QualificationDataItem/*02000009*/ item1,
                              valuetype System.AddIn.Hosting.QualificationDataItem/*02000009*/ item2) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method QualificationDataItem::op_Inequality

  .property /*1700000E*/ instance callconv(8) valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/
          Segment()
  {
    .get instance valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/ System.AddIn.Hosting.QualificationDataItem/*02000009*/::get_Segment() /* 06000022 */
  } // end of property QualificationDataItem::Segment
  .property /*1700000F*/ instance callconv(8) string
          Name()
  {
    .get instance string System.AddIn.Hosting.QualificationDataItem/*02000009*/::get_Name() /* 06000023 */
  } // end of property QualificationDataItem::Name
  .property /*17000010*/ instance callconv(8) string
          Value()
  {
    .get instance string System.AddIn.Hosting.QualificationDataItem/*02000009*/::get_Value() /* 06000024 */
  } // end of property QualificationDataItem::Value
} // end of class System.AddIn.Hosting.QualificationDataItem

.class /*0200000A*/ public auto ansi serializable sealed System.AddIn.Hosting.AddInSecurityLevel
       extends [mscorlib/*23000001*/]System.Enum/*01000005*/
{
  .field /*04000010*/ public specialname rtspecialname int32 value__
  .field /*04000011*/ public static literal valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ Internet = int32(0x00000000)
  .field /*04000012*/ public static literal valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ Intranet = int32(0x00000001)
  .field /*04000013*/ public static literal valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ FullTrust = int32(0x00000002)
  .field /*04000014*/ public static literal valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ Host = int32(0x00000003)
} // end of class System.AddIn.Hosting.AddInSecurityLevel

.class /*0200000B*/ public abstract auto ansi sealed beforefieldinit System.AddIn.Hosting.AddInStore
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .method /*06000029*/ public hidebysig static 
          string[]  Update(valuetype System.AddIn.Hosting.PipelineStoreLocation/*0200000E*/ location) cil managed
  // SIG: 00 01 1D 0E 11 38
  {
    .custom /*0C000038:0A00001E*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*0100003F*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method AddInStore::Update

  .method /*0600002A*/ public hidebysig static 
          string[]  Update(string pipelineRootFolderPath) cil managed
  // SIG: 00 01 1D 0E 0E
  {
    .custom /*0C000039:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C00003A:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method AddInStore::Update

  .method /*0600002B*/ public hidebysig static 
          string[]  UpdateAddIns(string addInsFolderPath) cil managed
  // SIG: 00 01 1D 0E 0E
  {
    .custom /*0C00003B:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method AddInStore::UpdateAddIns

  .method /*0600002C*/ public hidebysig static 
          string[]  RebuildAddIns(string addInsFolderPath) cil managed
  // SIG: 00 01 1D 0E 0E
  {
    .custom /*0C00003C:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method AddInStore::RebuildAddIns

  .method /*0600002D*/ public hidebysig static 
          string[]  Rebuild(string pipelineRootFolderPath) cil managed
  // SIG: 00 01 1D 0E 0E
  {
    .custom /*0C00003D:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C00003E:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method AddInStore::Rebuild

  .method /*0600002E*/ public hidebysig static 
          string[]  Rebuild(valuetype System.AddIn.Hosting.PipelineStoreLocation/*0200000E*/ location) cil managed
  // SIG: 00 01 1D 0E 11 38
  {
    .custom /*0C00003F:0A00001E*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*0100003F*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method AddInStore::Rebuild

  .method /*0600002F*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100000C*/<class System.AddIn.Hosting.AddInToken/*0200000C*/> 
          FindAddIns(class [mscorlib/*23000001*/]System.Type/*0100000D*/ hostViewOfAddIn,
                     valuetype System.AddIn.Hosting.PipelineStoreLocation/*0200000E*/ location) cil managed
  // SIG: 00 02 15 12 31 01 12 30 12 35 11 38
  {
    .custom /*0C000040:0A00001E*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*0100003F*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method AddInStore::FindAddIns

  .method /*06000030*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100000C*/<class System.AddIn.Hosting.AddInToken/*0200000C*/> 
          FindAddIns(class [mscorlib/*23000001*/]System.Type/*0100000D*/ hostViewOfAddIn,
                     valuetype System.AddIn.Hosting.PipelineStoreLocation/*0200000E*/ location,
                     string[] addInFolderPaths) cil managed
  // SIG: 00 03 15 12 31 01 12 30 12 35 11 38 1D 0E
  {
    .custom /*0C000041:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .param [3]/*08000020*/ 
    .custom /*0C000033:0A00001F*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000040*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method AddInStore::FindAddIns

  .method /*06000031*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100000C*/<class System.AddIn.Hosting.AddInToken/*0200000C*/> 
          FindAddIns(class [mscorlib/*23000001*/]System.Type/*0100000D*/ hostViewOfAddIn,
                     string pipelineRootFolderPath,
                     string[] addInFolderPaths) cil managed
  // SIG: 00 03 15 12 31 01 12 30 12 35 0E 1D 0E
  {
    .custom /*0C000042:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000043:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .param [3]/*08000023*/ 
    .custom /*0C000036:0A00001F*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000040*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method AddInStore::FindAddIns

  .method /*06000032*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100000C*/<class System.AddIn.Hosting.AddInToken/*0200000C*/> 
          FindAddIn(class [mscorlib/*23000001*/]System.Type/*0100000D*/ hostViewOfAddIn,
                    string pipelineRootFolderPath,
                    string addInFilePath,
                    string addInTypeName) cil managed
  // SIG: 00 04 15 12 31 01 12 30 12 35 0E 0E 0E
  {
    .custom /*0C000044:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method AddInStore::FindAddIn

  .method /*06000033*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method AddInStore::.cctor

} // end of class System.AddIn.Hosting.AddInStore

.class /*0200000C*/ public auto ansi serializable sealed beforefieldinit System.AddIn.Hosting.AddInToken
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
       implements class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000010*/<valuetype System.AddIn.Hosting.QualificationDataItem/*02000009*/>/*1B000002*/,
                  [mscorlib/*23000001*/]System.Collections.IEnumerable/*0100000E*/
{
  .method /*06000034*/ public hidebysig specialname static 
          bool  get_EnableDirectConnect() cil managed
  // SIG: 00 00 02
  {
    .custom /*0C000045:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method AddInToken::get_EnableDirectConnect

  .method /*06000035*/ public hidebysig specialname static 
          void  set_EnableDirectConnect(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    .custom /*0C000046:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method AddInToken::set_EnableDirectConnect

  .method /*06000036*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000011*/<valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/,class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000011*/<string,string>> 
          get_QualificationData() cil managed
  // SIG: 20 00 15 12 45 02 11 10 15 12 45 02 0E 0E
  {
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method AddInToken::get_QualificationData

  .method /*06000037*/ private hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100000F*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 3D
  {
    .custom /*0C000047:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*0100000E*/::GetEnumerator /*0100000E::0A000001*/ 
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method AddInToken::System.Collections.IEnumerable.GetEnumerator

  .method /*06000038*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000012*/<valuetype System.AddIn.Hosting.QualificationDataItem/*02000009*/> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 49 01 11 24
  {
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method AddInToken::GetEnumerator

  .method /*06000039*/ public hidebysig specialname 
          instance string  get_AddInFullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method AddInToken::get_AddInFullName

  .method /*0600003A*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method AddInToken::get_Name

  .method /*0600003B*/ public hidebysig specialname 
          instance string  get_Publisher() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method AddInToken::get_Publisher

  .method /*0600003C*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method AddInToken::get_Version

  .method /*0600003D*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method AddInToken::get_Description

  .method /*0600003E*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method AddInToken::ToString

  .method /*0600003F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*01000013*/ 
          get_AssemblyName() cil managed
  // SIG: 20 00 12 4D
  {
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method AddInToken::get_AssemblyName

  .method /*06000040*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method AddInToken::Equals

  .method /*06000041*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method AddInToken::GetHashCode

  .method /*06000042*/ public hidebysig instance !!T 
          Activate<T>(valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ trustLevel) cil managed
  // SIG: 30 01 01 1E 00 11 28
  {
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .method /*06000043*/ public hidebysig instance !!T 
          Activate<T>(valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ trustLevel,
                      string appDomainName) cil managed
  // SIG: 30 01 02 1E 00 11 28 0E
  {
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .method /*06000044*/ public hidebysig instance !!T 
          Activate<T>(class [mscorlib/*23000001*/]System.AppDomain/*01000007*/ target) cil managed
  // SIG: 30 01 01 1E 00 12 1D
  {
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .method /*06000045*/ public hidebysig instance !!T 
          Activate<T>(class [mscorlib/*23000001*/]System.Security.PermissionSet/*01000014*/ permissions) cil managed
  // SIG: 30 01 01 1E 00 12 51
  {
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .method /*06000046*/ public hidebysig instance !!T 
          Activate<T>(class System.AddIn.Hosting.AddInProcess/*02000008*/ process,
                      class [mscorlib/*23000001*/]System.Security.PermissionSet/*01000014*/ permissionSet) cil managed
  // SIG: 30 01 02 1E 00 12 20 12 51
  {
    .permissionset demand
               = {class 'System.Security.Permissions.PermissionSetAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property string 'Name' = string('FullTrust')}}
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .method /*06000047*/ public hidebysig instance !!T 
          Activate<T>(class System.AddIn.Hosting.AddInProcess/*02000008*/ process,
                      valuetype System.AddIn.Hosting.AddInSecurityLevel/*0200000A*/ level) cil managed
  // SIG: 30 01 02 1E 00 12 20 11 28
  {
    .permissionset demand
               = {class 'System.Security.Permissions.PermissionSetAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property string 'Name' = string('FullTrust')}}
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .method /*06000048*/ public hidebysig instance !!T 
          Activate<T>(class System.AddIn.Hosting.AddInEnvironment/*02000006*/ environment) cil managed
  // SIG: 30 01 01 1E 00 12 18
  {
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method AddInToken::Activate

  .property /*17000011*/ callconv(8) bool
          EnableDirectConnect()
  {
    .set void System.AddIn.Hosting.AddInToken/*0200000C*/::set_EnableDirectConnect(bool) /* 06000035 */
    .get bool System.AddIn.Hosting.AddInToken/*0200000C*/::get_EnableDirectConnect() /* 06000034 */
  } // end of property AddInToken::EnableDirectConnect
  .property /*17000012*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000011*/<valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/,class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000011*/<string,string>>
          QualificationData()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000011*/<valuetype System.AddIn.Hosting.AddInSegmentType/*02000004*/,class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000011*/<string,string>> System.AddIn.Hosting.AddInToken/*0200000C*/::get_QualificationData() /* 06000036 */
  } // end of property AddInToken::QualificationData
  .property /*17000013*/ instance callconv(8) string
          AddInFullName()
  {
    .get instance string System.AddIn.Hosting.AddInToken/*0200000C*/::get_AddInFullName() /* 06000039 */
  } // end of property AddInToken::AddInFullName
  .property /*17000014*/ instance callconv(8) string
          Name()
  {
    .get instance string System.AddIn.Hosting.AddInToken/*0200000C*/::get_Name() /* 0600003A */
  } // end of property AddInToken::Name
  .property /*17000015*/ instance callconv(8) string
          Publisher()
  {
    .get instance string System.AddIn.Hosting.AddInToken/*0200000C*/::get_Publisher() /* 0600003B */
  } // end of property AddInToken::Publisher
  .property /*17000016*/ instance callconv(8) string
          Version()
  {
    .get instance string System.AddIn.Hosting.AddInToken/*0200000C*/::get_Version() /* 0600003C */
  } // end of property AddInToken::Version
  .property /*17000017*/ instance callconv(8) string
          Description()
  {
    .get instance string System.AddIn.Hosting.AddInToken/*0200000C*/::get_Description() /* 0600003D */
  } // end of property AddInToken::Description
  .property /*17000018*/ instance callconv(8) class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*01000013*/
          AssemblyName()
  {
    .get instance class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*01000013*/ System.AddIn.Hosting.AddInToken/*0200000C*/::get_AssemblyName() /* 0600003F */
  } // end of property AddInToken::AssemblyName
} // end of class System.AddIn.Hosting.AddInToken

.class /*0200000D*/ public auto ansi serializable beforefieldinit System.AddIn.Hosting.InvalidPipelineStoreException
       extends [mscorlib/*23000001*/]System.Exception/*01000002*/
{
  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000048:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method InvalidPipelineStoreException::.ctor

  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000002*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 09
  {
    .custom /*0C000049:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method InvalidPipelineStoreException::.ctor

  .method /*0600004B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000003*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000004*/ context) cil managed
  // SIG: 20 02 01 12 0D 11 11
  {
    .custom /*0C00004A:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method InvalidPipelineStoreException::.ctor

  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method InvalidPipelineStoreException::.ctor

} // end of class System.AddIn.Hosting.InvalidPipelineStoreException

.class /*0200000E*/ public auto ansi serializable sealed System.AddIn.Hosting.PipelineStoreLocation
       extends [mscorlib/*23000001*/]System.Enum/*01000005*/
{
  .field /*04000015*/ public specialname rtspecialname int32 value__
  .field /*04000016*/ public static literal valuetype System.AddIn.Hosting.PipelineStoreLocation/*0200000E*/ ApplicationBase = int32(0x00000000)
} // end of class System.AddIn.Hosting.PipelineStoreLocation

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.AddIn.Pipeline.HostAdapterAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000001*/
{
  .custom /*0C000027:0A00001B*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000039*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003A*/) /* 0A00001B */ = ( 01 00 04 00 00 00 00 00 ) 
  .method /*0600004D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00004B:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method HostAdapterAttribute::.ctor

} // end of class System.AddIn.Pipeline.HostAdapterAttribute

.class /*02000010*/ public auto ansi sealed beforefieldinit System.AddIn.Pipeline.AddInAdapterAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000001*/
{
  .custom /*0C000028:0A00001B*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000039*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003A*/) /* 0A00001B */ = ( 01 00 04 00 00 00 00 00 ) 
  .method /*0600004E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00004C:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method AddInAdapterAttribute::.ctor

} // end of class System.AddIn.Pipeline.AddInAdapterAttribute

.class /*02000011*/ public auto ansi sealed beforefieldinit System.AddIn.Pipeline.AddInBaseAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000001*/
{
  .custom /*0C000029:0A00001B*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000039*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003A*/) /* 0A00001B */ = ( 01 00 04 04 00 00 00 00 ) 
  .method /*0600004F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*0100000D*/[] 
          get_ActivatableAs() cil managed
  // SIG: 20 00 1D 12 35
  {
    .custom /*0C00004D:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method AddInBaseAttribute::get_ActivatableAs

  .method /*06000050*/ public hidebysig specialname 
          instance void  set_ActivatableAs(class [mscorlib/*23000001*/]System.Type/*0100000D*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 35
  {
    .custom /*0C00004E:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method AddInBaseAttribute::set_ActivatableAs

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00004F:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method AddInBaseAttribute::.ctor

  .property /*17000019*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*0100000D*/[]
          ActivatableAs()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*0100000D*/[] System.AddIn.Pipeline.AddInBaseAttribute/*02000011*/::get_ActivatableAs() /* 0600004F */
    .set instance void System.AddIn.Pipeline.AddInBaseAttribute/*02000011*/::set_ActivatableAs(class [mscorlib/*23000001*/]System.Type/*0100000D*/[]) /* 06000050 */
  } // end of property AddInBaseAttribute::ActivatableAs
} // end of class System.AddIn.Pipeline.AddInBaseAttribute

.class /*02000012*/ public abstract auto ansi sealed beforefieldinit System.AddIn.Pipeline.CollectionAdapters
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .method /*06000052*/ public hidebysig static 
          class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IListContract`1/*01000015*/<!!TContract> 
          ToIListContract<TView,TContract>(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000016*/<!!TView> collection,
                                           class [mscorlib/*23000001*/]System.Converter`2/*01000017*/<!!TView,!!TContract> viewContractAdapter,
                                           class [mscorlib/*23000001*/]System.Converter`2/*01000017*/<!!TContract,!!TView> contractViewAdapter) cil managed
  // SIG: 10 02 03 15 12 55 01 1E 01 15 12 59 01 1E 00 15 12 5D 02 1E 00 1E 01 15 12 5D 02 1E 01 1E 00
  {
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method CollectionAdapters::ToIListContract

  .method /*06000053*/ public hidebysig static 
          class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IListContract`1/*01000015*/<!!T> 
          ToIListContract<T>(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000016*/<!!T> collection) cil managed
  // SIG: 10 01 01 15 12 55 01 1E 00 15 12 59 01 1E 00
  {
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method CollectionAdapters::ToIListContract

  .method /*06000054*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000016*/<!!TView> 
          ToIList<TContract,TView>(class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IListContract`1/*01000015*/<!!TContract> collection,
                                   class [mscorlib/*23000001*/]System.Converter`2/*01000017*/<!!TContract,!!TView> contractViewAdapter,
                                   class [mscorlib/*23000001*/]System.Converter`2/*01000017*/<!!TView,!!TContract> viewContractAdapter) cil managed
  // SIG: 10 02 03 15 12 59 01 1E 01 15 12 55 01 1E 00 15 12 5D 02 1E 00 1E 01 15 12 5D 02 1E 01 1E 00
  {
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method CollectionAdapters::ToIList

  .method /*06000055*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000016*/<!!T> 
          ToIList<T>(class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IListContract`1/*01000015*/<!!T> collection) cil managed
  // SIG: 10 01 01 15 12 59 01 1E 00 15 12 55 01 1E 00
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method CollectionAdapters::ToIList

} // end of class System.AddIn.Pipeline.CollectionAdapters

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit System.AddIn.Pipeline.ContractAdapter
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .method /*06000056*/ public hidebysig static 
          class System.AddIn.Pipeline.ContractHandle/*02000015*/ 
          ViewToContractAdapter(object view) cil managed
  // SIG: 00 01 12 54 1C
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method ContractAdapter::ViewToContractAdapter

  .method /*06000057*/ public hidebysig static 
          !!TView  ContractToViewAdapter<TView>(class System.AddIn.Pipeline.ContractHandle/*02000015*/ contract,
                                                valuetype System.AddIn.Hosting.PipelineStoreLocation/*0200000E*/ location) cil managed
  // SIG: 10 01 02 1E 00 12 54 11 38
  {
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method ContractAdapter::ContractToViewAdapter

  .method /*06000058*/ public hidebysig static 
          !!TView  ContractToViewAdapter<TView>(class System.AddIn.Pipeline.ContractHandle/*02000015*/ contract,
                                                string pipelineRoot) cil managed
  // SIG: 10 01 02 1E 00 12 54 0E
  {
    .custom /*0C000050:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method ContractAdapter::ContractToViewAdapter

} // end of class System.AddIn.Pipeline.ContractAdapter

.class /*02000014*/ public auto ansi beforefieldinit System.AddIn.Pipeline.ContractBase
       extends [mscorlib/*23000001*/]System.MarshalByRefObject/*01000018*/
       implements [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/,
                  [mscorlib/*23000001*/]System.Runtime.Remoting.Lifetime.ISponsor/*0100001A*/
{
  .method /*06000059*/ public hidebysig newslot virtual 
          instance bool  RemoteEquals(class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ contract) cil managed
  // SIG: 20 01 02 12 65
  {
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method ContractBase::RemoteEquals

  .method /*0600005A*/ public hidebysig newslot virtual 
          instance string  RemoteToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method ContractBase::RemoteToString

  .method /*0600005B*/ public hidebysig newslot virtual 
          instance int32  GetRemoteHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method ContractBase::GetRemoteHashCode

  .method /*0600005C*/ public hidebysig newslot virtual 
          instance class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ 
          QueryContract(string contractIdentifier) cil managed
  // SIG: 20 01 12 65 0E
  {
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method ContractBase::QueryContract

  .method /*0600005D*/ public hidebysig newslot virtual final 
          instance int32  AcquireLifetimeToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method ContractBase::AcquireLifetimeToken

  .method /*0600005E*/ public hidebysig newslot virtual final 
          instance void  RevokeLifetimeToken(int32 token) cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000051:0A00001E*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*0100003F*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method ContractBase::RevokeLifetimeToken

  .method /*0600005F*/ family hidebysig newslot virtual 
          instance void  OnFinalRevoke() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method ContractBase::OnFinalRevoke

  .method /*06000060*/ public hidebysig newslot virtual final 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000A*/ 
          Renewal(class [mscorlib/*23000001*/]System.Runtime.Remoting.Lifetime.ILease/*0100001B*/ lease) cil managed
  // SIG: 20 01 11 29 12 6D
  {
    .custom /*0C000052:0A000003*/ instance void [mscorlib/*23000001*/]System.Security.SecurityCriticalAttribute/*0100001E*/::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .permissionset linkcheck
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(4096)}}
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method ContractBase::Renewal

  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method ContractBase::.ctor

} // end of class System.AddIn.Pipeline.ContractBase

.class /*02000015*/ public auto ansi beforefieldinit System.AddIn.Pipeline.ContractHandle
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
       implements [mscorlib/*23000001*/]System.IDisposable/*0100001C*/
{
  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ contract) cil managed
  // SIG: 20 01 01 12 65
  {
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method ContractHandle::.ctor

  .method /*06000063*/ public hidebysig specialname 
          instance class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ 
          get_Contract() cil managed
  // SIG: 20 00 12 65
  {
    .custom /*0C000053:0A00001C*/ instance void [mscorlib/*23000001*/]System.Runtime.TargetedPatchingOptOutAttribute/*0100003B*/::.ctor(string) /* 0A00001C */ = ( 01 00 4F 50 65 72 66 6F 72 6D 61 6E 63 65 20 63   // ..OPerformance c
                                                                                                                                                                   72 69 74 69 63 61 6C 20 74 6F 20 69 6E 6C 69 6E   // ritical to inlin
                                                                                                                                                                   65 20 74 68 69 73 20 74 79 70 65 20 6F 66 20 6D   // e this type of m
                                                                                                                                                                   65 74 68 6F 64 20 61 63 72 6F 73 73 20 4E 47 65   // ethod across NGe
                                                                                                                                                                   6E 20 69 6D 61 67 65 20 62 6F 75 6E 64 61 72 69   // n image boundari
                                                                                                                                                                   65 73 00 00 )                                     // es..
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method ContractHandle::get_Contract

  .method /*06000064*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method ContractHandle::Finalize

  .method /*06000065*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method ContractHandle::Dispose

  .method /*06000066*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method ContractHandle::Dispose

  .method /*06000067*/ public hidebysig static 
          bool  ContractOwnsAppDomain(class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ contract,
                                      class [mscorlib/*23000001*/]System.AppDomain/*01000007*/ domain) cil managed
  // SIG: 00 02 02 12 65 12 1D
  {
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method ContractHandle::ContractOwnsAppDomain

  .method /*06000068*/ public hidebysig static 
          class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ 
          AppDomainOwner(class [mscorlib/*23000001*/]System.AppDomain/*01000007*/ domain) cil managed
  // SIG: 00 01 12 65 12 1D
  {
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method ContractHandle::AppDomainOwner

  .property /*1700001A*/ instance callconv(8) class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/
          Contract()
  {
    .get instance class [System.AddIn.Contract/*23000003*/]System.AddIn.Contract.IContract/*01000019*/ System.AddIn.Pipeline.ContractHandle/*02000015*/::get_Contract() /* 06000063 */
  } // end of property ContractHandle::Contract
} // end of class System.AddIn.Pipeline.ContractHandle

.class /*02000016*/ private abstract auto ansi sealed beforefieldinit FXAssembly
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .field /*04000017*/ static assembly literal string Version = "4.0.0.0"
} // end of class FXAssembly

.class /*02000017*/ private abstract auto ansi sealed beforefieldinit ThisAssembly
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .field /*04000018*/ static assembly literal string Title = "System.AddIn.dll"
  .field /*04000019*/ static assembly literal string Description = "System.AddIn.dll"
  .field /*0400001A*/ static assembly literal string DefaultAlias = "System.AddIn.dll"
  .field /*0400001B*/ static assembly literal string Copyright = bytearray (A9 00 20 00 4D 00 69 00 63 00 72 00 6F 00 73 00   // .. .M.i.c.r.o.s.
                                                                            6F 00 66 00 74 00 20 00 43 00 6F 00 72 00 70 00   // o.f.t. .C.o.r.p.
                                                                            6F 00 72 00 61 00 74 00 69 00 6F 00 6E 00 2E 00   // o.r.a.t.i.o.n...
                                                                            20 00 20 00 41 00 6C 00 6C 00 20 00 72 00 69 00   //  . .A.l.l. .r.i.
                                                                            67 00 68 00 74 00 73 00 20 00 72 00 65 00 73 00   // g.h.t.s. .r.e.s.
                                                                            65 00 72 00 76 00 65 00 64 00 2E 00 )             // e.r.v.e.d...
  .field /*0400001C*/ static assembly literal string Version = "4.0.0.0"
  .field /*0400001D*/ static assembly literal string InformationalVersion = "4.0.30319.1"
  .field /*0400001E*/ static assembly literal string DailyBuildNumberStr = "30319"
  .field /*0400001F*/ static assembly literal string BuildRevisionStr = "1"
  .field /*04000020*/ static assembly literal int32 DailyBuildNumber = int32(0x0000766F)
} // end of class ThisAssembly

.class /*02000018*/ private abstract auto ansi sealed beforefieldinit AssemblyRef
       extends [mscorlib/*23000001*/]System.Object/*01000006*/
{
  .field /*04000021*/ static assembly literal string EcmaPublicKey = "b77a5c561934e089"
  .field /*04000022*/ static assembly literal string EcmaPublicKeyToken = "b77a5c561934e089"
  .field /*04000023*/ static assembly literal string EcmaPublicKeyFull = "00000000000000000400000000000000"
  .field /*04000024*/ static assembly literal string SilverlightPublicKey = "31bf3856ad364e35"
  .field /*04000025*/ static assembly literal string SilverlightPublicKeyToken = "31bf3856ad364e35"
  .field /*04000026*/ static assembly literal string SilverlightPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "04000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA"
  + "31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC365"
  + "2BF5C308055DA9"
  .field /*04000027*/ static assembly literal string SilverlightPlatformPublicKey = "7cec85d7bea7798e"
  .field /*04000028*/ static assembly literal string SilverlightPlatformPublicKeyToken = "7cec85d7bea7798e"
  .field /*04000029*/ static assembly literal string SilverlightPlatformPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "040000010001008D56C76F9E8649383049F383C44BE0EC204181822A6C31CF5EB7EF486944D032188EA1D3920763712CCB12D75FB77E9811149E6148E5D32FBAAB37611C"
  + "1878DDC19E20EF135D0CB2CFF2BFEC3D115810C3D9069638FE4BE215DBF795861920E5AB6F7DB2E2CEEF136AC23D5DD2BF031700AEC232F6C6B1C785B4305C123B37AB"
  .field /*0400002A*/ static assembly literal string PlatformPublicKey = "b77a5c561934e089"
  .field /*0400002B*/ static assembly literal string PlatformPublicKeyToken = "b77a5c561934e089"
  .field /*0400002C*/ static assembly literal string PlatformPublicKeyFull = "00000000000000000400000000000000"
  .field /*0400002D*/ static assembly literal string Mscorlib = "mscorlib, Version=4.0.0.0, Culture=neutral, Public"
  + "KeyToken=b77a5c561934e089"
  .field /*0400002E*/ static assembly literal string SystemData = "System.Data, Version=4.0.0.0, Culture=neutral, Pub"
  + "licKeyToken=b77a5c561934e089"
  .field /*0400002F*/ static assembly literal string SystemDataOracleClient = "System.Data.OracleClient, Version=4.0.0.0, Culture"
  + "=neutral, PublicKeyToken=b77a5c561934e089"
  .field /*04000030*/ static assembly literal string System = "System, Version=4.0.0.0, Culture=neutral, PublicKe"
  + "yToken=b77a5c561934e089"
  .field /*04000031*/ static assembly literal string SystemCore = "System.Core, Version=4.0.0.0, Culture=neutral, Pub"
  + "licKeyToken=b77a5c561934e089"
  .field /*04000032*/ static assembly literal string SystemNumerics = "System.Numerics, Version=4.0.0.0, Culture=neutral,"
  + " PublicKeyToken=b77a5c561934e089"
  .field /*04000033*/ static assembly literal string SystemRuntimeRemoting = "System.Runtime.Remoting, Version=4.0.0.0, Culture="
  + "neutral, PublicKeyToken=b77a5c561934e089"
  .field /*04000034*/ static assembly literal string SystemWindowsForms = "System.Windows.Forms, Version=4.0.0.0, Culture=neu"
  + "tral, PublicKeyToken=b77a5c561934e089"
  .field /*04000035*/ static assembly literal string SystemXml = "System.Xml, Version=4.0.0.0, Culture=neutral, Publ"
  + "icKeyToken=b77a5c561934e089"
  .field /*04000036*/ static assembly literal string MicrosoftPublicKey = "b03f5f7f11d50a3a"
  .field /*04000037*/ static assembly literal string MicrosoftPublicKeyToken = "b03f5f7f11d50a3a"
  .field /*04000038*/ static assembly literal string MicrosoftPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "0400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4"
  + "E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518"
  + "206DC093344D5AD293"
  .field /*04000039*/ static assembly literal string SharedLibPublicKey = "31bf3856ad364e35"
  .field /*0400003A*/ static assembly literal string SharedLibPublicKeyToken = "31bf3856ad364e35"
  .field /*0400003B*/ static assembly literal string SharedLibPublicKeyFull = "00240000048000009400000006020000002400005253413100"
  + "04000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718"
  + "EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247AD"
  + "C3652BF5C308055DA9"
  .field /*0400003C*/ static assembly literal string SystemComponentModelDataAnnotations = "System.ComponentModel.DataAnnotations, Version=4.0"
  + ".0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400003D*/ static assembly literal string SystemConfiguration = "System.Configuration, Version=4.0.0.0, Culture=neu"
  + "tral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400003E*/ static assembly literal string SystemConfigurationInstall = "System.Configuration.Install, Version=4.0.0.0, Cul"
  + "ture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400003F*/ static assembly literal string SystemDeployment = "System.Deployment, Version=4.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000040*/ static assembly literal string SystemDesign = "System.Design, Version=4.0.0.0, Culture=neutral, P"
  + "ublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000041*/ static assembly literal string SystemDirectoryServices = "System.DirectoryServices, Version=4.0.0.0, Culture"
  + "=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000042*/ static assembly literal string SystemDrawingDesign = "System.Drawing.Design, Version=4.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000043*/ static assembly literal string SystemDrawing = "System.Drawing, Version=4.0.0.0, Culture=neutral, "
  + "PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000044*/ static assembly literal string SystemEnterpriseServices = "System.EnterpriseServices, Version=4.0.0.0, Cultur"
  + "e=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000045*/ static assembly literal string SystemManagement = "System.Management, Version=4.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000046*/ static assembly literal string SystemMessaging = "System.Messaging, Version=4.0.0.0, Culture=neutral"
  + ", PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000047*/ static assembly literal string SystemRuntimeSerializationFormattersSoap = "System.Runtime.Serialization.Formatters.Soap, Vers"
  + "ion=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000048*/ static assembly literal string SystemSecurity = "System.Security, Version=4.0.0.0, Culture=neutral,"
  + " PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000049*/ static assembly literal string SystemServiceProcess = "System.ServiceProcess, Version=4.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400004A*/ static assembly literal string SystemWeb = "System.Web, Version=4.0.0.0, Culture=neutral, Publ"
  + "icKeyToken=b03f5f7f11d50a3a"
  .field /*0400004B*/ static assembly literal string SystemWebAbstractions = "System.Web.Abstractions, Version=4.0.0.0, Culture="
  + "neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004C*/ static assembly literal string SystemWebDynamicData = "System.Web.DynamicData, Version=4.0.0.0, Culture=n"
  + "eutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004D*/ static assembly literal string SystemWebDynamicDataDesign = "System.Web.DynamicData.Design, Version=4.0.0.0, Cu"
  + "lture=neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*0400004E*/ static assembly literal string SystemWebEntityDesign = "System.Web.Entity.Design, Version=4.0.0.0, Culture"
  + "=neutral, PublicKeyToken=b77a5c561934e089"
  .field /*0400004F*/ static assembly literal string SystemWebExtensions = "System.Web.Extensions, Version=4.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=31bf3856ad364e35"
  .field /*04000050*/ static assembly literal string SystemWebExtensionsDesign = "System.Web.Extensions.Design, Version=4.0.0.0, Cul"
  + "ture=neutral, PublicKeyToken=31bf3856ad364e35"
  .field /*04000051*/ static assembly literal string SystemWebMobile = "System.Web.Mobile, Version=4.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000052*/ static assembly literal string SystemWebRegularExpressions = "System.Web.RegularExpressions, Version=4.0.0.0, Cu"
  + "lture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000053*/ static assembly literal string SystemWebRouting = "System.Web.Routing, Version=4.0.0.0, Culture=neutr"
  + "al, PublicKeyToken=31bf3856ad364e35"
  .field /*04000054*/ static assembly literal string SystemWebServices = "System.Web.Services, Version=4.0.0.0, Culture=neut"
  + "ral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000055*/ static assembly literal string WindowsBase = "WindowsBase, Version=4.0.0.0, Culture=neutral, Pub"
  + "licKeyToken=31bf3856ad364e35"
  .field /*04000056*/ static assembly literal string MicrosoftVisualStudio = "Microsoft.VisualStudio, Version=4.0.0.0, Culture=n"
  + "eutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000057*/ static assembly literal string MicrosoftVisualStudioWindowsForms = "Microsoft.VisualStudio.Windows.Forms, Version=4.0."
  + "0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000058*/ static assembly literal string VJSharpCodeProvider = "VJSharpCodeProvider, Version=4.0.0.0, Culture=neut"
  + "ral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*04000059*/ static assembly literal string ASPBrowserCapsPublicKey = "b7bd7678b977bd8f"
  .field /*0400005A*/ static assembly literal string ASPBrowserCapsFactory = "ASP.BrowserCapsFactory, Version=4.0.0.0, Culture=n"
  + "eutral, PublicKeyToken=b7bd7678b977bd8f"
  .field /*0400005B*/ static assembly literal string MicrosoftVSDesigner = "Microsoft.VSDesigner, Version=10.0.0.0, Culture=ne"
  + "utral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005C*/ static assembly literal string MicrosoftVisualStudioWeb = "Microsoft.VisualStudio.Web, Version=10.0.0.0, Cult"
  + "ure=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005D*/ static assembly literal string MicrosoftWebDesign = "Microsoft.Web.Design.Client, Version=10.0.0.0, Cul"
  + "ture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005E*/ static assembly literal string MicrosoftVSDesignerMobile = "Microsoft.VSDesigner.Mobile, Version=8.0.0.0, Cult"
  + "ure=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  .field /*0400005F*/ static assembly literal string MicrosoftJScript = "Microsoft.JScript, Version=8.0.0.0, Culture=neutra"
  + "l, PublicKeyToken=b03f5f7f11d50a3a"
} // end of class AssemblyRef


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
