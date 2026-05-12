


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
// Time-date stamp:            0xdb143b9f
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000382e
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
// Checksum:                       0x000070db
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000037dc [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000504] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002400 [0x00002918] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000037c0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001834 Virtual Size
//              0x00002000 Virtual Address
//              0x00001a00 Size of Raw Data
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
//              0x00000504 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001c00 Pointer to Raw Data
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
//              0x00002200 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000830
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000381e Import Name Table
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
// 0x00002110 [0x00001630] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003740 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000a64 Size
//              '#~' Name
//    Stream 2:
//              0x00000ad0 Offset
//              0x000007c8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001298 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000129c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000012ac Offset
//              0x00000384 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000903b41547 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 19736
// PE header size       : 512 (496 used)    ( 2.59%)
// PE additional info   : 12003             (60.82%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.36%)
// CLR meta-data size  : 5680               (28.78%)
// CLR additional info : 128                ( 0.65%)
// CLR method headers  : 76                 ( 0.39%)
// Managed code         : 112               ( 0.57%)
// Data                 : 2048              (10.38%)
// Unaccounted          : -895              (-4.53%)

// Num.of PE sections   : 3
//   .text    - 6656
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 5680
//   Module        -    1 (10 bytes)
//   TypeDef       -   12 (168 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   32 (192 bytes)
//   MethodDef     -   63 (882 bytes)     0 abstract, 0 native, 47 bodies
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -   68 (408 bytes)
//   MethodImpl    -    1 (6 bytes)
//   CustomAttribute-   59 (354 bytes)
//   PropertyMap   -    7 (28 bytes)
//   Property      -   16 (96 bytes)
//   MethodSemantic-   26 (156 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    3 (18 bytes)
//   Strings       -  1990 bytes
//   Blobs         -   900 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 76
//   Num.of method bodies  - 47
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 46
//   Num.of fat sections   - 0
//   Num.of small sections - 1

// Managed code : 112
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AsyncCompletedEventArgs        (public) (auto) (ansi) 
// Class AsyncCompletedEventHandler     (public) (auto) (ansi) (sealed) 
// Class AsyncOperation                 (public) (auto) (ansi) (sealed) 
// Class AsyncOperationManager          (public) (abstract) (auto) (ansi) (sealed) 
// Class BackgroundWorker               (public) (auto) (ansi) 
// Class DoWorkEventArgs                (public) (auto) (ansi) 
// Class DoWorkEventHandler             (public) (auto) (ansi) (sealed) 
// Class ProgressChangedEventArgs       (public) (auto) (ansi) 
// Class ProgressChangedEventHandler    (public) (auto) (ansi) (sealed) 
// Class RunWorkerCompletedEventArgs    (public) (auto) (ansi) 
// Class RunWorkerCompletedEventHandler (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000002*/ System.Threading
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000003*/ System.ComponentModel.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000004*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.ComponentModel.EventBasedAsync
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 25 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // ..%System.Compon
                                                                                                                                                                        65 6E 74 4D 6F 64 65 6C 2E 45 76 65 6E 74 42 61   // entModel.EventBa
                                                                                                                                                                        73 65 64 41 73 79 6E 63 00 00 )                   // sedAsync..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 25 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // ..%System.Compon
                                                                                                                                                                       65 6E 74 4D 6F 64 65 6C 2E 45 76 65 6E 74 42 61   // entModel.EventBa
                                                                                                                                                                       73 65 64 41 73 79 6E 63 00 00 )                   // sedAsync..
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 25 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // ..%System.Compon
                                                                                                                                                                 65 6E 74 4D 6F 64 65 6C 2E 45 76 65 6E 74 42 61   // entModel.EventBa
                                                                                                                                                                 73 65 64 41 73 79 6E 63 00 00 )                   // sedAsync..
  .custom /*0C000013:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
.module System.ComponentModel.EventBasedAsync.dll
// MVID: {93039088-1402-47e9-88ff-0d548a0c1df9}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.ComponentModel.AsyncCompletedEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000015*/
{
  .custom /*0C000014:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000015:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000016*/ 'error',
                               bool cancelled,
                               object userState) cil managed
  // SIG: 20 03 01 12 59 02 1C
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000015*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsyncCompletedEventArgs::.ctor

  .method /*06000002*/ public hidebysig specialname 
          instance bool  get_Cancelled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2058
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncCompletedEventArgs::get_Cancelled

  .method /*06000003*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*01000016*/ 
          get_Error() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x205b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncCompletedEventArgs::get_Error

  .method /*06000004*/ public hidebysig specialname 
          instance object  get_UserState() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x205e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncCompletedEventArgs::get_UserState

  .method /*06000005*/ family hidebysig instance void 
          RaiseExceptionIfNecessary() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2061
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncCompletedEventArgs::RaiseExceptionIfNecessary

  .property /*17000001*/ instance callconv(8) bool
          Cancelled()
  {
    .get instance bool System.ComponentModel.AsyncCompletedEventArgs/*02000002*/::get_Cancelled() /* 06000002 */
  } // end of property AsyncCompletedEventArgs::Cancelled
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*01000016*/
          Error()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*01000016*/ System.ComponentModel.AsyncCompletedEventArgs/*02000002*/::get_Error() /* 06000003 */
  } // end of property AsyncCompletedEventArgs::Error
  .property /*17000003*/ instance callconv(8) object
          UserState()
  {
    .get instance object System.ComponentModel.AsyncCompletedEventArgs/*02000002*/::get_UserState() /* 06000004 */
  } // end of property AsyncCompletedEventArgs::UserState
} // end of class System.ComponentModel.AsyncCompletedEventArgs

