


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
// Time-date stamp:            0x9410a7ca
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000059d2
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
// Checksum:                       0x000159c7
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00005980 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00004400 [0x00002920] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00005964 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000039d8 Virtual Size
//              0x00002000 Virtual Address
//              0x00003a00 Size of Raw Data
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
//              0x00000484 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00003c00 Pointer to Raw Data
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
//              0x00004200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00005000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000009d4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000059c2 Import Name Table
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
// 0x00002258 [0x0000368c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000058e4 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001dc8 Size
//              '#~' Name
//    Stream 2:
//              0x00001e34 Offset
//              0x00000fb8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00002dec Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00002df0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00002e00 Offset
//              0x0000088c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901b41547 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 27936
// PE header size       : 512 (496 used)    ( 1.83%)
// PE additional info   : 11883             (42.54%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.26%)
// CLR meta-data size  : 13964              (49.99%)
// CLR additional info : 128                ( 0.46%)
// CLR method headers  : 168                ( 0.60%)
// Managed code         : 352               ( 1.26%)
// Data                 : 2048              ( 7.33%)
// Unaccounted          : -1191             (-4.26%)

// Num.of PE sections   : 3
//   .text    - 14848
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 13964
//   Module        -    1 (10 bytes)
//   TypeDef       -   24 (336 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   47 (282 bytes)
//   MethodDef     -  212 (2968 bytes)    0 abstract, 0 native, 168 bodies
//   MemberRef     -   28 (168 bytes)
//   ParamDef      -  317 (1902 bytes)
//   CustomAttribute-  177 (1062 bytes)
//   PropertyMap   -   12 (48 bytes)
//   Property      -   27 (162 bytes)
//   MethodSemantic-   61 (366 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    7 (140 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -   12 (72 bytes)
//   Strings       -  4022 bytes
//   Blobs         -  2188 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   192 bytes

// CLR additional info : 128

// CLR method headers : 168
//   Num.of method bodies  - 168
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 168

// Managed code : 352
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class DownloadDataCompletedEventArgs (public) (auto) (ansi) 
// Class DownloadDataCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class DownloadProgressChangedEventArgs (public) (auto) (ansi) 
// Class DownloadProgressChangedEventHandler (public) (auto) (ansi) (sealed) 
// Class DownloadStringCompletedEventArgs (public) (auto) (ansi) 
// Class DownloadStringCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class OpenReadCompletedEventArgs     (public) (auto) (ansi) 
// Class OpenReadCompletedEventHandler  (public) (auto) (ansi) (sealed) 
// Class OpenWriteCompletedEventArgs    (public) (auto) (ansi) 
// Class OpenWriteCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class UploadDataCompletedEventArgs   (public) (auto) (ansi) 
// Class UploadDataCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class UploadFileCompletedEventArgs   (public) (auto) (ansi) 
// Class UploadFileCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class UploadProgressChangedEventArgs (public) (auto) (ansi) 
// Class UploadProgressChangedEventHandler (public) (auto) (ansi) (sealed) 
// Class UploadStringCompletedEventArgs (public) (auto) (ansi) 
// Class UploadStringCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class UploadValuesCompletedEventArgs (public) (auto) (ansi) 
// Class UploadValuesCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class WebClient                      (public) (auto) (ansi) 
// Class WriteStreamClosedEventArgs     (public) (auto) (ansi) 
// Class WriteStreamClosedEventHandler  (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000002*/ System.ComponentModel.EventBasedAsync
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000003*/ System.ComponentModel.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000004*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000005*/ System.Net.WebHeaderCollection
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000006*/ System.Collections.Specialized
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000007*/ System.Net.Requests
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.WebClient
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 38 2E 30 01 00 54 0E 14   // ersion=v8.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 38 2E 30 )          // Name..NET 8.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                        62 43 6C 69 65 6E 74 00 00 )                      // bClient..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                       62 43 6C 69 65 6E 74 00 00 )                      // bClient..
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                 62 43 6C 69 65 6E 74 00 00 )                      // bClient..
  .custom /*0C000015:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
  .ver 8:0:0:0
}
.module System.Net.WebClient.dll
// MVID: {fe8ea129-9cfc-4977-98d1-b8f59254c753}
.custom /*0C000001:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Net.DownloadDataCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000017:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method DownloadDataCompletedEventArgs::.ctor

  .method /*06000002*/ public hidebysig specialname 
          instance uint8[]  get_Result() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x205b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DownloadDataCompletedEventArgs::get_Result

  .property /*17000001*/ instance callconv(8) uint8[]
          Result()
  {
    .get instance uint8[] System.Net.DownloadDataCompletedEventArgs/*02000002*/::get_Result() /* 06000002 */
  } // end of property DownloadDataCompletedEventArgs::Result
} // end of class System.Net.DownloadDataCompletedEventArgs

.class /*02000003*/ public auto ansi sealed System.Net.DownloadDataCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method DownloadDataCompletedEventHandler::.ctor

  .method /*06000004*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.DownloadDataCompletedEventArgs/*02000002*/ e) runtime managed
  // SIG: 20 02 01 1C 12 08
  {
    .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method DownloadDataCompletedEventHandler::Invoke

  .method /*06000005*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.DownloadDataCompletedEventArgs/*02000002*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 08 12 69 1C
  {
    .param [1]/*08000005*/ 
    .custom /*0C00001C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000006*/ 
    .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method DownloadDataCompletedEventHandler::BeginInvoke

  .method /*06000006*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method DownloadDataCompletedEventHandler::EndInvoke

} // end of class System.Net.DownloadDataCompletedEventHandler

.class /*02000004*/ public auto ansi beforefieldinit System.Net.DownloadProgressChangedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.ProgressChangedEventArgs/*0100001B*/
{
  .method /*06000007*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x205e
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (0A)00001A       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.ProgressChangedEventArgs/*0100001B*/::.ctor(int32,
                                                                                                                                                                                          object) /* 0A00001A */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method DownloadProgressChangedEventArgs::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance int64  get_BytesReceived() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DownloadProgressChangedEventArgs::get_BytesReceived

  .method /*06000009*/ public hidebysig specialname 
          instance int64  get_TotalBytesToReceive() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DownloadProgressChangedEventArgs::get_TotalBytesToReceive

  .property /*17000002*/ instance callconv(8) int64
          BytesReceived()
  {
    .get instance int64 System.Net.DownloadProgressChangedEventArgs/*02000004*/::get_BytesReceived() /* 06000008 */
  } // end of property DownloadProgressChangedEventArgs::BytesReceived
  .property /*17000003*/ instance callconv(8) int64
          TotalBytesToReceive()
  {
    .get instance int64 System.Net.DownloadProgressChangedEventArgs/*02000004*/::get_TotalBytesToReceive() /* 06000009 */
  } // end of property DownloadProgressChangedEventArgs::TotalBytesToReceive
} // end of class System.Net.DownloadProgressChangedEventArgs

.class /*02000005*/ public auto ansi sealed System.Net.DownloadProgressChangedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method DownloadProgressChangedEventHandler::.ctor

  .method /*0600000B*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.DownloadProgressChangedEventArgs/*02000004*/ e) runtime managed
  // SIG: 20 02 01 1C 12 10
  {
    .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method DownloadProgressChangedEventHandler::Invoke

  .method /*0600000C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.DownloadProgressChangedEventArgs/*02000004*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 10 12 69 1C
  {
    .param [1]/*0800000E*/ 
    .custom /*0C000033:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800000F*/ 
    .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method DownloadProgressChangedEventHandler::BeginInvoke

  .method /*0600000D*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method DownloadProgressChangedEventHandler::EndInvoke

} // end of class System.Net.DownloadProgressChangedEventHandler

.class /*02000006*/ public auto ansi beforefieldinit System.Net.DownloadStringCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600000E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206e
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method DownloadStringCompletedEventArgs::.ctor

  .method /*0600000F*/ public hidebysig specialname 
          instance string  get_Result() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DownloadStringCompletedEventArgs::get_Result

  .property /*17000004*/ instance callconv(8) string
          Result()
  {
    .get instance string System.Net.DownloadStringCompletedEventArgs/*02000006*/::get_Result() /* 0600000F */
  } // end of property DownloadStringCompletedEventArgs::Result
} // end of class System.Net.DownloadStringCompletedEventArgs

