


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
// Time-date stamp:            0x84b88554
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001e00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003c02
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
// Checksum:                       0x0000fd7b
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003bb0 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000474] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002800 [0x00002928] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003b94 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001c08 Virtual Size
//              0x00002000 Virtual Address
//              0x00001e00 Size of Raw Data
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
//              0x00000474 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002000 Pointer to Raw Data
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
//              0x00002600 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000c04
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003bf2 Import Name Table
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
// 0x00002160 [0x000019b4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003b14 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000c64 Size
//              '#~' Name
//    Stream 2:
//              0x00000cd0 Offset
//              0x000008cc Size
//              '#Strings' Name
//    Stream 3:
//              0x0000159c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000015a0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000015b0 Offset
//              0x00000404 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20776
// PE header size       : 512 (496 used)    ( 2.46%)
// PE additional info   : 11875             (57.16%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 6580               (31.67%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 71                 ( 0.34%)
// Managed code         : 200               ( 0.96%)
// Data                 : 2048              ( 9.86%)
// Unaccounted          : -710              (-3.42%)

// Num.of PE sections   : 3
//   .text    - 7680
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6580
//   Module        -    1 (10 bytes)
//   TypeDef       -   13 (182 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   34 (204 bytes)
//   MethodDef     -   71 (994 bytes)     0 abstract, 0 native, 71 bodies
//   FieldDef      -   25 (150 bytes)     0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -   82 (492 bytes)
//   Constant      -   37 (222 bytes)
//   CustomAttribute-   49 (294 bytes)
//   InterfaceImpl -    4 (16 bytes)
//   PropertyMap   -    6 (24 bytes)
//   Property      -   21 (126 bytes)
//   MethodSemantic-   34 (204 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   Strings       -  2250 bytes
//   Blobs         -  1028 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 71
//   Num.of method bodies  - 71
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 71

// Managed code : 200
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class GnuTarEntry                    (public) (auto) (ansi) (sealed) 
// Class PaxGlobalExtendedAttributesTarEntry (public) (auto) (ansi) (sealed) 
// Class PaxTarEntry                    (public) (auto) (ansi) (sealed) 
// Class PosixTarEntry                  (public) (abstract) (auto) (ansi) 
// Class TarEntry                       (public) (abstract) (auto) (ansi) 
// Class TarEntryFormat                 (public) (auto) (ansi) (sealed) 
// Class TarEntryType                   (public) (auto) (ansi) (sealed) 
// Class TarFile                        (public) (abstract) (auto) (ansi) (sealed) 
// Class TarReader                      (public) (auto) (ansi) (sealed) 
// Class TarWriter                      (public) (auto) (ansi) (sealed) 
// Class UstarTarEntry                  (public) (auto) (ansi) (sealed) 
// Class V7TarEntry                     (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.Formats.Tar
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 12 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                        73 2E 54 61 72 00 00 )                            // s.Tar..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 12 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                       73 2E 54 61 72 00 00 )                            // s.Tar..
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 12 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                 73 2E 54 61 72 00 00 )                            // s.Tar..
  .custom /*0C000013:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 9:0:0:0
}
.module System.Formats.Tar.dll
// MVID: {4cec1306-9c3c-491c-91f3-9ebb4b7f83cc}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.Formats.Tar.GnuTarEntry
       extends System.Formats.Tar.PosixTarEntry/*02000005*/
{
  .custom /*0C000014:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000015:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*02000006*/ other) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*02000008*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 20 0E
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::.ctor

  .method /*06000003*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 
          get_AccessTime() cil managed
  // SIG: 20 00 11 55
  {
    // Method begins at RVA 0x2060
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GnuTarEntry::get_AccessTime

  .method /*06000004*/ public hidebysig specialname 
          instance void  set_AccessTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 11 55
  {
    // Method begins at RVA 0x2063
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::set_AccessTime

  .method /*06000005*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 
          get_ChangeTime() cil managed
  // SIG: 20 00 11 55
  {
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GnuTarEntry::get_ChangeTime

  .method /*06000006*/ public hidebysig specialname 
          instance void  set_ChangeTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 11 55
  {
    // Method begins at RVA 0x2068
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::set_ChangeTime

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/
          AccessTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ System.Formats.Tar.GnuTarEntry/*02000002*/::get_AccessTime() /* 06000003 */
    .set instance void System.Formats.Tar.GnuTarEntry/*02000002*/::set_AccessTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/) /* 06000004 */
  } // end of property GnuTarEntry::AccessTime
  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/
          ChangeTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ System.Formats.Tar.GnuTarEntry/*02000002*/::get_ChangeTime() /* 06000005 */
    .set instance void System.Formats.Tar.GnuTarEntry/*02000002*/::set_ChangeTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/) /* 06000006 */
  } // end of property GnuTarEntry::ChangeTime
} // end of class System.Formats.Tar.GnuTarEntry

