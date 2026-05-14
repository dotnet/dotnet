


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
// Time-date stamp:            0xfac18bd2
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00008600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000a5b2
// Base of code:                   0x00002000
// Base of data:                   0x0000c000
// Image base:                     0x10000000
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
// Checksum:                       0x0000f16e
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000a55f [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x00000444] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00009000 [0x00002878] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000a488 [0x00000054] address [size] of Debug Directory:           
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
//              0x000085b8 Virtual Size
//              0x00002000 Virtual Address
//              0x00008600 Size of Raw Data
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
//              0x00000444 Virtual Size
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00008800 Pointer to Raw Data
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
//              0x00008e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000a000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000005b4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000a5a1 Import Name Table
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
// 0x000025e4 [0x00007e24] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000a408 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00005364 Size
//              '#~' Name
//    Stream 2:
//              0x000053d0 Offset
//              0x00001560 Size
//              '#Strings' Name
//    Stream 3:
//              0x00006930 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00006934 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00006944 Offset
//              0x000014e0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000168909a05f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 47224
// PE header size       : 512 (496 used)    ( 1.08%)
// PE additional info   : 11707             (24.79%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.15%)
// CLR meta-data size  : 32292              (68.38%)
// CLR additional info : 128                ( 0.27%)
// CLR method headers  : 453                ( 0.96%)
// Managed code         : 972               ( 2.06%)
// Data                 : 2048              ( 4.34%)
// Unaccounted          : -960              (-2.03%)

// Num.of PE sections   : 3
//   .text    - 34304
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 32292
//   Module        -    1 (10 bytes)
//   TypeDef       -   29 (406 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   70 (420 bytes)
//   MethodDef     -  459 (6426 bytes)    6 abstract, 0 native, 453 bodies
//   FieldDef      -   22 (132 bytes)     1 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -  933 (5598 bytes)
//   Constant      -   61 (366 bytes)
//   CustomAttribute-  877 (5262 bytes)
//   InterfaceImpl -    4 (16 bytes)
//   PropertyMap   -    9 (36 bytes)
//   Property      -   34 (204 bytes)
//   MethodSemantic-   37 (222 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    9 (18 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -   12 (168 bytes)
//   NestedClass   -    2 (8 bytes)
//   GenericParam  -  166 (1328 bytes)
//   GenericParamConstraint-   94 (376 bytes)
//   Strings       -  5469 bytes
//   Blobs         -  5344 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   223 bytes

// CLR additional info : 128

// CLR method headers : 453
//   Num.of method bodies  - 453
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 453

// Managed code : 972
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class IsUnmanagedAttribute           (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ScopedRefAttribute             (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class SequencePosition               (public) (sequential) (ansi) (sealed) 
// Class MemoryExtensions               (public) (abstract) (auto) (ansi) (sealed) 
// Class EncodingExtensions             (public) (abstract) (auto) (ansi) (sealed) 
// Class SpanLineEnumerator             (public) (sequential) (ansi) (sealed) 
// Class SpanRuneEnumerator             (public) (sequential) (ansi) (sealed) 
// Class SequenceMarshal                (public) (abstract) (auto) (ansi) (sealed) 
// Class MemoryMarshal                  (public) (abstract) (auto) (ansi) (sealed) 
// Class ArrayBufferWriter`1            (public) (auto) (ansi) (sealed) 
// Class BuffersExtensions              (public) (abstract) (auto) (ansi) (sealed) 
// Interface IBufferWriter`1                (public) (abstract) (auto) (ansi) 
// Class MemoryPool`1                   (public) (abstract) (auto) (ansi) 
// Class ReadOnlySequenceSegment`1      (public) (abstract) (auto) (ansi) 
// Class ReadOnlySequence`1             (public) (sequential) (ansi) (sealed) 
// Class SequenceReaderExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class SequenceReader`1               (public) (sequential) (ansi) (sealed) 
// Class StandardFormat                 (public) (sequential) (ansi) (sealed) 
// Class Utf8Formatter                  (public) (abstract) (auto) (ansi) (sealed) 
// Class Utf8Parser                     (public) (abstract) (auto) (ansi) (sealed) 
// Class BinaryPrimitives               (public) (abstract) (auto) (ansi) (sealed) 
// Class TryWriteInterpolatedStringHandler (sequential) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Memory
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000005:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000006:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000007:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 0D 53 79 73 74 65 6D 2E 4D 65 6D 6F 72 79   // ...System.Memory
                                                                                                                                                                        00 00 ) 
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 53 79 73 74 65 6D 2E 4D 65 6D 6F 72 79   // ...System.Memory
                                                                                                                                                                       00 00 ) 
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 0D 53 79 73 74 65 6D 2E 4D 65 6D 6F 72 79   // ...System.Memory
                                                                                                                                                                 00 00 ) 
  .custom /*0C000017:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000013*/ = {property bool 'SkipVerification' = bool(true)}}
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
  .ver 7:0:0:0
}
.class extern /*27000001*/ forwarder System.Buffers.IMemoryOwner`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000002*/ forwarder System.Buffers.IPinnable
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000003*/ forwarder System.Buffers.MemoryHandle
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000004*/ forwarder System.Buffers.MemoryManager`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000005*/ forwarder System.Buffers.OperationStatus
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000006*/ forwarder System.Buffers.Text.Base64
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000007*/ forwarder System.Memory`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000008*/ forwarder System.ReadOnlyMemory`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000009*/ forwarder System.ReadOnlySpan`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000A*/ Enumerator
{
  .class extern System.ReadOnlySpan`1 /*27000009*/ 
}
.class extern /*2700000B*/ forwarder System.Span`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000C*/ Enumerator
{
  .class extern System.Span`1 /*2700000B*/ 
}
.module System.Memory.dll
// MVID: {228d1881-9b07-44e9-aeb7-d4e476ee41fe}
.custom /*0C000002:0A000012*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
.custom /*0C000003:06000008*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000008*/::.ctor(int32) /* 06000008 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000004:06000006*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000006*/::.ctor(bool) /* 06000006 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.IsUnmanagedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method IsUnmanagedAttribute::.ctor

} // end of class System.Runtime.CompilerServices.IsUnmanagedAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2060
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000046       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000046*/
    IL_000d:  /* 25   |                  */ dup
    IL_000e:  /* 16   |                  */ ldc.i4.0
    IL_000f:  /* 03   |                  */ ldarg.1
    IL_0010:  /* 9C   |                  */ stelem.i1
    IL_0011:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000004*/::NullableFlags /* 04000001 */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2078
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000004*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000002*/ public initonly uint8 Flag
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2087
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002C:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2096
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000006*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000007*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.ScopedRefAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000031:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000032:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 00 08 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ScopedRefAttribute::.ctor

} // end of class System.Runtime.CompilerServices.ScopedRefAttribute

.class /*02000008*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000036:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000037:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000004*/ public initonly int32 Version
  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 A_1) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20ad
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000008*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000009*/ public sequential ansi sealed beforefieldinit System.SequencePosition
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<valuetype System.SequencePosition/*02000009*/>/*1B000001*/
{
  .custom /*0C00003A:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<valuetype System.SequencePosition/*02000009*/>/*1B000001*/
  .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ private initonly object _dummy
  .field /*04000006*/ private initonly int32 _dummyPrimitive
  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               int32 integer) cil managed
  // SIG: 20 02 01 1C 08
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::.ctor

  .method /*0600000A*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00003F:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000003*/ 
    .custom /*0C00001F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::Equals

  .method /*0600000B*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.SequencePosition/*02000009*/ other) cil managed
  // SIG: 20 01 02 11 24
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::Equals

  .method /*0600000C*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000049:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::GetHashCode

  .method /*0600000D*/ public hidebysig instance int32 
          GetInteger() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00004E:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::GetInteger

  .method /*0600000E*/ public hidebysig instance object 
          GetObject() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000055:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::GetObject

} // end of class System.SequencePosition