.class /*02000007*/ public auto ansi sealed System.Net.DownloadStringCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method DownloadStringCompletedEventHandler::.ctor

  .method /*06000011*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.DownloadStringCompletedEventArgs/*02000006*/ e) runtime managed
  // SIG: 20 02 01 1C 12 18
  {
    .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method DownloadStringCompletedEventHandler::Invoke

  .method /*06000012*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.DownloadStringCompletedEventArgs/*02000006*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 18 12 69 1C
  {
    .param [1]/*08000017*/ 
    .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000018*/ 
    .custom /*0C000049:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method DownloadStringCompletedEventHandler::BeginInvoke

  .method /*06000013*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method DownloadStringCompletedEventHandler::EndInvoke

} // end of class System.Net.DownloadStringCompletedEventHandler

.class /*02000008*/ public auto ansi beforefieldinit System.Net.OpenReadCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000014*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207c
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method OpenReadCompletedEventArgs::.ctor

  .method /*06000015*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          get_Result() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2087
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpenReadCompletedEventArgs::get_Result

  .property /*17000005*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/
          Result()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ System.Net.OpenReadCompletedEventArgs/*02000008*/::get_Result() /* 06000015 */
  } // end of property OpenReadCompletedEventArgs::Result
} // end of class System.Net.OpenReadCompletedEventArgs

.class /*02000009*/ public auto ansi sealed System.Net.OpenReadCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method OpenReadCompletedEventHandler::.ctor

  .method /*06000017*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.OpenReadCompletedEventArgs/*02000008*/ e) runtime managed
  // SIG: 20 02 01 1C 12 20
  {
    .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method OpenReadCompletedEventHandler::Invoke

  .method /*06000018*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.OpenReadCompletedEventArgs/*02000008*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 20 12 69 1C
  {
    .param [1]/*08000020*/ 
    .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000021*/ 
    .custom /*0C00004F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method OpenReadCompletedEventHandler::BeginInvoke

  .method /*06000019*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method OpenReadCompletedEventHandler::EndInvoke

} // end of class System.Net.OpenReadCompletedEventHandler

.class /*0200000A*/ public auto ansi beforefieldinit System.Net.OpenWriteCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x208a
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method OpenWriteCompletedEventArgs::.ctor

  .method /*0600001B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          get_Result() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpenWriteCompletedEventArgs::get_Result

  .property /*17000006*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/
          Result()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ System.Net.OpenWriteCompletedEventArgs/*0200000A*/::get_Result() /* 0600001B */
  } // end of property OpenWriteCompletedEventArgs::Result
} // end of class System.Net.OpenWriteCompletedEventArgs

.class /*0200000B*/ public auto ansi sealed System.Net.OpenWriteCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method OpenWriteCompletedEventHandler::.ctor

  .method /*0600001D*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.OpenWriteCompletedEventArgs/*0200000A*/ e) runtime managed
  // SIG: 20 02 01 1C 12 28
  {
    .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method OpenWriteCompletedEventHandler::Invoke

  .method /*0600001E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.OpenWriteCompletedEventArgs/*0200000A*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 28 12 69 1C
  {
    .param [1]/*08000029*/ 
    .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800002A*/ 
    .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method OpenWriteCompletedEventHandler::BeginInvoke

  .method /*0600001F*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method OpenWriteCompletedEventHandler::EndInvoke

} // end of class System.Net.OpenWriteCompletedEventHandler

.class /*0200000C*/ public auto ansi beforefieldinit System.Net.UploadDataCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000020*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2098
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method UploadDataCompletedEventArgs::.ctor

  .method /*06000021*/ public hidebysig specialname 
          instance uint8[]  get_Result() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadDataCompletedEventArgs::get_Result

  .property /*17000007*/ instance callconv(8) uint8[]
          Result()
  {
    .get instance uint8[] System.Net.UploadDataCompletedEventArgs/*0200000C*/::get_Result() /* 06000021 */
  } // end of property UploadDataCompletedEventArgs::Result
} // end of class System.Net.UploadDataCompletedEventArgs

.class /*0200000D*/ public auto ansi sealed System.Net.UploadDataCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UploadDataCompletedEventHandler::.ctor

  .method /*06000023*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.UploadDataCompletedEventArgs/*0200000C*/ e) runtime managed
  // SIG: 20 02 01 1C 12 30
  {
    .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadDataCompletedEventHandler::Invoke

  .method /*06000024*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.UploadDataCompletedEventArgs/*0200000C*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 30 12 69 1C
  {
    .param [1]/*08000032*/ 
    .custom /*0C000055:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000033*/ 
    .custom /*0C000056:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadDataCompletedEventHandler::BeginInvoke

  .method /*06000025*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method UploadDataCompletedEventHandler::EndInvoke

} // end of class System.Net.UploadDataCompletedEventHandler

.class /*0200000E*/ public auto ansi beforefieldinit System.Net.UploadFileCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000026*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a6
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method UploadFileCompletedEventArgs::.ctor

  .method /*06000027*/ public hidebysig specialname 
          instance uint8[]  get_Result() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadFileCompletedEventArgs::get_Result

  .property /*17000008*/ instance callconv(8) uint8[]
          Result()
  {
    .get instance uint8[] System.Net.UploadFileCompletedEventArgs/*0200000E*/::get_Result() /* 06000027 */
  } // end of property UploadFileCompletedEventArgs::Result
} // end of class System.Net.UploadFileCompletedEventArgs

.class /*0200000F*/ public auto ansi sealed System.Net.UploadFileCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UploadFileCompletedEventHandler::.ctor

  .method /*06000029*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.UploadFileCompletedEventArgs/*0200000E*/ e) runtime managed
  // SIG: 20 02 01 1C 12 38
  {
    .custom /*0C000051:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadFileCompletedEventHandler::Invoke

  .method /*0600002A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.UploadFileCompletedEventArgs/*0200000E*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 38 12 69 1C
  {
    .param [1]/*0800003B*/ 
    .custom /*0C000058:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800003C*/ 
    .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadFileCompletedEventHandler::BeginInvoke

  .method /*0600002B*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method UploadFileCompletedEventHandler::EndInvoke

} // end of class System.Net.UploadFileCompletedEventHandler

.class /*02000010*/ public auto ansi beforefieldinit System.Net.UploadProgressChangedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.ProgressChangedEventArgs/*0100001B*/
{
  .method /*0600002C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b4
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (0A)00001A       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.ProgressChangedEventArgs/*0100001B*/::.ctor(int32,
                                                                                                                                                                                          object) /* 0A00001A */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method UploadProgressChangedEventArgs::.ctor

  .method /*0600002D*/ public hidebysig specialname 
          instance int64  get_BytesReceived() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadProgressChangedEventArgs::get_BytesReceived

  .method /*0600002E*/ public hidebysig specialname 
          instance int64  get_BytesSent() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadProgressChangedEventArgs::get_BytesSent

  .method /*0600002F*/ public hidebysig specialname 
          instance int64  get_TotalBytesToReceive() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadProgressChangedEventArgs::get_TotalBytesToReceive

  .method /*06000030*/ public hidebysig specialname 
          instance int64  get_TotalBytesToSend() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadProgressChangedEventArgs::get_TotalBytesToSend

  .property /*17000009*/ instance callconv(8) int64
          BytesReceived()
  {
    .get instance int64 System.Net.UploadProgressChangedEventArgs/*02000010*/::get_BytesReceived() /* 0600002D */
  } // end of property UploadProgressChangedEventArgs::BytesReceived
  .property /*1700000A*/ instance callconv(8) int64
          BytesSent()
  {
    .get instance int64 System.Net.UploadProgressChangedEventArgs/*02000010*/::get_BytesSent() /* 0600002E */
  } // end of property UploadProgressChangedEventArgs::BytesSent
  .property /*1700000B*/ instance callconv(8) int64
          TotalBytesToReceive()
  {
    .get instance int64 System.Net.UploadProgressChangedEventArgs/*02000010*/::get_TotalBytesToReceive() /* 0600002F */
  } // end of property UploadProgressChangedEventArgs::TotalBytesToReceive
  .property /*1700000C*/ instance callconv(8) int64
          TotalBytesToSend()
  {
    .get instance int64 System.Net.UploadProgressChangedEventArgs/*02000010*/::get_TotalBytesToSend() /* 06000030 */
  } // end of property UploadProgressChangedEventArgs::TotalBytesToSend
} // end of class System.Net.UploadProgressChangedEventArgs

.class /*02000011*/ public auto ansi sealed System.Net.UploadProgressChangedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000031*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UploadProgressChangedEventHandler::.ctor

  .method /*06000032*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.UploadProgressChangedEventArgs/*02000010*/ e) runtime managed
  // SIG: 20 02 01 1C 12 40
  {
    .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadProgressChangedEventHandler::Invoke

  .method /*06000033*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.UploadProgressChangedEventArgs/*02000010*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 40 12 69 1C
  {
    .param [1]/*08000044*/ 
    .custom /*0C00005C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000045*/ 
    .custom /*0C00005D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadProgressChangedEventHandler::BeginInvoke

  .method /*06000034*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method UploadProgressChangedEventHandler::EndInvoke

} // end of class System.Net.UploadProgressChangedEventHandler

.class /*02000012*/ public auto ansi beforefieldinit System.Net.UploadStringCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000035*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ca
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method UploadStringCompletedEventArgs::.ctor

  .method /*06000036*/ public hidebysig specialname 
          instance string  get_Result() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadStringCompletedEventArgs::get_Result

  .property /*1700000D*/ instance callconv(8) string
          Result()
  {
    .get instance string System.Net.UploadStringCompletedEventArgs/*02000012*/::get_Result() /* 06000036 */
  } // end of property UploadStringCompletedEventArgs::Result
} // end of class System.Net.UploadStringCompletedEventArgs

