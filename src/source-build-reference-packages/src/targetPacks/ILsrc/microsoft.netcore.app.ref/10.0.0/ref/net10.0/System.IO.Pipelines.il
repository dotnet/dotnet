
//  .NET IL Disassembler.  Version 11.0.0-alpha.1.26055.101


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
// Time-date stamp:            0xfa09b279
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
// Addr. of entry point:           0x00003cb6
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
// Checksum:                       0x000098a8
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003c64 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000478] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002800 [0x00002910] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003c48 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001cbc Virtual Size
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
//              0x00000478 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000cb8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003ca6 Import Name Table
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
// 0x00002120 [0x00001aa8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003bc8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000c08 Size
//              '#~' Name
//    Stream 2:
//              0x00000c74 Offset
//              0x000008d4 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001548 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000154c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000155c Offset
//              0x0000054c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000909a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20752
// PE header size       : 512 (496 used)    ( 2.47%)
// PE additional info   : 11855             (57.13%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 6824               (32.88%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 55                 ( 0.27%)
// Managed code         : 152               ( 0.73%)
// Data                 : 2048              ( 9.87%)
// Unaccounted          : -894              (-4.31%)

// Num.of PE sections   : 3
//   .text    - 7680
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6824
//   Module        -    1 (10 bytes)
//   TypeDef       -   12 (168 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   40 (240 bytes)
//   MethodDef     -   70 (980 bytes)     15 abstract, 0 native, 55 bodies
//   FieldDef      -    3 (18 bytes)      0 constant
//   MemberRef     -   24 (144 bytes)
//   ParamDef      -   67 (402 bytes)
//   Constant      -   33 (198 bytes)
//   CustomAttribute-   63 (378 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    9 (36 bytes)
//   Property      -   29 (174 bytes)
//   MethodSemantic-   29 (174 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   Strings       -  2259 bytes
//   Blobs         -  1356 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   199 bytes

// CLR additional info : 128

// CLR method headers : 55
//   Num.of method bodies  - 55
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 55

// Managed code : 152
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class FlushResult                    (public) (sequential) (ansi) (sealed) 
// Interface IDuplexPipe                    (public) (abstract) (auto) (ansi) 
// Class Pipe                           (public) (auto) (ansi) (sealed) 
// Class PipeOptions                    (public) (auto) (ansi) 
// Class PipeReader                     (public) (abstract) (auto) (ansi) 
// Class PipeScheduler                  (public) (abstract) (auto) (ansi) 
// Class PipeWriter                     (public) (abstract) (auto) (ansi) 
// Class ReadResult                     (public) (sequential) (ansi) (sealed) 
// Class StreamPipeExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class StreamPipeReaderOptions        (public) (auto) (ansi) 
// Class StreamPipeWriterOptions        (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.Pipelines
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 13 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ...System.IO.Pip
                                                                                                                                                                        65 6C 69 6E 65 73 00 00 )                         // elines..
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 13 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ...System.IO.Pip
                                                                                                                                                                       65 6C 69 6E 65 73 00 00 )                         // elines..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 13 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ...System.IO.Pip
                                                                                                                                                                 65 6C 69 6E 65 73 00 00 )                         // elines..
  .custom /*0C000016:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
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
  .ver 10:0:0:0
}
.module System.IO.Pipelines.dll
// MVID: {7b99afef-3941-44a1-a3ed-bd144a08890b}
.custom /*0C000001:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FE5C73FD000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public sequential ansi sealed beforefieldinit System.IO.Pipelines.FlushResult
       extends [System.Runtime/*23000001*/]System.ValueType/*01000014*/
{
  .field /*04000001*/ private int32 _dummyPrimitive
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isCanceled,
                               bool isCompleted) cil managed
  // SIG: 20 02 01 02 02
  {
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FlushResult::.ctor

  .method /*06000002*/ public hidebysig specialname 
          instance bool  get_IsCanceled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FlushResult::get_IsCanceled

  .method /*06000003*/ public hidebysig specialname 
          instance bool  get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FlushResult::get_IsCompleted

  .property /*17000001*/ instance callconv(8) bool
          IsCanceled()
  {
    .get instance bool System.IO.Pipelines.FlushResult/*02000002*/::get_IsCanceled() /* 06000002 */
  } // end of property FlushResult::IsCanceled
  .property /*17000002*/ instance callconv(8) bool
          IsCompleted()
  {
    .get instance bool System.IO.Pipelines.FlushResult/*02000002*/::get_IsCompleted() /* 06000003 */
  } // end of property FlushResult::IsCompleted
} // end of class System.IO.Pipelines.FlushResult

