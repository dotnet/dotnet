


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
// Time-date stamp:            0xab7b9dac
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003532
// Base of code:                   0x00002000
// Base of data:                   0x00004000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00008000
// Size of headers:                0x00000200
// Checksum:                       0x00007e80
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000034e0 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000468] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002000 [0x00002938] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000034c4 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001538 Virtual Size
//              0x00002000 Virtual Address
//              0x00001600 Size of Raw Data
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
//              0x00000468 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00006000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00001e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000534
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003522 Import Name Table
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
// 0x000020bc [0x00001388] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003444 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000078c Size
//              '#~' Name
//    Stream 2:
//              0x000007f8 Offset
//              0x0000068c Size
//              '#Strings' Name
//    Stream 3:
//              0x00000e84 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000e88 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000e98 Offset
//              0x000004f0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000900001d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 18744
// PE header size       : 512 (496 used)    ( 2.73%)
// PE additional info   : 11879             (63.37%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.38%)
// CLR meta-data size  : 5000               (26.68%)
// CLR additional info : 128                ( 0.68%)
// CLR method headers  : 21                 ( 0.11%)
// Managed code         : 87                ( 0.46%)
// Data                 : 2048              (10.93%)
// Unaccounted          : -1003             (-5.35%)

// Num.of PE sections   : 3
//   .text    - 5632
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 5000
//   Module        -    1 (10 bytes)
//   TypeDef       -    7 (98 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   35 (210 bytes)
//   MethodDef     -   21 (294 bytes)     0 abstract, 0 native, 21 bodies
//   FieldDef      -   19 (114 bytes)     0 constant
//   MemberRef     -   27 (162 bytes)
//   ParamDef      -   51 (306 bytes)
//   Constant      -   17 (102 bytes)
//   CustomAttribute-   84 (504 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   Strings       -  1674 bytes
//   Blobs         -  1264 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   180 bytes

// CLR additional info : 128

// CLR method headers : 21
//   Num.of method bodies  - 21
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 21