.class /*02000003*/ public auto ansi sealed beforefieldinit System.Formats.Tar.PaxGlobalExtendedAttributesTarEntry
       extends System.Formats.Tar.PosixTarEntry/*02000005*/
{
  .custom /*0C000016:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000016*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000017*/<string,string>> globalExtendedAttributes) cil managed
  // SIG: 20 01 01 15 12 59 01 15 11 5D 02 0E 0E
  {
    .param [1]/*08000006*/ 
    .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    // Method begins at RVA 0x206a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxGlobalExtendedAttributesTarEntry::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> 
          get_GlobalExtendedAttributes() cil managed
  // SIG: 20 00 15 12 61 02 0E 0E
  {
    // Method begins at RVA 0x2072
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PaxGlobalExtendedAttributesTarEntry::get_GlobalExtendedAttributes

  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string>
          GlobalExtendedAttributes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> System.Formats.Tar.PaxGlobalExtendedAttributesTarEntry/*02000003*/::get_GlobalExtendedAttributes() /* 06000008 */
  } // end of property PaxGlobalExtendedAttributesTarEntry::GlobalExtendedAttributes
} // end of class System.Formats.Tar.PaxGlobalExtendedAttributesTarEntry

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Formats.Tar.PaxTarEntry
       extends System.Formats.Tar.PosixTarEntry/*02000005*/
{
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*02000006*/ other) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2075
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxTarEntry::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*02000008*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 20 0E
  {
    // Method begins at RVA 0x207d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxTarEntry::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*02000008*/ entryType,
                               string entryName,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000016*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000017*/<string,string>> extendedAttributes) cil managed
  // SIG: 20 03 01 11 20 0E 15 12 59 01 15 11 5D 02 0E 0E
  {
    .param [3]/*0800000C*/ 
    .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    // Method begins at RVA 0x2085
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxTarEntry::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> 
          get_ExtendedAttributes() cil managed
  // SIG: 20 00 15 12 61 02 0E 0E
  {
    // Method begins at RVA 0x208d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PaxTarEntry::get_ExtendedAttributes

  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string>
          ExtendedAttributes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> System.Formats.Tar.PaxTarEntry/*02000004*/::get_ExtendedAttributes() /* 0600000C */
  } // end of property PaxTarEntry::ExtendedAttributes
} // end of class System.Formats.Tar.PaxTarEntry