.class /*02000003*/ interface public abstract auto ansi beforefieldinit System.IO.Pipelines.IDuplexPipe
{
  .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*06000004*/ public hidebysig newslot specialname abstract virtual 
          instance class System.IO.Pipelines.PipeReader/*02000006*/ 
          get_Input() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x0
  } // end of method IDuplexPipe::get_Input

  .method /*06000005*/ public hidebysig newslot specialname abstract virtual 
          instance class System.IO.Pipelines.PipeWriter/*02000008*/ 
          get_Output() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x0
  } // end of method IDuplexPipe::get_Output

  .property /*17000003*/ instance callconv(8) class System.IO.Pipelines.PipeReader/*02000006*/
          Input()
  {
    .get instance class System.IO.Pipelines.PipeReader/*02000006*/ System.IO.Pipelines.IDuplexPipe/*02000003*/::get_Input() /* 06000004 */
  } // end of property IDuplexPipe::Input
  .property /*17000004*/ instance callconv(8) class System.IO.Pipelines.PipeWriter/*02000008*/
          Output()
  {
    .get instance class System.IO.Pipelines.PipeWriter/*02000008*/ System.IO.Pipelines.IDuplexPipe/*02000003*/::get_Output() /* 06000005 */
  } // end of property IDuplexPipe::Output
} // end of class System.IO.Pipelines.IDuplexPipe

.class /*02000004*/ public auto ansi sealed beforefieldinit System.IO.Pipelines.Pipe
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000019:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2059
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Pipe::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.IO.Pipelines.PipeOptions/*02000005*/ options) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x2061
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Pipe::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance class System.IO.Pipelines.PipeReader/*02000006*/ 
          get_Reader() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x2069
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Pipe::get_Reader

  .method /*06000009*/ public hidebysig specialname 
          instance class System.IO.Pipelines.PipeWriter/*02000008*/ 
          get_Writer() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x206c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Pipe::get_Writer

  .method /*0600000A*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Pipe::Reset

  .property /*17000005*/ instance callconv(8) class System.IO.Pipelines.PipeReader/*02000006*/
          Reader()
  {
    .get instance class System.IO.Pipelines.PipeReader/*02000006*/ System.IO.Pipelines.Pipe/*02000004*/::get_Reader() /* 06000008 */
  } // end of property Pipe::Reader
  .property /*17000006*/ instance callconv(8) class System.IO.Pipelines.PipeWriter/*02000008*/
          Writer()
  {
    .get instance class System.IO.Pipelines.PipeWriter/*02000008*/ System.IO.Pipelines.Pipe/*02000004*/::get_Writer() /* 06000009 */
  } // end of property Pipe::Writer
} // end of class System.IO.Pipelines.Pipe

