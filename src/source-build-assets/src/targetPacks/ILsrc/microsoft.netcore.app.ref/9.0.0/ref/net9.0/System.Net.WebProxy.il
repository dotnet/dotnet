


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
// Time-date stamp:            0xe883ae29
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000033be
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
// Checksum:                       0x000064f6
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000336c [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000474] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00001e00 [0x00002938] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003350 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000013c4 Virtual Size
//              0x00002000 Virtual Address
//              0x00001400 Size of Raw Data
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
//              0x00001600 Pointer to Raw Data
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
//              0x00001c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000003c0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000033ae Import Name Table
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
// 0x000020d4 [0x000011fc] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000032d0 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000678 Size
//              '#~' Name
//    Stream 2:
//              0x000006e4 Offset
//              0x000005e8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00000ccc Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000cd0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000ce0 Offset
//              0x0000051c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000903a01747 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 18232
// PE header size       : 512 (496 used)    ( 2.81%)
// PE additional info   : 11891             (65.22%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.39%)
// CLR meta-data size  : 4604               (25.25%)
// CLR additional info : 128                ( 0.70%)
// CLR method headers  : 27                 ( 0.15%)
// Managed code         : 102               ( 0.56%)
// Data                 : 2048              (11.23%)
// Unaccounted          : -1152             (-6.32%)

// Num.of PE sections   : 3
//   .text    - 5120
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 4604
//   Module        -    1 (10 bytes)
//   TypeDef       -    3 (42 bytes)      1 interfaces, 0 explicit layout
//   TypeRef       -   35 (210 bytes)
//   MethodDef     -   30 (420 bytes)     3 abstract, 0 native, 27 bodies
//   MemberRef     -   26 (156 bytes)
//   ParamDef      -   41 (246 bytes)
//   MethodImpl    -    1 (6 bytes)
//   CustomAttribute-   48 (288 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    6 (36 bytes)
//   MethodSemantic-   11 (66 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   Strings       -  1509 bytes
//   Blobs         -  1308 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   193 bytes

// CLR additional info : 128

// CLR method headers : 27
//   Num.of method bodies  - 27
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 27

// Managed code : 102
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Interface IWebProxyScript                (public) (abstract) (auto) (ansi) 
// Class WebProxy                       (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000002*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000003*/ System.Text.RegularExpressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.WebProxy
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                        62 50 72 6F 78 79 00 00 )                         // bProxy..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                       62 50 72 6F 78 79 00 00 )                         // bProxy..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                 62 50 72 6F 78 79 00 00 )                         // bProxy..
  .custom /*0C000014:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
.module System.Net.WebProxy.dll
// MVID: {580233b2-9269-45ed-942a-3e621d65b3ae}
.custom /*0C000002:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ interface public abstract auto ansi beforefieldinit System.Net.IWebProxyScript
{
  .custom /*0C000015:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .method /*06000001*/ public hidebysig newslot abstract virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IWebProxyScript::Close

  .method /*06000002*/ public hidebysig newslot abstract virtual 
          instance bool  Load(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ scriptLocation,
                              string script,
                              class [System.Runtime/*23000001*/]System.Type/*01000015*/ helperType) cil managed
  // SIG: 20 03 02 12 51 0E 12 55
  {
    // Method begins at RVA 0x0
  } // end of method IWebProxyScript::Load

  .method /*06000003*/ public hidebysig newslot abstract virtual 
          instance string  Run(string url,
                               string host) cil managed
  // SIG: 20 02 0E 0E 0E
  {
    // Method begins at RVA 0x0
  } // end of method IWebProxyScript::Run

} // end of class System.Net.IWebProxyScript