.class /*02000003*/ public auto ansi sealed System.ComponentModel.AsyncCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000018*/
{
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method AsyncCompletedEventHandler::.ctor

  .method /*06000007*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.ComponentModel.AsyncCompletedEventArgs/*02000002*/ e) runtime managed
  // SIG: 20 02 01 1C 12 08
  {
    .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000006*/ 
    .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method AsyncCompletedEventHandler::Invoke

  .method /*06000008*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.ComponentModel.AsyncCompletedEventArgs/*02000002*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 08 12 69 1C
  {
    .param [1]/*08000008*/ 
    .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000009*/ 
    .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method AsyncCompletedEventHandler::BeginInvoke

  .method /*06000009*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method AsyncCompletedEventHandler::EndInvoke

} // end of class System.ComponentModel.AsyncCompletedEventHandler

.class /*02000004*/ public auto ansi sealed beforefieldinit System.ComponentModel.AsyncOperation
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000016:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600000A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2063
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsyncOperation::.ctor

  .method /*0600000B*/ public hidebysig specialname 
          instance class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/ 
          get_SynchronizationContext() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncOperation::get_SynchronizationContext

  .method /*0600000C*/ public hidebysig specialname 
          instance object  get_UserSuppliedState() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncOperation::get_UserSuppliedState

  .method /*0600000D*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000017*/::Finalize /*01000017::0A000014*/ 
    // Method begins at RVA 0x2074
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::Finalize() /* 0A000014 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method AsyncOperation::Finalize

  .method /*0600000E*/ public hidebysig instance void 
          OperationCompleted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncOperation::OperationCompleted

  .method /*0600000F*/ public hidebysig instance void 
          Post(class [System.Threading/*23000002*/]System.Threading.SendOrPostCallback/*0100001C*/ d,
               object arg) cil managed
  // SIG: 20 02 01 12 71 1C
  {
    .param [2]/*0800000E*/ 
    .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x209e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncOperation::Post

  .method /*06000010*/ public hidebysig instance void 
          PostOperationCompleted(class [System.Threading/*23000002*/]System.Threading.SendOrPostCallback/*0100001C*/ d,
                                 object arg) cil managed
  // SIG: 20 02 01 12 71 1C
  {
    .param [2]/*08000010*/ 
    .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncOperation::PostOperationCompleted

  .property /*17000004*/ instance callconv(8) class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/
          SynchronizationContext()
  {
    .get instance class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/ System.ComponentModel.AsyncOperation/*02000004*/::get_SynchronizationContext() /* 0600000B */
  } // end of property AsyncOperation::SynchronizationContext
  .property /*17000005*/ instance callconv(8) object
          UserSuppliedState()
  {
    .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance object System.ComponentModel.AsyncOperation/*02000004*/::get_UserSuppliedState() /* 0600000C */
  } // end of property AsyncOperation::UserSuppliedState
} // end of class System.ComponentModel.AsyncOperation

.class /*02000005*/ public abstract auto ansi sealed beforefieldinit System.ComponentModel.AsyncOperationManager
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000018:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000011*/ public hidebysig specialname static 
          class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/ 
          get_SynchronizationContext() cil managed
  // SIG: 00 00 12 6D
  {
    // Method begins at RVA 0x20a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncOperationManager::get_SynchronizationContext

  .method /*06000012*/ public hidebysig specialname static 
          void  set_SynchronizationContext(class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/ 'value') cil managed
  // SIG: 00 01 01 12 6D
  {
    // Method begins at RVA 0x20a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncOperationManager::set_SynchronizationContext

  .method /*06000013*/ public hidebysig static 
          class System.ComponentModel.AsyncOperation/*02000004*/ 
          CreateOperation(object userSuppliedState) cil managed
  // SIG: 00 01 12 10 1C
  {
    .param [1]/*08000012*/ 
    .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncOperationManager::CreateOperation

  .property /*17000006*/ callconv(8) class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/
          SynchronizationContext()
  {
    .custom /*0C00001E:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000015 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/ System.ComponentModel.AsyncOperationManager/*02000005*/::get_SynchronizationContext() /* 06000011 */
    .set void System.ComponentModel.AsyncOperationManager/*02000005*/::set_SynchronizationContext(class [System.Threading/*23000002*/]System.Threading.SynchronizationContext/*0100001B*/) /* 06000012 */
  } // end of property AsyncOperationManager::SynchronizationContext
} // end of class System.ComponentModel.AsyncOperationManager

.class /*02000006*/ public auto ansi beforefieldinit System.ComponentModel.BackgroundWorker
       extends [System.ComponentModel.Primitives/*23000003*/]System.ComponentModel.Component/*0100001F*/
{
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20aa
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.ComponentModel.Primitives/*23000003*/]System.ComponentModel.Component/*0100001F*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::.ctor

  .method /*06000015*/ public hidebysig specialname 
          instance bool  get_CancellationPending() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BackgroundWorker::get_CancellationPending

  .method /*06000016*/ public hidebysig specialname 
          instance bool  get_IsBusy() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BackgroundWorker::get_IsBusy

  .method /*06000017*/ public hidebysig specialname 
          instance bool  get_WorkerReportsProgress() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BackgroundWorker::get_WorkerReportsProgress

  .method /*06000018*/ public hidebysig specialname 
          instance void  set_WorkerReportsProgress(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::set_WorkerReportsProgress

  .method /*06000019*/ public hidebysig specialname 
          instance bool  get_WorkerSupportsCancellation() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BackgroundWorker::get_WorkerSupportsCancellation

  .method /*0600001A*/ public hidebysig specialname 
          instance void  set_WorkerSupportsCancellation(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::set_WorkerSupportsCancellation

  .method /*0600001B*/ public hidebysig specialname 
          instance void  add_DoWork(class System.ComponentModel.DoWorkEventHandler/*02000008*/ 'value') cil managed
  // SIG: 20 01 01 12 20
  {
    // Method begins at RVA 0x20c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::add_DoWork

  .method /*0600001C*/ public hidebysig specialname 
          instance void  remove_DoWork(class System.ComponentModel.DoWorkEventHandler/*02000008*/ 'value') cil managed
  // SIG: 20 01 01 12 20
  {
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::remove_DoWork

  .method /*0600001D*/ public hidebysig specialname 
          instance void  add_ProgressChanged(class System.ComponentModel.ProgressChangedEventHandler/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x20c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::add_ProgressChanged

  .method /*0600001E*/ public hidebysig specialname 
          instance void  remove_ProgressChanged(class System.ComponentModel.ProgressChangedEventHandler/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x20c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::remove_ProgressChanged

  .method /*0600001F*/ public hidebysig specialname 
          instance void  add_RunWorkerCompleted(class System.ComponentModel.RunWorkerCompletedEventHandler/*0200000C*/ 'value') cil managed
  // SIG: 20 01 01 12 30
  {
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::add_RunWorkerCompleted

  .method /*06000020*/ public hidebysig specialname 
          instance void  remove_RunWorkerCompleted(class System.ComponentModel.RunWorkerCompletedEventHandler/*0200000C*/ 'value') cil managed
  // SIG: 20 01 01 12 30
  {
    // Method begins at RVA 0x20cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::remove_RunWorkerCompleted

  .method /*06000021*/ public hidebysig instance void 
          CancelAsync() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ce
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::CancelAsync

  .method /*06000022*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::Dispose

  .method /*06000023*/ family hidebysig newslot virtual 
          instance void  OnDoWork(class System.ComponentModel.DoWorkEventArgs/*02000007*/ e) cil managed
  // SIG: 20 01 01 12 1C
  {
    .custom /*0C00002D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::OnDoWork

  .method /*06000024*/ family hidebysig newslot virtual 
          instance void  OnProgressChanged(class System.ComponentModel.ProgressChangedEventArgs/*02000009*/ e) cil managed
  // SIG: 20 01 01 12 24
  {
    .custom /*0C00002E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::OnProgressChanged

  .method /*06000025*/ family hidebysig newslot virtual 
          instance void  OnRunWorkerCompleted(class System.ComponentModel.RunWorkerCompletedEventArgs/*0200000B*/ e) cil managed
  // SIG: 20 01 01 12 2C
  {
    .custom /*0C00002F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::OnRunWorkerCompleted

  .method /*06000026*/ public hidebysig instance void 
          ReportProgress(int32 percentProgress) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::ReportProgress

  .method /*06000027*/ public hidebysig instance void 
          ReportProgress(int32 percentProgress,
                         object userState) cil managed
  // SIG: 20 02 01 08 1C
  {
    // Method begins at RVA 0x20da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::ReportProgress

  .method /*06000028*/ public hidebysig instance void 
          RunWorkerAsync() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::RunWorkerAsync

  .method /*06000029*/ public hidebysig instance void 
          RunWorkerAsync(object argument) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BackgroundWorker::RunWorkerAsync

  .event /*14000001*/ System.ComponentModel.DoWorkEventHandler/*02000008*/ DoWork
  {
    .addon instance void System.ComponentModel.BackgroundWorker/*02000006*/::add_DoWork(class System.ComponentModel.DoWorkEventHandler/*02000008*/) /* 0600001B */
    .removeon instance void System.ComponentModel.BackgroundWorker/*02000006*/::remove_DoWork(class System.ComponentModel.DoWorkEventHandler/*02000008*/) /* 0600001C */
  } // end of event BackgroundWorker::DoWork
  .event /*14000002*/ System.ComponentModel.ProgressChangedEventHandler/*0200000A*/ ProgressChanged
  {
    .addon instance void System.ComponentModel.BackgroundWorker/*02000006*/::add_ProgressChanged(class System.ComponentModel.ProgressChangedEventHandler/*0200000A*/) /* 0600001D */
    .removeon instance void System.ComponentModel.BackgroundWorker/*02000006*/::remove_ProgressChanged(class System.ComponentModel.ProgressChangedEventHandler/*0200000A*/) /* 0600001E */
  } // end of event BackgroundWorker::ProgressChanged
  .event /*14000003*/ System.ComponentModel.RunWorkerCompletedEventHandler/*0200000C*/ RunWorkerCompleted
  {
    .addon instance void System.ComponentModel.BackgroundWorker/*02000006*/::add_RunWorkerCompleted(class System.ComponentModel.RunWorkerCompletedEventHandler/*0200000C*/) /* 0600001F */
    .removeon instance void System.ComponentModel.BackgroundWorker/*02000006*/::remove_RunWorkerCompleted(class System.ComponentModel.RunWorkerCompletedEventHandler/*0200000C*/) /* 06000020 */
  } // end of event BackgroundWorker::RunWorkerCompleted
  .property /*17000007*/ instance callconv(8) bool
          CancellationPending()
  {
    .get instance bool System.ComponentModel.BackgroundWorker/*02000006*/::get_CancellationPending() /* 06000015 */
  } // end of property BackgroundWorker::CancellationPending
  .property /*17000008*/ instance callconv(8) bool
          IsBusy()
  {
    .get instance bool System.ComponentModel.BackgroundWorker/*02000006*/::get_IsBusy() /* 06000016 */
  } // end of property BackgroundWorker::IsBusy
  .property /*17000009*/ instance callconv(8) bool
          WorkerReportsProgress()
  {
    .get instance bool System.ComponentModel.BackgroundWorker/*02000006*/::get_WorkerReportsProgress() /* 06000017 */
    .set instance void System.ComponentModel.BackgroundWorker/*02000006*/::set_WorkerReportsProgress(bool) /* 06000018 */
  } // end of property BackgroundWorker::WorkerReportsProgress
  .property /*1700000A*/ instance callconv(8) bool
          WorkerSupportsCancellation()
  {
    .get instance bool System.ComponentModel.BackgroundWorker/*02000006*/::get_WorkerSupportsCancellation() /* 06000019 */
    .set instance void System.ComponentModel.BackgroundWorker/*02000006*/::set_WorkerSupportsCancellation(bool) /* 0600001A */
  } // end of property BackgroundWorker::WorkerSupportsCancellation
} // end of class System.ComponentModel.BackgroundWorker

.class /*02000007*/ public auto ansi beforefieldinit System.ComponentModel.DoWorkEventArgs
       extends [System.ComponentModel/*23000004*/]System.ComponentModel.CancelEventArgs/*01000020*/
{
  .custom /*0C000020:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object argument) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20e0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel/*23000004*/]System.ComponentModel.CancelEventArgs/*01000020*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DoWorkEventArgs::.ctor

  .method /*0600002B*/ public hidebysig specialname 
          instance object  get_Argument() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DoWorkEventArgs::get_Argument

  .method /*0600002C*/ public hidebysig specialname 
          instance object  get_Result() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DoWorkEventArgs::get_Result

  .method /*0600002D*/ public hidebysig specialname 
          instance void  set_Result(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DoWorkEventArgs::set_Result

  .property /*1700000B*/ instance callconv(8) object
          Argument()
  {
    .get instance object System.ComponentModel.DoWorkEventArgs/*02000007*/::get_Argument() /* 0600002B */
  } // end of property DoWorkEventArgs::Argument
  .property /*1700000C*/ instance callconv(8) object
          Result()
  {
    .get instance object System.ComponentModel.DoWorkEventArgs/*02000007*/::get_Result() /* 0600002C */
    .set instance void System.ComponentModel.DoWorkEventArgs/*02000007*/::set_Result(object) /* 0600002D */
  } // end of property DoWorkEventArgs::Result
} // end of class System.ComponentModel.DoWorkEventArgs

.class /*02000008*/ public auto ansi sealed System.ComponentModel.DoWorkEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000018*/
{
  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method DoWorkEventHandler::.ctor

  .method /*0600002F*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.ComponentModel.DoWorkEventArgs/*02000007*/ e) runtime managed
  // SIG: 20 02 01 1C 12 1C
  {
    .custom /*0C000033:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000027*/ 
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method DoWorkEventHandler::Invoke

  .method /*06000030*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.ComponentModel.DoWorkEventArgs/*02000007*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 1C 12 69 1C
  {
    .param [1]/*08000029*/ 
    .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800002A*/ 
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method DoWorkEventHandler::BeginInvoke

  .method /*06000031*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method DoWorkEventHandler::EndInvoke

} // end of class System.ComponentModel.DoWorkEventHandler

.class /*02000009*/ public auto ansi beforefieldinit System.ComponentModel.ProgressChangedEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000015*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000032*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 progressPercentage,
                               object userState) cil managed
  // SIG: 20 02 01 08 1C
  {
    // Method begins at RVA 0x20f0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000015*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProgressChangedEventArgs::.ctor

  .method /*06000033*/ public hidebysig specialname 
          instance int32  get_ProgressPercentage() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressChangedEventArgs::get_ProgressPercentage

  .method /*06000034*/ public hidebysig specialname 
          instance object  get_UserState() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressChangedEventArgs::get_UserState

  .property /*1700000D*/ instance callconv(8) int32
          ProgressPercentage()
  {
    .get instance int32 System.ComponentModel.ProgressChangedEventArgs/*02000009*/::get_ProgressPercentage() /* 06000033 */
  } // end of property ProgressChangedEventArgs::ProgressPercentage
  .property /*1700000E*/ instance callconv(8) object
          UserState()
  {
    .get instance object System.ComponentModel.ProgressChangedEventArgs/*02000009*/::get_UserState() /* 06000034 */
  } // end of property ProgressChangedEventArgs::UserState
} // end of class System.ComponentModel.ProgressChangedEventArgs

.class /*0200000A*/ public auto ansi sealed System.ComponentModel.ProgressChangedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000018*/
{
  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ProgressChangedEventHandler::.ctor

  .method /*06000036*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.ComponentModel.ProgressChangedEventArgs/*02000009*/ e) runtime managed
  // SIG: 20 02 01 1C 12 24
  {
    .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000032*/ 
    .custom /*0C000034:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method ProgressChangedEventHandler::Invoke

  .method /*06000037*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.ComponentModel.ProgressChangedEventArgs/*02000009*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 24 12 69 1C
  {
    .param [1]/*08000034*/ 
    .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000035*/ 
    .custom /*0C000036:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method ProgressChangedEventHandler::BeginInvoke

  .method /*06000038*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method ProgressChangedEventHandler::EndInvoke

} // end of class System.ComponentModel.ProgressChangedEventHandler

.class /*0200000B*/ public auto ansi beforefieldinit System.ComponentModel.RunWorkerCompletedEventArgs
       extends System.ComponentModel.AsyncCompletedEventArgs/*02000002*/
{
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000039*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object result,
                               class [System.Runtime/*23000001*/]System.Exception/*01000016*/ 'error',
                               bool cancelled) cil managed
  // SIG: 20 03 01 1C 12 59 02
  {
    // Method begins at RVA 0x20fe
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (06)000001       */ call       instance void System.ComponentModel.AsyncCompletedEventArgs/*02000002*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*01000016*/,
                                                                                                                                      bool,
                                                                                                                                      object) /* 06000001 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method RunWorkerCompletedEventArgs::.ctor

  .method /*0600003A*/ public hidebysig specialname 
          instance object  get_Result() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RunWorkerCompletedEventArgs::get_Result

  .method /*0600003B*/ public hidebysig specialname 
          instance object  get_UserState() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RunWorkerCompletedEventArgs::get_UserState

  .property /*1700000F*/ instance callconv(8) object
          Result()
  {
    .get instance object System.ComponentModel.RunWorkerCompletedEventArgs/*0200000B*/::get_Result() /* 0600003A */
  } // end of property RunWorkerCompletedEventArgs::Result
  .property /*17000010*/ instance callconv(8) object
          UserState()
  {
    .custom /*0C00002B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .get instance object System.ComponentModel.RunWorkerCompletedEventArgs/*0200000B*/::get_UserState() /* 0600003B */
  } // end of property RunWorkerCompletedEventArgs::UserState
} // end of class System.ComponentModel.RunWorkerCompletedEventArgs

.class /*0200000C*/ public auto ansi sealed System.ComponentModel.RunWorkerCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000018*/
{
  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method RunWorkerCompletedEventHandler::.ctor

  .method /*0600003D*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.ComponentModel.RunWorkerCompletedEventArgs/*0200000B*/ e) runtime managed
  // SIG: 20 02 01 1C 12 2C
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800003E*/ 
    .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method RunWorkerCompletedEventHandler::Invoke

  .method /*0600003E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.ComponentModel.RunWorkerCompletedEventArgs/*0200000B*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 2C 12 69 1C
  {
    .param [1]/*08000040*/ 
    .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000041*/ 
    .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method RunWorkerCompletedEventHandler::BeginInvoke

  .method /*0600003F*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method RunWorkerCompletedEventHandler::EndInvoke

} // end of class System.ComponentModel.RunWorkerCompletedEventHandler


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