.class /*02000005*/ public auto ansi beforefieldinit System.IO.Pipelines.PipeOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([opt] class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> pool,
                               [opt] class System.IO.Pipelines.PipeScheduler/*02000007*/ readerScheduler,
                               [opt] class System.IO.Pipelines.PipeScheduler/*02000007*/ writerScheduler,
                               [opt] int64 pauseWriterThreshold,
                               [opt] int64 resumeWriterThreshold,
                               [opt] int32 minimumSegmentSize,
                               [opt] bool useSynchronizationContext) cil managed
  // SIG: 20 07 01 15 12 61 01 05 12 1C 12 1C 0A 0A 08 02
  {
    .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000004*/  = nullref
    .param [2]/*08000005*/  = nullref
    .param [3]/*08000006*/  = nullref
    .param [4]/*08000007*/  = int64(0xFFFFFFFFFFFFFFFF)
    .param [5]/*08000008*/  = int64(0xFFFFFFFFFFFFFFFF)
    .param [6]/*08000009*/  = int32(0xFFFFFFFF)
    .param [7]/*0800000A*/  = bool(true)
    // Method begins at RVA 0x2071
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PipeOptions::.ctor

  .method /*0600000C*/ public hidebysig specialname static 
          class System.IO.Pipelines.PipeOptions/*02000005*/ 
          get_Default() cil managed
  // SIG: 00 00 12 14
  {
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_Default

  .method /*0600000D*/ public hidebysig specialname 
          instance int32  get_MinimumSegmentSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x207c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_MinimumSegmentSize

  .method /*0600000E*/ public hidebysig specialname 
          instance int64  get_PauseWriterThreshold() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x207f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_PauseWriterThreshold

  .method /*0600000F*/ public hidebysig specialname 
          instance class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> 
          get_Pool() cil managed
  // SIG: 20 00 15 12 61 01 05
  {
    // Method begins at RVA 0x2082
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_Pool

  .method /*06000010*/ public hidebysig specialname 
          instance class System.IO.Pipelines.PipeScheduler/*02000007*/ 
          get_ReaderScheduler() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x2085
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_ReaderScheduler

  .method /*06000011*/ public hidebysig specialname 
          instance int64  get_ResumeWriterThreshold() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2088
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_ResumeWriterThreshold

  .method /*06000012*/ public hidebysig specialname 
          instance bool  get_UseSynchronizationContext() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x208b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_UseSynchronizationContext

  .method /*06000013*/ public hidebysig specialname 
          instance class System.IO.Pipelines.PipeScheduler/*02000007*/ 
          get_WriterScheduler() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x208e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeOptions::get_WriterScheduler

  .property /*17000007*/ callconv(8) class System.IO.Pipelines.PipeOptions/*02000005*/
          Default()
  {
    .get class System.IO.Pipelines.PipeOptions/*02000005*/ System.IO.Pipelines.PipeOptions/*02000005*/::get_Default() /* 0600000C */
  } // end of property PipeOptions::Default
  .property /*17000008*/ instance callconv(8) int32
          MinimumSegmentSize()
  {
    .get instance int32 System.IO.Pipelines.PipeOptions/*02000005*/::get_MinimumSegmentSize() /* 0600000D */
  } // end of property PipeOptions::MinimumSegmentSize
  .property /*17000009*/ instance callconv(8) int64
          PauseWriterThreshold()
  {
    .get instance int64 System.IO.Pipelines.PipeOptions/*02000005*/::get_PauseWriterThreshold() /* 0600000E */
  } // end of property PipeOptions::PauseWriterThreshold
  .property /*1700000A*/ instance callconv(8) class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8>
          Pool()
  {
    .get instance class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> System.IO.Pipelines.PipeOptions/*02000005*/::get_Pool() /* 0600000F */
  } // end of property PipeOptions::Pool
  .property /*1700000B*/ instance callconv(8) class System.IO.Pipelines.PipeScheduler/*02000007*/
          ReaderScheduler()
  {
    .get instance class System.IO.Pipelines.PipeScheduler/*02000007*/ System.IO.Pipelines.PipeOptions/*02000005*/::get_ReaderScheduler() /* 06000010 */
  } // end of property PipeOptions::ReaderScheduler
  .property /*1700000C*/ instance callconv(8) int64
          ResumeWriterThreshold()
  {
    .get instance int64 System.IO.Pipelines.PipeOptions/*02000005*/::get_ResumeWriterThreshold() /* 06000011 */
  } // end of property PipeOptions::ResumeWriterThreshold
  .property /*1700000D*/ instance callconv(8) bool
          UseSynchronizationContext()
  {
    .get instance bool System.IO.Pipelines.PipeOptions/*02000005*/::get_UseSynchronizationContext() /* 06000012 */
  } // end of property PipeOptions::UseSynchronizationContext
  .property /*1700000E*/ instance callconv(8) class System.IO.Pipelines.PipeScheduler/*02000007*/
          WriterScheduler()
  {
    .get instance class System.IO.Pipelines.PipeScheduler/*02000007*/ System.IO.Pipelines.PipeOptions/*02000005*/::get_WriterScheduler() /* 06000013 */
  } // end of property PipeOptions::WriterScheduler
} // end of class System.IO.Pipelines.PipeOptions

.class /*02000006*/ public abstract auto ansi beforefieldinit System.IO.Pipelines.PipeReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000014*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2091
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PipeReader::.ctor

  .method /*06000015*/ public hidebysig newslot abstract virtual 
          instance void  AdvanceTo(valuetype [System.Memory/*23000002*/]System.SequencePosition/*01000019*/ consumed) cil managed
  // SIG: 20 01 01 11 65
  {
    // Method begins at RVA 0x0
  } // end of method PipeReader::AdvanceTo

  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance void  AdvanceTo(valuetype [System.Memory/*23000002*/]System.SequencePosition/*01000019*/ consumed,
                                   valuetype [System.Memory/*23000002*/]System.SequencePosition/*01000019*/ examined) cil managed
  // SIG: 20 02 01 11 65 11 65
  {
    // Method begins at RVA 0x0
  } // end of method PipeReader::AdvanceTo

  .method /*06000017*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 
          AsStream([opt] bool leaveOpen) cil managed
  // SIG: 20 01 12 69 02
  {
    .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800000E*/  = bool(false)
    // Method begins at RVA 0x2099
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::AsStream

  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance void  CancelPendingRead() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method PipeReader::CancelPendingRead

  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance void  Complete([opt] class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ exception) cil managed
  // SIG: 20 01 01 12 6D
  {
    .custom /*0C000028:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800000F*/  = nullref
    // Method begins at RVA 0x0
  } // end of method PipeReader::Complete

  .method /*0600001A*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          CompleteAsync([opt] class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ exception) cil managed
  // SIG: 20 01 11 71 12 6D
  {
    .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000010*/  = nullref
    // Method begins at RVA 0x209c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::CompleteAsync

  .method /*0600001B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          CopyToAsync(class System.IO.Pipelines.PipeWriter/*02000008*/ destination,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 02 12 75 12 20 11 79
  {
    .custom /*0C00002B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000012*/  = nullref
    // Method begins at RVA 0x209f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::CopyToAsync

  .method /*0600001C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ destination,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 02 12 75 12 69 11 79
  {
    .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000014*/  = nullref
    // Method begins at RVA 0x20a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::CopyToAsync

  .method /*0600001D*/ public hidebysig static 
          class System.IO.Pipelines.PipeReader/*02000006*/ 
          Create(valuetype [System.Memory/*23000002*/]System.Buffers.ReadOnlySequence`1/*0100001F*/<uint8> sequence) cil managed
  // SIG: 00 01 12 18 15 11 7D 01 05
  {
    .param [0]/*08000015*/ 
    .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::Create

  .method /*0600001E*/ public hidebysig static 
          class System.IO.Pipelines.PipeReader/*02000006*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 'stream',
                 [opt] class System.IO.Pipelines.StreamPipeReaderOptions/*0200000B*/ readerOptions) cil managed
  // SIG: 00 02 12 18 12 69 12 2C
  {
    .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000018*/  = nullref
    .custom /*0C000027:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::Create

  .method /*0600001F*/ public hidebysig newslot virtual 
          instance void  OnWriterCompleted(class [System.Runtime/*23000001*/]System.Action`2/*01000021*/<class [System.Runtime/*23000001*/]System.Exception/*0100001B*/,object> callback,
                                           object state) cil managed
  // SIG: 20 02 01 15 12 80 85 02 12 6D 1C 1C
  {
    .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000015 */ = ( 01 00 62 4F 6E 57 72 69 74 65 72 43 6F 6D 70 6C   // ..bOnWriterCompl
                                                                                                                                                   65 74 65 64 20 68 61 73 20 62 65 65 6E 20 64 65   // eted has been de
                                                                                                                                                   70 72 65 63 61 74 65 64 20 61 6E 64 20 6D 61 79   // precated and may
                                                                                                                                                   20 6E 6F 74 20 62 65 20 69 6E 76 6F 6B 65 64 20   //  not be invoked 
                                                                                                                                                   6F 6E 20 61 6C 6C 20 69 6D 70 6C 65 6D 65 6E 74   // on all implement
                                                                                                                                                   61 74 69 6F 6E 73 20 6F 66 20 50 69 70 65 52 65   // ations of PipeRe
                                                                                                                                                   61 64 65 72 2E 00 00 )                            // ader...
    .param [1]/*08000019*/ 
    .custom /*0C000029:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 01 02 02 00 00 ) 
    // Method begins at RVA 0x20ab
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeReader::OnWriterCompleted

  .method /*06000020*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<valuetype System.IO.Pipelines.ReadResult/*02000009*/> 
          ReadAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 89 01 11 24 11 79
  {
    .param [1]/*0800001B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method PipeReader::ReadAsync

  .method /*06000021*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<valuetype System.IO.Pipelines.ReadResult/*02000009*/> 
          ReadAtLeastAsync(int32 minimumSize,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 89 01 11 24 08 11 79
  {
    .param [2]/*0800001D*/  = nullref
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::ReadAtLeastAsync

  .method /*06000022*/ family hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<valuetype System.IO.Pipelines.ReadResult/*02000009*/> 
          ReadAtLeastAsyncCore(int32 minimumSize,
                               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 89 01 11 24 08 11 79
  {
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeReader::ReadAtLeastAsyncCore

  .method /*06000023*/ public hidebysig newslot abstract virtual 
          instance bool  TryRead([out] valuetype System.IO.Pipelines.ReadResult/*02000009*/& result) cil managed
  // SIG: 20 01 02 10 11 24
  {
    // Method begins at RVA 0x0
  } // end of method PipeReader::TryRead

} // end of class System.IO.Pipelines.PipeReader

.class /*02000007*/ public abstract auto ansi beforefieldinit System.IO.Pipelines.PipeScheduler
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000024*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PipeScheduler::.ctor

  .method /*06000025*/ public hidebysig specialname static 
          class System.IO.Pipelines.PipeScheduler/*02000007*/ 
          get_Inline() cil managed
  // SIG: 00 00 12 1C
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeScheduler::get_Inline

  .method /*06000026*/ public hidebysig specialname static 
          class System.IO.Pipelines.PipeScheduler/*02000007*/ 
          get_ThreadPool() cil managed
  // SIG: 00 00 12 1C
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeScheduler::get_ThreadPool

  .method /*06000027*/ public hidebysig newslot abstract virtual 
          instance void  Schedule(class [System.Runtime/*23000001*/]System.Action`1/*01000023*/<object> action,
                                  object state) cil managed
  // SIG: 20 02 01 15 12 80 8D 01 1C 1C
  {
    .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000021*/ 
    .custom /*0C000030:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method PipeScheduler::Schedule

  .property /*1700000F*/ callconv(8) class System.IO.Pipelines.PipeScheduler/*02000007*/
          Inline()
  {
    .get class System.IO.Pipelines.PipeScheduler/*02000007*/ System.IO.Pipelines.PipeScheduler/*02000007*/::get_Inline() /* 06000025 */
  } // end of property PipeScheduler::Inline
  .property /*17000010*/ callconv(8) class System.IO.Pipelines.PipeScheduler/*02000007*/
          ThreadPool()
  {
    .get class System.IO.Pipelines.PipeScheduler/*02000007*/ System.IO.Pipelines.PipeScheduler/*02000007*/::get_ThreadPool() /* 06000026 */
  } // end of property PipeScheduler::ThreadPool
} // end of class System.IO.Pipelines.PipeScheduler

.class /*02000008*/ public abstract auto ansi beforefieldinit System.IO.Pipelines.PipeWriter
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Memory/*23000002*/]System.Buffers.IBufferWriter`1/*01000024*/<uint8>/*1B000001*/
{
  .method /*06000028*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PipeWriter::.ctor

  .method /*06000029*/ public hidebysig newslot abstract virtual 
          instance void  Advance(int32 bytes) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method PipeWriter::Advance

  .method /*0600002A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 
          AsStream([opt] bool leaveOpen) cil managed
  // SIG: 20 01 12 69 02
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000024*/  = bool(false)
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::AsStream

  .method /*0600002B*/ public hidebysig newslot abstract virtual 
          instance void  CancelPendingFlush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method PipeWriter::CancelPendingFlush

  .method /*0600002C*/ public hidebysig newslot specialname virtual 
          instance bool  get_CanGetUnflushedBytes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::get_CanGetUnflushedBytes

  .method /*0600002D*/ public hidebysig newslot abstract virtual 
          instance void  Complete([opt] class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ exception) cil managed
  // SIG: 20 01 01 12 6D
  {
    .custom /*0C000034:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000025*/  = nullref
    // Method begins at RVA 0x0
  } // end of method PipeWriter::Complete

  .method /*0600002E*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          CompleteAsync([opt] class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ exception) cil managed
  // SIG: 20 01 11 71 12 6D
  {
    .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000026*/  = nullref
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::CompleteAsync

  .method /*0600002F*/ famorassem hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          CopyFromAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ source,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 02 12 75 12 69 11 79
  {
    .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000028*/  = nullref
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::CopyFromAsync

  .method /*06000030*/ public hidebysig static 
          class System.IO.Pipelines.PipeWriter/*02000008*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 'stream',
                 [opt] class System.IO.Pipelines.StreamPipeWriterOptions/*0200000C*/ writerOptions) cil managed
  // SIG: 00 02 12 20 12 69 12 30
  {
    .custom /*0C000038:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800002A*/  = nullref
    .custom /*0C000033:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::Create

  .method /*06000031*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<valuetype System.IO.Pipelines.FlushResult/*02000002*/> 
          FlushAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 89 01 11 08 11 79
  {
    .param [1]/*0800002B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method PipeWriter::FlushAsync

  .method /*06000032*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 95 01 05 08
  {
    .param [1]/*0800002C*/  = int32(0x00000000)
    // Method begins at RVA 0x0
  } // end of method PipeWriter::GetMemory

  .method /*06000033*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 99 01 05 08
  {
    .param [1]/*0800002D*/  = int32(0x00000000)
    // Method begins at RVA 0x0
  } // end of method PipeWriter::GetSpan

  .method /*06000034*/ public hidebysig newslot virtual 
          instance void  OnReaderCompleted(class [System.Runtime/*23000001*/]System.Action`2/*01000021*/<class [System.Runtime/*23000001*/]System.Exception/*0100001B*/,object> callback,
                                           object state) cil managed
  // SIG: 20 02 01 15 12 80 85 02 12 6D 1C 1C
  {
    .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00003A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000015 */ = ( 01 00 62 4F 6E 52 65 61 64 65 72 43 6F 6D 70 6C   // ..bOnReaderCompl
                                                                                                                                                   65 74 65 64 20 68 61 73 20 62 65 65 6E 20 64 65   // eted has been de
                                                                                                                                                   70 72 65 63 61 74 65 64 20 61 6E 64 20 6D 61 79   // precated and may
                                                                                                                                                   20 6E 6F 74 20 62 65 20 69 6E 76 6F 6B 65 64 20   //  not be invoked 
                                                                                                                                                   6F 6E 20 61 6C 6C 20 69 6D 70 6C 65 6D 65 6E 74   // on all implement
                                                                                                                                                   61 74 69 6F 6E 73 20 6F 66 20 50 69 70 65 57 72   // ations of PipeWr
                                                                                                                                                   69 74 65 72 2E 00 00 )                            // iter...
    .param [1]/*0800002E*/ 
    .custom /*0C000036:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 03 00 00 00 01 02 02 00 00 ) 
    // Method begins at RVA 0x20d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeWriter::OnReaderCompleted

  .method /*06000035*/ public hidebysig newslot specialname virtual 
          instance int64  get_UnflushedBytes() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::get_UnflushedBytes

  .method /*06000036*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<valuetype System.IO.Pipelines.FlushResult/*02000002*/> 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000027*/<uint8> source,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 89 01 11 08 15 11 80 9D 01 05 11 79
  {
    .param [2]/*08000031*/  = nullref
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriter::WriteAsync

  .property /*17000011*/ instance callconv(8) bool
          CanGetUnflushedBytes()
  {
    .get instance bool System.IO.Pipelines.PipeWriter/*02000008*/::get_CanGetUnflushedBytes() /* 0600002C */
  } // end of property PipeWriter::CanGetUnflushedBytes
  .property /*17000012*/ instance callconv(8) int64
          UnflushedBytes()
  {
    .get instance int64 System.IO.Pipelines.PipeWriter/*02000008*/::get_UnflushedBytes() /* 06000035 */
  } // end of property PipeWriter::UnflushedBytes
} // end of class System.IO.Pipelines.PipeWriter

.class /*02000009*/ public sequential ansi sealed beforefieldinit System.IO.Pipelines.ReadResult
       extends [System.Runtime/*23000001*/]System.ValueType/*01000014*/
{
  .custom /*0C00001E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000028*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .field /*04000002*/ private initonly object _dummy
  .field /*04000003*/ private initonly int32 _dummyPrimitive
  .method /*06000037*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Memory/*23000002*/]System.Buffers.ReadOnlySequence`1/*0100001F*/<uint8> buffer,
                               bool isCanceled,
                               bool isCompleted) cil managed
  // SIG: 20 03 01 15 11 7D 01 05 02 02
  {
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadResult::.ctor

  .method /*06000038*/ public hidebysig specialname 
          instance valuetype [System.Memory/*23000002*/]System.Buffers.ReadOnlySequence`1/*0100001F*/<uint8> 
          get_Buffer() cil managed
  // SIG: 20 00 15 11 7D 01 05
  {
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadResult::get_Buffer

  .method /*06000039*/ public hidebysig specialname 
          instance bool  get_IsCanceled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadResult::get_IsCanceled

  .method /*0600003A*/ public hidebysig specialname 
          instance bool  get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadResult::get_IsCompleted

  .property /*17000013*/ instance callconv(8) valuetype [System.Memory/*23000002*/]System.Buffers.ReadOnlySequence`1/*0100001F*/<uint8>
          Buffer()
  {
    .get instance valuetype [System.Memory/*23000002*/]System.Buffers.ReadOnlySequence`1/*0100001F*/<uint8> System.IO.Pipelines.ReadResult/*02000009*/::get_Buffer() /* 06000038 */
  } // end of property ReadResult::Buffer
  .property /*17000014*/ instance callconv(8) bool
          IsCanceled()
  {
    .get instance bool System.IO.Pipelines.ReadResult/*02000009*/::get_IsCanceled() /* 06000039 */
  } // end of property ReadResult::IsCanceled
  .property /*17000015*/ instance callconv(8) bool
          IsCompleted()
  {
    .get instance bool System.IO.Pipelines.ReadResult/*02000009*/::get_IsCompleted() /* 0600003A */
  } // end of property ReadResult::IsCompleted
} // end of class System.IO.Pipelines.ReadResult

.class /*0200000A*/ public abstract auto ansi sealed beforefieldinit System.IO.Pipelines.StreamPipeExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00001F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600003B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ source,
                      class System.IO.Pipelines.PipeWriter/*02000008*/ destination,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 03 12 75 12 69 12 20 11 79
  {
    .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00003C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000037*/  = nullref
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeExtensions::CopyToAsync

} // end of class System.IO.Pipelines.StreamPipeExtensions

.class /*0200000B*/ public auto ansi beforefieldinit System.IO.Pipelines.StreamPipeReaderOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000022:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> pool,
                               int32 bufferSize,
                               int32 minimumReadSize,
                               bool leaveOpen) cil managed
  // SIG: 20 04 01 15 12 61 01 05 08 08 02
  {
    .custom /*0C00003D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StreamPipeReaderOptions::.ctor

  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([opt] class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> pool,
                               [opt] int32 bufferSize,
                               [opt] int32 minimumReadSize,
                               [opt] bool leaveOpen,
                               [opt] bool useZeroByteReads) cil managed
  // SIG: 20 05 01 15 12 61 01 05 08 08 02 02
  {
    .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003C*/  = nullref
    .param [2]/*0800003D*/  = int32(0xFFFFFFFF)
    .param [3]/*0800003E*/  = int32(0xFFFFFFFF)
    .param [4]/*0800003F*/  = bool(false)
    .param [5]/*08000040*/  = bool(false)
    // Method begins at RVA 0x20f7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StreamPipeReaderOptions::.ctor

  .method /*0600003E*/ public hidebysig specialname 
          instance int32  get_BufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeReaderOptions::get_BufferSize

  .method /*0600003F*/ public hidebysig specialname 
          instance bool  get_LeaveOpen() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeReaderOptions::get_LeaveOpen

  .method /*06000040*/ public hidebysig specialname 
          instance int32  get_MinimumReadSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeReaderOptions::get_MinimumReadSize

  .method /*06000041*/ public hidebysig specialname 
          instance class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> 
          get_Pool() cil managed
  // SIG: 20 00 15 12 61 01 05
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeReaderOptions::get_Pool

  .method /*06000042*/ public hidebysig specialname 
          instance bool  get_UseZeroByteReads() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeReaderOptions::get_UseZeroByteReads

  .property /*17000016*/ instance callconv(8) int32
          BufferSize()
  {
    .get instance int32 System.IO.Pipelines.StreamPipeReaderOptions/*0200000B*/::get_BufferSize() /* 0600003E */
  } // end of property StreamPipeReaderOptions::BufferSize
  .property /*17000017*/ instance callconv(8) bool
          LeaveOpen()
  {
    .get instance bool System.IO.Pipelines.StreamPipeReaderOptions/*0200000B*/::get_LeaveOpen() /* 0600003F */
  } // end of property StreamPipeReaderOptions::LeaveOpen
  .property /*17000018*/ instance callconv(8) int32
          MinimumReadSize()
  {
    .get instance int32 System.IO.Pipelines.StreamPipeReaderOptions/*0200000B*/::get_MinimumReadSize() /* 06000040 */
  } // end of property StreamPipeReaderOptions::MinimumReadSize
  .property /*17000019*/ instance callconv(8) class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8>
          Pool()
  {
    .get instance class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> System.IO.Pipelines.StreamPipeReaderOptions/*0200000B*/::get_Pool() /* 06000041 */
  } // end of property StreamPipeReaderOptions::Pool
  .property /*1700001A*/ instance callconv(8) bool
          UseZeroByteReads()
  {
    .get instance bool System.IO.Pipelines.StreamPipeReaderOptions/*0200000B*/::get_UseZeroByteReads() /* 06000042 */
  } // end of property StreamPipeReaderOptions::UseZeroByteReads
} // end of class System.IO.Pipelines.StreamPipeReaderOptions

.class /*0200000C*/ public auto ansi beforefieldinit System.IO.Pipelines.StreamPipeWriterOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000043*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([opt] class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> pool,
                               [opt] int32 minimumBufferSize,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 15 12 61 01 05 08 02
  {
    .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000041*/  = nullref
    .param [2]/*08000042*/  = int32(0xFFFFFFFF)
    .param [3]/*08000043*/  = bool(false)
    // Method begins at RVA 0x210e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StreamPipeWriterOptions::.ctor

  .method /*06000044*/ public hidebysig specialname 
          instance bool  get_LeaveOpen() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeWriterOptions::get_LeaveOpen

  .method /*06000045*/ public hidebysig specialname 
          instance int32  get_MinimumBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeWriterOptions::get_MinimumBufferSize

  .method /*06000046*/ public hidebysig specialname 
          instance class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> 
          get_Pool() cil managed
  // SIG: 20 00 15 12 61 01 05
  {
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamPipeWriterOptions::get_Pool

  .property /*1700001B*/ instance callconv(8) bool
          LeaveOpen()
  {
    .get instance bool System.IO.Pipelines.StreamPipeWriterOptions/*0200000C*/::get_LeaveOpen() /* 06000044 */
  } // end of property StreamPipeWriterOptions::LeaveOpen
  .property /*1700001C*/ instance callconv(8) int32
          MinimumBufferSize()
  {
    .get instance int32 System.IO.Pipelines.StreamPipeWriterOptions/*0200000C*/::get_MinimumBufferSize() /* 06000045 */
  } // end of property StreamPipeWriterOptions::MinimumBufferSize
  .property /*1700001D*/ instance callconv(8) class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8>
          Pool()
  {
    .get instance class [System.Memory/*23000002*/]System.Buffers.MemoryPool`1/*01000018*/<uint8> System.IO.Pipelines.StreamPipeWriterOptions/*0200000C*/::get_Pool() /* 06000046 */
  } // end of property StreamPipeWriterOptions::Pool
} // end of class System.IO.Pipelines.StreamPipeWriterOptions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