.class /*0200000A*/ public abstract auto ansi sealed beforefieldinit System.MemoryExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000040:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200001C*/ sequential ansi sealed nested public beforefieldinit TryWriteInterpolatedStringHandler
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
  {
    .custom /*0C000097:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*0100002F*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .custom /*0C000098:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000030*/::.ctor(string,
                                                                                                                        bool) /* 0A00001B */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                                 62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                                 73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                                 74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                                 69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                                 69 6C 65 72 2E 01 00 00 )                         // iler....
    .custom /*0C000099:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000031*/::.ctor(string) /* 0A00001C */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
    .custom /*0C00009A:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C00009B:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InterpolatedStringHandlerAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .field /*04000013*/ private initonly object _dummy
    .field /*04000014*/ private initonly int32 _dummyPrimitive
    .method /*060001BD*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 literalLength,
                                 int32 formattedCount,
                                 valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                 [out] bool& shouldAppend) cil managed
    // SIG: 20 04 01 08 08 15 11 80 99 01 03 10 02
    {
      // Method begins at RVA 0x25b4
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::.ctor

    .method /*060001BE*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 literalLength,
                                 int32 formattedCount,
                                 valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                 class [System.Runtime/*23000001*/]System.IFormatProvider/*0100002E*/ provider,
                                 [out] bool& shouldAppend) cil managed
    // SIG: 20 05 01 08 08 15 11 80 99 01 03 12 80 B9 10 02
    {
      .param [4]/*0800038E*/ 
      .custom /*0C000365:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x25b7
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::.ctor

    .method /*060001BF*/ public hidebysig 
            instance bool  AppendLiteral(string 'value') cil managed
    // SIG: 20 01 02 0E
    {
      .custom /*0C00031D:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x25ba
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendLiteral

    .method /*060001C0*/ public hidebysig 
            instance bool  AppendFormatted(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value') cil managed
    // SIG: 20 01 02 15 11 80 95 01 03
    {
      .param [1]/*08000391*/ 
      .custom /*0C000366:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x25bd
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C1*/ public hidebysig 
            instance bool  AppendFormatted(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                                           [opt] int32 'alignment',
                                           [opt] string format) cil managed
    // SIG: 20 03 02 15 11 80 95 01 03 08 0E
    {
      .param [1]/*08000392*/ 
      .custom /*0C000367:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
      .param [2]/*08000393*/  = int32(0x00000000)
      .param [3]/*08000394*/  = nullref
      .custom /*0C000368:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x25c0
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C2*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value') cil managed
    // SIG: 30 01 01 02 1E 00
    {
      .custom /*0C000321:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
      .param type T /*2A0000A3*/ 
        .custom /*0C0001FD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x25c3
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C3*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value',
                                              string format) cil managed
    // SIG: 30 01 02 02 1E 00 0E
    {
      .custom /*0C000322:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
      .param [1]/*08000396*/ 
      .custom /*0C000369:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x25c6
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C4*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value',
                                              int32 'alignment') cil managed
    // SIG: 30 01 02 02 1E 00 08
    {
      .custom /*0C000323:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
      .param type T /*2A0000A5*/ 
        .custom /*0C000201:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x25c9
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C5*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value',
                                              int32 'alignment',
                                              string format) cil managed
    // SIG: 30 01 03 02 1E 00 08 0E
    {
      .custom /*0C000324:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
      .param [1]/*0800039A*/ 
      .custom /*0C00036A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x25cc
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C6*/ public hidebysig 
            instance bool  AppendFormatted(object 'value',
                                           [opt] int32 'alignment',
                                           [opt] string format) cil managed
    // SIG: 20 03 02 1C 08 0E
    {
      .custom /*0C000325:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
      .param [2]/*0800039E*/  = int32(0x00000000)
      .param [3]/*0800039F*/  = nullref
      // Method begins at RVA 0x25cf
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C7*/ public hidebysig 
            instance bool  AppendFormatted(string 'value') cil managed
    // SIG: 20 01 02 0E
    {
      .custom /*0C000327:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x25d2
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*060001C8*/ public hidebysig 
            instance bool  AppendFormatted(string 'value',
                                           [opt] int32 'alignment',
                                           [opt] string format) cil managed
    // SIG: 20 03 02 0E 08 0E
    {
      .custom /*0C000329:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
      .param [2]/*080003A2*/  = int32(0x00000000)
      .param [3]/*080003A3*/  = nullref
      // Method begins at RVA 0x25d5
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

  } // end of class TryWriteInterpolatedStringHandler

  .method /*0600000F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text) cil managed
  // SIG: 00 01 15 11 80 81 01 03 0E
  {
    .custom /*0C00005A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000005*/ 
    .custom /*0C000029:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000010*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 00 02 15 11 80 81 01 03 0E 11 80 85
  {
    .custom /*0C00005F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000006*/ 
    .custom /*0C00002D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000011*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   int32 start) cil managed
  // SIG: 00 02 15 11 80 81 01 03 0E 08
  {
    .custom /*0C000062:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000008*/ 
    .custom /*0C000038:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000012*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   int32 start,
                   int32 length) cil managed
  // SIG: 00 03 15 11 80 81 01 03 0E 08 08
  {
    .custom /*0C000066:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800000A*/ 
    .custom /*0C000041:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000013*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 00 02 15 11 80 81 01 03 0E 11 80 89
  {
    .custom /*0C00006A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800000D*/ 
    .custom /*0C000053:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000014*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment) cil managed
  // SIG: 10 01 01 15 11 80 8D 01 1E 00 15 11 80 91 01 1E 00
  {
    .custom /*0C00006E:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00006F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800000F*/ 
    .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000010*/ 
    .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000015*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                      int32 start) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 91 01 1E 00 08
  {
    .custom /*0C000072:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000073:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000011*/ 
    .custom /*0C000064:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000012*/ 
    .custom /*0C000068:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000016*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                      int32 start,
                      int32 length) cil managed
  // SIG: 10 01 03 15 11 80 8D 01 1E 00 15 11 80 91 01 1E 00 08 08
  {
    .custom /*0C000079:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000014*/ 
    .custom /*0C000070:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000015*/ 
    .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000017*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array') cil managed
  // SIG: 10 01 01 15 11 80 8D 01 1E 00 1D 1E 00
  {
    .custom /*0C00007D:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000018*/ 
    .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000019*/ 
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000018*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 1D 1E 00 11 80 85
  {
    .custom /*0C000083:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000084:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800001A*/ 
    .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800001B*/ 
    .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000019*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      int32 start) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 1D 1E 00 08
  {
    .custom /*0C000088:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000089:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800001D*/ 
    .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800001E*/ 
    .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600001A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      int32 start,
                      int32 length) cil managed
  // SIG: 10 01 03 15 11 80 8D 01 1E 00 1D 1E 00 08 08
  {
    .custom /*0C00008C:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000020*/ 
    .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000021*/ 
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600001B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 1D 1E 00 11 80 89
  {
    .custom /*0C000091:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000092:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000024*/ 
    .custom /*0C0000C0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000025*/ 
    .custom /*0C0000C4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600001C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text) cil managed
  // SIG: 00 01 15 11 80 95 01 03 0E
  {
    .custom /*0C000096:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000027*/ 
    .custom /*0C0000CB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text,
                 int32 start) cil managed
  // SIG: 00 02 15 11 80 95 01 03 0E 08
  {
    .custom /*0C00009E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000028*/ 
    .custom /*0C0000CF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text,
                 int32 start,
                 int32 length) cil managed
  // SIG: 00 03 15 11 80 95 01 03 0E 08 08
  {
    .custom /*0C0000A3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800002A*/ 
    .custom /*0C0000D6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment) cil managed
  // SIG: 10 01 01 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00
  {
    .custom /*0C0000A7:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800002D*/ 
    .custom /*0C0000E0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800002E*/ 
    .custom /*0C0000E4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000020*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 11 80 85
  {
    .custom /*0C0000AB:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000AC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800002F*/ 
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000030*/ 
    .custom /*0C0000ED:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000021*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    int32 start) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 08
  {
    .custom /*0C0000B0:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000032*/ 
    .custom /*0C0000F5:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000033*/ 
    .custom /*0C0000F9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000022*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    int32 start,
                    int32 length) cil managed
  // SIG: 10 01 03 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 08 08
  {
    .custom /*0C0000B5:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000035*/ 
    .custom /*0C0000FF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000036*/ 
    .custom /*0C000103:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000023*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 11 80 89
  {
    .custom /*0C0000BA:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000039*/ 
    .custom /*0C00010D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800003A*/ 
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000024*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array') cil managed
  // SIG: 10 01 01 15 11 80 99 01 1E 00 1D 1E 00
  {
    .custom /*0C0000BE:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800003C*/ 
    .custom /*0C000117:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800003D*/ 
    .custom /*0C00011A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000025*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 1D 1E 00 11 80 85
  {
    .custom /*0C0000C2:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800003E*/ 
    .custom /*0C00011D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800003F*/ 
    .custom /*0C000120:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000026*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    int32 start) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 1D 1E 00 08
  {
    .custom /*0C0000C6:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000041*/ 
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000042*/ 
    .custom /*0C000129:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000027*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    int32 start,
                    int32 length) cil managed
  // SIG: 10 01 03 15 11 80 99 01 1E 00 1D 1E 00 08 08
  {
    .custom /*0C0000C9:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000044*/ 
    .custom /*0C000131:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000045*/ 
    .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000028*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 1D 1E 00 11 80 89
  {
    .custom /*0C0000CD:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000048*/ 
    .custom /*0C000140:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000049*/ 
    .custom /*0C000145:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000029*/ public hidebysig static 
          int32  BinarySearch<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                 class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T> comparable) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 12 80 9D 01 1E 00
  {
    .custom /*0C0000D1:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001A*/ 
      .custom /*0C00008F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800004B*/ 
    .custom /*0C00014B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*0600002A*/ public hidebysig static 
          int32  BinarySearch<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                 class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T> comparable) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 12 80 9D 01 1E 00
  {
    .custom /*0C0000D4:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001B*/ 
      .custom /*0C000094:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800004D*/ 
    .custom /*0C000153:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*0600002B*/ public hidebysig static 
          int32  BinarySearch<T,(class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                                   !!T 'value',
                                                                                                                                                   !!TComparer comparer) cil managed
  // SIG: 10 02 03 08 15 11 80 95 01 1E 00 1E 00 1E 01
  {
    .custom /*0C0000D8:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001C*/ 
      .custom /*0C00009C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type TComparer /*2A00001D*/ 
      .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800004F*/ 
    .custom /*0C000159:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*0600002C*/ public hidebysig static 
          int32  BinarySearch<T,(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) TComparable>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                   !!TComparable comparable) cil managed
  // SIG: 10 02 02 08 15 11 80 95 01 1E 00 1E 01
  {
    .custom /*0C0000DB:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000DC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001E*/ 
      .custom /*0C0000A5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type TComparable /*2A00001F*/ 
      .custom /*0C0000A9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000052*/ 
    .custom /*0C00015D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*0600002D*/ public hidebysig static 
          int32  BinarySearch<T,(class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                                   !!T 'value',
                                                                                                                                                   !!TComparer comparer) cil managed
  // SIG: 10 02 03 08 15 11 80 99 01 1E 00 1E 00 1E 01
  {
    .custom /*0C0000DE:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000020*/ 
      .custom /*0C0000AE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type TComparer /*2A000021*/ 
      .custom /*0C0000B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000054*/ 
    .custom /*0C000160:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*0600002E*/ public hidebysig static 
          int32  BinarySearch<T,(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) TComparable>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                   !!TComparable comparable) cil managed
  // SIG: 10 02 02 08 15 11 80 99 01 1E 00 1E 01
  {
    .custom /*0C0000E2:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000022*/ 
      .custom /*0C0000B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type TComparable /*2A000023*/ 
      .custom /*0C0000BC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000057*/ 
    .custom /*0C000165:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*0600002F*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0000E6:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000059*/ 
    .custom /*0C000169:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800005A*/ 
    .custom /*0C00016D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*06000030*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000EB:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000EC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800005B*/ 
    .custom /*0C000171:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800005C*/ 
    .custom /*0C000173:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*0800005D*/ 
    .custom /*0C000176:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*06000031*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0000F0:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800005E*/ 
    .custom /*0C000179:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800005F*/ 
    .custom /*0C00017D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*06000032*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000F3:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000060*/ 
    .custom /*0C00017F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000061*/ 
    .custom /*0C000182:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000062*/ 
    .custom /*0C000185:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*06000033*/ public hidebysig static 
          int32  CompareTo(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                           valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> other,
                           valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C0000F8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CompareTo

  .method /*06000034*/ public hidebysig static 
          bool  Contains(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                         valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C0000FC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Contains

  .method /*06000035*/ public hidebysig static 
          bool  Contains<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0000FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000028*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000069*/ 
    .custom /*0C000191:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800006A*/ 
    .custom /*0C000194:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Contains

  .method /*06000036*/ public hidebysig static 
          bool  Contains<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C000102:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000029*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800006B*/ 
    .custom /*0C000197:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800006C*/ 
    .custom /*0C00019A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Contains

  .method /*06000037*/ public hidebysig static 
          void  CopyTo<T>(!!T[] source,
                          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> destination) cil managed
  // SIG: 10 01 02 01 1D 1E 00 15 11 80 8D 01 1E 00
  {
    .custom /*0C000106:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000107:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800006D*/ 
    .custom /*0C00019C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*0800006E*/ 
    .custom /*0C00019E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2146
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::CopyTo

  .method /*06000038*/ public hidebysig static 
          void  CopyTo<T>(!!T[] source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> destination) cil managed
  // SIG: 10 01 02 01 1D 1E 00 15 11 80 99 01 1E 00
  {
    .custom /*0C000109:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00010A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800006F*/ 
    .custom /*0C0001A0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*08000070*/ 
    .custom /*0C0001A2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2148
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::CopyTo

  .method /*06000039*/ public hidebysig static 
          bool  EndsWith(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                         valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C00010C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*0600003A*/ public hidebysig static 
          bool  EndsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000110:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000074*/ 
    .custom /*0C0001AB:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000075*/ 
    .custom /*0C0001AE:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*0600003B*/ public hidebysig static 
          bool  EndsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000114:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000076*/ 
    .custom /*0C0001B1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000077*/ 
    .custom /*0C0001B3:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*0600003C*/ public hidebysig static 
          valuetype System.Text.SpanLineEnumerator/*0200000C*/ 
          EnumerateLines(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 11 30 15 11 80 95 01 03
  {
    .custom /*0C000116:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateLines

  .method /*0600003D*/ public hidebysig static 
          valuetype System.Text.SpanLineEnumerator/*0200000C*/ 
          EnumerateLines(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 11 30 15 11 80 99 01 03
  {
    .custom /*0C000119:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateLines

  .method /*0600003E*/ public hidebysig static 
          valuetype System.Text.SpanRuneEnumerator/*0200000D*/ 
          EnumerateRunes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 11 34 15 11 80 95 01 03
  {
    .custom /*0C00011C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2159
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateRunes

  .method /*0600003F*/ public hidebysig static 
          valuetype System.Text.SpanRuneEnumerator/*0200000D*/ 
          EnumerateRunes(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 11 34 15 11 80 99 01 03
  {
    .custom /*0C00011F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateRunes

  .method /*06000040*/ public hidebysig static 
          bool  Equals(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> other,
                       valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C000122:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Equals

  .method /*06000041*/ public hidebysig static 
          int32  IndexOf(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                         valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C000124:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000042*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000128:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000048:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000082*/ 
    .custom /*0C0001C0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000083*/ 
    .custom /*0C0001C2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2165
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000043*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00012C:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00012D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002F*/ 
      .custom /*0C0000E9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00002F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000084*/ 
    .custom /*0C0001C4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2168
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000044*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1,
                                                                                                                    !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C00012F:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000130:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000030*/ 
      .custom /*0C0000EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000030*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000087*/ 
    .custom /*0C0001C9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000045*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000134:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000031*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800008B*/ 
    .custom /*0C0001CE:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800008C*/ 
    .custom /*0C0001D0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x216e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000046*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000138:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000139:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000032*/ 
      .custom /*0C0000F6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000032*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800008D*/ 
    .custom /*0C0001D2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000047*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1,
                                                                                                                    !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C00013B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00013C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000033*/ 
      .custom /*0C0000FA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000033*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000090*/ 
    .custom /*0C0001D6:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2174
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000048*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C00013F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000034*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000094*/ 
    .custom /*0C0001DF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000095*/ 
    .custom /*0C0001E2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000049*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000143:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000144:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000035*/ 
      .custom /*0C000100:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000035*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000096*/ 
    .custom /*0C0001E4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x217a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600004A*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1,
                                                                                                                          !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000147:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000148:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000036*/ 
      .custom /*0C000104:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000036*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000099*/ 
    .custom /*0C0001E9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600004B*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00014A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000037*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800009D*/ 
    .custom /*0C0001F0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800009E*/ 
    .custom /*0C0001F2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600004C*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C00014E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000038*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800009F*/ 
    .custom /*0C0001F4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000A0*/ 
    .custom /*0C0001F6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600004D*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000151:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000152:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000039*/ 
      .custom /*0C00010E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000039*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000A1*/ 
    .custom /*0C0001F8:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600004E*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1,
                                                                                                                          !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000155:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000156:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003A*/ 
      .custom /*0C000112:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00003A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000A4*/ 
    .custom /*0C0001FF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2189
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600004F*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000158:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00003B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000087:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000A8*/ 
    .custom /*0C000205:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000A9*/ 
    .custom /*0C000207:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000050*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00015A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00003C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000AA*/ 
    .custom /*0C000209:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000AB*/ 
    .custom /*0C00020B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000051*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C00015B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00003D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000AC*/ 
    .custom /*0C00020D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000AD*/ 
    .custom /*0C00020F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2192
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000052*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00015C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00003E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000AE*/ 
    .custom /*0C000211:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000AF*/ 
    .custom /*0C000213:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000053*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C00015E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00003F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00009D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000B0*/ 
    .custom /*0C000215:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000B1*/ 
    .custom /*0C000217:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2198
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000054*/ public hidebysig static 
          bool  IsWhiteSpace(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 02 15 11 80 95 01 03
  {
    .custom /*0C00015F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IsWhiteSpace

  .method /*06000055*/ public hidebysig static 
          int32  LastIndexOf(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                             valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                             valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C000161:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*06000056*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000162:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000040*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000B6*/ 
    .custom /*0C00021F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000B7*/ 
    .custom /*0C000221:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*06000057*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000163:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000164:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000041*/ 
      .custom /*0C000126:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000041*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000A6:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000B8*/ 
    .custom /*0C000222:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*06000058*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1,
                                                                                                                        !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000166:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000167:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000042*/ 
      .custom /*0C00012A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000042*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000BB*/ 
    .custom /*0C000223:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*06000059*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000168:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000043*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000AF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000BF*/ 
    .custom /*0C000224:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000C0*/ 
    .custom /*0C000225:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600005A*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00016B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00016C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000044*/ 
      .custom /*0C000132:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000044*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000B4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000C1*/ 
    .custom /*0C000227:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600005B*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1,
                                                                                                                        !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C00016F:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000170:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000045*/ 
      .custom /*0C000136:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000045*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000B9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000C4*/ 
    .custom /*0C000229:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600005C*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C000172:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000046*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000C8*/ 
    .custom /*0C00022A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000C9*/ 
    .custom /*0C00022C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*0600005D*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000174:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000175:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000047*/ 
      .custom /*0C00013D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000047*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000CA*/ 
    .custom /*0C00022E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*0600005E*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1,
                                                                                                                              !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000177:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000178:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000048*/ 
      .custom /*0C000141:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000048*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000CD*/ 
    .custom /*0C000232:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*0600005F*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00017C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000049*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D1*/ 
    .custom /*0C000237:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000D2*/ 
    .custom /*0C000238:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000060*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C00017E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000CC:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D3*/ 
    .custom /*0C00023A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000D4*/ 
    .custom /*0C00023C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000061*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000180:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000181:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004B*/ 
      .custom /*0C00014C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00004B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000D0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D5*/ 
    .custom /*0C00023E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000062*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1,
                                                                                                                              !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000183:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000184:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004C*/ 
      .custom /*0C00014F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00004C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000D3:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D8*/ 
    .custom /*0C000240:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000063*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000186:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000DC*/ 
    .custom /*0C000241:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000DD*/ 
    .custom /*0C000242:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000064*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000188:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000DA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000DE*/ 
    .custom /*0C000243:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000DF*/ 
    .custom /*0C000244:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*06000065*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                     !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000189:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000DD:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000E0*/ 
    .custom /*0C000245:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E1*/ 
    .custom /*0C000246:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*06000066*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00018A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000050*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000E1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*080000E2*/ 
    .custom /*0C000247:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E3*/ 
    .custom /*0C000248:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*06000067*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                     !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C00018B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000051*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000E5:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000E4*/ 
    .custom /*0C000249:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E5*/ 
    .custom /*0C00024A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*06000068*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00018D:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00018E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E6*/ 
    .custom /*0C00024D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E7*/ 
    .custom /*0C00024F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*06000069*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                            [out] int32& elementOffset) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 10 08
  {
    .custom /*0C00018F:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000190:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E8*/ 
    .custom /*0C000252:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E9*/ 
    .custom /*0C000255:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*0600006A*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000192:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000193:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000EB*/ 
    .custom /*0C000256:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000EC*/ 
    .custom /*0C000257:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*0600006B*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                            [out] int32& elementOffset) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00 10 08
  {
    .custom /*0C000195:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000196:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000ED*/ 
    .custom /*0C000258:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000EE*/ 
    .custom /*0C000259:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*0600006C*/ public hidebysig static 
          void  Reverse<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span) cil managed
  // SIG: 10 01 01 01 15 11 80 99 01 1E 00
  {
    .custom /*0C000198:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000199:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000F0*/ 
    .custom /*0C00025A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Reverse

  .method /*0600006D*/ public hidebysig static 
          int32  SequenceCompareTo<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00019B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000057*/ , class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/
      .custom /*0C0000EA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F1*/ 
    .custom /*0C00025B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F2*/ 
    .custom /*0C00025C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceCompareTo

  .method /*0600006E*/ public hidebysig static 
          int32  SequenceCompareTo<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00019D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000058*/ , class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/
      .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F3*/ 
    .custom /*0C00025D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F4*/ 
    .custom /*0C00025E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceCompareTo

  .method /*0600006F*/ public hidebysig static 
          bool  SequenceEqual<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00019F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000059*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000F2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F5*/ 
    .custom /*0C00025F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F6*/ 
    .custom /*0C000260:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*06000070*/ public hidebysig static 
          bool  SequenceEqual<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00005A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000F7:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F7*/ 
    .custom /*0C000261:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F8*/ 
    .custom /*0C000262:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*06000071*/ public hidebysig static 
          bool  SequenceEqual<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                 [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0001A3:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000F9*/ 
    .custom /*0C000263:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000FA*/ 
    .custom /*0C000264:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080000FB*/  = nullref
    .custom /*0C000265:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*06000072*/ public hidebysig static 
          bool  SequenceEqual<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                 [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0001A5:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001A6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000FC*/ 
    .custom /*0C000266:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000FD*/ 
    .custom /*0C000267:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080000FE*/  = nullref
    .custom /*0C000268:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*06000073*/ public hidebysig static 
          void  Sort<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span) cil managed
  // SIG: 10 01 01 01 15 11 80 99 01 1E 00
  {
    .custom /*0C0001A7:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001A8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000FF*/ 
    .custom /*0C000269:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*06000074*/ public hidebysig static 
          void  Sort<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                        class [System.Runtime/*23000001*/]System.Comparison`1/*0100002B*/<!!T> comparison) cil managed
  // SIG: 10 01 02 01 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001A9:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001AA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005E*/ 
      .custom /*0C00017A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000100*/ 
    .custom /*0C00026A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*06000075*/ public hidebysig static 
          void  Sort<TKey,TValue>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TKey> keys,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TValue> items) cil managed
  // SIG: 10 02 02 01 15 11 80 99 01 1E 00 15 11 80 99 01 1E 01
  {
    .custom /*0C0001AC:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001AD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000102*/ 
    .custom /*0C00026B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000103*/ 
    .custom /*0C00026C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*06000076*/ public hidebysig static 
          void  Sort<TKey,TValue>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TKey> keys,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TValue> items,
                                  class [System.Runtime/*23000001*/]System.Comparison`1/*0100002B*/<!!TKey> comparison) cil managed
  // SIG: 10 02 03 01 15 11 80 99 01 1E 00 15 11 80 99 01 1E 01 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001AF:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000104*/ 
    .custom /*0C00026D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000105*/ 
    .custom /*0C00026E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000106*/ 
    .custom /*0C00026F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*06000077*/ public hidebysig static 
          void  Sort<T,(class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                          !!TComparer comparer) cil managed
  // SIG: 10 02 02 01 15 11 80 99 01 1E 00 1E 01
  {
    .custom /*0C0001B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000063*/ 
      .custom /*0C000187:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param constraint   TComparer/*2A000064*/ , class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/
      .custom /*0C0000FB:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000107*/ 
    .custom /*0C000270:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000108*/ 
    .custom /*0C000271:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*06000078*/ public hidebysig static 
          void  Sort<TKey,TValue,(class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!TKey>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TKey> keys,
                                                                                                                                                       valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TValue> items,
                                                                                                                                                       !!TComparer comparer) cil managed
  // SIG: 10 03 03 01 15 11 80 99 01 1E 00 15 11 80 99 01 1E 01 1E 02
  {
    .custom /*0C0001B4:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TComparer /*2A000067*/ 
      .custom /*0C00018C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   TComparer/*2A000067*/ , class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!TKey>/*1B000002*/
      .custom /*0C0000FD:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000109*/ 
    .custom /*0C000272:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800010A*/ 
    .custom /*0C000273:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*0800010B*/ 
    .custom /*0C000274:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2201
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*06000079*/ public hidebysig static 
          bool  StartsWith(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                           valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                           valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C0001B6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*0600007A*/ public hidebysig static 
          bool  StartsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                   valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000068*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000101:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800010F*/ 
    .custom /*0C000275:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000110*/ 
    .custom /*0C000276:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*0600007B*/ public hidebysig static 
          bool  StartsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                   valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001B8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000069*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000105:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000111*/ 
    .custom /*0C000277:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000112*/ 
    .custom /*0C000278:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*0600007C*/ public hidebysig static 
          int32  ToLower(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                         valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100002C*/ culture) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 99 01 03 12 80 B1
  {
    .custom /*0C0001B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000115*/ 
    .custom /*0C00027B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToLower

  .method /*0600007D*/ public hidebysig static 
          int32  ToLowerInvariant(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination) cil managed
  // SIG: 00 02 08 15 11 80 95 01 03 15 11 80 99 01 03
  {
    .custom /*0C0001BA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToLowerInvariant

  .method /*0600007E*/ public hidebysig static 
          int32  ToUpper(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                         valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100002C*/ culture) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 99 01 03 12 80 B1
  {
    .custom /*0C0001BB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800011A*/ 
    .custom /*0C000280:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToUpper

  .method /*0600007F*/ public hidebysig static 
          int32  ToUpperInvariant(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination) cil managed
  // SIG: 00 02 08 15 11 80 95 01 03 15 11 80 99 01 03
  {
    .custom /*0C0001BC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToUpperInvariant

  .method /*06000080*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 8D 01 03 15 11 80 8D 01 03
  {
    .custom /*0C0001BD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*06000081*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 81 01 03 15 11 80 81 01 03
  {
    .custom /*0C0001BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*06000082*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C0001BF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*06000083*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
               char trimChar) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 03
  {
    .custom /*0C0001C1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*06000084*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> trimChars) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C0001C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*06000085*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 15 11 80 99 01 03 15 11 80 99 01 03
  {
    .custom /*0C0001C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*06000086*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 8D 01 03 15 11 80 8D 01 03
  {
    .custom /*0C0001C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000087*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 81 01 03 15 11 80 81 01 03
  {
    .custom /*0C0001C8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000088*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C0001CA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000089*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                  char trimChar) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 03
  {
    .custom /*0C0001CB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*0600008A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> trimChars) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C0001CC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*0600008B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 15 11 80 99 01 03 15 11 80 99 01 03
  {
    .custom /*0C0001CD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*0600008C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001CF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000108:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800012D*/ 
    .custom /*0C00028B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800012E*/ 
    .custom /*0C00028C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800012F*/ 
    .custom /*0C00028D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*0600008D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 1E 00
  {
    .custom /*0C0001D1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00010B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000130*/ 
    .custom /*0C00028E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000131*/ 
    .custom /*0C000290:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000132*/ 
    .custom /*0C000292:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*0600008E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001D3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00010F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000133*/ 
    .custom /*0C000294:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000134*/ 
    .custom /*0C000295:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000135*/ 
    .custom /*0C000296:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*0600008F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 1E 00
  {
    .custom /*0C0001D4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000113:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000136*/ 
    .custom /*0C000297:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000137*/ 
    .custom /*0C000298:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000138*/ 
    .custom /*0C000299:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000090*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001D5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000115:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000139*/ 
    .custom /*0C00029A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800013A*/ 
    .custom /*0C00029B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800013B*/ 
    .custom /*0C00029C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000091*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0001D8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800013C*/ 
    .custom /*0C00029D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800013D*/ 
    .custom /*0C00029E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800013E*/ 
    .custom /*0C00029F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000092*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001DA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000070*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00011B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800013F*/ 
    .custom /*0C0002A0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000140*/ 
    .custom /*0C0002A1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000141*/ 
    .custom /*0C0002A2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000093*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C0001DC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000071*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00011E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000142*/ 
    .custom /*0C0002A3:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000143*/ 
    .custom /*0C0002A4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000144*/ 
    .custom /*0C0002A6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*06000094*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 8D 01 03 15 11 80 8D 01 03
  {
    .custom /*0C0001DE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*06000095*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 81 01 03 15 11 80 81 01 03
  {
    .custom /*0C0001E1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*06000096*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C0001E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*06000097*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                    char trimChar) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 03
  {
    .custom /*0C0001E6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*06000098*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> trimChars) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C0001E7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*06000099*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 15 11 80 99 01 03 15 11 80 99 01 03
  {
    .custom /*0C0001E8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*0600009A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001EB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000072*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000121:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800014D*/ 
    .custom /*0C0002A9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800014E*/ 
    .custom /*0C0002AA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800014F*/ 
    .custom /*0C0002AB:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*0600009B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 1E 00
  {
    .custom /*0C0001EC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000073*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000123:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000150*/ 
    .custom /*0C0002AC:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000151*/ 
    .custom /*0C0002AD:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000152*/ 
    .custom /*0C0002AE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*0600009C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001EE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000074*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000127:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000153*/ 
    .custom /*0C0002AF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000154*/ 
    .custom /*0C0002B0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000155*/ 
    .custom /*0C0002B1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*0600009D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 1E 00
  {
    .custom /*0C0001EF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000075*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00012B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000156*/ 
    .custom /*0C0002B2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000157*/ 
    .custom /*0C0002B4:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000158*/ 
    .custom /*0C0002B5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*0600009E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001F1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000076*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00012E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000159*/ 
    .custom /*0C0002B6:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800015A*/ 
    .custom /*0C0002B8:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800015B*/ 
    .custom /*0C0002BA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*0600009F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0001F3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000077*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000133:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800015C*/ 
    .custom /*0C0002BC:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800015D*/ 
    .custom /*0C0002BD:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800015E*/ 
    .custom /*0C0002BE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000A0*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001F5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000078*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000137:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800015F*/ 
    .custom /*0C0002BF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000160*/ 
    .custom /*0C0002C0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000161*/ 
    .custom /*0C0002C1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000A1*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C0001F7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000079*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00013A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000162*/ 
    .custom /*0C0002C2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000163*/ 
    .custom /*0C0002C3:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000164*/ 
    .custom /*0C0002C4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000A2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001FA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00013E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000165*/ 
    .custom /*0C0002C5:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000166*/ 
    .custom /*0C0002C6:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000167*/ 
    .custom /*0C0002C8:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 1E 00
  {
    .custom /*0C0001FC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000142:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000168*/ 
    .custom /*0C0002C9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000169*/ 
    .custom /*0C0002CA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800016A*/ 
    .custom /*0C0002CC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000146:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800016B*/ 
    .custom /*0C0002CE:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800016C*/ 
    .custom /*0C0002D0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800016D*/ 
    .custom /*0C0002D1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A5*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 1E 00
  {
    .custom /*0C000200:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000149:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800016E*/ 
    .custom /*0C0002D2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800016F*/ 
    .custom /*0C0002D3:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000170*/ 
    .custom /*0C0002D4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A6*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000202:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00014D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000171*/ 
    .custom /*0C0002D5:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000172*/ 
    .custom /*0C0002D6:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000173*/ 
    .custom /*0C0002D7:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000203:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000150:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000174*/ 
    .custom /*0C0002D8:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000175*/ 
    .custom /*0C0002D9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000176*/ 
    .custom /*0C0002DA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A8*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000204:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000080*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000154:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000177*/ 
    .custom /*0C0002DB:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000178*/ 
    .custom /*0C0002DC:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000179*/ 
    .custom /*0C0002DE:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000A9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C000206:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000081*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000157:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800017A*/ 
    .custom /*0C0002E0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800017B*/ 
    .custom /*0C0002E2:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800017C*/ 
    .custom /*0C0002E4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000AA*/ public hidebysig static 
          bool  TryWrite(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         valuetype System.MemoryExtensions/*0200000A*//TryWriteInterpolatedStringHandler/*0200001C*/& 'handler',
                         [out] int32& charsWritten) cil managed
  // SIG: 00 03 02 15 11 80 99 01 03 10 11 70 10 08
  {
    .custom /*0C000208:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800017E*/ 
    .custom /*0C0002E7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute/*0100002D*/::.ctor(string) /* 0A000018 */ = ( 01 00 0B 64 65 73 74 69 6E 61 74 69 6F 6E 00 00 ) // ...destination..
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000AB*/ public hidebysig static 
          bool  TryWrite(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.IFormatProvider/*0100002E*/ provider,
                         valuetype System.MemoryExtensions/*0200000A*//TryWriteInterpolatedStringHandler/*0200001C*/& 'handler',
                         [out] int32& charsWritten) cil managed
  // SIG: 00 04 02 15 11 80 99 01 03 12 80 B9 10 11 70 10 08
  {
    .custom /*0C00020A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000181*/ 
    .custom /*0C0002E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000182*/ 
    .custom /*0C0002E9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute/*0100002D*/::.ctor(string[]) /* 0A000019 */ = ( 01 00 02 00 00 00 0B 64 65 73 74 69 6E 61 74 69   // .......destinati
                                                                                                                                                                                                       6F 6E 08 70 72 6F 76 69 64 65 72 00 00 )          // on.provider..
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

} // end of class System.MemoryExtensions

.class /*0200000B*/ public abstract auto ansi sealed beforefieldinit System.Text.EncodingExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000044:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000045:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000046:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000AC*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Decoder/*01000033*/ decoder,
                        [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<uint8>& bytes,
                        class System.Buffers.IBufferWriter`1/*02000012*/<char> writer,
                        bool flush,
                        [out] int64& charsUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 CD 10 15 11 54 01 05 15 12 48 01 03 02 10 0A 10 02
  {
    .custom /*0C00020C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000185*/ 
    .custom /*0C0002ED:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C0002EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000AD*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Decoder/*01000033*/ decoder,
                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> bytes,
                        class System.Buffers.IBufferWriter`1/*02000012*/<char> writer,
                        bool flush,
                        [out] int64& charsUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 CD 15 11 80 95 01 05 15 12 48 01 03 02 10 0A 10 02
  {
    .custom /*0C00020E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800018B*/ 
    .custom /*0C0002F0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000AE*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Encoder/*01000034*/ encoder,
                        [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<char>& chars,
                        class System.Buffers.IBufferWriter`1/*02000012*/<uint8> writer,
                        bool flush,
                        [out] int64& bytesUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 D1 10 15 11 54 01 03 15 12 48 01 05 02 10 0A 10 02
  {
    .custom /*0C000210:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000191*/ 
    .custom /*0C0002F1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C0002F2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000AF*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Encoder/*01000034*/ encoder,
                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> chars,
                        class System.Buffers.IBufferWriter`1/*02000012*/<uint8> writer,
                        bool flush,
                        [out] int64& bytesUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 D1 15 11 80 95 01 03 15 12 48 01 05 02 10 0A 10 02
  {
    .custom /*0C000212:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000197*/ 
    .custom /*0C0002F8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000B0*/ public hidebysig static 
          uint8[]  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                            [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<char>& chars) cil managed
  // SIG: 00 02 1D 05 12 80 D5 10 15 11 54 01 03
  {
    .custom /*0C000214:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800019D*/ 
    .custom /*0C0002FA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C0002FB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000B1*/ public hidebysig static 
          int64  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<char>& chars,
                          class System.Buffers.IBufferWriter`1/*02000012*/<uint8> writer) cil managed
  // SIG: 00 03 0A 12 80 D5 10 15 11 54 01 03 15 12 48 01 05
  {
    .custom /*0C000216:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800019F*/ 
    .custom /*0C0002FC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C0002FD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000B2*/ public hidebysig static 
          int32  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<char>& chars,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> bytes) cil managed
  // SIG: 00 03 08 12 80 D5 10 15 11 54 01 03 15 11 80 99 01 05
  {
    .custom /*0C000218:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000219:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080001A1*/ 
    .custom /*0C0002FE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001A2*/ 
    .custom /*0C0002FF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000B3*/ public hidebysig static 
          int64  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> chars,
                          class System.Buffers.IBufferWriter`1/*02000012*/<uint8> writer) cil managed
  // SIG: 00 03 0A 12 80 D5 15 11 80 95 01 03 15 12 48 01 05
  {
    .custom /*0C00021A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*080001A5*/ 
    .custom /*0C000301:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000B4*/ public hidebysig static 
          int64  GetChars(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<uint8>& bytes,
                          class System.Buffers.IBufferWriter`1/*02000012*/<char> writer) cil managed
  // SIG: 00 03 0A 12 80 D5 10 15 11 54 01 05 15 12 48 01 03
  {
    .custom /*0C00021B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*080001A8*/ 
    .custom /*0C000305:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C000306:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetChars

  .method /*060000B5*/ public hidebysig static 
          int32  GetChars(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<uint8>& bytes,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> chars) cil managed
  // SIG: 00 03 08 12 80 D5 10 15 11 54 01 05 15 11 80 99 01 03
  {
    .custom /*0C00021C:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00021D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080001AA*/ 
    .custom /*0C000309:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001AB*/ 
    .custom /*0C00030A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetChars

  .method /*060000B6*/ public hidebysig static 
          int64  GetChars(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> bytes,
                          class System.Buffers.IBufferWriter`1/*02000012*/<char> writer) cil managed
  // SIG: 00 03 0A 12 80 D5 15 11 80 95 01 05 15 12 48 01 03
  {
    .custom /*0C00021E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*080001AE*/ 
    .custom /*0C00030B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetChars

  .method /*060000B7*/ public hidebysig static 
          string  GetString(class [System.Runtime/*23000001*/]System.Text.Encoding/*01000035*/ encoding,
                            [in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<uint8>& bytes) cil managed
  // SIG: 00 02 0E 12 80 D5 10 15 11 54 01 05
  {
    .custom /*0C000220:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*080001B1*/ 
    .custom /*0C00030C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C00030D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetString

} // end of class System.Text.EncodingExtensions

.class /*0200000C*/ public sequential ansi sealed beforefieldinit System.Text.SpanLineEnumerator
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C00004A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*0100002F*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .custom /*0C00004B:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000030*/::.ctor(string,
                                                                                                                      bool) /* 0A00001B */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C00004C:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000031*/::.ctor(string) /* 0A00001C */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
  .field /*04000007*/ private object _dummy
  .field /*04000008*/ private int32 _dummyPrimitive
  .method /*060000B8*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          get_Current() cil managed
  // SIG: 20 00 15 11 80 95 01 03
  {
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanLineEnumerator::get_Current

  .method /*060000B9*/ public hidebysig instance valuetype System.Text.SpanLineEnumerator/*0200000C*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanLineEnumerator::GetEnumerator

  .method /*060000BA*/ public hidebysig instance bool 
          MoveNext() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanLineEnumerator::MoveNext

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char>
          Current()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> System.Text.SpanLineEnumerator/*0200000C*/::get_Current() /* 060000B8 */
  } // end of property SpanLineEnumerator::Current
} // end of class System.Text.SpanLineEnumerator

.class /*0200000D*/ public sequential ansi sealed beforefieldinit System.Text.SpanRuneEnumerator
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C000050:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*0100002F*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .custom /*0C000051:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000030*/::.ctor(string,
                                                                                                                      bool) /* 0A00001B */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C000052:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000031*/::.ctor(string) /* 0A00001C */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
  .field /*04000009*/ private object _dummy
  .field /*0400000A*/ private int32 _dummyPrimitive
  .method /*060000BB*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Text.Rune/*01000036*/ 
          get_Current() cil managed
  // SIG: 20 00 11 80 D9
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanRuneEnumerator::get_Current

  .method /*060000BC*/ public hidebysig instance valuetype System.Text.SpanRuneEnumerator/*0200000D*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 34
  {
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanRuneEnumerator::GetEnumerator

  .method /*060000BD*/ public hidebysig instance bool 
          MoveNext() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanRuneEnumerator::MoveNext

  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Text.Rune/*01000036*/
          Current()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Text.Rune/*01000036*/ System.Text.SpanRuneEnumerator/*0200000D*/::get_Current() /* 060000BB */
  } // end of property SpanRuneEnumerator::Current
} // end of class System.Text.SpanRuneEnumerator

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.SequenceMarshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000056:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000057:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .method /*060000BE*/ public hidebysig static 
          bool  TryGetArray<T>(valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!!T> sequence,
                               [out] valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T>& segment) cil managed
  // SIG: 10 01 02 02 15 11 54 01 1E 00 10 15 11 80 91 01 1E 00
  {
    .param [1]/*080001B2*/ 
    .custom /*0C00030E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001B3*/ 
    .custom /*0C00030F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryGetArray

  .method /*060000BF*/ public hidebysig static 
          bool  TryGetReadOnlyMemory<T>(valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!!T> sequence,
                                        [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T>& memory) cil managed
  // SIG: 10 01 02 02 15 11 54 01 1E 00 10 15 11 80 81 01 1E 00
  {
    .param [1]/*080001B4*/ 
    .custom /*0C000310:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001B5*/ 
    .custom /*0C000311:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryGetReadOnlyMemory

  .method /*060000C0*/ public hidebysig static 
          bool  TryGetReadOnlySequenceSegment<T>(valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!!T> sequence,
                                                 [out] class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!!T>& startSegment,
                                                 [out] int32& startIndex,
                                                 [out] class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!!T>& endSegment,
                                                 [out] int32& endIndex) cil managed
  // SIG: 10 01 05 02 15 11 54 01 1E 00 10 15 12 50 01 1E 00 10 08 10 15 12 50 01 1E 00 10 08
  {
    .param [1]/*080001B6*/ 
    .custom /*0C000312:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001B7*/ 
    .custom /*0C000314:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C000315:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [4]/*080001B9*/ 
    .custom /*0C000318:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C000319:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryGetReadOnlySequenceSegment

  .method /*060000C1*/ public hidebysig static 
          bool  TryRead<valuetype .ctor (class [System.Runtime/*23000001*/]System.ValueType/*0100001A*/ modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.UnmanagedType/*01000037*/)/*1B000005*/) T>(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                                                                                                                                                                                                     [out] !!T& 'value') cil managed
  // SIG: 10 01 02 02 10 15 11 5C 01 05 10 1E 00
  {
    .custom /*0C000226:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000085*/ 
      .custom /*0C0001C6:06000002*/ instance void System.Runtime.CompilerServices.IsUnmanagedAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryRead

} // end of class System.Runtime.InteropServices.SequenceMarshal

.class /*0200000F*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.MemoryMarshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*060000C2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> 
          AsBytes<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span) cil managed
  // SIG: 10 01 01 15 11 80 95 01 05 15 11 80 95 01 1E 00
  {
    // Method begins at RVA 0x22de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::AsBytes

  .method /*060000C3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> 
          AsBytes<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span) cil managed
  // SIG: 10 01 01 15 11 80 99 01 05 15 11 80 99 01 1E 00
  {
    // Method begins at RVA 0x22e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::AsBytes

  .method /*060000C4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory) cil managed
  // SIG: 10 01 01 15 11 80 8D 01 1E 00 15 11 80 81 01 1E 00
  {
    .custom /*0C000228:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001BF*/ 
    .custom /*0C00031E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001C0*/ 
    .custom /*0C00031F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::AsMemory

  .method /*060000C5*/ public hidebysig static 
          !!T& modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.InAttribute/*01000038*/) 
          AsRef<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> span) cil managed
  // SIG: 10 01 01 1F 80 E1 10 1E 00 15 11 80 95 01 05
  {
    .param [0]/*080001C1*/ 
    .custom /*0C000320:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::AsRef

  .method /*060000C6*/ public hidebysig static 
          !!T&  AsRef<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> span) cil managed
  // SIG: 10 01 01 10 1E 00 15 11 80 99 01 05
  {
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::AsRef

  .method /*060000C7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!TTo> 
          Cast<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) TFrom,valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) TTo>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!TFrom> span) cil managed
  // SIG: 10 02 01 15 11 80 95 01 1E 01 15 11 80 95 01 1E 00
  {
    // Method begins at RVA 0x22ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::Cast

  .method /*060000C8*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TTo> 
          Cast<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) TFrom,valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) TTo>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TFrom> span) cil managed
  // SIG: 10 02 01 15 11 80 99 01 1E 01 15 11 80 99 01 1E 00
  {
    // Method begins at RVA 0x22f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::Cast

  .method /*060000C9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          CreateFromPinnedArray<T>(!!T[] 'array',
                                   int32 start,
                                   int32 length) cil managed
  // SIG: 10 01 03 15 11 80 8D 01 1E 00 1D 1E 00 08 08
  {
    .custom /*0C00022B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001C6*/ 
    .custom /*0C000326:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001C7*/ 
    .custom /*0C000328:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::CreateFromPinnedArray

  .method /*060000CA*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          CreateReadOnlySpan<T>(!!T& reference,
                                int32 length) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 10 1E 00 08
  {
    .custom /*0C00022D:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000090*/ 
      .custom /*0C0001D7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001CA*/ 
    .custom /*0C00032A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001CB*/ 
    .custom /*0C00032B:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::CreateReadOnlySpan

  .method /*060000CB*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> 
          CreateReadOnlySpanFromNullTerminated(uint8* 'value') cil managed
  // SIG: 00 01 15 11 80 95 01 05 0F 05
  {
    .custom /*0C00022F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::CreateReadOnlySpanFromNullTerminated

  .method /*060000CC*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          CreateReadOnlySpanFromNullTerminated(char* 'value') cil managed
  // SIG: 00 01 15 11 80 95 01 03 0F 03
  {
    .custom /*0C000230:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::CreateReadOnlySpanFromNullTerminated

  .method /*060000CD*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          CreateSpan<T>(!!T& reference,
                        int32 length) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 10 1E 00 08
  {
    .custom /*0C000231:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000091*/ 
      .custom /*0C0001D9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001CF*/ 
    .custom /*0C00032C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001D0*/ 
    .custom /*0C00032D:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::CreateSpan

  .method /*060000CE*/ public hidebysig static 
          !!T&  GetArrayDataReference<T>(!!T[] 'array') cil managed
  // SIG: 10 01 01 10 1E 00 1D 1E 00
  {
    .custom /*0C000233:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000092*/ 
      .custom /*0C0001DB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::GetArrayDataReference

  .method /*060000CF*/ public hidebysig static 
          uint8&  GetArrayDataReference(class [System.Runtime/*23000001*/]System.Array/*01000039*/ 'array') cil managed
  // SIG: 00 01 10 05 12 80 E5
  {
    .custom /*0C000234:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::GetArrayDataReference

  .method /*060000D0*/ public hidebysig static 
          !!T&  GetReference<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span) cil managed
  // SIG: 10 01 01 10 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000235:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000093*/ 
      .custom /*0C0001DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001D4*/ 
    .custom /*0C00032E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::GetReference

  .method /*060000D1*/ public hidebysig static 
          !!T&  GetReference<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span) cil managed
  // SIG: 10 01 01 10 1E 00 15 11 80 99 01 1E 00
  {
    .custom /*0C000236:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000094*/ 
      .custom /*0C0001E0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001D5*/ 
    .custom /*0C00032F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x230b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::GetReference

  .method /*060000D2*/ public hidebysig static 
          !!T  Read<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 10 01 01 1E 00 15 11 80 95 01 05
  {
    // Method begins at RVA 0x230e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::Read

  .method /*060000D3*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100003A*/<!!T> 
          ToEnumerable<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory) cil managed
  // SIG: 10 01 01 15 12 80 E9 01 1E 00 15 11 80 81 01 1E 00
  {
    .custom /*0C000239:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000096*/ 
      .custom /*0C0001E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001D7*/ 
    .custom /*0C000330:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::ToEnumerable

  .method /*060000D4*/ public hidebysig static 
          bool  TryGetArray<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                               [out] valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T>& segment) cil managed
  // SIG: 10 01 02 02 15 11 80 81 01 1E 00 10 15 11 80 91 01 1E 00
  {
    .custom /*0C00023B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001D8*/ 
    .custom /*0C000331:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001D9*/ 
    .custom /*0C000332:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::TryGetArray

  .method /*060000D5*/ public hidebysig static 
          bool  TryGetMemoryManager<T,(class [System.Runtime/*23000001*/]System.Buffers.MemoryManager`1/*0100003B*/<!!T>/*1B000006*/) TManager>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                                                                [out] !!TManager& manager) cil managed
  // SIG: 10 02 02 02 15 11 80 81 01 1E 00 10 1E 01
  {
    .custom /*0C00023D:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param type TManager /*2A000099*/ 
      .custom /*0C0001EA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   TManager/*2A000099*/ , class [System.Runtime/*23000001*/]System.Buffers.MemoryManager`1/*0100003B*/<!!T>/*1B000006*/
      .custom /*0C00016A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080001DA*/ 
    .custom /*0C000333:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001DB*/ 
    .custom /*0C000334:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2317
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::TryGetMemoryManager

  .method /*060000D6*/ public hidebysig static 
          bool  TryGetMemoryManager<T,(class [System.Runtime/*23000001*/]System.Buffers.MemoryManager`1/*0100003B*/<!!T>/*1B000006*/) TManager>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                                                                [out] !!TManager& manager,
                                                                                                                                                [out] int32& start,
                                                                                                                                                [out] int32& length) cil managed
  // SIG: 10 02 04 02 15 11 80 81 01 1E 00 10 1E 01 10 08 10 08
  {
    .custom /*0C00023F:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param type TManager /*2A00009B*/ 
      .custom /*0C0001ED:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
    .param constraint   TManager/*2A00009B*/ , class [System.Runtime/*23000001*/]System.Buffers.MemoryManager`1/*0100003B*/<!!T>/*1B000006*/
      .custom /*0C00016E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080001DC*/ 
    .custom /*0C000335:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001DD*/ 
    .custom /*0C000336:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x231a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::TryGetMemoryManager

  .method /*060000D7*/ public hidebysig static 
          bool  TryGetString(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory,
                             [out] string& text,
                             [out] int32& start,
                             [out] int32& length) cil managed
  // SIG: 00 04 02 15 11 80 81 01 03 10 0E 10 08 10 08
  {
    .param [2]/*080001E1*/ 
    .custom /*0C000337:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000338:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x231d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::TryGetString

  .method /*060000D8*/ public hidebysig static 
          bool  TryRead<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                                                                                      [out] !!T& 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 05 10 1E 00
  {
    // Method begins at RVA 0x2320
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::TryRead

  .method /*060000D9*/ public hidebysig static 
          bool  TryWrite<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                                                                                       !!T& 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 05 10 1E 00
  {
    // Method begins at RVA 0x2323
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryMarshal::TryWrite

  .method /*060000DA*/ public hidebysig static 
          void  Write<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*0100001A*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                                                                                    !!T& 'value') cil managed
  // SIG: 10 01 02 01 15 11 80 99 01 05 10 1E 00
  {
    // Method begins at RVA 0x2326
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryMarshal::Write

} // end of class System.Runtime.InteropServices.MemoryMarshal

.class /*02000010*/ public auto ansi sealed beforefieldinit System.Buffers.ArrayBufferWriter`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
       implements class System.Buffers.IBufferWriter`1/*02000012*/<!T>/*1B000007*/
{
  .interfaceimpl type class System.Buffers.IBufferWriter`1/*02000012*/<!T>/*1B000007*/
  .custom /*0C00001B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000001*/ 
    .custom /*0C000018:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
  .method /*060000DB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2328
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::.ctor

  .method /*060000DC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCapacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2330
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::.ctor

  .method /*060000DD*/ public hidebysig specialname 
          instance int32  get_Capacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_Capacity

  .method /*060000DE*/ public hidebysig specialname 
          instance int32  get_FreeCapacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x233b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_FreeCapacity

  .method /*060000DF*/ public hidebysig specialname 
          instance int32  get_WrittenCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x233e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_WrittenCount

  .method /*060000E0*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
          get_WrittenMemory() cil managed
  // SIG: 20 00 15 11 80 81 01 13 00
  {
    .param [0]/*080001EB*/ 
    .custom /*0C000339:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2341
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_WrittenMemory

  .method /*060000E1*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_WrittenSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .param [0]/*080001EC*/ 
    .custom /*0C00033A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2344
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_WrittenSpan

  .method /*060000E2*/ public hidebysig newslot virtual final 
          instance void  Advance(int32 count) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2347
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::Advance

  .method /*060000E3*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2349
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::Clear

  .method /*060000E4*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!T> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 8D 01 13 00 08
  {
    .param [0]/*080001EE*/ 
    .custom /*0C00033B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001EF*/  = int32(0x00000000)
    // Method begins at RVA 0x234b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::GetMemory

  .method /*060000E5*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!T> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 99 01 13 00 08
  {
    .param [0]/*080001F0*/ 
    .custom /*0C00033C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001F1*/  = int32(0x00000000)
    // Method begins at RVA 0x234e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::GetSpan

  .property /*17000003*/ instance callconv(8) int32
          Capacity()
  {
    .get instance int32 System.Buffers.ArrayBufferWriter`1/*02000010*/::get_Capacity() /* 060000DD */
  } // end of property ArrayBufferWriter`1::Capacity
  .property /*17000004*/ instance callconv(8) int32
          FreeCapacity()
  {
    .get instance int32 System.Buffers.ArrayBufferWriter`1/*02000010*/::get_FreeCapacity() /* 060000DE */
  } // end of property ArrayBufferWriter`1::FreeCapacity
  .property /*17000005*/ instance callconv(8) int32
          WrittenCount()
  {
    .get instance int32 System.Buffers.ArrayBufferWriter`1/*02000010*/::get_WrittenCount() /* 060000DF */
  } // end of property ArrayBufferWriter`1::WrittenCount
  .property /*17000006*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
          WrittenMemory()
  {
    .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ArrayBufferWriter`1/*02000010*/::get_WrittenMemory() /* 060000E0 */
  } // end of property ArrayBufferWriter`1::WrittenMemory
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          WrittenSpan()
  {
    .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.ArrayBufferWriter`1/*02000010*/::get_WrittenSpan() /* 060000E1 */
  } // end of property ArrayBufferWriter`1::WrittenSpan
} // end of class System.Buffers.ArrayBufferWriter`1

.class /*02000011*/ public abstract auto ansi sealed beforefieldinit System.Buffers.BuffersExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000063:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000E6*/ public hidebysig static 
          void  CopyTo<T>([in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!!T>& source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> destination) cil managed
  // SIG: 10 01 02 01 10 15 11 54 01 1E 00 15 11 80 99 01 1E 00
  {
    .custom /*0C00024B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00024C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080001F2*/ 
    .custom /*0C00033D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C00033E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001F3*/ 
    .custom /*0C00033F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2351
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BuffersExtensions::CopyTo

  .method /*060000E7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003C*/<valuetype System.SequencePosition/*02000009*/> 
          PositionOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>([in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!!T>& source,
                                                                                                             !!T 'value') cil managed
  // SIG: 10 01 02 15 11 80 F1 01 11 24 10 15 11 54 01 1E 00 1E 00
  {
    .custom /*0C00024E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A0*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00017B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080001F4*/ 
    .custom /*0C000340:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C000341:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001F5*/ 
    .custom /*0C000342:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2353
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BuffersExtensions::PositionOf

  .method /*060000E8*/ public hidebysig static 
          !!T[]  ToArray<T>([in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!!T>& sequence) cil managed
  // SIG: 10 01 01 1D 1E 00 10 15 11 54 01 1E 00
  {
    .custom /*0C000250:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000251:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A0000A1*/ 
      .custom /*0C0001F9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001F6*/ 
    .custom /*0C000343:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .custom /*0C000344:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2356
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BuffersExtensions::ToArray

  .method /*060000E9*/ public hidebysig static 
          void  Write<T>(class System.Buffers.IBufferWriter`1/*02000012*/<!!T> writer,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 01 15 12 48 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000253:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000254:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A0000A2*/ 
      .custom /*0C0001FB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001F8*/ 
    .custom /*0C000345:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2359
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BuffersExtensions::Write

} // end of class System.Buffers.BuffersExtensions

.class /*02000012*/ interface public abstract auto ansi System.Buffers.IBufferWriter`1<T>
{
  .custom /*0C000067:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
  .method /*060000EA*/ public hidebysig newslot abstract virtual 
          instance void  Advance(int32 count) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IBufferWriter`1::Advance

  .method /*060000EB*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!T> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 8D 01 13 00 08
  {
    .param [0]/*080001FA*/ 
    .custom /*0C000346:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001FB*/  = int32(0x00000000)
    // Method begins at RVA 0x0
  } // end of method IBufferWriter`1::GetMemory

  .method /*060000EC*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!T> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 99 01 13 00 08
  {
    .param [0]/*080001FC*/ 
    .custom /*0C000347:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001FD*/  = int32(0x00000000)
    // Method begins at RVA 0x0
  } // end of method IBufferWriter`1::GetSpan

} // end of class System.Buffers.IBufferWriter`1

.class /*02000013*/ public abstract auto ansi beforefieldinit System.Buffers.MemoryPool`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100003D*/
{
  .custom /*0C00006B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100003D*/
  .custom /*0C000020:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000003*/ 
    .custom /*0C000021:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
  .method /*060000ED*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x235b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MemoryPool`1::.ctor

  .method /*060000EE*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_MaxBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method MemoryPool`1::get_MaxBufferSize

  .method /*060000EF*/ public hidebysig specialname static 
          class System.Buffers.MemoryPool`1/*02000013*/<!T> 
          get_Shared() cil managed
  // SIG: 00 00 15 12 4C 01 13 00
  {
    // Method begins at RVA 0x2363
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPool`1::get_Shared

  .method /*060000F0*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2366
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryPool`1::Dispose

  .method /*060000F1*/ family hidebysig newslot abstract virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method MemoryPool`1::Dispose

  .method /*060000F2*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Buffers.IMemoryOwner`1/*0100003E*/<!T> 
          Rent([opt] int32 minBufferSize) cil managed
  // SIG: 20 01 15 12 80 F9 01 13 00 08
  {
    .param [1]/*080001FF*/  = int32(0xFFFFFFFF)
    // Method begins at RVA 0x0
  } // end of method MemoryPool`1::Rent

  .property /*17000008*/ instance callconv(8) int32
          MaxBufferSize()
  {
    .get instance int32 System.Buffers.MemoryPool`1/*02000013*/::get_MaxBufferSize() /* 060000EE */
  } // end of property MemoryPool`1::MaxBufferSize
  .property /*17000009*/ callconv(8) class System.Buffers.MemoryPool`1/*02000013*/<!T>
          Shared()
  {
    .get class System.Buffers.MemoryPool`1/*02000013*/<!T> System.Buffers.MemoryPool`1/*02000013*/::get_Shared() /* 060000EF */
  } // end of property MemoryPool`1::Shared
} // end of class System.Buffers.MemoryPool`1

.class /*02000014*/ public abstract auto ansi beforefieldinit System.Buffers.ReadOnlySequenceSegment`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .param type T /*2A000004*/ 
    .custom /*0C000025:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
  .method /*060000F3*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2368
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::.ctor

  .method /*060000F4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
          get_Memory() cil managed
  // SIG: 20 00 15 11 80 81 01 13 00
  {
    .param [0]/*08000200*/ 
    .custom /*0C000348:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2370
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequenceSegment`1::get_Memory

  .method /*060000F5*/ family hidebysig specialname 
          instance void  set_Memory(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 'value') cil managed
  // SIG: 20 01 01 15 11 80 81 01 13 00
  {
    .param [1]/*08000201*/ 
    .custom /*0C000349:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2373
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::set_Memory

  .method /*060000F6*/ public hidebysig specialname 
          instance class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T> 
          get_Next() cil managed
  // SIG: 20 00 15 12 50 01 13 00
  {
    .param [0]/*08000202*/ 
    .custom /*0C00034A:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2375
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequenceSegment`1::get_Next

  .method /*060000F7*/ family hidebysig specialname 
          instance void  set_Next(class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T> 'value') cil managed
  // SIG: 20 01 01 15 12 50 01 13 00
  {
    .param [1]/*08000203*/ 
    .custom /*0C00034B:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2378
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::set_Next

  .method /*060000F8*/ public hidebysig specialname 
          instance int64  get_RunningIndex() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequenceSegment`1::get_RunningIndex

  .method /*060000F9*/ family hidebysig specialname 
          instance void  set_RunningIndex(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x237d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::set_RunningIndex

  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
          Memory()
  {
    .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ReadOnlySequenceSegment`1/*02000014*/::get_Memory() /* 060000F4 */
    .set instance void System.Buffers.ReadOnlySequenceSegment`1/*02000014*/::set_Memory(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>) /* 060000F5 */
  } // end of property ReadOnlySequenceSegment`1::Memory
  .property /*1700000B*/ instance callconv(8) class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T>
          Next()
  {
    .custom /*0C000047:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T> System.Buffers.ReadOnlySequenceSegment`1/*02000014*/::get_Next() /* 060000F6 */
    .set instance void System.Buffers.ReadOnlySequenceSegment`1/*02000014*/::set_Next(class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T>) /* 060000F7 */
  } // end of property ReadOnlySequenceSegment`1::Next
  .property /*1700000C*/ instance callconv(8) int64
          RunningIndex()
  {
    .get instance int64 System.Buffers.ReadOnlySequenceSegment`1/*02000014*/::get_RunningIndex() /* 060000F8 */
    .set instance void System.Buffers.ReadOnlySequenceSegment`1/*02000014*/::set_RunningIndex(int64) /* 060000F9 */
  } // end of property ReadOnlySequenceSegment`1::RunningIndex
} // end of class System.Buffers.ReadOnlySequenceSegment`1

.class /*02000015*/ public sequential ansi sealed beforefieldinit System.Buffers.ReadOnlySequence`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C000074:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000075:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000076:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .param type T /*2A000006*/ 
    .custom /*0C00002F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
  .class /*0200001D*/ sequential ansi sealed nested public beforefieldinit Enumerator<T>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
  {
    .custom /*0C00009F:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A00000F*/ 
      .custom /*0C00005D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .field /*04000015*/ private object _dummy
    .field /*04000016*/ private int32 _dummyPrimitive
    .method /*060001C9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor([in] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>& sequence) cil managed
    // SIG: 20 01 01 10 15 11 54 01 13 00
    {
      .param [1]/*080003A4*/ 
      .custom /*0C00036B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
      .custom /*0C00036C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x25d8
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::.ctor

    .method /*060001CA*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
            get_Current() cil managed
    // SIG: 20 00 15 11 80 81 01 13 00
    {
      .param [0]/*080003A5*/ 
      .custom /*0C00036D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x25db
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*060001CB*/ public hidebysig 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x25de
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .property /*17000022*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
            Current()
    {
      .custom /*0C0000B7:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ReadOnlySequence`1/*02000015*//Enumerator/*0200001D*/::get_Current() /* 060001CA */
    } // end of property Enumerator::Current
  } // end of class Enumerator

  .field /*0400000B*/ private initonly object _dummy
  .field /*0400000C*/ private initonly int32 _dummyPrimitive
  .field /*0400000D*/ public static initonly valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> Empty
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*060000FA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T> startSegment,
                               int32 startIndex,
                               class System.Buffers.ReadOnlySequenceSegment`1/*02000014*/<!T> endSegment,
                               int32 endIndex) cil managed
  // SIG: 20 04 01 15 12 50 01 13 00 08 15 12 50 01 13 00 08
  {
    // Method begins at RVA 0x237f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*060000FB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> memory) cil managed
  // SIG: 20 01 01 15 11 80 81 01 13 00
  {
    .param [1]/*08000209*/ 
    .custom /*0C00034C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2382
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*060000FC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T[] 'array') cil managed
  // SIG: 20 01 01 1D 13 00
  {
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*060000FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T[] 'array',
                               int32 start,
                               int32 length) cil managed
  // SIG: 20 03 01 1D 13 00 08 08
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*060000FE*/ public hidebysig specialname 
          instance valuetype System.SequencePosition/*02000009*/ 
          get_End() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_End

  .method /*060000FF*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
          get_First() cil managed
  // SIG: 20 00 15 11 80 81 01 13 00
  {
    .param [0]/*0800020E*/ 
    .custom /*0C00034D:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x238e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_First

  .method /*06000100*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_FirstSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .param [0]/*0800020F*/ 
    .custom /*0C00034E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2391
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_FirstSpan

  .method /*06000101*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2394
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_IsEmpty

  .method /*06000102*/ public hidebysig specialname 
          instance bool  get_IsSingleSegment() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2397
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_IsSingleSegment

  .method /*06000103*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_Length

  .method /*06000104*/ public hidebysig specialname 
          instance valuetype System.SequencePosition/*02000009*/ 
          get_Start() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x239d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_Start

  .method /*06000105*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*//Enumerator/*0200001D*/<!T> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 11 74 01 13 00
  {
    .param [0]/*08000210*/ 
    .custom /*0C00034F:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetEnumerator

  .method /*06000106*/ public hidebysig instance int64 
          GetOffset(valuetype System.SequencePosition/*02000009*/ position) cil managed
  // SIG: 20 01 0A 11 24
  {
    // Method begins at RVA 0x23a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetOffset

  .method /*06000107*/ public hidebysig instance valuetype System.SequencePosition/*02000009*/ 
          GetPosition(int64 offset) cil managed
  // SIG: 20 01 11 24 0A
  {
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetPosition

  .method /*06000108*/ public hidebysig instance valuetype System.SequencePosition/*02000009*/ 
          GetPosition(int64 offset,
                      valuetype System.SequencePosition/*02000009*/ origin) cil managed
  // SIG: 20 02 11 24 0A 11 24
  {
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetPosition

  .method /*06000109*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(int32 start,
                int32 length) cil managed
  // SIG: 20 02 15 11 54 01 13 00 08 08
  {
    .param [0]/*08000215*/ 
    .custom /*0C000350:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600010A*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(int32 start,
                valuetype System.SequencePosition/*02000009*/ end) cil managed
  // SIG: 20 02 15 11 54 01 13 00 08 11 24
  {
    .param [0]/*08000218*/ 
    .custom /*0C000351:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600010B*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(int64 start) cil managed
  // SIG: 20 01 15 11 54 01 13 00 0A
  {
    .param [0]/*0800021B*/ 
    .custom /*0C000352:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600010C*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(int64 start,
                int64 length) cil managed
  // SIG: 20 02 15 11 54 01 13 00 0A 0A
  {
    .param [0]/*0800021D*/ 
    .custom /*0C000353:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600010D*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(int64 start,
                valuetype System.SequencePosition/*02000009*/ end) cil managed
  // SIG: 20 02 15 11 54 01 13 00 0A 11 24
  {
    .param [0]/*08000220*/ 
    .custom /*0C000354:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600010E*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(valuetype System.SequencePosition/*02000009*/ start) cil managed
  // SIG: 20 01 15 11 54 01 13 00 11 24
  {
    .param [0]/*08000223*/ 
    .custom /*0C000355:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600010F*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(valuetype System.SequencePosition/*02000009*/ start,
                int32 length) cil managed
  // SIG: 20 02 15 11 54 01 13 00 11 24 08
  {
    .param [0]/*08000225*/ 
    .custom /*0C000356:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000110*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(valuetype System.SequencePosition/*02000009*/ start,
                int64 length) cil managed
  // SIG: 20 02 15 11 54 01 13 00 11 24 0A
  {
    .param [0]/*08000228*/ 
    .custom /*0C000357:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000111*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          Slice(valuetype System.SequencePosition/*02000009*/ start,
                valuetype System.SequencePosition/*02000009*/ end) cil managed
  // SIG: 20 02 15 11 54 01 13 00 11 24 11 24
  {
    .param [0]/*0800022B*/ 
    .custom /*0C000358:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000112*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::ToString

  .method /*06000113*/ public hidebysig instance bool 
          TryGet(valuetype System.SequencePosition/*02000009*/& position,
                 [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>& memory,
                 [opt] bool advance) cil managed
  // SIG: 20 03 02 10 11 24 10 15 11 80 81 01 13 00 02
  {
    .param [2]/*0800022F*/ 
    .custom /*0C000359:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000230*/  = bool(true)
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::TryGet

  .property /*1700000D*/ instance callconv(8) valuetype System.SequencePosition/*02000009*/
          End()
  {
    .get instance valuetype System.SequencePosition/*02000009*/ System.Buffers.ReadOnlySequence`1/*02000015*/::get_End() /* 060000FE */
  } // end of property ReadOnlySequence`1::End
  .property /*1700000E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
          First()
  {
    .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ReadOnlySequence`1/*02000015*/::get_First() /* 060000FF */
  } // end of property ReadOnlySequence`1::First
  .property /*1700000F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          FirstSpan()
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.ReadOnlySequence`1/*02000015*/::get_FirstSpan() /* 06000100 */
  } // end of property ReadOnlySequence`1::FirstSpan
  .property /*17000010*/ instance callconv(8) bool
          IsEmpty()
  {
    .get instance bool System.Buffers.ReadOnlySequence`1/*02000015*/::get_IsEmpty() /* 06000101 */
  } // end of property ReadOnlySequence`1::IsEmpty
  .property /*17000011*/ instance callconv(8) bool
          IsSingleSegment()
  {
    .get instance bool System.Buffers.ReadOnlySequence`1/*02000015*/::get_IsSingleSegment() /* 06000102 */
  } // end of property ReadOnlySequence`1::IsSingleSegment
  .property /*17000012*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Buffers.ReadOnlySequence`1/*02000015*/::get_Length() /* 06000103 */
  } // end of property ReadOnlySequence`1::Length
  .property /*17000013*/ instance callconv(8) valuetype System.SequencePosition/*02000009*/
          Start()
  {
    .get instance valuetype System.SequencePosition/*02000009*/ System.Buffers.ReadOnlySequence`1/*02000015*/::get_Start() /* 06000104 */
  } // end of property ReadOnlySequence`1::Start
} // end of class System.Buffers.ReadOnlySequence`1

.class /*02000016*/ public abstract auto ansi sealed beforefieldinit System.Buffers.SequenceReaderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C00007B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000114*/ public hidebysig static 
          bool  TryReadBigEndian(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                 [out] int16& 'value') cil managed
  // SIG: 00 02 02 10 15 11 5C 01 05 10 06
  {
    .custom /*0C000279:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadBigEndian

  .method /*06000115*/ public hidebysig static 
          bool  TryReadBigEndian(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                 [out] int32& 'value') cil managed
  // SIG: 00 02 02 10 15 11 5C 01 05 10 08
  {
    .custom /*0C00027A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadBigEndian

  .method /*06000116*/ public hidebysig static 
          bool  TryReadBigEndian(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                 [out] int64& 'value') cil managed
  // SIG: 00 02 02 10 15 11 5C 01 05 10 0A
  {
    .custom /*0C00027C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadBigEndian

  .method /*06000117*/ public hidebysig static 
          bool  TryReadLittleEndian(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                    [out] int16& 'value') cil managed
  // SIG: 00 02 02 10 15 11 5C 01 05 10 06
  {
    .custom /*0C00027D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadLittleEndian

  .method /*06000118*/ public hidebysig static 
          bool  TryReadLittleEndian(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                    [out] int32& 'value') cil managed
  // SIG: 00 02 02 10 15 11 5C 01 05 10 08
  {
    .custom /*0C00027E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadLittleEndian

  .method /*06000119*/ public hidebysig static 
          bool  TryReadLittleEndian(valuetype System.Buffers.SequenceReader`1/*02000017*/<uint8>& reader,
                                    [out] int64& 'value') cil managed
  // SIG: 00 02 02 10 15 11 5C 01 05 10 0A
  {
    .custom /*0C00027F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadLittleEndian

} // end of class System.Buffers.SequenceReaderExtensions

.class /*02000017*/ public sequential ansi sealed beforefieldinit System.Buffers.SequenceReader`1<valuetype .ctor (class [System.Runtime/*23000001*/]System.ValueType/*0100001A*/ modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.UnmanagedType/*01000037*/)/*1B000005*/, class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!T>/*1B000008*/) T>
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C00007F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*0100002F*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .custom /*0C000080:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000030*/::.ctor(string,
                                                                                                                      bool) /* 0A00001B */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C000081:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000031*/::.ctor(string) /* 0A00001C */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
  .param type T /*2A000009*/ 
    .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.IsUnmanagedAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
  .param constraint   T/*2A000009*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!T>/*1B000008*/
    .custom /*0C00001C:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
  .field /*0400000E*/ private object _dummy
  .field /*0400000F*/ private int32 _dummyPrimitive
  .method /*0600011A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> sequence) cil managed
  // SIG: 20 01 01 15 11 54 01 13 00
  {
    // Method begins at RVA 0x23df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::.ctor

  .method /*0600011B*/ public hidebysig specialname 
          instance int64  get_Consumed() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000281:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Consumed

  .method /*0600011C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_CurrentSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .custom /*0C000282:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_CurrentSpan

  .method /*0600011D*/ public hidebysig specialname 
          instance int32  get_CurrentSpanIndex() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000283:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_CurrentSpanIndex

  .method /*0600011E*/ public hidebysig specialname 
          instance bool  get_End() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000284:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_End

  .method /*0600011F*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000285:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Length

  .method /*06000120*/ public hidebysig specialname 
          instance valuetype System.SequencePosition/*02000009*/ 
          get_Position() cil managed
  // SIG: 20 00 11 24
  {
    .custom /*0C000286:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Position

  .method /*06000121*/ public hidebysig specialname 
          instance int64  get_Remaining() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000287:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Remaining

  .method /*06000122*/ public hidebysig specialname 
          instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          get_Sequence() cil managed
  // SIG: 20 00 15 11 54 01 13 00
  {
    .custom /*0C000288:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Sequence

  .method /*06000123*/ public hidebysig specialname 
          instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> 
          get_UnreadSequence() cil managed
  // SIG: 20 00 15 11 54 01 13 00
  {
    .custom /*0C000289:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_UnreadSequence

  .method /*06000124*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_UnreadSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .custom /*0C00028A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_UnreadSpan

  .method /*06000125*/ public hidebysig instance void 
          Advance(int64 count) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2400
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SequenceReader`1::Advance

  .method /*06000126*/ public hidebysig instance int64 
          AdvancePast(!T 'value') cil managed
  // SIG: 20 01 0A 13 00
  {
    // Method begins at RVA 0x2402
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePast

  .method /*06000127*/ public hidebysig instance int64 
          AdvancePastAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> values) cil managed
  // SIG: 20 01 0A 15 11 80 95 01 13 00
  {
    .param [1]/*08000240*/ 
    .custom /*0C00035A:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2405
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*06000128*/ public hidebysig instance int64 
          AdvancePastAny(!T value0,
                         !T value1) cil managed
  // SIG: 20 02 0A 13 00 13 00
  {
    // Method begins at RVA 0x2408
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*06000129*/ public hidebysig instance int64 
          AdvancePastAny(!T value0,
                         !T value1,
                         !T value2) cil managed
  // SIG: 20 03 0A 13 00 13 00 13 00
  {
    // Method begins at RVA 0x240b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*0600012A*/ public hidebysig instance int64 
          AdvancePastAny(!T value0,
                         !T value1,
                         !T value2,
                         !T value3) cil managed
  // SIG: 20 04 0A 13 00 13 00 13 00 13 00
  {
    // Method begins at RVA 0x240e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*0600012B*/ public hidebysig instance void 
          AdvanceToEnd() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2411
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvanceToEnd

  .method /*0600012C*/ public hidebysig instance bool 
          IsNext(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> next,
                 [opt] bool advancePast) cil managed
  // SIG: 20 02 02 15 11 80 95 01 13 00 02
  {
    .param [1]/*0800024A*/ 
    .custom /*0C00035B:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    .param [2]/*0800024B*/  = bool(false)
    // Method begins at RVA 0x2414
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::IsNext

  .method /*0600012D*/ public hidebysig instance bool 
          IsNext(!T next,
                 [opt] bool advancePast) cil managed
  // SIG: 20 02 02 13 00 02
  {
    .param [2]/*0800024D*/  = bool(false)
    // Method begins at RVA 0x2417
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::IsNext

  .method /*0600012E*/ public hidebysig instance void 
          Rewind(int64 count) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x241a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SequenceReader`1::Rewind

  .method /*0600012F*/ public hidebysig instance bool 
          TryAdvanceTo(!T delimiter,
                       [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 02 02 13 00 02
  {
    .param [2]/*08000250*/  = bool(true)
    // Method begins at RVA 0x241c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryAdvanceTo

  .method /*06000130*/ public hidebysig instance bool 
          TryAdvanceToAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiters,
                          [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 02 02 15 11 80 95 01 13 00 02
  {
    .param [1]/*08000251*/ 
    .custom /*0C00035C:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    .param [2]/*08000252*/  = bool(true)
    // Method begins at RVA 0x241f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryAdvanceToAny

  .method /*06000131*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!T> destination) cil managed
  // SIG: 20 01 02 15 11 80 99 01 13 00
  {
    .custom /*0C00028F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2422
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryCopyTo

  .method /*06000132*/ public hidebysig instance bool 
          TryPeek([out] !T& 'value') cil managed
  // SIG: 20 01 02 10 13 00
  {
    .custom /*0C000291:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2425
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryPeek

  .method /*06000133*/ public hidebysig instance bool 
          TryPeek(int64 offset,
                  [out] !T& 'value') cil managed
  // SIG: 20 02 02 0A 10 13 00
  {
    .custom /*0C000293:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2428
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryPeek

  .method /*06000134*/ public hidebysig instance bool 
          TryRead([out] !T& 'value') cil managed
  // SIG: 20 01 02 10 13 00
  {
    // Method begins at RVA 0x242b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryRead

  .method /*06000135*/ public hidebysig instance bool 
          TryReadTo([out] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>& sequence,
                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 54 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*08000259*/ 
    .custom /*0C00035D:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    .param [3]/*0800025A*/  = bool(true)
    // Method begins at RVA 0x242e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000136*/ public hidebysig instance bool 
          TryReadTo([out] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>& sequence,
                    !T delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 54 01 13 00 13 00 02
  {
    .param [3]/*0800025D*/  = bool(true)
    // Method begins at RVA 0x2431
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000137*/ public hidebysig instance bool 
          TryReadTo([out] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>& sequence,
                    !T delimiter,
                    !T delimiterEscape,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 04 02 10 15 11 54 01 13 00 13 00 13 00 02
  {
    .param [4]/*08000261*/  = bool(true)
    // Method begins at RVA 0x2434
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000138*/ public hidebysig instance bool 
          TryReadTo([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 80 95 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*08000263*/ 
    .custom /*0C00035E:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    .param [3]/*08000264*/  = bool(true)
    // Method begins at RVA 0x2437
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000139*/ public hidebysig instance bool 
          TryReadTo([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                    !T delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 80 95 01 13 00 13 00 02
  {
    .param [3]/*08000267*/  = bool(true)
    // Method begins at RVA 0x243a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*0600013A*/ public hidebysig instance bool 
          TryReadTo([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                    !T delimiter,
                    !T delimiterEscape,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 04 02 10 15 11 80 95 01 13 00 13 00 13 00 02
  {
    .param [4]/*0800026B*/  = bool(true)
    // Method begins at RVA 0x243d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*0600013B*/ public hidebysig instance bool 
          TryReadToAny([out] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>& sequence,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiters,
                       [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 54 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*0800026D*/ 
    .custom /*0C00035F:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    .param [3]/*0800026E*/  = bool(true)
    // Method begins at RVA 0x2440
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadToAny

  .method /*0600013C*/ public hidebysig instance bool 
          TryReadToAny([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiters,
                       [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 80 95 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*08000270*/ 
    .custom /*0C000360:06000007*/ instance void System.Runtime.CompilerServices.ScopedRefAttribute/*02000007*/::.ctor() /* 06000007 */ = ( 01 00 00 00 ) 
    .param [3]/*08000271*/  = bool(true)
    // Method begins at RVA 0x2443
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadToAny

  .method /*0600013D*/ public hidebysig instance bool 
          TryReadExact(int32 count,
                       [out] valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>& sequence) cil managed
  // SIG: 20 02 02 08 10 15 11 54 01 13 00
  {
    // Method begins at RVA 0x2446
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadExact

  .property /*17000014*/ instance callconv(8) int64
          Consumed()
  {
    .get instance int64 System.Buffers.SequenceReader`1/*02000017*/::get_Consumed() /* 0600011B */
  } // end of property SequenceReader`1::Consumed
  .property /*17000015*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          CurrentSpan()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.SequenceReader`1/*02000017*/::get_CurrentSpan() /* 0600011C */
  } // end of property SequenceReader`1::CurrentSpan
  .property /*17000016*/ instance callconv(8) int32
          CurrentSpanIndex()
  {
    .get instance int32 System.Buffers.SequenceReader`1/*02000017*/::get_CurrentSpanIndex() /* 0600011D */
  } // end of property SequenceReader`1::CurrentSpanIndex
  .property /*17000017*/ instance callconv(8) bool
          End()
  {
    .get instance bool System.Buffers.SequenceReader`1/*02000017*/::get_End() /* 0600011E */
  } // end of property SequenceReader`1::End
  .property /*17000018*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Buffers.SequenceReader`1/*02000017*/::get_Length() /* 0600011F */
  } // end of property SequenceReader`1::Length
  .property /*17000019*/ instance callconv(8) valuetype System.SequencePosition/*02000009*/
          Position()
  {
    .get instance valuetype System.SequencePosition/*02000009*/ System.Buffers.SequenceReader`1/*02000017*/::get_Position() /* 06000120 */
  } // end of property SequenceReader`1::Position
  .property /*1700001A*/ instance callconv(8) int64
          Remaining()
  {
    .get instance int64 System.Buffers.SequenceReader`1/*02000017*/::get_Remaining() /* 06000121 */
  } // end of property SequenceReader`1::Remaining
  .property /*1700001B*/ instance callconv(8) valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>
          Sequence()
  {
    .get instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> System.Buffers.SequenceReader`1/*02000017*/::get_Sequence() /* 06000122 */
  } // end of property SequenceReader`1::Sequence
  .property /*1700001C*/ instance callconv(8) valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T>
          UnreadSequence()
  {
    .get instance valuetype System.Buffers.ReadOnlySequence`1/*02000015*/<!T> System.Buffers.SequenceReader`1/*02000017*/::get_UnreadSequence() /* 06000123 */
  } // end of property SequenceReader`1::UnreadSequence
  .property /*1700001D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          UnreadSpan()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.SequenceReader`1/*02000017*/::get_UnreadSpan() /* 06000124 */
  } // end of property SequenceReader`1::UnreadSpan
} // end of class System.Buffers.SequenceReader`1

.class /*02000018*/ public sequential ansi sealed beforefieldinit System.Buffers.StandardFormat
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<valuetype System.Buffers.StandardFormat/*02000018*/>/*1B000009*/
{
  .custom /*0C000085:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000010*/ private initonly int32 _dummyPrimitive
  .field /*04000011*/ public static literal uint8 MaxPrecision = uint8(0x63)
  .field /*04000012*/ public static literal uint8 NoPrecision = uint8(0xFF)
  .method /*0600013E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(char symbol,
                               [opt] uint8 precision) cil managed
  // SIG: 20 02 01 03 05
  {
    .param [2]/*08000275*/  = uint8(0xFF)
    // Method begins at RVA 0x2449
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::.ctor

  .method /*0600013F*/ public hidebysig specialname 
          instance bool  get_HasPrecision() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_HasPrecision

  .method /*06000140*/ public hidebysig specialname 
          instance bool  get_IsDefault() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_IsDefault

  .method /*06000141*/ public hidebysig specialname 
          instance uint8  get_Precision() cil managed
  // SIG: 20 00 05
  {
    // Method begins at RVA 0x2452
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_Precision

  .method /*06000142*/ public hidebysig specialname 
          instance char  get_Symbol() cil managed
  // SIG: 20 00 03
  {
    // Method begins at RVA 0x2455
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_Symbol

  .method /*06000143*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Buffers.StandardFormat/*02000018*/ other) cil managed
  // SIG: 20 01 02 11 60
  {
    // Method begins at RVA 0x2458
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Equals

  .method /*06000144*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0002A5:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000277*/ 
    .custom /*0C000361:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x245b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Equals

  .method /*06000145*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x245e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::GetHashCode

  .method /*06000146*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Buffers.StandardFormat/*02000018*/ left,
                            valuetype System.Buffers.StandardFormat/*02000018*/ right) cil managed
  // SIG: 00 02 02 11 60 11 60
  {
    // Method begins at RVA 0x2461
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::op_Equality

  .method /*06000147*/ public hidebysig specialname static 
          valuetype System.Buffers.StandardFormat/*02000018*/ 
          op_Implicit(char symbol) cil managed
  // SIG: 00 01 11 60 03
  {
    // Method begins at RVA 0x2464
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::op_Implicit

  .method /*06000148*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Buffers.StandardFormat/*02000018*/ left,
                              valuetype System.Buffers.StandardFormat/*02000018*/ right) cil managed
  // SIG: 00 02 02 11 60 11 60
  {
    // Method begins at RVA 0x2467
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::op_Inequality

  .method /*06000149*/ public hidebysig static 
          valuetype System.Buffers.StandardFormat/*02000018*/ 
          Parse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> format) cil managed
  // SIG: 00 01 11 60 15 11 80 95 01 03
  {
    .param [1]/*0800027D*/ 
    .custom /*0C000362:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100003F*/::.ctor(string) /* 0A00001E */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x246a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Parse

  .method /*0600014A*/ public hidebysig static 
          valuetype System.Buffers.StandardFormat/*02000018*/ 
          Parse(string format) cil managed
  // SIG: 00 01 11 60 0E
  {
    .custom /*0C0002A7:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800027E*/ 
    .custom /*0C000363:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100003F*/::.ctor(string) /* 0A00001E */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x246d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Parse

  .method /*0600014B*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0002A8:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2470
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::ToString

  .method /*0600014C*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> format,
                         [out] valuetype System.Buffers.StandardFormat/*02000018*/& result) cil managed
  // SIG: 00 02 02 15 11 80 95 01 03 10 11 60
  {
    .param [1]/*0800027F*/ 
    .custom /*0C000364:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100003F*/::.ctor(string) /* 0A00001E */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2473
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::TryParse

  .property /*1700001E*/ instance callconv(8) bool
          HasPrecision()
  {
    .get instance bool System.Buffers.StandardFormat/*02000018*/::get_HasPrecision() /* 0600013F */
  } // end of property StandardFormat::HasPrecision
  .property /*1700001F*/ instance callconv(8) bool
          IsDefault()
  {
    .get instance bool System.Buffers.StandardFormat/*02000018*/::get_IsDefault() /* 06000140 */
  } // end of property StandardFormat::IsDefault
  .property /*17000020*/ instance callconv(8) uint8
          Precision()
  {
    .get instance uint8 System.Buffers.StandardFormat/*02000018*/::get_Precision() /* 06000141 */
  } // end of property StandardFormat::Precision
  .property /*17000021*/ instance callconv(8) char
          Symbol()
  {
    .get instance char System.Buffers.StandardFormat/*02000018*/::get_Symbol() /* 06000142 */
  } // end of property StandardFormat::Symbol
} // end of class System.Buffers.StandardFormat

.class /*02000019*/ public abstract auto ansi sealed beforefieldinit System.Buffers.Text.Utf8Formatter
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*0600014D*/ public hidebysig static 
          bool  TryFormat(bool 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 02 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*08000284*/  = nullref
    // Method begins at RVA 0x2476
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600014E*/ public hidebysig static 
          bool  TryFormat(uint8 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 05 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*08000288*/  = nullref
    // Method begins at RVA 0x2479
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600014F*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000040*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 11 81 01 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*0800028C*/  = nullref
    // Method begins at RVA 0x247c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000150*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000041*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 11 81 05 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*08000290*/  = nullref
    // Method begins at RVA 0x247f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000151*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.Decimal/*01000042*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 11 81 09 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*08000294*/  = nullref
    // Method begins at RVA 0x2482
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000152*/ public hidebysig static 
          bool  TryFormat(float64 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 0D 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*08000298*/  = nullref
    // Method begins at RVA 0x2485
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000153*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.Guid/*01000043*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 11 81 0D 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*0800029C*/  = nullref
    // Method begins at RVA 0x2488
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000154*/ public hidebysig static 
          bool  TryFormat(int16 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 06 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*080002A0*/  = nullref
    // Method begins at RVA 0x248b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000155*/ public hidebysig static 
          bool  TryFormat(int32 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 08 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*080002A4*/  = nullref
    // Method begins at RVA 0x248e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000156*/ public hidebysig static 
          bool  TryFormat(int64 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 0A 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*080002A8*/  = nullref
    // Method begins at RVA 0x2491
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000157*/ public hidebysig static 
          bool  TryFormat(int8 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 04 15 11 80 99 01 05 10 08 11 60
  {
    .custom /*0C0002B3:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002AC*/  = nullref
    // Method begins at RVA 0x2494
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000158*/ public hidebysig static 
          bool  TryFormat(float32 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 0C 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*080002B0*/  = nullref
    // Method begins at RVA 0x2497
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000159*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000044*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 11 81 11 15 11 80 99 01 05 10 08 11 60
  {
    .param [4]/*080002B4*/  = nullref
    // Method begins at RVA 0x249a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600015A*/ public hidebysig static 
          bool  TryFormat(uint16 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 07 15 11 80 99 01 05 10 08 11 60
  {
    .custom /*0C0002B7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002B8*/  = nullref
    // Method begins at RVA 0x249d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600015B*/ public hidebysig static 
          bool  TryFormat(uint32 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 09 15 11 80 99 01 05 10 08 11 60
  {
    .custom /*0C0002B9:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002BC*/  = nullref
    // Method begins at RVA 0x24a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600015C*/ public hidebysig static 
          bool  TryFormat(uint64 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000018*/ format) cil managed
  // SIG: 00 04 02 0B 15 11 80 99 01 05 10 08 11 60
  {
    .custom /*0C0002BB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002C0*/  = nullref
    // Method begins at RVA 0x24a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

} // end of class System.Buffers.Text.Utf8Formatter

.class /*0200001A*/ public abstract auto ansi sealed beforefieldinit System.Buffers.Text.Utf8Parser
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*0600015D*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] bool& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 02 10 08 03
  {
    .param [4]/*080002C4*/  = char(0x0000)
    // Method begins at RVA 0x24a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600015E*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint8& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 05 10 08 03
  {
    .param [4]/*080002C8*/  = char(0x0000)
    // Method begins at RVA 0x24a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600015F*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.DateTime/*01000040*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 01 10 08 03
  {
    .param [4]/*080002CC*/  = char(0x0000)
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000160*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000041*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 05 10 08 03
  {
    .param [4]/*080002D0*/  = char(0x0000)
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000161*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.Decimal/*01000042*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 09 10 08 03
  {
    .param [4]/*080002D4*/  = char(0x0000)
    // Method begins at RVA 0x24b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000162*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] float64& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0D 10 08 03
  {
    .param [4]/*080002D8*/  = char(0x0000)
    // Method begins at RVA 0x24b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000163*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.Guid/*01000043*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 0D 10 08 03
  {
    .param [4]/*080002DC*/  = char(0x0000)
    // Method begins at RVA 0x24b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000164*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int16& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 06 10 08 03
  {
    .param [4]/*080002E0*/  = char(0x0000)
    // Method begins at RVA 0x24bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000165*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int32& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 08 10 08 03
  {
    .param [4]/*080002E4*/  = char(0x0000)
    // Method begins at RVA 0x24be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000166*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int64& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0A 10 08 03
  {
    .param [4]/*080002E8*/  = char(0x0000)
    // Method begins at RVA 0x24c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000167*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int8& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 04 10 08 03
  {
    .custom /*0C0002C7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002EC*/  = char(0x0000)
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000168*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] float32& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0C 10 08 03
  {
    .param [4]/*080002F0*/  = char(0x0000)
    // Method begins at RVA 0x24c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000169*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000044*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 11 10 08 03
  {
    .param [4]/*080002F4*/  = char(0x0000)
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600016A*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint16& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 07 10 08 03
  {
    .custom /*0C0002CB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002F8*/  = char(0x0000)
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600016B*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint32& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 09 10 08 03
  {
    .custom /*0C0002CD:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*080002FC*/  = char(0x0000)
    // Method begins at RVA 0x24d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600016C*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint64& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0B 10 08 03
  {
    .custom /*0C0002CF:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*08000300*/  = char(0x0000)
    // Method begins at RVA 0x24d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

} // end of class System.Buffers.Text.Utf8Parser

.class /*0200001B*/ public abstract auto ansi sealed beforefieldinit System.Buffers.Binary.BinaryPrimitives
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*0600016D*/ public hidebysig static 
          float64  ReadDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0D 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadDoubleBigEndian

  .method /*0600016E*/ public hidebysig static 
          float64  ReadDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0D 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadDoubleLittleEndian

  .method /*0600016F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/ 
          ReadHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 15 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadHalfBigEndian

  .method /*06000170*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/ 
          ReadHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 15 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadHalfLittleEndian

  .method /*06000171*/ public hidebysig static 
          int16  ReadInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 06 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt16BigEndian

  .method /*06000172*/ public hidebysig static 
          int16  ReadInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 06 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt16LittleEndian

  .method /*06000173*/ public hidebysig static 
          int32  ReadInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 08 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt32BigEndian

  .method /*06000174*/ public hidebysig static 
          int32  ReadInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 08 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt32LittleEndian

  .method /*06000175*/ public hidebysig static 
          int64  ReadInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0A 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt64BigEndian

  .method /*06000176*/ public hidebysig static 
          int64  ReadInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0A 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt64LittleEndian

  .method /*06000177*/ public hidebysig static 
          float32  ReadSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0C 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadSingleBigEndian

  .method /*06000178*/ public hidebysig static 
          float32  ReadSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0C 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadSingleLittleEndian

  .method /*06000179*/ public hidebysig static 
          uint16  ReadUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 07 15 11 80 95 01 05
  {
    .custom /*0C0002DD:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt16BigEndian

  .method /*0600017A*/ public hidebysig static 
          uint16  ReadUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 07 15 11 80 95 01 05
  {
    .custom /*0C0002DF:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt16LittleEndian

  .method /*0600017B*/ public hidebysig static 
          uint32  ReadUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 09 15 11 80 95 01 05
  {
    .custom /*0C0002E1:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt32BigEndian

  .method /*0600017C*/ public hidebysig static 
          uint32  ReadUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 09 15 11 80 95 01 05
  {
    .custom /*0C0002E3:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt32LittleEndian

  .method /*0600017D*/ public hidebysig static 
          uint64  ReadUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0B 15 11 80 95 01 05
  {
    .custom /*0C0002E5:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt64BigEndian

  .method /*0600017E*/ public hidebysig static 
          uint64  ReadUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0B 15 11 80 95 01 05
  {
    .custom /*0C0002E6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2509
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt64LittleEndian

  .method /*0600017F*/ public hidebysig static 
          uint8  ReverseEndianness(uint8 'value') cil managed
  // SIG: 00 01 05 05
  {
    // Method begins at RVA 0x250c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000180*/ public hidebysig static 
          int16  ReverseEndianness(int16 'value') cil managed
  // SIG: 00 01 06 06
  {
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000181*/ public hidebysig static 
          int32  ReverseEndianness(int32 'value') cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000182*/ public hidebysig static 
          int64  ReverseEndianness(int64 'value') cil managed
  // SIG: 00 01 0A 0A
  {
    // Method begins at RVA 0x2515
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000183*/ public hidebysig static 
          int8  ReverseEndianness(int8 'value') cil managed
  // SIG: 00 01 04 04
  {
    .custom /*0C0002EA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2518
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000184*/ public hidebysig static 
          uint16  ReverseEndianness(uint16 'value') cil managed
  // SIG: 00 01 07 07
  {
    .custom /*0C0002EB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x251b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000185*/ public hidebysig static 
          uint32  ReverseEndianness(uint32 'value') cil managed
  // SIG: 00 01 09 09
  {
    .custom /*0C0002EC:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000186*/ public hidebysig static 
          uint64  ReverseEndianness(uint64 'value') cil managed
  // SIG: 00 01 0B 0B
  {
    .custom /*0C0002EF:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*06000187*/ public hidebysig static 
          bool  TryReadDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] float64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0D
  {
    // Method begins at RVA 0x2524
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadDoubleBigEndian

  .method /*06000188*/ public hidebysig static 
          bool  TryReadDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] float64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0D
  {
    // Method begins at RVA 0x2527
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadDoubleLittleEndian

  .method /*06000189*/ public hidebysig static 
          bool  TryReadHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                     [out] valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 15
  {
    // Method begins at RVA 0x252a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadHalfBigEndian

  .method /*0600018A*/ public hidebysig static 
          bool  TryReadHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                        [out] valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 15
  {
    // Method begins at RVA 0x252d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadHalfLittleEndian

  .method /*0600018B*/ public hidebysig static 
          bool  TryReadInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                      [out] int16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 06
  {
    // Method begins at RVA 0x2530
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt16BigEndian

  .method /*0600018C*/ public hidebysig static 
          bool  TryReadInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                         [out] int16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 06
  {
    // Method begins at RVA 0x2533
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt16LittleEndian

  .method /*0600018D*/ public hidebysig static 
          bool  TryReadInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                      [out] int32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 08
  {
    // Method begins at RVA 0x2536
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt32BigEndian

  .method /*0600018E*/ public hidebysig static 
          bool  TryReadInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                         [out] int32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 08
  {
    // Method begins at RVA 0x2539
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt32LittleEndian

  .method /*0600018F*/ public hidebysig static 
          bool  TryReadInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                      [out] int64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0A
  {
    // Method begins at RVA 0x253c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt64BigEndian

  .method /*06000190*/ public hidebysig static 
          bool  TryReadInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                         [out] int64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0A
  {
    // Method begins at RVA 0x253f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt64LittleEndian

  .method /*06000191*/ public hidebysig static 
          bool  TryReadSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] float32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0C
  {
    // Method begins at RVA 0x2542
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadSingleBigEndian

  .method /*06000192*/ public hidebysig static 
          bool  TryReadSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] float32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0C
  {
    // Method begins at RVA 0x2545
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadSingleLittleEndian

  .method /*06000193*/ public hidebysig static 
          bool  TryReadUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] uint16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 07
  {
    .custom /*0C0002F3:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2548
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt16BigEndian

  .method /*06000194*/ public hidebysig static 
          bool  TryReadUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] uint16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 07
  {
    .custom /*0C0002F4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x254b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt16LittleEndian

  .method /*06000195*/ public hidebysig static 
          bool  TryReadUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] uint32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 09
  {
    .custom /*0C0002F5:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x254e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt32BigEndian

  .method /*06000196*/ public hidebysig static 
          bool  TryReadUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] uint32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 09
  {
    .custom /*0C0002F6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2551
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt32LittleEndian

  .method /*06000197*/ public hidebysig static 
          bool  TryReadUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] uint64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0B
  {
    .custom /*0C0002F7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2554
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt64BigEndian

  .method /*06000198*/ public hidebysig static 
          bool  TryReadUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] uint64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0B
  {
    .custom /*0C0002F9:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2557
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt64LittleEndian

  .method /*06000199*/ public hidebysig static 
          bool  TryWriteDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x255a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteDoubleBigEndian

  .method /*0600019A*/ public hidebysig static 
          bool  TryWriteDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           float64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x255d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteDoubleLittleEndian

  .method /*0600019B*/ public hidebysig static 
          bool  TryWriteHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                      valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 15
  {
    // Method begins at RVA 0x2560
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteHalfBigEndian

  .method /*0600019C*/ public hidebysig static 
          bool  TryWriteHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                         valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 15
  {
    // Method begins at RVA 0x2563
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteHalfLittleEndian

  .method /*0600019D*/ public hidebysig static 
          bool  TryWriteInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x2566
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt16BigEndian

  .method /*0600019E*/ public hidebysig static 
          bool  TryWriteInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                          int16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x2569
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt16LittleEndian

  .method /*0600019F*/ public hidebysig static 
          bool  TryWriteInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x256c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt32BigEndian

  .method /*060001A0*/ public hidebysig static 
          bool  TryWriteInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                          int32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x256f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt32LittleEndian

  .method /*060001A1*/ public hidebysig static 
          bool  TryWriteInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x2572
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt64BigEndian

  .method /*060001A2*/ public hidebysig static 
          bool  TryWriteInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                          int64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x2575
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt64LittleEndian

  .method /*060001A3*/ public hidebysig static 
          bool  TryWriteSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x2578
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteSingleBigEndian

  .method /*060001A4*/ public hidebysig static 
          bool  TryWriteSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           float32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x257b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteSingleLittleEndian

  .method /*060001A5*/ public hidebysig static 
          bool  TryWriteUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 07
  {
    .custom /*0C000300:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x257e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt16BigEndian

  .method /*060001A6*/ public hidebysig static 
          bool  TryWriteUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           uint16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 07
  {
    .custom /*0C000302:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2581
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt16LittleEndian

  .method /*060001A7*/ public hidebysig static 
          bool  TryWriteUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 09
  {
    .custom /*0C000303:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2584
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt32BigEndian

  .method /*060001A8*/ public hidebysig static 
          bool  TryWriteUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           uint32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 09
  {
    .custom /*0C000304:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2587
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt32LittleEndian

  .method /*060001A9*/ public hidebysig static 
          bool  TryWriteUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0B
  {
    .custom /*0C000307:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x258a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt64BigEndian

  .method /*060001AA*/ public hidebysig static 
          bool  TryWriteUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           uint64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0B
  {
    .custom /*0C000308:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x258d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt64LittleEndian

  .method /*060001AB*/ public hidebysig static 
          void  WriteDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     float64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x2590
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteDoubleBigEndian

  .method /*060001AC*/ public hidebysig static 
          void  WriteDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x2592
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteDoubleLittleEndian

  .method /*060001AD*/ public hidebysig static 
          void  WriteHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                   valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 15
  {
    // Method begins at RVA 0x2594
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteHalfBigEndian

  .method /*060001AE*/ public hidebysig static 
          void  WriteHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                      valuetype [System.Runtime/*23000001*/]System.Half/*01000045*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 15
  {
    // Method begins at RVA 0x2596
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteHalfLittleEndian

  .method /*060001AF*/ public hidebysig static 
          void  WriteInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                    int16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x2598
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt16BigEndian

  .method /*060001B0*/ public hidebysig static 
          void  WriteInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x259a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt16LittleEndian

  .method /*060001B1*/ public hidebysig static 
          void  WriteInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                    int32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x259c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt32BigEndian

  .method /*060001B2*/ public hidebysig static 
          void  WriteInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x259e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt32LittleEndian

  .method /*060001B3*/ public hidebysig static 
          void  WriteInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                    int64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x25a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt64BigEndian

  .method /*060001B4*/ public hidebysig static 
          void  WriteInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x25a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt64LittleEndian

  .method /*060001B5*/ public hidebysig static 
          void  WriteSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     float32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x25a4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteSingleBigEndian

  .method /*060001B6*/ public hidebysig static 
          void  WriteSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x25a6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteSingleLittleEndian

  .method /*060001B7*/ public hidebysig static 
          void  WriteUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     uint16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 07
  {
    .custom /*0C000313:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt16BigEndian

  .method /*060001B8*/ public hidebysig static 
          void  WriteUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 07
  {
    .custom /*0C000316:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt16LittleEndian

  .method /*060001B9*/ public hidebysig static 
          void  WriteUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     uint32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 09
  {
    .custom /*0C000317:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt32BigEndian

  .method /*060001BA*/ public hidebysig static 
          void  WriteUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 09
  {
    .custom /*0C00031A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ae
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt32LittleEndian

  .method /*060001BB*/ public hidebysig static 
          void  WriteUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     uint64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0B
  {
    .custom /*0C00031B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt64BigEndian

  .method /*060001BC*/ public hidebysig static 
          void  WriteUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0B
  {
    .custom /*0C00031C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25b2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt64LittleEndian

} // end of class System.Buffers.Binary.BinaryPrimitives


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