.class /*02000005*/ public abstract auto ansi beforefieldinit System.Formats.Tar.PosixTarEntry
       extends System.Formats.Tar.TarEntry/*02000006*/
{
  .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600000D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2090
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000016       */ call       instance void System.Formats.Tar.TarEntry/*02000006*/::.ctor() /* 06000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::.ctor

  .method /*0600000E*/ public hidebysig specialname 
          instance int32  get_DeviceMajor() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_DeviceMajor

  .method /*0600000F*/ public hidebysig specialname 
          instance void  set_DeviceMajor(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x209b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_DeviceMajor

  .method /*06000010*/ public hidebysig specialname 
          instance int32  get_DeviceMinor() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x209d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_DeviceMinor

  .method /*06000011*/ public hidebysig specialname 
          instance void  set_DeviceMinor(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_DeviceMinor

  .method /*06000012*/ public hidebysig specialname 
          instance string  get_GroupName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_GroupName

  .method /*06000013*/ public hidebysig specialname 
          instance void  set_GroupName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_GroupName

  .method /*06000014*/ public hidebysig specialname 
          instance string  get_UserName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_UserName

  .method /*06000015*/ public hidebysig specialname 
          instance void  set_UserName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_UserName

  .property /*17000005*/ instance callconv(8) int32
          DeviceMajor()
  {
    .get instance int32 System.Formats.Tar.PosixTarEntry/*02000005*/::get_DeviceMajor() /* 0600000E */
    .set instance void System.Formats.Tar.PosixTarEntry/*02000005*/::set_DeviceMajor(int32) /* 0600000F */
  } // end of property PosixTarEntry::DeviceMajor
  .property /*17000006*/ instance callconv(8) int32
          DeviceMinor()
  {
    .get instance int32 System.Formats.Tar.PosixTarEntry/*02000005*/::get_DeviceMinor() /* 06000010 */
    .set instance void System.Formats.Tar.PosixTarEntry/*02000005*/::set_DeviceMinor(int32) /* 06000011 */
  } // end of property PosixTarEntry::DeviceMinor
  .property /*17000007*/ instance callconv(8) string
          GroupName()
  {
    .get instance string System.Formats.Tar.PosixTarEntry/*02000005*/::get_GroupName() /* 06000012 */
    .set instance void System.Formats.Tar.PosixTarEntry/*02000005*/::set_GroupName(string) /* 06000013 */
  } // end of property PosixTarEntry::GroupName
  .property /*17000008*/ instance callconv(8) string
          UserName()
  {
    .get instance string System.Formats.Tar.PosixTarEntry/*02000005*/::get_UserName() /* 06000014 */
    .set instance void System.Formats.Tar.PosixTarEntry/*02000005*/::set_UserName(string) /* 06000015 */
  } // end of property PosixTarEntry::UserName
} // end of class System.Formats.Tar.PosixTarEntry

.class /*02000006*/ public abstract auto ansi beforefieldinit System.Formats.Tar.TarEntry
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000016*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarEntry::.ctor

  .method /*06000017*/ public hidebysig specialname 
          instance int32  get_Checksum() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Checksum

  .method /*06000018*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 
          get_DataStream() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_DataStream

  .method /*06000019*/ public hidebysig specialname 
          instance void  set_DataStream(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    .custom /*0C00002C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_DataStream

  .method /*0600001A*/ public hidebysig specialname 
          instance valuetype System.Formats.Tar.TarEntryType/*02000008*/ 
          get_EntryType() cil managed
  // SIG: 20 00 11 20
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_EntryType

  .method /*0600001B*/ public hidebysig specialname 
          instance valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ 
          get_Format() cil managed
  // SIG: 20 00 11 1C
  {
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Format

  .method /*0600001C*/ public hidebysig specialname 
          instance int32  get_Gid() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Gid

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_Gid(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Gid

  .method /*0600001E*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Length

  .method /*0600001F*/ public hidebysig specialname 
          instance string  get_LinkName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_LinkName

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_LinkName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_LinkName

  .method /*06000021*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Mode

  .method /*06000022*/ public hidebysig specialname 
          instance void  set_Mode(valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 11 6D
  {
    // Method begins at RVA 0x20d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Mode

  .method /*06000023*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 
          get_ModificationTime() cil managed
  // SIG: 20 00 11 55
  {
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_ModificationTime

  .method /*06000024*/ public hidebysig specialname 
          instance void  set_ModificationTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 11 55
  {
    // Method begins at RVA 0x20d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_ModificationTime

  .method /*06000025*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Name

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Name

  .method /*06000027*/ public hidebysig specialname 
          instance int64  get_DataOffset() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_DataOffset

  .method /*06000028*/ public hidebysig specialname 
          instance int32  get_Uid() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Uid

  .method /*06000029*/ public hidebysig specialname 
          instance void  set_Uid(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Uid

  .method /*0600002A*/ public hidebysig instance void 
          ExtractToFile(string destinationFileName,
                        bool overwrite) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x20e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::ExtractToFile

  .method /*0600002B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ExtractToFileAsync(string destinationFileName,
                             bool overwrite,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 0E 02 11 75
  {
    .param [3]/*0800001C*/  = nullref
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::ExtractToFileAsync

  .method /*0600002C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::ToString

  .property /*17000009*/ instance callconv(8) int32
          Checksum()
  {
    .get instance int32 System.Formats.Tar.TarEntry/*02000006*/::get_Checksum() /* 06000017 */
  } // end of property TarEntry::Checksum
  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/
          DataStream()
  {
    .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ System.Formats.Tar.TarEntry/*02000006*/::get_DataStream() /* 06000018 */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_DataStream(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/) /* 06000019 */
  } // end of property TarEntry::DataStream
  .property /*1700000B*/ instance callconv(8) valuetype System.Formats.Tar.TarEntryType/*02000008*/
          EntryType()
  {
    .get instance valuetype System.Formats.Tar.TarEntryType/*02000008*/ System.Formats.Tar.TarEntry/*02000006*/::get_EntryType() /* 0600001A */
  } // end of property TarEntry::EntryType
  .property /*1700000C*/ instance callconv(8) valuetype System.Formats.Tar.TarEntryFormat/*02000007*/
          Format()
  {
    .get instance valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ System.Formats.Tar.TarEntry/*02000006*/::get_Format() /* 0600001B */
  } // end of property TarEntry::Format
  .property /*1700000D*/ instance callconv(8) int32
          Gid()
  {
    .get instance int32 System.Formats.Tar.TarEntry/*02000006*/::get_Gid() /* 0600001C */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_Gid(int32) /* 0600001D */
  } // end of property TarEntry::Gid
  .property /*1700000E*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Formats.Tar.TarEntry/*02000006*/::get_Length() /* 0600001E */
  } // end of property TarEntry::Length
  .property /*1700000F*/ instance callconv(8) string
          LinkName()
  {
    .get instance string System.Formats.Tar.TarEntry/*02000006*/::get_LinkName() /* 0600001F */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_LinkName(string) /* 06000020 */
  } // end of property TarEntry::LinkName
  .property /*17000010*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/
          Mode()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/ System.Formats.Tar.TarEntry/*02000006*/::get_Mode() /* 06000021 */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_Mode(valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/) /* 06000022 */
  } // end of property TarEntry::Mode
  .property /*17000011*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/
          ModificationTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ System.Formats.Tar.TarEntry/*02000006*/::get_ModificationTime() /* 06000023 */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_ModificationTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/) /* 06000024 */
  } // end of property TarEntry::ModificationTime
  .property /*17000012*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Formats.Tar.TarEntry/*02000006*/::get_Name() /* 06000025 */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_Name(string) /* 06000026 */
  } // end of property TarEntry::Name
  .property /*17000013*/ instance callconv(8) int64
          DataOffset()
  {
    .get instance int64 System.Formats.Tar.TarEntry/*02000006*/::get_DataOffset() /* 06000027 */
  } // end of property TarEntry::DataOffset
  .property /*17000014*/ instance callconv(8) int32
          Uid()
  {
    .get instance int32 System.Formats.Tar.TarEntry/*02000006*/::get_Uid() /* 06000028 */
    .set instance void System.Formats.Tar.TarEntry/*02000006*/::set_Uid(int32) /* 06000029 */
  } // end of property TarEntry::Uid
} // end of class System.Formats.Tar.TarEntry

.class /*02000007*/ public auto ansi sealed System.Formats.Tar.TarEntryFormat
       extends [System.Runtime/*23000001*/]System.Enum/*0100001E*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ Unknown = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ V7 = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ Ustar = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ Pax = int32(0x00000003)
  .field /*04000006*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ Gnu = int32(0x00000004)
} // end of class System.Formats.Tar.TarEntryFormat

.class /*02000008*/ public auto ansi sealed System.Formats.Tar.TarEntryType
       extends [System.Runtime/*23000001*/]System.Enum/*0100001E*/
{
  .field /*04000007*/ public specialname rtspecialname uint8 value__
  .field /*04000008*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ V7RegularFile = uint8(0x00)
  .field /*04000009*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ RegularFile = uint8(0x30)
  .field /*0400000A*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ HardLink = uint8(0x31)
  .field /*0400000B*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ SymbolicLink = uint8(0x32)
  .field /*0400000C*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ CharacterDevice = uint8(0x33)
  .field /*0400000D*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ BlockDevice = uint8(0x34)
  .field /*0400000E*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ Directory = uint8(0x35)
  .field /*0400000F*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ Fifo = uint8(0x36)
  .field /*04000010*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ ContiguousFile = uint8(0x37)
  .field /*04000011*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ DirectoryList = uint8(0x44)
  .field /*04000012*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ LongLink = uint8(0x4B)
  .field /*04000013*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ LongPath = uint8(0x4C)
  .field /*04000014*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ MultiVolume = uint8(0x4D)
  .field /*04000015*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ RenamedOrSymlinked = uint8(0x4E)
  .field /*04000016*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ SparseFile = uint8(0x53)
  .field /*04000017*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ TapeVolume = uint8(0x56)
  .field /*04000018*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ GlobalExtendedAttributes = uint8(0x67)
  .field /*04000019*/ public static literal valuetype System.Formats.Tar.TarEntryType/*02000008*/ ExtendedAttributes = uint8(0x78)
} // end of class System.Formats.Tar.TarEntryType

.class /*02000009*/ public abstract auto ansi sealed beforefieldinit System.Formats.Tar.TarFile
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600002D*/ public hidebysig static 
          void  CreateFromDirectory(string sourceDirectoryName,
                                    class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ destination,
                                    bool includeBaseDirectory) cil managed
  // SIG: 00 03 01 0E 12 69 02
  {
    // Method begins at RVA 0x20ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::CreateFromDirectory

  .method /*0600002E*/ public hidebysig static 
          void  CreateFromDirectory(string sourceDirectoryName,
                                    string destinationFileName,
                                    bool includeBaseDirectory) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x20f0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::CreateFromDirectory

  .method /*0600002F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          CreateFromDirectoryAsync(string sourceDirectoryName,
                                   class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ destination,
                                   bool includeBaseDirectory,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 0E 12 69 02 11 75
  {
    .param [4]/*08000026*/  = nullref
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::CreateFromDirectoryAsync

  .method /*06000030*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          CreateFromDirectoryAsync(string sourceDirectoryName,
                                   string destinationFileName,
                                   bool includeBaseDirectory,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 0E 0E 02 11 75
  {
    .param [4]/*0800002A*/  = nullref
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::CreateFromDirectoryAsync

  .method /*06000031*/ public hidebysig static 
          void  ExtractToDirectory(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ source,
                                   string destinationDirectoryName,
                                   bool overwriteFiles) cil managed
  // SIG: 00 03 01 12 69 0E 02
  {
    // Method begins at RVA 0x20f8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::ExtractToDirectory

  .method /*06000032*/ public hidebysig static 
          void  ExtractToDirectory(string sourceFileName,
                                   string destinationDirectoryName,
                                   bool overwriteFiles) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x20fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::ExtractToDirectory

  .method /*06000033*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ExtractToDirectoryAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ source,
                                  string destinationDirectoryName,
                                  bool overwriteFiles,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 12 69 0E 02 11 75
  {
    .param [4]/*08000034*/  = nullref
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::ExtractToDirectoryAsync

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ExtractToDirectoryAsync(string sourceFileName,
                                  string destinationDirectoryName,
                                  bool overwriteFiles,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 0E 0E 02 11 75
  {
    .param [4]/*08000038*/  = nullref
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::ExtractToDirectoryAsync

} // end of class System.Formats.Tar.TarFile

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Formats.Tar.TarReader
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001F*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000020*/
{
  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 02 01 12 69 02
  {
    .custom /*0C00002D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800003A*/  = bool(false)
    // Method begins at RVA 0x2102
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarReader::.ctor

  .method /*06000036*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarReader::Dispose

  .method /*06000037*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000021*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarReader::DisposeAsync

  .method /*06000038*/ public hidebysig instance class System.Formats.Tar.TarEntry/*02000006*/ 
          GetNextEntry([opt] bool copyData) cil managed
  // SIG: 20 01 12 18 02
  {
    .custom /*0C00002E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003B*/  = bool(false)
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarReader::GetNextEntry

  .method /*06000039*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<class System.Formats.Tar.TarEntry/*02000006*/> 
          GetNextEntryAsync([opt] bool copyData,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 89 01 12 18 02 11 75
  {
    .param [0]/*0800003C*/ 
    .custom /*0C00002F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*0800003D*/  = bool(false)
    .param [2]/*0800003E*/  = nullref
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarReader::GetNextEntryAsync

} // end of class System.Formats.Tar.TarReader

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Formats.Tar.TarWriter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001F*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000020*/
{
  .custom /*0C000024:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001F*/
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000020*/
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600003A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2115
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarWriter::.ctor

  .method /*0600003B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 02 01 12 69 02
  {
    .param [2]/*08000041*/  = bool(false)
    // Method begins at RVA 0x211d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarWriter::.ctor

  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream,
                               [opt] valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ format,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 12 69 11 1C 02
  {
    .param [2]/*08000043*/  = int32(0x00000003)
    .param [3]/*08000044*/  = bool(false)
    // Method begins at RVA 0x2125
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarWriter::.ctor

  .method /*0600003D*/ public hidebysig specialname 
          instance valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ 
          get_Format() cil managed
  // SIG: 20 00 11 1C
  {
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::get_Format

  .method /*0600003E*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2130
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarWriter::Dispose

  .method /*0600003F*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000021*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x2132
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::DisposeAsync

  .method /*06000040*/ public hidebysig instance void 
          WriteEntry(class System.Formats.Tar.TarEntry/*02000006*/ entry) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2135
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarWriter::WriteEntry

  .method /*06000041*/ public hidebysig instance void 
          WriteEntry(string fileName,
                     string entryName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000047*/ 
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarWriter::WriteEntry

  .method /*06000042*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          WriteEntryAsync(class System.Formats.Tar.TarEntry/*02000006*/ entry,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 12 71 12 18 11 75
  {
    .param [2]/*08000049*/  = nullref
    // Method begins at RVA 0x2139
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::WriteEntryAsync

  .method /*06000043*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          WriteEntryAsync(string fileName,
                          string entryName,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 0E 0E 11 75
  {
    .param [2]/*0800004B*/ 
    .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800004C*/  = nullref
    // Method begins at RVA 0x213c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::WriteEntryAsync

  .property /*17000015*/ instance callconv(8) valuetype System.Formats.Tar.TarEntryFormat/*02000007*/
          Format()
  {
    .get instance valuetype System.Formats.Tar.TarEntryFormat/*02000007*/ System.Formats.Tar.TarWriter/*0200000B*/::get_Format() /* 0600003D */
  } // end of property TarWriter::Format
} // end of class System.Formats.Tar.TarWriter

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Formats.Tar.UstarTarEntry
       extends System.Formats.Tar.PosixTarEntry/*02000005*/
{
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000044*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*02000006*/ other) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x213f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UstarTarEntry::.ctor

  .method /*06000045*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*02000008*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 20 0E
  {
    // Method begins at RVA 0x2147
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Formats.Tar.PosixTarEntry/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UstarTarEntry::.ctor

} // end of class System.Formats.Tar.UstarTarEntry

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Formats.Tar.V7TarEntry
       extends System.Formats.Tar.TarEntry/*02000006*/
{
  .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000046*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*02000006*/ other) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x214f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000016       */ call       instance void System.Formats.Tar.TarEntry/*02000006*/::.ctor() /* 06000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method V7TarEntry::.ctor

  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*02000008*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 20 0E
  {
    // Method begins at RVA 0x2157
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000016       */ call       instance void System.Formats.Tar.TarEntry/*02000006*/::.ctor() /* 06000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method V7TarEntry::.ctor

} // end of class System.Formats.Tar.V7TarEntry


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