.class /*02000013*/ public auto ansi sealed System.Net.UploadStringCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*06000037*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UploadStringCompletedEventHandler::.ctor

  .method /*06000038*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.UploadStringCompletedEventArgs/*02000012*/ e) runtime managed
  // SIG: 20 02 01 1C 12 48
  {
    .custom /*0C000057:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadStringCompletedEventHandler::Invoke

  .method /*06000039*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.UploadStringCompletedEventArgs/*02000012*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 48 12 69 1C
  {
    .param [1]/*0800004D*/ 
    .custom /*0C000062:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800004E*/ 
    .custom /*0C000063:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadStringCompletedEventHandler::BeginInvoke

  .method /*0600003A*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method UploadStringCompletedEventHandler::EndInvoke

} // end of class System.Net.UploadStringCompletedEventHandler

.class /*02000014*/ public auto ansi beforefieldinit System.Net.UploadValuesCompletedEventArgs
       extends [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/
{
  .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600003B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d8
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (0A)000019       */ call       instance void [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/::.ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002F*/,
                                                                                                                                                                                         bool,
                                                                                                                                                                                         object) /* 0A000019 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method UploadValuesCompletedEventArgs::.ctor

  .method /*0600003C*/ public hidebysig specialname 
          instance uint8[]  get_Result() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UploadValuesCompletedEventArgs::get_Result

  .property /*1700000E*/ instance callconv(8) uint8[]
          Result()
  {
    .get instance uint8[] System.Net.UploadValuesCompletedEventArgs/*02000014*/::get_Result() /* 0600003C */
  } // end of property UploadValuesCompletedEventArgs::Result
} // end of class System.Net.UploadValuesCompletedEventArgs

.class /*02000015*/ public auto ansi sealed System.Net.UploadValuesCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UploadValuesCompletedEventHandler::.ctor

  .method /*0600003E*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.UploadValuesCompletedEventArgs/*02000014*/ e) runtime managed
  // SIG: 20 02 01 1C 12 50
  {
    .custom /*0C00005A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadValuesCompletedEventHandler::Invoke

  .method /*0600003F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.UploadValuesCompletedEventArgs/*02000014*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 50 12 69 1C
  {
    .param [1]/*08000056*/ 
    .custom /*0C000067:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000057*/ 
    .custom /*0C000068:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method UploadValuesCompletedEventHandler::BeginInvoke

  .method /*06000040*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method UploadValuesCompletedEventHandler::EndInvoke

} // end of class System.Net.UploadValuesCompletedEventHandler

.class /*02000016*/ public auto ansi beforefieldinit System.Net.WebClient
       extends [System.ComponentModel.Primitives/*23000003*/]System.ComponentModel.Component/*0100001D*/
{
  .custom /*0C000040:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000041*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00005B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string) /* 0A000017 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x20e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.ComponentModel.Primitives/*23000003*/]System.ComponentModel.Component/*0100001D*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebClient::.ctor

  .method /*06000042*/ public hidebysig specialname 
          instance bool  get_AllowReadStreamBuffering() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_AllowReadStreamBuffering

  .method /*06000043*/ public hidebysig specialname 
          instance void  set_AllowReadStreamBuffering(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_AllowReadStreamBuffering

  .method /*06000044*/ public hidebysig specialname 
          instance bool  get_AllowWriteStreamBuffering() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_AllowWriteStreamBuffering

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_AllowWriteStreamBuffering(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20f6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_AllowWriteStreamBuffering

  .method /*06000046*/ public hidebysig specialname 
          instance string  get_BaseAddress() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_BaseAddress

  .method /*06000047*/ public hidebysig specialname 
          instance void  set_BaseAddress(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800005D*/ 
    .custom /*0C00006F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000022*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_BaseAddress

  .method /*06000048*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000004*/]System.Net.Cache.RequestCachePolicy/*01000023*/ 
          get_CachePolicy() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C00005E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_CachePolicy

  .method /*06000049*/ public hidebysig specialname 
          instance void  set_CachePolicy(class [System.Net.Primitives/*23000004*/]System.Net.Cache.RequestCachePolicy/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C00005F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_CachePolicy

  .method /*0600004A*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000004*/]System.Net.ICredentials/*01000024*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_Credentials

  .method /*0600004B*/ public hidebysig specialname 
          instance void  set_Credentials(class [System.Net.Primitives/*23000004*/]System.Net.ICredentials/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    .custom /*0C000061:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_Credentials

  .method /*0600004C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/ 
          get_Encoding() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_Encoding

  .method /*0600004D*/ public hidebysig specialname 
          instance void  set_Encoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/ 'value') cil managed
  // SIG: 20 01 01 12 80 95
  {
    // Method begins at RVA 0x210a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_Encoding

  .method /*0600004E*/ public hidebysig specialname 
          instance class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 80 99
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_Headers

  .method /*0600004F*/ public hidebysig specialname 
          instance void  set_Headers(class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 12 80 99
  {
    .param [1]/*08000061*/ 
    .custom /*0C000074:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000022*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_Headers

  .method /*06000050*/ public hidebysig specialname 
          instance bool  get_IsBusy() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_IsBusy

  .method /*06000051*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000004*/]System.Net.IWebProxy/*01000027*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_Proxy

  .method /*06000052*/ public hidebysig specialname 
          instance void  set_Proxy(class [System.Net.Primitives/*23000004*/]System.Net.IWebProxy/*01000027*/ 'value') cil managed
  // SIG: 20 01 01 12 80 9D
  {
    .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2117
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_Proxy

  .method /*06000053*/ public hidebysig specialname 
          instance class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ 
          get_QueryString() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_QueryString

  .method /*06000054*/ public hidebysig specialname 
          instance void  set_QueryString(class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .param [1]/*08000063*/ 
    .custom /*0C000077:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000022*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_QueryString

  .method /*06000055*/ public hidebysig specialname 
          instance class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/ 
          get_ResponseHeaders() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C000066:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_ResponseHeaders

  .method /*06000056*/ public hidebysig specialname 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2121
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::get_UseDefaultCredentials

  .method /*06000057*/ public hidebysig specialname 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2124
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::set_UseDefaultCredentials

  .method /*06000058*/ public hidebysig specialname 
          instance void  add_DownloadDataCompleted(class System.Net.DownloadDataCompletedEventHandler/*02000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0C
  {
    .custom /*0C000069:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2126
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_DownloadDataCompleted

  .method /*06000059*/ public hidebysig specialname 
          instance void  remove_DownloadDataCompleted(class System.Net.DownloadDataCompletedEventHandler/*02000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0C
  {
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_DownloadDataCompleted

  .method /*0600005A*/ public hidebysig specialname 
          instance void  add_DownloadFileCompleted(class [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventHandler/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C00006B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_DownloadFileCompleted

  .method /*0600005B*/ public hidebysig specialname 
          instance void  remove_DownloadFileCompleted(class [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventHandler/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C00006C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_DownloadFileCompleted

  .method /*0600005C*/ public hidebysig specialname 
          instance void  add_DownloadProgressChanged(class System.Net.DownloadProgressChangedEventHandler/*02000005*/ 'value') cil managed
  // SIG: 20 01 01 12 14
  {
    .custom /*0C00006D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_DownloadProgressChanged

  .method /*0600005D*/ public hidebysig specialname 
          instance void  remove_DownloadProgressChanged(class System.Net.DownloadProgressChangedEventHandler/*02000005*/ 'value') cil managed
  // SIG: 20 01 01 12 14
  {
    .custom /*0C00006E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2130
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_DownloadProgressChanged

  .method /*0600005E*/ public hidebysig specialname 
          instance void  add_DownloadStringCompleted(class System.Net.DownloadStringCompletedEventHandler/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1C
  {
    .custom /*0C000070:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2132
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_DownloadStringCompleted

  .method /*0600005F*/ public hidebysig specialname 
          instance void  remove_DownloadStringCompleted(class System.Net.DownloadStringCompletedEventHandler/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1C
  {
    .custom /*0C000071:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_DownloadStringCompleted

  .method /*06000060*/ public hidebysig specialname 
          instance void  add_OpenReadCompleted(class System.Net.OpenReadCompletedEventHandler/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 12 24
  {
    .custom /*0C000072:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2136
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_OpenReadCompleted

  .method /*06000061*/ public hidebysig specialname 
          instance void  remove_OpenReadCompleted(class System.Net.OpenReadCompletedEventHandler/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 12 24
  {
    .custom /*0C000073:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2138
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_OpenReadCompleted

  .method /*06000062*/ public hidebysig specialname 
          instance void  add_OpenWriteCompleted(class System.Net.OpenWriteCompletedEventHandler/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2C
  {
    .custom /*0C000075:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_OpenWriteCompleted

  .method /*06000063*/ public hidebysig specialname 
          instance void  remove_OpenWriteCompleted(class System.Net.OpenWriteCompletedEventHandler/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2C
  {
    .custom /*0C000076:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_OpenWriteCompleted

  .method /*06000064*/ public hidebysig specialname 
          instance void  add_UploadDataCompleted(class System.Net.UploadDataCompletedEventHandler/*0200000D*/ 'value') cil managed
  // SIG: 20 01 01 12 34
  {
    .custom /*0C000078:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_UploadDataCompleted

  .method /*06000065*/ public hidebysig specialname 
          instance void  remove_UploadDataCompleted(class System.Net.UploadDataCompletedEventHandler/*0200000D*/ 'value') cil managed
  // SIG: 20 01 01 12 34
  {
    .custom /*0C000079:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2140
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_UploadDataCompleted

  .method /*06000066*/ public hidebysig specialname 
          instance void  add_UploadFileCompleted(class System.Net.UploadFileCompletedEventHandler/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 12 3C
  {
    .custom /*0C00007A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2142
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_UploadFileCompleted

  .method /*06000067*/ public hidebysig specialname 
          instance void  remove_UploadFileCompleted(class System.Net.UploadFileCompletedEventHandler/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 12 3C
  {
    .custom /*0C00007B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2144
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_UploadFileCompleted

  .method /*06000068*/ public hidebysig specialname 
          instance void  add_UploadProgressChanged(class System.Net.UploadProgressChangedEventHandler/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C00007C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2146
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_UploadProgressChanged

  .method /*06000069*/ public hidebysig specialname 
          instance void  remove_UploadProgressChanged(class System.Net.UploadProgressChangedEventHandler/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C00007D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2148
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_UploadProgressChanged

  .method /*0600006A*/ public hidebysig specialname 
          instance void  add_UploadStringCompleted(class System.Net.UploadStringCompletedEventHandler/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C00007E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_UploadStringCompleted

  .method /*0600006B*/ public hidebysig specialname 
          instance void  remove_UploadStringCompleted(class System.Net.UploadStringCompletedEventHandler/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C00007F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_UploadStringCompleted

  .method /*0600006C*/ public hidebysig specialname 
          instance void  add_UploadValuesCompleted(class System.Net.UploadValuesCompletedEventHandler/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    .custom /*0C000080:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_UploadValuesCompleted

  .method /*0600006D*/ public hidebysig specialname 
          instance void  remove_UploadValuesCompleted(class System.Net.UploadValuesCompletedEventHandler/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    .custom /*0C000081:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2150
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_UploadValuesCompleted

  .method /*0600006E*/ public hidebysig specialname 
          instance void  add_WriteStreamClosed(class System.Net.WriteStreamClosedEventHandler/*02000018*/ 'value') cil managed
  // SIG: 20 01 01 12 60
  {
    .custom /*0C000082:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2152
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::add_WriteStreamClosed

  .method /*0600006F*/ public hidebysig specialname 
          instance void  remove_WriteStreamClosed(class System.Net.WriteStreamClosedEventHandler/*02000018*/ 'value') cil managed
  // SIG: 20 01 01 12 60
  {
    .custom /*0C000083:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2154
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::remove_WriteStreamClosed

  .method /*06000070*/ public hidebysig instance void 
          CancelAsync() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2156
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::CancelAsync

  .method /*06000071*/ public hidebysig instance uint8[] 
          DownloadData(string address) cil managed
  // SIG: 20 01 1D 05 0E
  {
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadData

  .method /*06000072*/ public hidebysig instance uint8[] 
          DownloadData(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 1D 05 12 80 A5
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadData

  .method /*06000073*/ public hidebysig instance void 
          DownloadDataAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x215e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadDataAsync

  .method /*06000074*/ public hidebysig instance void 
          DownloadDataAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            object userToken) cil managed
  // SIG: 20 02 01 12 80 A5 1C
  {
    .param [2]/*08000081*/ 
    .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2160
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadDataAsync

  .method /*06000075*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          DownloadDataTaskAsync(string address) cil managed
  // SIG: 20 01 15 12 80 A9 01 1D 05 0E
  {
    // Method begins at RVA 0x2162
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadDataTaskAsync

  .method /*06000076*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          DownloadDataTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 15 12 80 A9 01 1D 05 12 80 A5
  {
    // Method begins at RVA 0x2165
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadDataTaskAsync

  .method /*06000077*/ public hidebysig instance void 
          DownloadFile(string address,
                       string fileName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2168
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadFile

  .method /*06000078*/ public hidebysig instance void 
          DownloadFile(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                       string fileName) cil managed
  // SIG: 20 02 01 12 80 A5 0E
  {
    // Method begins at RVA 0x216a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadFile

  .method /*06000079*/ public hidebysig instance void 
          DownloadFileAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string fileName) cil managed
  // SIG: 20 02 01 12 80 A5 0E
  {
    // Method begins at RVA 0x216c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadFileAsync

  .method /*0600007A*/ public hidebysig instance void 
          DownloadFileAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string fileName,
                            object userToken) cil managed
  // SIG: 20 03 01 12 80 A5 0E 1C
  {
    .param [3]/*0800008C*/ 
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadFileAsync

  .method /*0600007B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          DownloadFileTaskAsync(string address,
                                string fileName) cil managed
  // SIG: 20 02 12 80 AD 0E 0E
  {
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadFileTaskAsync

  .method /*0600007C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          DownloadFileTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                                string fileName) cil managed
  // SIG: 20 02 12 80 AD 12 80 A5 0E
  {
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadFileTaskAsync

  .method /*0600007D*/ public hidebysig instance string 
          DownloadString(string address) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadString

  .method /*0600007E*/ public hidebysig instance string 
          DownloadString(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 0E 12 80 A5
  {
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadString

  .method /*0600007F*/ public hidebysig instance void 
          DownloadStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x217c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadStringAsync

  .method /*06000080*/ public hidebysig instance void 
          DownloadStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                              object userToken) cil managed
  // SIG: 20 02 01 12 80 A5 1C
  {
    .param [2]/*08000095*/ 
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x217e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::DownloadStringAsync

  .method /*06000081*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<string> 
          DownloadStringTaskAsync(string address) cil managed
  // SIG: 20 01 15 12 80 A9 01 0E 0E
  {
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadStringTaskAsync

  .method /*06000082*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<string> 
          DownloadStringTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 15 12 80 A9 01 0E 12 80 A5
  {
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::DownloadStringTaskAsync

  .method /*06000083*/ family hidebysig newslot virtual 
          instance class [System.Net.Requests/*23000007*/]System.Net.WebRequest/*0100002C*/ 
          GetWebRequest(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 12 80 B1 12 80 A5
  {
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::GetWebRequest

  .method /*06000084*/ family hidebysig newslot virtual 
          instance class [System.Net.Requests/*23000007*/]System.Net.WebResponse/*0100002D*/ 
          GetWebResponse(class [System.Net.Requests/*23000007*/]System.Net.WebRequest/*0100002C*/ 'request') cil managed
  // SIG: 20 01 12 80 B5 12 80 B1
  {
    // Method begins at RVA 0x2189
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::GetWebResponse

  .method /*06000085*/ family hidebysig newslot virtual 
          instance class [System.Net.Requests/*23000007*/]System.Net.WebResponse/*0100002D*/ 
          GetWebResponse(class [System.Net.Requests/*23000007*/]System.Net.WebRequest/*0100002C*/ 'request',
                         class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) cil managed
  // SIG: 20 02 12 80 B5 12 80 B1 12 65
  {
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::GetWebResponse

  .method /*06000086*/ family hidebysig newslot virtual 
          instance void  OnDownloadDataCompleted(class System.Net.DownloadDataCompletedEventArgs/*02000002*/ e) cil managed
  // SIG: 20 01 01 12 08
  {
    // Method begins at RVA 0x218f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnDownloadDataCompleted

  .method /*06000087*/ family hidebysig newslot virtual 
          instance void  OnDownloadFileCompleted(class [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventArgs/*01000016*/ e) cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x2191
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnDownloadFileCompleted

  .method /*06000088*/ family hidebysig newslot virtual 
          instance void  OnDownloadProgressChanged(class System.Net.DownloadProgressChangedEventArgs/*02000004*/ e) cil managed
  // SIG: 20 01 01 12 10
  {
    // Method begins at RVA 0x2193
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnDownloadProgressChanged

  .method /*06000089*/ family hidebysig newslot virtual 
          instance void  OnDownloadStringCompleted(class System.Net.DownloadStringCompletedEventArgs/*02000006*/ e) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2195
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnDownloadStringCompleted

  .method /*0600008A*/ family hidebysig newslot virtual 
          instance void  OnOpenReadCompleted(class System.Net.OpenReadCompletedEventArgs/*02000008*/ e) cil managed
  // SIG: 20 01 01 12 20
  {
    // Method begins at RVA 0x2197
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnOpenReadCompleted

  .method /*0600008B*/ family hidebysig newslot virtual 
          instance void  OnOpenWriteCompleted(class System.Net.OpenWriteCompletedEventArgs/*0200000A*/ e) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x2199
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnOpenWriteCompleted

  .method /*0600008C*/ family hidebysig newslot virtual 
          instance void  OnUploadDataCompleted(class System.Net.UploadDataCompletedEventArgs/*0200000C*/ e) cil managed
  // SIG: 20 01 01 12 30
  {
    // Method begins at RVA 0x219b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnUploadDataCompleted

  .method /*0600008D*/ family hidebysig newslot virtual 
          instance void  OnUploadFileCompleted(class System.Net.UploadFileCompletedEventArgs/*0200000E*/ e) cil managed
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x219d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnUploadFileCompleted

  .method /*0600008E*/ family hidebysig newslot virtual 
          instance void  OnUploadProgressChanged(class System.Net.UploadProgressChangedEventArgs/*02000010*/ e) cil managed
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x219f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnUploadProgressChanged

  .method /*0600008F*/ family hidebysig newslot virtual 
          instance void  OnUploadStringCompleted(class System.Net.UploadStringCompletedEventArgs/*02000012*/ e) cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x21a1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnUploadStringCompleted

  .method /*06000090*/ family hidebysig newslot virtual 
          instance void  OnUploadValuesCompleted(class System.Net.UploadValuesCompletedEventArgs/*02000014*/ e) cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x21a3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnUploadValuesCompleted

  .method /*06000091*/ family hidebysig newslot virtual 
          instance void  OnWriteStreamClosed(class System.Net.WriteStreamClosedEventArgs/*02000017*/ e) cil managed
  // SIG: 20 01 01 12 5C
  {
    .custom /*0C000086:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000087:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string,
                                                                                                                        bool) /* 0A000016 */ = ( 01 00 6B 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..kThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 46 72   // orts the .NET Fr
                                                                                                                                                 61 6D 65 77 6F 72 6B 20 69 6E 66 72 61 73 74 72   // amework infrastr
                                                                                                                                                 75 63 74 75 72 65 20 61 6E 64 20 69 73 20 6E 6F   // ucture and is no
                                                                                                                                                 74 20 69 6E 74 65 6E 64 65 64 20 74 6F 20 62 65   // t intended to be
                                                                                                                                                 20 75 73 65 64 20 64 69 72 65 63 74 6C 79 20 66   //  used directly f
                                                                                                                                                 72 6F 6D 20 79 6F 75 72 20 63 6F 64 65 2E 01 00   // rom your code...
                                                                                                                                                 00 ) 
    // Method begins at RVA 0x21a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OnWriteStreamClosed

  .method /*06000092*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          OpenRead(string address) cil managed
  // SIG: 20 01 12 71 0E
  {
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenRead

  .method /*06000093*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          OpenRead(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 12 71 12 80 A5
  {
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenRead

  .method /*06000094*/ public hidebysig instance void 
          OpenReadAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x21ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OpenReadAsync

  .method /*06000095*/ public hidebysig instance void 
          OpenReadAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                        object userToken) cil managed
  // SIG: 20 02 01 12 80 A5 1C
  {
    .param [2]/*080000AC*/ 
    .custom /*0C00008A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OpenReadAsync

  .method /*06000096*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenReadTaskAsync(string address) cil managed
  // SIG: 20 01 15 12 80 A9 01 12 71 0E
  {
    // Method begins at RVA 0x21b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenReadTaskAsync

  .method /*06000097*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenReadTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 15 12 80 A9 01 12 71 12 80 A5
  {
    // Method begins at RVA 0x21b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenReadTaskAsync

  .method /*06000098*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          OpenWrite(string address) cil managed
  // SIG: 20 01 12 71 0E
  {
    // Method begins at RVA 0x21b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWrite

  .method /*06000099*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          OpenWrite(string address,
                    string 'method') cil managed
  // SIG: 20 02 12 71 0E 0E
  {
    .param [2]/*080000B1*/ 
    .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWrite

  .method /*0600009A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          OpenWrite(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 12 71 12 80 A5
  {
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWrite

  .method /*0600009B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          OpenWrite(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                    string 'method') cil managed
  // SIG: 20 02 12 71 12 80 A5 0E
  {
    .param [2]/*080000B4*/ 
    .custom /*0C00008C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWrite

  .method /*0600009C*/ public hidebysig instance void 
          OpenWriteAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x21c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OpenWriteAsync

  .method /*0600009D*/ public hidebysig instance void 
          OpenWriteAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                         string 'method') cil managed
  // SIG: 20 02 01 12 80 A5 0E
  {
    .param [2]/*080000B7*/ 
    .custom /*0C00008D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OpenWriteAsync

  .method /*0600009E*/ public hidebysig instance void 
          OpenWriteAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                         string 'method',
                         object userToken) cil managed
  // SIG: 20 03 01 12 80 A5 0E 1C
  {
    .custom /*0C000089:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000B8*/ 
    .custom /*0C00008E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::OpenWriteAsync

  .method /*0600009F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenWriteTaskAsync(string address) cil managed
  // SIG: 20 01 15 12 80 A9 01 12 71 0E
  {
    // Method begins at RVA 0x21c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWriteTaskAsync

  .method /*060000A0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenWriteTaskAsync(string address,
                             string 'method') cil managed
  // SIG: 20 02 15 12 80 A9 01 12 71 0E 0E
  {
    .param [2]/*080000BD*/ 
    .custom /*0C00008F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWriteTaskAsync

  .method /*060000A1*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenWriteTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address) cil managed
  // SIG: 20 01 15 12 80 A9 01 12 71 12 80 A5
  {
    // Method begins at RVA 0x21cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWriteTaskAsync

  .method /*060000A2*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenWriteTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                             string 'method') cil managed
  // SIG: 20 02 15 12 80 A9 01 12 71 12 80 A5 0E
  {
    .param [2]/*080000C0*/ 
    .custom /*0C000090:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::OpenWriteTaskAsync

  .method /*060000A3*/ public hidebysig instance uint8[] 
          UploadData(string address,
                     uint8[] data) cil managed
  // SIG: 20 02 1D 05 0E 1D 05
  {
    // Method begins at RVA 0x21d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadData

  .method /*060000A4*/ public hidebysig instance uint8[] 
          UploadData(string address,
                     string 'method',
                     uint8[] data) cil managed
  // SIG: 20 03 1D 05 0E 0E 1D 05
  {
    .param [2]/*080000C4*/ 
    .custom /*0C000091:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadData

  .method /*060000A5*/ public hidebysig instance uint8[] 
          UploadData(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                     uint8[] data) cil managed
  // SIG: 20 02 1D 05 12 80 A5 1D 05
  {
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadData

  .method /*060000A6*/ public hidebysig instance uint8[] 
          UploadData(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                     string 'method',
                     uint8[] data) cil managed
  // SIG: 20 03 1D 05 12 80 A5 0E 1D 05
  {
    .param [2]/*080000C9*/ 
    .custom /*0C000092:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadData

  .method /*060000A7*/ public hidebysig instance void 
          UploadDataAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                          uint8[] data) cil managed
  // SIG: 20 02 01 12 80 A5 1D 05
  {
    // Method begins at RVA 0x21e1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadDataAsync

  .method /*060000A8*/ public hidebysig instance void 
          UploadDataAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                          string 'method',
                          uint8[] data) cil managed
  // SIG: 20 03 01 12 80 A5 0E 1D 05
  {
    .param [2]/*080000CE*/ 
    .custom /*0C000093:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadDataAsync

  .method /*060000A9*/ public hidebysig instance void 
          UploadDataAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                          string 'method',
                          uint8[] data,
                          object userToken) cil managed
  // SIG: 20 04 01 12 80 A5 0E 1D 05 1C
  {
    .param [2]/*080000D1*/ 
    .custom /*0C000096:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000D3*/ 
    .custom /*0C000098:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadDataAsync

  .method /*060000AA*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadDataTaskAsync(string address,
                              uint8[] data) cil managed
  // SIG: 20 02 15 12 80 A9 01 1D 05 0E 1D 05
  {
    // Method begins at RVA 0x21e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadDataTaskAsync

  .method /*060000AB*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadDataTaskAsync(string address,
                              string 'method',
                              uint8[] data) cil managed
  // SIG: 20 03 15 12 80 A9 01 1D 05 0E 0E 1D 05
  {
    .param [2]/*080000D7*/ 
    .custom /*0C000099:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadDataTaskAsync

  .method /*060000AC*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadDataTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                              uint8[] data) cil managed
  // SIG: 20 02 15 12 80 A9 01 1D 05 12 80 A5 1D 05
  {
    // Method begins at RVA 0x21ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadDataTaskAsync

  .method /*060000AD*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadDataTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                              string 'method',
                              uint8[] data) cil managed
  // SIG: 20 03 15 12 80 A9 01 1D 05 12 80 A5 0E 1D 05
  {
    .param [2]/*080000DC*/ 
    .custom /*0C00009A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadDataTaskAsync

  .method /*060000AE*/ public hidebysig instance uint8[] 
          UploadFile(string address,
                     string fileName) cil managed
  // SIG: 20 02 1D 05 0E 0E
  {
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFile

  .method /*060000AF*/ public hidebysig instance uint8[] 
          UploadFile(string address,
                     string 'method',
                     string fileName) cil managed
  // SIG: 20 03 1D 05 0E 0E 0E
  {
    .param [2]/*080000E1*/ 
    .custom /*0C00009B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFile

  .method /*060000B0*/ public hidebysig instance uint8[] 
          UploadFile(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                     string fileName) cil managed
  // SIG: 20 02 1D 05 12 80 A5 0E
  {
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFile

  .method /*060000B1*/ public hidebysig instance uint8[] 
          UploadFile(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                     string 'method',
                     string fileName) cil managed
  // SIG: 20 03 1D 05 12 80 A5 0E 0E
  {
    .param [2]/*080000E6*/ 
    .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFile

  .method /*060000B2*/ public hidebysig instance void 
          UploadFileAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                          string fileName) cil managed
  // SIG: 20 02 01 12 80 A5 0E
  {
    // Method begins at RVA 0x21ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadFileAsync

  .method /*060000B3*/ public hidebysig instance void 
          UploadFileAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                          string 'method',
                          string fileName) cil managed
  // SIG: 20 03 01 12 80 A5 0E 0E
  {
    .param [2]/*080000EB*/ 
    .custom /*0C00009D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2201
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadFileAsync

  .method /*060000B4*/ public hidebysig instance void 
          UploadFileAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                          string 'method',
                          string fileName,
                          object userToken) cil managed
  // SIG: 20 04 01 12 80 A5 0E 0E 1C
  {
    .param [2]/*080000EE*/ 
    .custom /*0C00009E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000F0*/ 
    .custom /*0C00009F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2203
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadFileAsync

  .method /*060000B5*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadFileTaskAsync(string address,
                              string fileName) cil managed
  // SIG: 20 02 15 12 80 A9 01 1D 05 0E 0E
  {
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFileTaskAsync

  .method /*060000B6*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadFileTaskAsync(string address,
                              string 'method',
                              string fileName) cil managed
  // SIG: 20 03 15 12 80 A9 01 1D 05 0E 0E 0E
  {
    .param [2]/*080000F4*/ 
    .custom /*0C0000A0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFileTaskAsync

  .method /*060000B7*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadFileTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                              string fileName) cil managed
  // SIG: 20 02 15 12 80 A9 01 1D 05 12 80 A5 0E
  {
    // Method begins at RVA 0x220b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFileTaskAsync

  .method /*060000B8*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadFileTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                              string 'method',
                              string fileName) cil managed
  // SIG: 20 03 15 12 80 A9 01 1D 05 12 80 A5 0E 0E
  {
    .param [2]/*080000F9*/ 
    .custom /*0C0000A1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadFileTaskAsync

  .method /*060000B9*/ public hidebysig instance string 
          UploadString(string address,
                       string data) cil managed
  // SIG: 20 02 0E 0E 0E
  {
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadString

  .method /*060000BA*/ public hidebysig instance string 
          UploadString(string address,
                       string 'method',
                       string data) cil managed
  // SIG: 20 03 0E 0E 0E 0E
  {
    .param [2]/*080000FE*/ 
    .custom /*0C0000A2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadString

  .method /*060000BB*/ public hidebysig instance string 
          UploadString(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                       string data) cil managed
  // SIG: 20 02 0E 12 80 A5 0E
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadString

  .method /*060000BC*/ public hidebysig instance string 
          UploadString(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                       string 'method',
                       string data) cil managed
  // SIG: 20 03 0E 12 80 A5 0E 0E
  {
    .param [2]/*08000103*/ 
    .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadString

  .method /*060000BD*/ public hidebysig instance void 
          UploadStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string data) cil managed
  // SIG: 20 02 01 12 80 A5 0E
  {
    // Method begins at RVA 0x221d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadStringAsync

  .method /*060000BE*/ public hidebysig instance void 
          UploadStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string 'method',
                            string data) cil managed
  // SIG: 20 03 01 12 80 A5 0E 0E
  {
    .param [2]/*08000108*/ 
    .custom /*0C0000A4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadStringAsync

  .method /*060000BF*/ public hidebysig instance void 
          UploadStringAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string 'method',
                            string data,
                            object userToken) cil managed
  // SIG: 20 04 01 12 80 A5 0E 0E 1C
  {
    .param [2]/*0800010B*/ 
    .custom /*0C0000A5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800010D*/ 
    .custom /*0C0000A6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2221
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadStringAsync

  .method /*060000C0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<string> 
          UploadStringTaskAsync(string address,
                                string data) cil managed
  // SIG: 20 02 15 12 80 A9 01 0E 0E 0E
  {
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadStringTaskAsync

  .method /*060000C1*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<string> 
          UploadStringTaskAsync(string address,
                                string 'method',
                                string data) cil managed
  // SIG: 20 03 15 12 80 A9 01 0E 0E 0E 0E
  {
    .param [2]/*08000111*/ 
    .custom /*0C0000A7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2226
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadStringTaskAsync

  .method /*060000C2*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<string> 
          UploadStringTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                                string data) cil managed
  // SIG: 20 02 15 12 80 A9 01 0E 12 80 A5 0E
  {
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadStringTaskAsync

  .method /*060000C3*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<string> 
          UploadStringTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                                string 'method',
                                string data) cil managed
  // SIG: 20 03 15 12 80 A9 01 0E 12 80 A5 0E 0E
  {
    .param [2]/*08000116*/ 
    .custom /*0C0000A8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadStringTaskAsync

  .method /*060000C4*/ public hidebysig instance uint8[] 
          UploadValues(string address,
                       class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 02 1D 05 0E 12 80 A1
  {
    // Method begins at RVA 0x222f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValues

  .method /*060000C5*/ public hidebysig instance uint8[] 
          UploadValues(string address,
                       string 'method',
                       class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 03 1D 05 0E 0E 12 80 A1
  {
    .param [2]/*0800011B*/ 
    .custom /*0C0000A9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2232
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValues

  .method /*060000C6*/ public hidebysig instance uint8[] 
          UploadValues(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                       class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 02 1D 05 12 80 A5 12 80 A1
  {
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValues

  .method /*060000C7*/ public hidebysig instance uint8[] 
          UploadValues(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                       string 'method',
                       class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 03 1D 05 12 80 A5 0E 12 80 A1
  {
    .param [2]/*08000120*/ 
    .custom /*0C0000AA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2238
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValues

  .method /*060000C8*/ public hidebysig instance void 
          UploadValuesAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 02 01 12 80 A5 12 80 A1
  {
    // Method begins at RVA 0x223b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadValuesAsync

  .method /*060000C9*/ public hidebysig instance void 
          UploadValuesAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string 'method',
                            class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 03 01 12 80 A5 0E 12 80 A1
  {
    .param [2]/*08000125*/ 
    .custom /*0C0000AB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadValuesAsync

  .method /*060000CA*/ public hidebysig instance void 
          UploadValuesAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                            string 'method',
                            class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data,
                            object userToken) cil managed
  // SIG: 20 04 01 12 80 A5 0E 12 80 A1 1C
  {
    .param [2]/*08000128*/ 
    .custom /*0C0000AC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800012A*/ 
    .custom /*0C0000AD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebClient::UploadValuesAsync

  .method /*060000CB*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadValuesTaskAsync(string address,
                                class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 02 15 12 80 A9 01 1D 05 0E 12 80 A1
  {
    // Method begins at RVA 0x2241
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValuesTaskAsync

  .method /*060000CC*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadValuesTaskAsync(string address,
                                string 'method',
                                class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 03 15 12 80 A9 01 1D 05 0E 0E 12 80 A1
  {
    .param [2]/*0800012E*/ 
    .custom /*0C0000AE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2244
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValuesTaskAsync

  .method /*060000CD*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadValuesTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                                class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 02 15 12 80 A9 01 1D 05 12 80 A5 12 80 A1
  {
    // Method begins at RVA 0x2247
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValuesTaskAsync

  .method /*060000CE*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<uint8[]> 
          UploadValuesTaskAsync(class [System.Runtime/*23000001*/]System.Uri/*01000029*/ address,
                                string 'method',
                                class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ data) cil managed
  // SIG: 20 03 15 12 80 A9 01 1D 05 12 80 A5 0E 12 80 A1
  {
    .param [2]/*08000133*/ 
    .custom /*0C0000AF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebClient::UploadValuesTaskAsync

  .event /*14000001*/ System.Net.DownloadDataCompletedEventHandler/*02000003*/ DownloadDataCompleted
  {
    .custom /*0C000003:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_DownloadDataCompleted(class System.Net.DownloadDataCompletedEventHandler/*02000003*/) /* 06000058 */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_DownloadDataCompleted(class System.Net.DownloadDataCompletedEventHandler/*02000003*/) /* 06000059 */
  } // end of event WebClient::DownloadDataCompleted
  .event /*14000002*/ [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventHandler/*0100001E*/ DownloadFileCompleted
  {
    .custom /*0C000018:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_DownloadFileCompleted(class [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventHandler/*0100001E*/) /* 0600005A */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_DownloadFileCompleted(class [System.ComponentModel.EventBasedAsync/*23000002*/]System.ComponentModel.AsyncCompletedEventHandler/*0100001E*/) /* 0600005B */
  } // end of event WebClient::DownloadFileCompleted
  .event /*14000003*/ System.Net.DownloadProgressChangedEventHandler/*02000005*/ DownloadProgressChanged
  {
    .custom /*0C000019:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_DownloadProgressChanged(class System.Net.DownloadProgressChangedEventHandler/*02000005*/) /* 0600005C */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_DownloadProgressChanged(class System.Net.DownloadProgressChangedEventHandler/*02000005*/) /* 0600005D */
  } // end of event WebClient::DownloadProgressChanged
  .event /*14000004*/ System.Net.DownloadStringCompletedEventHandler/*02000007*/ DownloadStringCompleted
  {
    .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_DownloadStringCompleted(class System.Net.DownloadStringCompletedEventHandler/*02000007*/) /* 0600005E */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_DownloadStringCompleted(class System.Net.DownloadStringCompletedEventHandler/*02000007*/) /* 0600005F */
  } // end of event WebClient::DownloadStringCompleted
  .event /*14000005*/ System.Net.OpenReadCompletedEventHandler/*02000009*/ OpenReadCompleted
  {
    .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_OpenReadCompleted(class System.Net.OpenReadCompletedEventHandler/*02000009*/) /* 06000060 */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_OpenReadCompleted(class System.Net.OpenReadCompletedEventHandler/*02000009*/) /* 06000061 */
  } // end of event WebClient::OpenReadCompleted
  .event /*14000006*/ System.Net.OpenWriteCompletedEventHandler/*0200000B*/ OpenWriteCompleted
  {
    .custom /*0C000021:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_OpenWriteCompleted(class System.Net.OpenWriteCompletedEventHandler/*0200000B*/) /* 06000062 */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_OpenWriteCompleted(class System.Net.OpenWriteCompletedEventHandler/*0200000B*/) /* 06000063 */
  } // end of event WebClient::OpenWriteCompleted
  .event /*14000007*/ System.Net.UploadDataCompletedEventHandler/*0200000D*/ UploadDataCompleted
  {
    .custom /*0C000022:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_UploadDataCompleted(class System.Net.UploadDataCompletedEventHandler/*0200000D*/) /* 06000064 */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_UploadDataCompleted(class System.Net.UploadDataCompletedEventHandler/*0200000D*/) /* 06000065 */
  } // end of event WebClient::UploadDataCompleted
  .event /*14000008*/ System.Net.UploadFileCompletedEventHandler/*0200000F*/ UploadFileCompleted
  {
    .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_UploadFileCompleted(class System.Net.UploadFileCompletedEventHandler/*0200000F*/) /* 06000066 */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_UploadFileCompleted(class System.Net.UploadFileCompletedEventHandler/*0200000F*/) /* 06000067 */
  } // end of event WebClient::UploadFileCompleted
  .event /*14000009*/ System.Net.UploadProgressChangedEventHandler/*02000011*/ UploadProgressChanged
  {
    .custom /*0C000026:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_UploadProgressChanged(class System.Net.UploadProgressChangedEventHandler/*02000011*/) /* 06000068 */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_UploadProgressChanged(class System.Net.UploadProgressChangedEventHandler/*02000011*/) /* 06000069 */
  } // end of event WebClient::UploadProgressChanged
  .event /*1400000A*/ System.Net.UploadStringCompletedEventHandler/*02000013*/ UploadStringCompleted
  {
    .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_UploadStringCompleted(class System.Net.UploadStringCompletedEventHandler/*02000013*/) /* 0600006A */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_UploadStringCompleted(class System.Net.UploadStringCompletedEventHandler/*02000013*/) /* 0600006B */
  } // end of event WebClient::UploadStringCompleted
  .event /*1400000B*/ System.Net.UploadValuesCompletedEventHandler/*02000015*/ UploadValuesCompleted
  {
    .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_UploadValuesCompleted(class System.Net.UploadValuesCompletedEventHandler/*02000015*/) /* 0600006C */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_UploadValuesCompleted(class System.Net.UploadValuesCompletedEventHandler/*02000015*/) /* 0600006D */
  } // end of event WebClient::UploadValuesCompleted
  .event /*1400000C*/ System.Net.WriteStreamClosedEventHandler/*02000018*/ WriteStreamClosed
  {
    .custom /*0C00002E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000030:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string,
                                                                                                                        bool) /* 0A000016 */ = ( 01 00 6B 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..kThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 46 72   // orts the .NET Fr
                                                                                                                                                 61 6D 65 77 6F 72 6B 20 69 6E 66 72 61 73 74 72   // amework infrastr
                                                                                                                                                 75 63 74 75 72 65 20 61 6E 64 20 69 73 20 6E 6F   // ucture and is no
                                                                                                                                                 74 20 69 6E 74 65 6E 64 65 64 20 74 6F 20 62 65   // t intended to be
                                                                                                                                                 20 75 73 65 64 20 64 69 72 65 63 74 6C 79 20 66   //  used directly f
                                                                                                                                                 72 6F 6D 20 79 6F 75 72 20 63 6F 64 65 2E 01 00   // rom your code...
                                                                                                                                                 00 ) 
    .addon instance void System.Net.WebClient/*02000016*/::add_WriteStreamClosed(class System.Net.WriteStreamClosedEventHandler/*02000018*/) /* 0600006E */
    .removeon instance void System.Net.WebClient/*02000016*/::remove_WriteStreamClosed(class System.Net.WriteStreamClosedEventHandler/*02000018*/) /* 0600006F */
  } // end of event WebClient::WriteStreamClosed
  .property /*1700000F*/ instance callconv(8) bool
          AllowReadStreamBuffering()
  {
    .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000036:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string,
                                                                                                                        bool) /* 0A000016 */ = ( 01 00 6B 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..kThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 46 72   // orts the .NET Fr
                                                                                                                                                 61 6D 65 77 6F 72 6B 20 69 6E 66 72 61 73 74 72   // amework infrastr
                                                                                                                                                 75 63 74 75 72 65 20 61 6E 64 20 69 73 20 6E 6F   // ucture and is no
                                                                                                                                                 74 20 69 6E 74 65 6E 64 65 64 20 74 6F 20 62 65   // t intended to be
                                                                                                                                                 20 75 73 65 64 20 64 69 72 65 63 74 6C 79 20 66   //  used directly f
                                                                                                                                                 72 6F 6D 20 79 6F 75 72 20 63 6F 64 65 2E 01 00   // rom your code...
                                                                                                                                                 00 ) 
    .get instance bool System.Net.WebClient/*02000016*/::get_AllowReadStreamBuffering() /* 06000042 */
    .set instance void System.Net.WebClient/*02000016*/::set_AllowReadStreamBuffering(bool) /* 06000043 */
  } // end of property WebClient::AllowReadStreamBuffering
  .property /*17000010*/ instance callconv(8) bool
          AllowWriteStreamBuffering()
  {
    .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000038:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string,
                                                                                                                        bool) /* 0A000016 */ = ( 01 00 6B 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..kThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 46 72   // orts the .NET Fr
                                                                                                                                                 61 6D 65 77 6F 72 6B 20 69 6E 66 72 61 73 74 72   // amework infrastr
                                                                                                                                                 75 63 74 75 72 65 20 61 6E 64 20 69 73 20 6E 6F   // ucture and is no
                                                                                                                                                 74 20 69 6E 74 65 6E 64 65 64 20 74 6F 20 62 65   // t intended to be
                                                                                                                                                 20 75 73 65 64 20 64 69 72 65 63 74 6C 79 20 66   //  used directly f
                                                                                                                                                 72 6F 6D 20 79 6F 75 72 20 63 6F 64 65 2E 01 00   // rom your code...
                                                                                                                                                 00 ) 
    .get instance bool System.Net.WebClient/*02000016*/::get_AllowWriteStreamBuffering() /* 06000044 */
    .set instance void System.Net.WebClient/*02000016*/::set_AllowWriteStreamBuffering(bool) /* 06000045 */
  } // end of property WebClient::AllowWriteStreamBuffering
  .property /*17000011*/ instance callconv(8) string
          BaseAddress()
  {
    .get instance string System.Net.WebClient/*02000016*/::get_BaseAddress() /* 06000046 */
    .set instance void System.Net.WebClient/*02000016*/::set_BaseAddress(string) /* 06000047 */
  } // end of property WebClient::BaseAddress
  .property /*17000012*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.Cache.RequestCachePolicy/*01000023*/
          CachePolicy()
  {
    .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.Cache.RequestCachePolicy/*01000023*/ System.Net.WebClient/*02000016*/::get_CachePolicy() /* 06000048 */
    .set instance void System.Net.WebClient/*02000016*/::set_CachePolicy(class [System.Net.Primitives/*23000004*/]System.Net.Cache.RequestCachePolicy/*01000023*/) /* 06000049 */
  } // end of property WebClient::CachePolicy
  .property /*17000013*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.ICredentials/*01000024*/
          Credentials()
  {
    .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.ICredentials/*01000024*/ System.Net.WebClient/*02000016*/::get_Credentials() /* 0600004A */
    .set instance void System.Net.WebClient/*02000016*/::set_Credentials(class [System.Net.Primitives/*23000004*/]System.Net.ICredentials/*01000024*/) /* 0600004B */
  } // end of property WebClient::Credentials
  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/
          Encoding()
  {
    .get instance class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/ System.Net.WebClient/*02000016*/::get_Encoding() /* 0600004C */
    .set instance void System.Net.WebClient/*02000016*/::set_Encoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/) /* 0600004D */
  } // end of property WebClient::Encoding
  .property /*17000015*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/
          Headers()
  {
    .get instance class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/ System.Net.WebClient/*02000016*/::get_Headers() /* 0600004E */
    .set instance void System.Net.WebClient/*02000016*/::set_Headers(class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/) /* 0600004F */
  } // end of property WebClient::Headers
  .property /*17000016*/ instance callconv(8) bool
          IsBusy()
  {
    .get instance bool System.Net.WebClient/*02000016*/::get_IsBusy() /* 06000050 */
  } // end of property WebClient::IsBusy
  .property /*17000017*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.IWebProxy/*01000027*/
          Proxy()
  {
    .custom /*0C000047:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.IWebProxy/*01000027*/ System.Net.WebClient/*02000016*/::get_Proxy() /* 06000051 */
    .set instance void System.Net.WebClient/*02000016*/::set_Proxy(class [System.Net.Primitives/*23000004*/]System.Net.IWebProxy/*01000027*/) /* 06000052 */
  } // end of property WebClient::Proxy
  .property /*17000018*/ instance callconv(8) class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/
          QueryString()
  {
    .get instance class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/ System.Net.WebClient/*02000016*/::get_QueryString() /* 06000053 */
    .set instance void System.Net.WebClient/*02000016*/::set_QueryString(class [System.Collections.Specialized/*23000006*/]System.Collections.Specialized.NameValueCollection/*01000028*/) /* 06000054 */
  } // end of property WebClient::QueryString
  .property /*17000019*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/
          ResponseHeaders()
  {
    .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.WebHeaderCollection/*23000005*/]System.Net.WebHeaderCollection/*01000026*/ System.Net.WebClient/*02000016*/::get_ResponseHeaders() /* 06000055 */
  } // end of property WebClient::ResponseHeaders
  .property /*1700001A*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.WebClient/*02000016*/::get_UseDefaultCredentials() /* 06000056 */
    .set instance void System.Net.WebClient/*02000016*/::set_UseDefaultCredentials(bool) /* 06000057 */
  } // end of property WebClient::UseDefaultCredentials
} // end of class System.Net.WebClient

.class /*02000017*/ public auto ansi beforefieldinit System.Net.WriteStreamClosedEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100002E*/
{
  .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000045:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000094:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000095:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string,
                                                                                                                        bool) /* 0A000016 */ = ( 01 00 6B 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..kThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 46 72   // orts the .NET Fr
                                                                                                                                                 61 6D 65 77 6F 72 6B 20 69 6E 66 72 61 73 74 72   // amework infrastr
                                                                                                                                                 75 63 74 75 72 65 20 61 6E 64 20 69 73 20 6E 6F   // ucture and is no
                                                                                                                                                 74 20 69 6E 74 65 6E 64 65 64 20 74 6F 20 62 65   // t intended to be
                                                                                                                                                 20 75 73 65 64 20 64 69 72 65 63 74 6C 79 20 66   //  used directly f
                                                                                                                                                 72 6F 6D 20 79 6F 75 72 20 63 6F 64 65 2E 01 00   // rom your code...
                                                                                                                                                 00 ) 
    // Method begins at RVA 0x224d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WriteStreamClosedEventArgs::.ctor

  .method /*060000D0*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*0100002F*/ 
          get_Error() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x2255
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteStreamClosedEventArgs::get_Error

  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*0100002F*/
          Error()
  {
    .custom /*0C00004B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C00004C:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000021*/::.ctor(string,
                                                                                                                        bool) /* 0A000016 */ = ( 01 00 6B 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..kThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 46 72   // orts the .NET Fr
                                                                                                                                                 61 6D 65 77 6F 72 6B 20 69 6E 66 72 61 73 74 72   // amework infrastr
                                                                                                                                                 75 63 74 75 72 65 20 61 6E 64 20 69 73 20 6E 6F   // ucture and is no
                                                                                                                                                 74 20 69 6E 74 65 6E 64 65 64 20 74 6F 20 62 65   // t intended to be
                                                                                                                                                 20 75 73 65 64 20 64 69 72 65 63 74 6C 79 20 66   //  used directly f
                                                                                                                                                 72 6F 6D 20 79 6F 75 72 20 63 6F 64 65 2E 01 00   // rom your code...
                                                                                                                                                 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Exception/*0100002F*/ System.Net.WriteStreamClosedEventArgs/*02000017*/::get_Error() /* 060000D0 */
  } // end of property WriteStreamClosedEventArgs::Error
} // end of class System.Net.WriteStreamClosedEventArgs

.class /*02000018*/ public auto ansi sealed System.Net.WriteStreamClosedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000017*/
{
  .custom /*0C000048:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001F*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000D1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method WriteStreamClosedEventHandler::.ctor

  .method /*060000D2*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Net.WriteStreamClosedEventArgs/*02000017*/ e) runtime managed
  // SIG: 20 02 01 1C 12 5C
  {
    .custom /*0C000097:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method WriteStreamClosedEventHandler::Invoke

  .method /*060000D3*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ 
          BeginInvoke(object sender,
                      class System.Net.WriteStreamClosedEventArgs/*02000017*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 65 1C 12 5C 12 69 1C
  {
    .param [1]/*08000139*/ 
    .custom /*0C0000B0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800013A*/ 
    .custom /*0C0000B1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method WriteStreamClosedEventHandler::BeginInvoke

  .method /*060000D4*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000019*/ result) runtime managed
  // SIG: 20 01 01 12 65
  {
  } // end of method WriteStreamClosedEventHandler::EndInvoke

} // end of class System.Net.WriteStreamClosedEventHandler


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
