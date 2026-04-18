


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
// Time-date stamp:            0x84d6dd1b
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000414e
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
// Checksum:                       0x0000db60
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000040fc [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000454] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002c00 [0x00002910] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000040e0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00002154 Virtual Size
//              0x00002000 Virtual Address
//              0x00002200 Size of Raw Data
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
//              0x00000454 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002400 Pointer to Raw Data
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
//              0x00002a00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00004000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000150
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000413e Import Name Table
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
// 0x000022ac [0x00001db4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00004060 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000e84 Size
//              '#~' Name
//    Stream 2:
//              0x00000ef0 Offset
//              0x00000a30 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001920 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001924 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001934 Offset
//              0x00000480 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000903a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 21776
// PE header size       : 512 (496 used)    ( 2.35%)
// PE additional info   : 11819             (54.28%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.33%)
// CLR meta-data size  : 7604               (34.92%)
// CLR additional info : 128                ( 0.59%)
// CLR method headers  : 215                ( 0.99%)
// Managed code         : 379               ( 1.74%)
// Data                 : 2048              ( 9.40%)
// Unaccounted          : -1001             (-4.60%)

// Num.of PE sections   : 3
//   .text    - 8704
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 7604
//   Module        -    1 (10 bytes)
//   TypeDef       -   11 (154 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   43 (258 bytes)
//   MethodDef     -  103 (1442 bytes)    0 abstract, 0 native, 99 bodies
//   FieldDef      -   14 (84 bytes)      0 constant
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -  138 (828 bytes)
//   MethodImpl    -    4 (24 bytes)
//   Constant      -   13 (78 bytes)
//   CustomAttribute-   45 (270 bytes)
//   PropertyMap   -    5 (20 bytes)
//   Property      -   21 (126 bytes)
//   MethodSemantic-   24 (144 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   Strings       -  2607 bytes
//   Blobs         -  1152 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   195 bytes

// CLR additional info : 128

// CLR method headers : 215
//   Num.of method bodies  - 99
//   Num.of fat headers    - 4
//   Num.of tiny headers   - 95
//   Num.of fat sections   - 0
//   Num.of small sections - 4