.class /*02000003*/ public auto ansi beforefieldinit System.Net.WebProxy
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*0100001B*/,
                  [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000018*/
{
  .custom /*0C000018:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*0100001B*/
  .custom /*0C000001:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000018*/
  .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*06000005*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000019*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001A*/ streamingContext) cil managed
  // SIG: 20 02 01 12 65 11 69
  {
    .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00001C:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000015 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C00001D:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string Address) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string Address,
                               bool BypassOnLocal) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x2068
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string Address,
                               bool BypassOnLocal,
                               string[] BypassList) cil managed
  // SIG: 20 03 01 0E 02 1D 0E
  {
    .param [3]/*0800000D*/ 
    .custom /*0C00001F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C000020:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000020*/::.ctor(string,
                                                                                                                                                     object[]) /* 0A000018 */ = ( 01 00 05 52 65 67 65 78 01 00 00 00 55 80 8E 53   // ...Regex....U..S
                                                                                                                                                                                  79 73 74 65 6D 2E 54 65 78 74 2E 52 65 67 75 6C   // ystem.Text.Regul
                                                                                                                                                                                  61 72 45 78 70 72 65 73 73 69 6F 6E 73 2E 52 65   // arExpressions.Re
                                                                                                                                                                                  67 65 78 4F 70 74 69 6F 6E 73 2C 20 53 79 73 74   // gexOptions, Syst
                                                                                                                                                                                  65 6D 2E 54 65 78 74 2E 52 65 67 75 6C 61 72 45   // em.Text.RegularE
                                                                                                                                                                                  78 70 72 65 73 73 69 6F 6E 73 2C 20 56 65 72 73   // xpressions, Vers
                                                                                                                                                                                  69 6F 6E 3D 39 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=9.0.0.0, Cul
                                                                                                                                                                                  74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                  62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33   // blicKeyToken=b03
                                                                                                                                                                                  66 35 66 37 66 31 31 64 35 30 61 33 61 01 02 00   // f5f7f11d50a3a...
                                                                                                                                                                                  00 00 00 ) 
    // Method begins at RVA 0x2070
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string Address,
                               bool BypassOnLocal,
                               string[] BypassList,
                               class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/ Credentials) cil managed
  // SIG: 20 04 01 0E 02 1D 0E 12 80 85
  {
    .param [3]/*08000010*/ 
    .custom /*0C000021:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C000022:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000020*/::.ctor(string,
                                                                                                                                                     object[]) /* 0A000018 */ = ( 01 00 05 52 65 67 65 78 01 00 00 00 55 80 8E 53   // ...Regex....U..S
                                                                                                                                                                                  79 73 74 65 6D 2E 54 65 78 74 2E 52 65 67 75 6C   // ystem.Text.Regul
                                                                                                                                                                                  61 72 45 78 70 72 65 73 73 69 6F 6E 73 2E 52 65   // arExpressions.Re
                                                                                                                                                                                  67 65 78 4F 70 74 69 6F 6E 73 2C 20 53 79 73 74   // gexOptions, Syst
                                                                                                                                                                                  65 6D 2E 54 65 78 74 2E 52 65 67 75 6C 61 72 45   // em.Text.RegularE
                                                                                                                                                                                  78 70 72 65 73 73 69 6F 6E 73 2C 20 56 65 72 73   // xpressions, Vers
                                                                                                                                                                                  69 6F 6E 3D 39 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=9.0.0.0, Cul
                                                                                                                                                                                  74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                  62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33   // blicKeyToken=b03
                                                                                                                                                                                  66 35 66 37 66 31 31 64 35 30 61 33 61 01 02 00   // f5f7f11d50a3a...
                                                                                                                                                                                  00 00 00 ) 
    // Method begins at RVA 0x2078
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string Host,
                               int32 Port) cil managed
  // SIG: 20 02 01 0E 08
  {
    .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ Address) cil managed
  // SIG: 20 01 01 12 51
  {
    // Method begins at RVA 0x2088
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ Address,
                               bool BypassOnLocal) cil managed
  // SIG: 20 02 01 12 51 02
  {
    // Method begins at RVA 0x2090
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ Address,
                               bool BypassOnLocal,
                               string[] BypassList) cil managed
  // SIG: 20 03 01 12 51 02 1D 0E
  {
    .param [3]/*08000019*/ 
    .custom /*0C000026:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C000027:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000020*/::.ctor(string,
                                                                                                                                                     object[]) /* 0A000018 */ = ( 01 00 05 52 65 67 65 78 01 00 00 00 55 80 8E 53   // ...Regex....U..S
                                                                                                                                                                                  79 73 74 65 6D 2E 54 65 78 74 2E 52 65 67 75 6C   // ystem.Text.Regul
                                                                                                                                                                                  61 72 45 78 70 72 65 73 73 69 6F 6E 73 2E 52 65   // arExpressions.Re
                                                                                                                                                                                  67 65 78 4F 70 74 69 6F 6E 73 2C 20 53 79 73 74   // gexOptions, Syst
                                                                                                                                                                                  65 6D 2E 54 65 78 74 2E 52 65 67 75 6C 61 72 45   // em.Text.RegularE
                                                                                                                                                                                  78 70 72 65 73 73 69 6F 6E 73 2C 20 56 65 72 73   // xpressions, Vers
                                                                                                                                                                                  69 6F 6E 3D 39 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=9.0.0.0, Cul
                                                                                                                                                                                  74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                  62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33   // blicKeyToken=b03
                                                                                                                                                                                  66 35 66 37 66 31 31 64 35 30 61 33 61 01 02 00   // f5f7f11d50a3a...
                                                                                                                                                                                  00 00 00 ) 
    // Method begins at RVA 0x2098
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ Address,
                               bool BypassOnLocal,
                               string[] BypassList,
                               class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/ Credentials) cil managed
  // SIG: 20 04 01 12 51 02 1D 0E 12 80 85
  {
    .param [3]/*0800001C*/ 
    .custom /*0C00002C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C00002D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000020*/::.ctor(string,
                                                                                                                                                     object[]) /* 0A000018 */ = ( 01 00 05 52 65 67 65 78 01 00 00 00 55 80 8E 53   // ...Regex....U..S
                                                                                                                                                                                  79 73 74 65 6D 2E 54 65 78 74 2E 52 65 67 75 6C   // ystem.Text.Regul
                                                                                                                                                                                  61 72 45 78 70 72 65 73 73 69 6F 6E 73 2E 52 65   // arExpressions.Re
                                                                                                                                                                                  67 65 78 4F 70 74 69 6F 6E 73 2C 20 53 79 73 74   // gexOptions, Syst
                                                                                                                                                                                  65 6D 2E 54 65 78 74 2E 52 65 67 75 6C 61 72 45   // em.Text.RegularE
                                                                                                                                                                                  78 70 72 65 73 73 69 6F 6E 73 2C 20 56 65 72 73   // xpressions, Vers
                                                                                                                                                                                  69 6F 6E 3D 39 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=9.0.0.0, Cul
                                                                                                                                                                                  74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                  62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33   // blicKeyToken=b03
                                                                                                                                                                                  66 35 66 37 66 31 31 64 35 30 61 33 61 01 02 00   // f5f7f11d50a3a...
                                                                                                                                                                                  00 00 00 ) 
    // Method begins at RVA 0x20a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebProxy::.ctor

  .method /*0600000F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000014*/ 
          get_Address() cil managed
  // SIG: 20 00 12 51
  {
    // Method begins at RVA 0x20a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::get_Address

  .method /*06000010*/ public hidebysig specialname 
          instance void  set_Address(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ 'value') cil managed
  // SIG: 20 01 01 12 51
  {
    // Method begins at RVA 0x20ab
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::set_Address

  .method /*06000011*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000022*/ 
          get_BypassArrayList() cil managed
  // SIG: 20 00 12 80 89
  {
    .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::get_BypassArrayList

  .method /*06000012*/ public hidebysig specialname 
          instance string[]  get_BypassList() cil managed
  // SIG: 20 00 1D 0E
  {
    .custom /*0C000024:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::get_BypassList

  .method /*06000013*/ public hidebysig specialname 
          instance void  set_BypassList(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    .custom /*0C000025:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800001F*/ 
    .custom /*0C00002F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000023*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::set_BypassList

  .method /*06000014*/ public hidebysig specialname 
          instance bool  get_BypassProxyOnLocal() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::get_BypassProxyOnLocal

  .method /*06000015*/ public hidebysig specialname 
          instance void  set_BypassProxyOnLocal(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::set_BypassProxyOnLocal

  .method /*06000016*/ public hidebysig newslot specialname virtual final 
          instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 80 85
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::get_Credentials

  .method /*06000017*/ public hidebysig newslot specialname virtual final 
          instance void  set_Credentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/ 'value') cil managed
  // SIG: 20 01 01 12 80 85
  {
    // Method begins at RVA 0x20bd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::set_Credentials

  .method /*06000018*/ public hidebysig specialname 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::get_UseDefaultCredentials

  .method /*06000019*/ public hidebysig specialname 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::set_UseDefaultCredentials

  .method /*0600001A*/ public hidebysig static 
          class System.Net.WebProxy/*02000003*/ 
          GetDefaultProxy() cil managed
  // SIG: 00 00 12 0C
  {
    .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000029:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000015 */ = ( 01 00 58 57 65 62 50 72 6F 78 79 2E 47 65 74 44   // ..XWebProxy.GetD
                                                                                                                                                   65 66 61 75 6C 74 50 72 6F 78 79 20 68 61 73 20   // efaultProxy has 
                                                                                                                                                   62 65 65 6E 20 64 65 70 72 65 63 61 74 65 64 2E   // been deprecated.
                                                                                                                                                   20 55 73 65 20 74 68 65 20 70 72 6F 78 79 20 73   //  Use the proxy s
                                                                                                                                                   65 6C 65 63 74 65 64 20 66 6F 72 20 79 6F 75 20   // elected for you 
                                                                                                                                                   62 79 20 64 65 66 61 75 6C 74 2E 00 00 )          // by default...
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::GetDefaultProxy

  .method /*0600001B*/ family hidebysig newslot virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000019*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001A*/ streamingContext) cil managed
  // SIG: 20 02 01 12 65 11 69
  {
    .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::GetObjectData

  .method /*0600001C*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000014*/ 
          GetProxy(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ destination) cil managed
  // SIG: 20 01 12 51 12 51
  {
    .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000025*/ 
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::GetProxy

  .method /*0600001D*/ public hidebysig newslot virtual final 
          instance bool  IsBypassed(class [System.Runtime/*23000001*/]System.Uri/*01000014*/ host) cil managed
  // SIG: 20 01 02 12 51
  {
    .custom /*0C00002E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebProxy::IsBypassed

  .method /*0600001E*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000019*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001A*/ streamingContext) cil managed
  // SIG: 20 02 01 12 65 11 69
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000018*/::GetObjectData /*01000018::0A000014*/ 
    // Method begins at RVA 0x20cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebProxy::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000014*/
          Address()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000014*/ System.Net.WebProxy/*02000003*/::get_Address() /* 0600000F */
    .set instance void System.Net.WebProxy/*02000003*/::set_Address(class [System.Runtime/*23000001*/]System.Uri/*01000014*/) /* 06000010 */
  } // end of property WebProxy::Address
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000022*/
          BypassArrayList()
  {
    .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.ArrayList/*01000022*/ System.Net.WebProxy/*02000003*/::get_BypassArrayList() /* 06000011 */
  } // end of property WebProxy::BypassArrayList
  .property /*17000003*/ instance callconv(8) string[]
          BypassList()
  {
    .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance string[] System.Net.WebProxy/*02000003*/::get_BypassList() /* 06000012 */
    .set instance void System.Net.WebProxy/*02000003*/::set_BypassList(string[]) /* 06000013 */
  } // end of property WebProxy::BypassList
  .property /*17000004*/ instance callconv(8) bool
          BypassProxyOnLocal()
  {
    .get instance bool System.Net.WebProxy/*02000003*/::get_BypassProxyOnLocal() /* 06000014 */
    .set instance void System.Net.WebProxy/*02000003*/::set_BypassProxyOnLocal(bool) /* 06000015 */
  } // end of property WebProxy::BypassProxyOnLocal
  .property /*17000005*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/
          Credentials()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/ System.Net.WebProxy/*02000003*/::get_Credentials() /* 06000016 */
    .set instance void System.Net.WebProxy/*02000003*/::set_Credentials(class [System.Net.Primitives/*23000002*/]System.Net.ICredentials/*01000021*/) /* 06000017 */
  } // end of property WebProxy::Credentials
  .property /*17000006*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.WebProxy/*02000003*/::get_UseDefaultCredentials() /* 06000018 */
    .set instance void System.Net.WebProxy/*02000003*/::set_UseDefaultCredentials(bool) /* 06000019 */
  } // end of property WebProxy::UseDefaultCredentials
} // end of class System.Net.WebProxy


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