// Managed code : 87
//   Ave method size - 4
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class Binder                         (public) (abstract) (auto) (ansi) (sealed) 
// Class CSharpArgumentInfo             (public) (auto) (ansi) (sealed) 
// Class CSharpArgumentInfoFlags        (public) (auto) (ansi) (sealed) 
// Class CSharpBinderFlags              (public) (auto) (ansi) (sealed) 
// Class RuntimeBinderException         (public) (auto) (ansi) 
// Class RuntimeBinderInternalCompilerException (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Linq.Expressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform Microsoft.CSharp
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000005:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 2E 43 53 68   // ...Microsoft.CSh
                                                                                                                                                                        61 72 70 00 00 )                                  // arp..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000010:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 2E 43 53 68   // ...Microsoft.CSh
                                                                                                                                                                       61 72 70 00 00 )                                  // arp..
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 2E 43 53 68   // ...Microsoft.CSh
                                                                                                                                                                 61 72 70 00 00 )                                  // arp..
  .custom /*0C000017:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
.module Microsoft.CSharp.dll
// MVID: {867e1125-3bcd-4315-a1c3-b791a597f45c}
.custom /*0C000003:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000004:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit Microsoft.CSharp.RuntimeBinder.Binder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00001C:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000015*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          BinaryOperation(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                          valuetype [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.ExpressionType/*0100001B*/ operation,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 04 12 69 11 14 11 6D 12 71 15 12 75 01 12 0C
  {
    .custom /*0C000001:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000002:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*08000003*/ 
    .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000004*/ 
    .custom /*0C000025:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::BinaryOperation

  .method /*06000002*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          Convert(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                  class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                  class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context) cil managed
  // SIG: 00 03 12 69 11 14 12 71 12 71
  {
    .custom /*0C000018:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000019:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*08000007*/ 
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::Convert

  .method /*06000003*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          GetIndex(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                   class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 03 12 69 11 14 12 71 15 12 75 01 12 0C
  {
    .custom /*0C00001D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C00001E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [2]/*08000009*/ 
    .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800000A*/ 
    .custom /*0C00003A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::GetIndex

  .method /*06000004*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          GetMember(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                    string name,
                    class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 04 12 69 11 14 0E 12 71 15 12 75 01 12 0C
  {
    .custom /*0C000021:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000022:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*0800000D*/ 
    .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800000E*/ 
    .custom /*0C00003F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::GetMember

  .method /*06000005*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          Invoke(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                 class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 03 12 69 11 14 12 71 15 12 75 01 12 0C
  {
    .custom /*0C000026:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000027:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [2]/*08000010*/ 
    .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000011*/ 
    .custom /*0C000044:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::Invoke

  .method /*06000006*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          InvokeConstructor(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 03 12 69 11 14 12 71 15 12 75 01 12 0C
  {
    .custom /*0C00002A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C00002B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [2]/*08000013*/ 
    .custom /*0C000048:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000014*/ 
    .custom /*0C000049:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::InvokeConstructor

  .method /*06000007*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          InvokeMember(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                       string name,
                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> typeArguments,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 05 12 69 11 14 0E 15 12 75 01 12 71 12 71 15 12 75 01 12 0C
  {
    .custom /*0C00002E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C00002F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*08000017*/ 
    .custom /*0C00004A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000018*/ 
    .custom /*0C00004B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000019*/ 
    .custom /*0C00004C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::InvokeMember

  .method /*06000008*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          IsEvent(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                  string name,
                  class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context) cil managed
  // SIG: 00 03 12 69 11 14 0E 12 71
  {
    .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000034:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*0800001C*/ 
    .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::IsEvent

  .method /*06000009*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          SetIndex(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                   class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 03 12 69 11 14 12 71 15 12 75 01 12 0C
  {
    .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000036:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [2]/*0800001E*/ 
    .custom /*0C00004E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800001F*/ 
    .custom /*0C00004F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::SetIndex

  .method /*0600000A*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          SetMember(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                    string name,
                    class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 04 12 69 11 14 0E 12 71 15 12 75 01 12 0C
  {
    .custom /*0C000038:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C000039:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*08000022*/ 
    .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000023*/ 
    .custom /*0C000051:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::SetMember

  .method /*0600000B*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.CallSiteBinder/*0100001A*/ 
          UnaryOperation(valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ 'flags',
                         valuetype [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.ExpressionType/*0100001B*/ operation,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/ context,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/> argumentInfo) cil managed
  // SIG: 00 04 12 69 11 14 11 6D 12 71 15 12 75 01 12 0C
  {
    .custom /*0C00003B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4A 55 73 69 6E 67 20 64 79 6E 61 6D 69 63   // ..JUsing dynamic
                                                                                                                                                                                            20 74 79 70 65 73 20 6D 69 67 68 74 20 63 61 75   //  types might cau
                                                                                                                                                                                            73 65 20 74 79 70 65 73 20 6F 72 20 6D 65 6D 62   // se types or memb
                                                                                                                                                                                            65 72 73 20 74 6F 20 62 65 20 72 65 6D 6F 76 65   // ers to be remove
                                                                                                                                                                                            64 20 62 79 20 74 72 69 6D 6D 65 72 2E 00 00 )    // d by trimmer...
    .custom /*0C00003C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 57 54 68 65 20 27 64 79 6E 61 6D 69 63 27   // ..WThe 'dynamic'
                                                                                                                                                                                       20 66 65 61 74 75 72 65 20 72 65 71 75 69 72 65   //  feature require
                                                                                                                                                                                       73 20 72 75 6E 74 69 6D 65 2D 63 6F 64 65 20 67   // s runtime-code g
                                                                                                                                                                                       65 6E 65 72 61 74 69 6F 6E 2C 20 77 68 69 63 68   // eneration, which
                                                                                                                                                                                       20 69 73 20 69 6E 63 6F 6D 70 61 74 69 62 6C 65   //  is incompatible
                                                                                                                                                                                       20 77 69 74 68 20 41 4F 54 2E 00 00 )             //  with AOT...
    .param [3]/*08000026*/ 
    .custom /*0C000052:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000027*/ 
    .custom /*0C000053:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Binder::UnaryOperation

} // end of class Microsoft.CSharp.RuntimeBinder.Binder

.class /*02000003*/ public auto ansi sealed beforefieldinit Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000015*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600000C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2071
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CSharpArgumentInfo::.ctor

  .method /*0600000D*/ public hidebysig static 
          class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo/*02000003*/ 
          Create(valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ 'flags',
                 string name) cil managed
  // SIG: 00 02 12 0C 11 10 0E
  {
    .custom /*0C00003D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000029*/ 
    .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CSharpArgumentInfo::Create

} // end of class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo

.class /*02000004*/ public auto ansi sealed Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000015*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000024:0A000018*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001E*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ None = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ UseCompileTimeType = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ Constant = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ NamedArgument = int32(0x00000004)
  .field /*04000006*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ IsRef = int32(0x00000008)
  .field /*04000007*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ IsOut = int32(0x00000010)
  .field /*04000008*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags/*02000004*/ IsStaticType = int32(0x00000020)
} // end of class Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags

.class /*02000005*/ public auto ansi sealed Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000015*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000029:0A000018*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001E*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .field /*04000009*/ public specialname rtspecialname int32 value__
  .field /*0400000A*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ None = int32(0x00000000)
  .field /*0400000B*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ CheckedContext = int32(0x00000001)
  .field /*0400000C*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ InvokeSimpleName = int32(0x00000002)
  .field /*0400000D*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ InvokeSpecialName = int32(0x00000004)
  .field /*0400000E*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ BinaryOperationLogical = int32(0x00000008)
  .field /*0400000F*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ ConvertExplicit = int32(0x00000010)
  .field /*04000010*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ ConvertArrayIndex = int32(0x00000020)
  .field /*04000011*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ ResultIndexed = int32(0x00000040)
  .field /*04000012*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ ValueFromCompoundAssignment = int32(0x00000080)
  .field /*04000013*/ public static literal valuetype Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags/*02000005*/ ResultDiscarded = int32(0x00000100)
} // end of class Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags

.class /*02000006*/ public auto ansi beforefieldinit Microsoft.CSharp.RuntimeBinder.RuntimeBinderException
       extends [System.Runtime/*23000001*/]System.Exception/*01000020*/
{
  .custom /*0C00002C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderException::.ctor

  .method /*0600000F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C000040:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000041:0A000019*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string) /* 0A000019 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C000042:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000015*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2084
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderException::.ctor

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x208c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderException::.ctor

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x2094
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderException::.ctor

} // end of class Microsoft.CSharp.RuntimeBinder.RuntimeBinderException

.class /*02000007*/ public auto ansi beforefieldinit Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException
       extends [System.Runtime/*23000001*/]System.Exception/*01000020*/
{
  .custom /*0C000030:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderInternalCompilerException::.ctor

  .method /*06000013*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C000045:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000046:0A000019*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string) /* 0A000019 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C000047:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000015*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20a4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderInternalCompilerException::.ctor

  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderInternalCompilerException::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x20b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*01000020*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeBinderInternalCompilerException::.ctor

} // end of class Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