// Managed code : 379
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AnonymousPipeClientStream      (public) (auto) (ansi) (sealed) 
// Class AnonymousPipeServerStream      (public) (auto) (ansi) (sealed) 
// Class NamedPipeClientStream          (public) (auto) (ansi) (sealed) 
// Class NamedPipeServerStream          (public) (auto) (ansi) (sealed) 
// Class PipeDirection                  (public) (auto) (ansi) (sealed) 
// Class PipeOptions                    (public) (auto) (ansi) (sealed) 
// Class PipeStream                     (public) (abstract) (auto) (ansi) 
// Class PipeStreamImpersonationWorker  (public) (auto) (ansi) (sealed) 
// Class PipeTransmissionMode           (public) (auto) (ansi) (sealed) 
// Class SafePipeHandle                 (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.Pipes
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 38 2E 30 01 00 54 0E 14   // ersion=v8.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 38 2E 30 )          // Name..NET 8.0
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 0F 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ...System.IO.Pip
                                                                                                                                                                        65 73 00 00 )                                     // es..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0F 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ...System.IO.Pip
                                                                                                                                                                       65 73 00 00 )                                     // es..
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 0F 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ...System.IO.Pip
                                                                                                                                                                 65 73 00 00 )                                     // es..
  .custom /*0C000014:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
  .ver 8:0:0:0
}
.module System.IO.Pipes.dll
// MVID: {baa44fd7-d012-45b4-a247-9ea030a6b5bc}
.custom /*0C000001:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.IO.Pipes.AnonymousPipeClientStream
       extends System.IO.Pipes.PipeStream/*02000008*/
{
  .custom /*0C000015:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000016:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ safePipeHandle) cil managed
  // SIG: 20 02 01 11 18 12 2C
  {
    // Method begins at RVA 0x2050
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeClientStream::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               string pipeHandleAsString) cil managed
  // SIG: 20 02 01 11 18 0E
  {
    // Method begins at RVA 0x205a
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeClientStream::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeHandleAsString) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2064
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeClientStream::.ctor

  .method /*06000004*/ public hidebysig specialname virtual 
          instance void  set_ReadMode(valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x206e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeClientStream::set_ReadMode

  .method /*06000005*/ public hidebysig specialname virtual 
          instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 
          get_TransmissionMode() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x2070
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AnonymousPipeClientStream::get_TransmissionMode

  .method /*06000006*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize /*01000016::0A000015*/ 
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
      IL_0003:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize() /* 0A000015 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeClientStream::Finalize

  .property /*17000001*/ instance callconv(8) valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/
          ReadMode()
  {
    .set instance void System.IO.Pipes.AnonymousPipeClientStream/*02000002*/::set_ReadMode(valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/) /* 06000004 */
  } // end of property AnonymousPipeClientStream::ReadMode
  .property /*17000002*/ instance callconv(8) valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/
          TransmissionMode()
  {
    .get instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ System.IO.Pipes.AnonymousPipeClientStream/*02000002*/::get_TransmissionMode() /* 06000005 */
  } // end of property AnonymousPipeClientStream::TransmissionMode
} // end of class System.IO.Pipes.AnonymousPipeClientStream

.class /*02000003*/ public auto ansi sealed beforefieldinit System.IO.Pipes.AnonymousPipeServerStream
       extends System.IO.Pipes.PipeStream/*02000008*/
{
  .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000018:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209c
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction) cil managed
  // SIG: 20 01 01 11 18
  {
    // Method begins at RVA 0x20a6
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ serverSafePipeHandle,
                               class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ clientSafePipeHandle) cil managed
  // SIG: 20 03 01 11 18 12 2C 12 2C
  {
    // Method begins at RVA 0x20b0
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               valuetype [System.Runtime/*23000001*/]System.IO.HandleInheritability/*01000017*/ inheritability) cil managed
  // SIG: 20 02 01 11 18 11 5D
  {
    // Method begins at RVA 0x20ba
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               valuetype [System.Runtime/*23000001*/]System.IO.HandleInheritability/*01000017*/ inheritability,
                               int32 bufferSize) cil managed
  // SIG: 20 03 01 11 18 11 5D 08
  {
    // Method begins at RVA 0x20c4
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ 
          get_ClientSafePipeHandle() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AnonymousPipeServerStream::get_ClientSafePipeHandle

  .method /*0600000D*/ public hidebysig specialname virtual 
          instance void  set_ReadMode(valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x20d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::set_ReadMode

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 
          get_TransmissionMode() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AnonymousPipeServerStream::get_TransmissionMode

  .method /*0600000F*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::Dispose

  .method /*06000010*/ public hidebysig instance void 
          DisposeLocalCopyOfClientHandle() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::DisposeLocalCopyOfClientHandle

  .method /*06000011*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize /*01000016::0A000015*/ 
    // Method begins at RVA 0x20dc
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize() /* 0A000015 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method AnonymousPipeServerStream::Finalize

  .method /*06000012*/ public hidebysig instance string 
          GetClientHandleAsString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AnonymousPipeServerStream::GetClientHandleAsString

  .property /*17000003*/ instance callconv(8) class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/
          ClientSafePipeHandle()
  {
    .get instance class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ System.IO.Pipes.AnonymousPipeServerStream/*02000003*/::get_ClientSafePipeHandle() /* 0600000C */
  } // end of property AnonymousPipeServerStream::ClientSafePipeHandle
  .property /*17000004*/ instance callconv(8) valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/
          ReadMode()
  {
    .set instance void System.IO.Pipes.AnonymousPipeServerStream/*02000003*/::set_ReadMode(valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/) /* 0600000D */
  } // end of property AnonymousPipeServerStream::ReadMode
  .property /*17000005*/ instance callconv(8) valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/
          TransmissionMode()
  {
    .get instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ System.IO.Pipes.AnonymousPipeServerStream/*02000003*/::get_TransmissionMode() /* 0600000E */
  } // end of property AnonymousPipeServerStream::TransmissionMode
} // end of class System.IO.Pipes.AnonymousPipeServerStream

.class /*02000004*/ public auto ansi sealed beforefieldinit System.IO.Pipes.NamedPipeClientStream
       extends System.IO.Pipes.PipeStream/*02000008*/
{
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000013*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               bool isAsync,
                               bool isConnected,
                               class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ safePipeHandle) cil managed
  // SIG: 20 04 01 11 18 02 02 12 2C
  {
    // Method begins at RVA 0x2107
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2111
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serverName,
                               string pipeName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x211b
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serverName,
                               string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction) cil managed
  // SIG: 20 03 01 0E 0E 11 18
  {
    // Method begins at RVA 0x2125
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serverName,
                               string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               valuetype System.IO.Pipes.PipeOptions/*02000007*/ options) cil managed
  // SIG: 20 04 01 0E 0E 11 18 11 1C
  {
    // Method begins at RVA 0x212f
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serverName,
                               string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               valuetype System.IO.Pipes.PipeOptions/*02000007*/ options,
                               valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*01000018*/ impersonationLevel) cil managed
  // SIG: 20 05 01 0E 0E 11 18 11 1C 11 61
  {
    // Method begins at RVA 0x2139
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serverName,
                               string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               valuetype System.IO.Pipes.PipeOptions/*02000007*/ options,
                               valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*01000018*/ impersonationLevel,
                               valuetype [System.Runtime/*23000001*/]System.IO.HandleInheritability/*01000017*/ inheritability) cil managed
  // SIG: 20 06 01 0E 0E 11 18 11 1C 11 61 11 5D
  {
    // Method begins at RVA 0x2143
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::.ctor

  .method /*0600001A*/ public hidebysig specialname 
          instance int32  get_NumberOfServerInstances() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeClientStream::get_NumberOfServerInstances

  .method /*0600001B*/ famorassem hidebysig virtual 
          instance void  CheckPipePropertyOperations() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2150
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::CheckPipePropertyOperations

  .method /*0600001C*/ public hidebysig instance void 
          Connect() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2152
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::Connect

  .method /*0600001D*/ public hidebysig instance void 
          Connect(int32 timeout) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2154
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::Connect

  .method /*0600001E*/ public hidebysig instance void 
          Connect(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 01 11 65
  {
    // Method begins at RVA 0x2156
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::Connect

  .method /*0600001F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          ConnectAsync() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeClientStream::ConnectAsync

  .method /*06000020*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          ConnectAsync(int32 timeout) cil managed
  // SIG: 20 01 12 69 08
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeClientStream::ConnectAsync

  .method /*06000021*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          ConnectAsync(int32 timeout,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 12 69 08 11 6D
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeClientStream::ConnectAsync

  .method /*06000022*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          ConnectAsync(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 12 69 11 65 11 6D
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeClientStream::ConnectAsync

  .method /*06000023*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          ConnectAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeClientStream::ConnectAsync

  .method /*06000024*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize /*01000016::0A000015*/ 
    // Method begins at RVA 0x2168
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize() /* 0A000015 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method NamedPipeClientStream::Finalize

  .property /*17000006*/ instance callconv(8) int32
          NumberOfServerInstances()
  {
    .custom /*0C00001D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .get instance int32 System.IO.Pipes.NamedPipeClientStream/*02000004*/::get_NumberOfServerInstances() /* 0600001A */
  } // end of property NamedPipeClientStream::NumberOfServerInstances
} // end of class System.IO.Pipes.NamedPipeClientStream

.class /*02000005*/ public auto ansi sealed beforefieldinit System.IO.Pipes.NamedPipeServerStream
       extends System.IO.Pipes.PipeStream/*02000008*/
{
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .field /*04000001*/ public static literal int32 MaxAllowedServerInstances = int32(0xFFFFFFFF)
  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               bool isAsync,
                               bool isConnected,
                               class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ safePipeHandle) cil managed
  // SIG: 20 04 01 11 18 02 02 12 2C
  {
    // Method begins at RVA 0x2190
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x219a
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction) cil managed
  // SIG: 20 02 01 0E 11 18
  {
    // Method begins at RVA 0x21a4
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               int32 maxNumberOfServerInstances) cil managed
  // SIG: 20 03 01 0E 11 18 08
  {
    // Method begins at RVA 0x21ae
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               int32 maxNumberOfServerInstances,
                               valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ transmissionMode) cil managed
  // SIG: 20 04 01 0E 11 18 08 11 28
  {
    // Method begins at RVA 0x21b8
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               int32 maxNumberOfServerInstances,
                               valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ transmissionMode,
                               valuetype System.IO.Pipes.PipeOptions/*02000007*/ options) cil managed
  // SIG: 20 05 01 0E 11 18 08 11 28 11 1C
  {
    // Method begins at RVA 0x21c2
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string pipeName,
                               valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               int32 maxNumberOfServerInstances,
                               valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ transmissionMode,
                               valuetype System.IO.Pipes.PipeOptions/*02000007*/ options,
                               int32 inBufferSize,
                               int32 outBufferSize) cil managed
  // SIG: 20 07 01 0E 11 18 08 11 28 11 1C 08 08
  {
    // Method begins at RVA 0x21cc
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000035       */ call       instance void System.IO.Pipes.PipeStream/*02000008*/::.ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/,
                                                                                                                   int32) /* 06000035 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::.ctor

  .method /*0600002C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ 
          BeginWaitForConnection(class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001E*/ callback,
                                 object state) cil managed
  // SIG: 20 02 12 75 12 79 1C
  {
    .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800004D*/ 
    .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeServerStream::BeginWaitForConnection

  .method /*0600002D*/ public hidebysig instance void 
          Disconnect() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::Disconnect

  .method /*0600002E*/ public hidebysig instance void 
          EndWaitForConnection(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ asyncResult) cil managed
  // SIG: 20 01 01 12 75
  {
    // Method begins at RVA 0x21db
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::EndWaitForConnection

  .method /*0600002F*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize /*01000016::0A000015*/ 
    // Method begins at RVA 0x21e0
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::Finalize() /* 0A000015 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::Finalize

  .method /*06000030*/ public hidebysig instance string 
          GetImpersonationUserName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeServerStream::GetImpersonationUserName

  .method /*06000031*/ public hidebysig instance void 
          RunAsClient(class System.IO.Pipes.PipeStreamImpersonationWorker/*02000009*/ impersonationWorker) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x220b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::RunAsClient

  .method /*06000032*/ public hidebysig instance void 
          WaitForConnection() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x220d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NamedPipeServerStream::WaitForConnection

  .method /*06000033*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WaitForConnectionAsync() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeServerStream::WaitForConnectionAsync

  .method /*06000034*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WaitForConnectionAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NamedPipeServerStream::WaitForConnectionAsync

} // end of class System.IO.Pipes.NamedPipeServerStream

.class /*02000006*/ public auto ansi sealed System.IO.Pipes.PipeDirection
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .field /*04000002*/ public specialname rtspecialname int32 value__
  .field /*04000003*/ public static literal valuetype System.IO.Pipes.PipeDirection/*02000006*/ In = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.IO.Pipes.PipeDirection/*02000006*/ Out = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.IO.Pipes.PipeDirection/*02000006*/ InOut = int32(0x00000003)
} // end of class System.IO.Pipes.PipeDirection

.class /*02000007*/ public auto ansi sealed System.IO.Pipes.PipeOptions
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .custom /*0C00001E:0A000017*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype System.IO.Pipes.PipeOptions/*02000007*/ WriteThrough = int32(0x80000000)
  .field /*04000008*/ public static literal valuetype System.IO.Pipes.PipeOptions/*02000007*/ None = int32(0x00000000)
  .field /*04000009*/ public static literal valuetype System.IO.Pipes.PipeOptions/*02000007*/ CurrentUserOnly = int32(0x20000000)
  .field /*0400000A*/ public static literal valuetype System.IO.Pipes.PipeOptions/*02000007*/ Asynchronous = int32(0x40000000)
  .field /*0400000B*/ public static literal valuetype System.IO.Pipes.PipeOptions/*02000007*/ FirstPipeInstance = int32(0x00080000)
} // end of class System.IO.Pipes.PipeOptions

.class /*02000008*/ public abstract auto ansi beforefieldinit System.IO.Pipes.PipeStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000021*/
{
  .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000035*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               int32 bufferSize) cil managed
  // SIG: 20 02 01 11 18 08
  {
    // Method begins at RVA 0x2215
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000021*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PipeStream::.ctor

  .method /*06000036*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.IO.Pipes.PipeDirection/*02000006*/ direction,
                               valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ transmissionMode,
                               int32 outBufferSize) cil managed
  // SIG: 20 03 01 11 18 11 28 08
  {
    // Method begins at RVA 0x221d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000021*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PipeStream::.ctor

  .method /*06000037*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2225
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_CanRead

  .method /*06000038*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2228
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_CanSeek

  .method /*06000039*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x222b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_CanWrite

  .method /*0600003A*/ public hidebysig newslot specialname virtual 
          instance int32  get_InBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x222e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_InBufferSize

  .method /*0600003B*/ public hidebysig specialname 
          instance bool  get_IsAsync() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_IsAsync

  .method /*0600003C*/ public hidebysig specialname 
          instance bool  get_IsConnected() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2234
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_IsConnected

  .method /*0600003D*/ family hidebysig specialname 
          instance void  set_IsConnected(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2237
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::set_IsConnected

  .method /*0600003E*/ family hidebysig specialname 
          instance bool  get_IsHandleExposed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_IsHandleExposed

  .method /*0600003F*/ public hidebysig specialname 
          instance bool  get_IsMessageComplete() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_IsMessageComplete

  .method /*06000040*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_Length

  .method /*06000041*/ public hidebysig newslot specialname virtual 
          instance int32  get_OutBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_OutBufferSize

  .method /*06000042*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_Position

  .method /*06000043*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2248
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::set_Position

  .method /*06000044*/ public hidebysig newslot specialname virtual 
          instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 
          get_ReadMode() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_ReadMode

  .method /*06000045*/ public hidebysig newslot specialname virtual 
          instance void  set_ReadMode(valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x224d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::set_ReadMode

  .method /*06000046*/ public hidebysig specialname 
          instance class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ 
          get_SafePipeHandle() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x224f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_SafePipeHandle

  .method /*06000047*/ public hidebysig newslot specialname virtual 
          instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ 
          get_TransmissionMode() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x2252
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::get_TransmissionMode

  .method /*06000048*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001E*/ callback,
                    object state) cil managed
  // SIG: 20 05 12 75 1D 05 08 08 12 79 1C
  {
    .param [4]/*0800005E*/ 
    .custom /*0C00002A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800005F*/ 
    .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2255
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::BeginRead

  .method /*06000049*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001E*/ callback,
                     object state) cil managed
  // SIG: 20 05 12 75 1D 05 08 08 12 79 1C
  {
    .param [4]/*08000063*/ 
    .custom /*0C00002C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000064*/ 
    .custom /*0C00002D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2258
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::BeginWrite

  .method /*0600004A*/ famorassem hidebysig newslot virtual 
          instance void  CheckPipePropertyOperations() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::CheckPipePropertyOperations

  .method /*0600004B*/ famorassem hidebysig 
          instance void  CheckReadOperations() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::CheckReadOperations

  .method /*0600004C*/ famorassem hidebysig 
          instance void  CheckWriteOperations() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::CheckWriteOperations

  .method /*0600004D*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2261
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::Dispose

  .method /*0600004E*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ asyncResult) cil managed
  // SIG: 20 01 08 12 75
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::EndRead

  .method /*0600004F*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ asyncResult) cil managed
  // SIG: 20 01 01 12 75
  {
    // Method begins at RVA 0x2266
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::EndWrite

  .method /*06000050*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2268
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::Flush

  .method /*06000051*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x226a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::FlushAsync

  .method /*06000052*/ family hidebysig instance void 
          InitializeHandle(class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ handle,
                           bool isExposed,
                           bool isAsync) cil managed
  // SIG: 20 03 01 12 2C 02 02
  {
    .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x226d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::InitializeHandle

  .method /*06000053*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::Read

  .method /*06000054*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 80 89 01 05
  {
    .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::Read

  .method /*06000055*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000023*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 8D 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::ReadAsync

  .method /*06000056*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000024*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 91 01 08 15 11 80 95 01 05 11 6D
  {
    .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000075*/  = nullref
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::ReadAsync

  .method /*06000057*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::ReadByte

  .method /*06000058*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000026*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 99
  {
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::Seek

  .method /*06000059*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2281
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::SetLength

  .method /*0600005A*/ public hidebysig instance void 
          WaitForPipeDrain() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000027:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2283
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::WaitForPipeDrain

  .method /*0600005B*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2285
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::Write

  .method /*0600005C*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000027*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 9D 01 05
  {
    .custom /*0C000028:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2287
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::Write

  .method /*0600005D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::WriteAsync

  .method /*0600005E*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000028*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000029*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 A1 15 11 80 A5 01 05 11 6D
  {
    .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000082*/  = nullref
    // Method begins at RVA 0x228c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeStream::WriteAsync

  .method /*0600005F*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x228f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PipeStream::WriteByte

  .property /*17000007*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_CanRead() /* 06000037 */
  } // end of property PipeStream::CanRead
  .property /*17000008*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_CanSeek() /* 06000038 */
  } // end of property PipeStream::CanSeek
  .property /*17000009*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_CanWrite() /* 06000039 */
  } // end of property PipeStream::CanWrite
  .property /*1700000A*/ instance callconv(8) int32
          InBufferSize()
  {
    .get instance int32 System.IO.Pipes.PipeStream/*02000008*/::get_InBufferSize() /* 0600003A */
  } // end of property PipeStream::InBufferSize
  .property /*1700000B*/ instance callconv(8) bool
          IsAsync()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_IsAsync() /* 0600003B */
  } // end of property PipeStream::IsAsync
  .property /*1700000C*/ instance callconv(8) bool
          IsConnected()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_IsConnected() /* 0600003C */
    .set instance void System.IO.Pipes.PipeStream/*02000008*/::set_IsConnected(bool) /* 0600003D */
  } // end of property PipeStream::IsConnected
  .property /*1700000D*/ instance callconv(8) bool
          IsHandleExposed()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_IsHandleExposed() /* 0600003E */
  } // end of property PipeStream::IsHandleExposed
  .property /*1700000E*/ instance callconv(8) bool
          IsMessageComplete()
  {
    .get instance bool System.IO.Pipes.PipeStream/*02000008*/::get_IsMessageComplete() /* 0600003F */
  } // end of property PipeStream::IsMessageComplete
  .property /*1700000F*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Pipes.PipeStream/*02000008*/::get_Length() /* 06000040 */
  } // end of property PipeStream::Length
  .property /*17000010*/ instance callconv(8) int32
          OutBufferSize()
  {
    .get instance int32 System.IO.Pipes.PipeStream/*02000008*/::get_OutBufferSize() /* 06000041 */
  } // end of property PipeStream::OutBufferSize
  .property /*17000011*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Pipes.PipeStream/*02000008*/::get_Position() /* 06000042 */
    .set instance void System.IO.Pipes.PipeStream/*02000008*/::set_Position(int64) /* 06000043 */
  } // end of property PipeStream::Position
  .property /*17000012*/ instance callconv(8) valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/
          ReadMode()
  {
    .get instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ System.IO.Pipes.PipeStream/*02000008*/::get_ReadMode() /* 06000044 */
    .set instance void System.IO.Pipes.PipeStream/*02000008*/::set_ReadMode(valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/) /* 06000045 */
  } // end of property PipeStream::ReadMode
  .property /*17000013*/ instance callconv(8) class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/
          SafePipeHandle()
  {
    .get instance class Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/ System.IO.Pipes.PipeStream/*02000008*/::get_SafePipeHandle() /* 06000046 */
  } // end of property PipeStream::SafePipeHandle
  .property /*17000014*/ instance callconv(8) valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/
          TransmissionMode()
  {
    .get instance valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ System.IO.Pipes.PipeStream/*02000008*/::get_TransmissionMode() /* 06000047 */
  } // end of property PipeStream::TransmissionMode
} // end of class System.IO.Pipes.PipeStream

.class /*02000009*/ public auto ansi sealed System.IO.Pipes.PipeStreamImpersonationWorker
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100002A*/
{
  .method /*06000060*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method PipeStreamImpersonationWorker::.ctor

  .method /*06000061*/ public hidebysig newslot virtual 
          instance void  Invoke() runtime managed
  // SIG: 20 00 01
  {
  } // end of method PipeStreamImpersonationWorker::Invoke

  .method /*06000062*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ 
          BeginInvoke(class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001E*/ callback,
                      object 'object') runtime managed
  // SIG: 20 02 12 75 12 79 1C
  {
  } // end of method PipeStreamImpersonationWorker::BeginInvoke

  .method /*06000063*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001D*/ result) runtime managed
  // SIG: 20 01 01 12 75
  {
  } // end of method PipeStreamImpersonationWorker::EndInvoke

} // end of class System.IO.Pipes.PipeStreamImpersonationWorker

.class /*0200000A*/ public auto ansi sealed System.IO.Pipes.PipeTransmissionMode
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .field /*0400000C*/ public specialname rtspecialname int32 value__
  .field /*0400000D*/ public static literal valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ Byte = int32(0x00000000)
  .field /*0400000E*/ public static literal valuetype System.IO.Pipes.PipeTransmissionMode/*0200000A*/ Message = int32(0x00000001)
  .custom /*0C000021:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
} // end of class System.IO.Pipes.PipeTransmissionMode

.class /*0200000B*/ public auto ansi sealed beforefieldinit Microsoft.Win32.SafeHandles.SafePipeHandle
       extends [System.Runtime/*23000001*/]Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid/*0100002B*/
{
  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2291
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid/*0100002B*/::.ctor(bool) /* 0A000019 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method SafePipeHandle::.ctor

  .method /*06000065*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(native int preexistingHandle,
                               bool ownsHandle) cil managed
  // SIG: 20 02 01 18 02
  {
    // Method begins at RVA 0x229a
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid/*0100002B*/::.ctor(bool) /* 0A000019 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method SafePipeHandle::.ctor

  .method /*06000066*/ public hidebysig specialname virtual 
          instance bool  get_IsInvalid() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SafePipeHandle::get_IsInvalid

  .method /*06000067*/ family hidebysig virtual 
          instance bool  ReleaseHandle() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SafePipeHandle::ReleaseHandle

  .property /*17000015*/ instance callconv(8) bool
          IsInvalid()
  {
    .get instance bool Microsoft.Win32.SafeHandles.SafePipeHandle/*0200000B*/::get_IsInvalid() /* 06000066 */
  } // end of property SafePipeHandle::IsInvalid
} // end of class Microsoft.Win32.SafeHandles.SafePipeHandle


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
