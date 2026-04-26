


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
// Time-date stamp:            0xea7c6e22
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00009a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000b95e
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
// Checksum:                       0x00017ef6
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000b90c [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x00000444] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000a400 [0x00002938] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000b8f0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00009964 Virtual Size
//              0x00002000 Virtual Address
//              0x00009a00 Size of Raw Data
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
//              0x00009c00 Pointer to Raw Data
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
//              0x0000a200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000b000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000960
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000b94e Import Name Table
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
// 0x00002670 [0x00009200] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000b870 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00006418 Size
//              '#~' Name
//    Stream 2:
//              0x00006484 Offset
//              0x00001708 Size
//              '#Strings' Name
//    Stream 3:
//              0x00007b8c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00007b90 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00007ba0 Offset
//              0x00001660 Size
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

// File size            : 52536
// PE header size       : 512 (496 used)    ( 0.97%)
// PE additional info   : 11843             (22.54%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.14%)
// CLR meta-data size  : 37376              (71.14%)
// CLR additional info : 128                ( 0.24%)
// CLR method headers  : 535                ( 1.02%)
// Managed code         : 1032              ( 1.96%)
// Data                 : 2048              ( 3.90%)
// Unaccounted          : -1010             (-1.92%)

// Num.of PE sections   : 3
//   .text    - 39424
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 37376
//   Module        -    1 (10 bytes)
//   TypeDef       -   22 (308 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   75 (450 bytes)
//   MethodDef     -  541 (7574 bytes)    6 abstract, 0 native, 535 bodies
//   FieldDef      -   20 (120 bytes)     1 constant
//   MemberRef     -   39 (234 bytes)
//   ParamDef      - 1156 (6936 bytes)
//   Constant      -   65 (390 bytes)
//   CustomAttribute- 1105 (6630 bytes)
//   InterfaceImpl -    4 (16 bytes)
//   PropertyMap   -   10 (40 bytes)
//   Property      -   35 (210 bytes)
//   MethodSemantic-   38 (228 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    8 (16 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -   13 (182 bytes)
//   NestedClass   -    3 (12 bytes)
//   GenericParam  -  198 (1584 bytes)
//   GenericParamConstraint-  131 (524 bytes)
//   Strings       -  5893 bytes
//   Blobs         -  5728 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   223 bytes

// CLR additional info : 128

// CLR method headers : 535
//   Num.of method bodies  - 535
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 535

// Managed code : 1032
//   Ave method size - 1
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class SequencePosition               (public) (sequential) (ansi) (sealed) 
// Class MemoryExtensions               (public) (abstract) (auto) (ansi) (sealed) 
// Class EncodingExtensions             (public) (abstract) (auto) (ansi) (sealed) 
// Class SpanLineEnumerator             (public) (sequential) (ansi) (sealed) 
// Class SpanRuneEnumerator             (public) (sequential) (ansi) (sealed) 
// Class SequenceMarshal                (public) (abstract) (auto) (ansi) (sealed) 
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
// Class SpanSplitEnumerator`1          (sequential) (ansi) (sealed) (nested public) 
// Class TryWriteInterpolatedStringHandler (sequential) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 0D 53 79 73 74 65 6D 2E 4D 65 6D 6F 72 79   // ...System.Memory
                                                                                                                                                                        00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 53 79 73 74 65 6D 2E 4D 65 6D 6F 72 79   // ...System.Memory
                                                                                                                                                                       00 00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 0D 53 79 73 74 65 6D 2E 4D 65 6D 6F 72 79   // ...System.Memory
                                                                                                                                                                 00 00 ) 
  .custom /*0C000016:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
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
  .ver 9:0:0:0
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
.class extern /*2700000B*/ forwarder System.Runtime.InteropServices.MemoryMarshal
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000C*/ forwarder System.Span`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000D*/ Enumerator
{
  .class extern System.Span`1 /*2700000C*/ 
}
.module System.Memory.dll
// MVID: {9c78d05c-9a3d-406f-890d-440edf63bc25}
.custom /*0C000002:0A000012*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
.custom /*0C000003:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000015*/::.ctor(int32) /* 0A000013 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000004:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000016*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public sequential ansi sealed beforefieldinit System.SequencePosition
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<valuetype System.SequencePosition/*02000002*/>/*1B000001*/
{
  .custom /*0C000019:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00001B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<valuetype System.SequencePosition/*02000002*/>/*1B000001*/
  .custom /*0C000001:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .field /*04000001*/ private initonly object _dummy
  .field /*04000002*/ private initonly int32 _dummyPrimitive
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               int32 integer) cil managed
  // SIG: 20 02 01 1C 08
  {
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::.ctor

  .method /*06000002*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000018:0A000018*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000018 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000003*/ 
    .custom /*0C00001F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000019 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::Equals

  .method /*06000003*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.SequencePosition/*02000002*/ other) cil managed
  // SIG: 20 01 02 11 08
  {
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::Equals

  .method /*06000004*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000023:0A000018*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000018 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::GetHashCode

  .method /*06000005*/ public hidebysig instance int32 
          GetInteger() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000028:0A000018*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000018 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::GetInteger

  .method /*06000006*/ public hidebysig instance object 
          GetObject() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00002D:0A000018*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000018 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequencePosition::GetObject

} // end of class System.SequencePosition

.class /*02000003*/ public abstract auto ansi sealed beforefieldinit System.MemoryExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C00001E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000014*/ sequential ansi sealed nested public beforefieldinit SpanSplitEnumerator`1<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!T>/*1B000005*/) T>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
  {
    .custom /*0C000073:0A000021*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*01000036*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
    .custom /*0C000074:0A000022*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000037*/::.ctor(string,
                                                                                                                        bool) /* 0A000022 */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                                 62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                                 73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                                 74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                                 69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                                 69 6C 65 72 2E 01 00 00 )                         // iler....
    .custom /*0C000075:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000038*/::.ctor(string) /* 0A000023 */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
    .param constraint   T/*2A00000F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!T>/*1B000005*/
      .custom /*0C000022:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .field /*0400000F*/ private object _dummy
    .field /*04000010*/ private int32 _dummyPrimitive
    .method /*0600020C*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ 
            get_Current() cil managed
    // SIG: 20 00 11 80 89
    {
      .custom /*0C0003D4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x2639
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method SpanSplitEnumerator`1::get_Current

    .method /*0600020D*/ public hidebysig 
            instance valuetype System.MemoryExtensions/*02000003*//SpanSplitEnumerator`1/*02000014*/<!T> 
            GetEnumerator() cil managed
    // SIG: 20 00 15 11 50 01 13 00
    {
      .param [0]/*08000465*/ 
      .custom /*0C000448:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x263c
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method SpanSplitEnumerator`1::GetEnumerator

    .method /*0600020E*/ public hidebysig 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x263f
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method SpanSplitEnumerator`1::MoveNext

    .property /*17000022*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/
            Current()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ System.MemoryExtensions/*02000003*//SpanSplitEnumerator`1/*02000014*/::get_Current() /* 0600020C */
    } // end of property SpanSplitEnumerator`1::Current
  } // end of class SpanSplitEnumerator`1

  .class /*02000015*/ sequential ansi sealed nested public beforefieldinit TryWriteInterpolatedStringHandler
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
  {
    .custom /*0C000079:0A000021*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*01000036*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
    .custom /*0C00007A:0A000022*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000037*/::.ctor(string,
                                                                                                                        bool) /* 0A000022 */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                                 62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                                 73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                                 74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                                 69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                                 69 6C 65 72 2E 01 00 00 )                         // iler....
    .custom /*0C00007B:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000038*/::.ctor(string) /* 0A000023 */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
    .custom /*0C00007C:0A000018*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000018 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C00007D:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InterpolatedStringHandlerAttribute/*01000039*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .field /*04000011*/ private initonly object _dummy
    .field /*04000012*/ private initonly int32 _dummyPrimitive
    .method /*0600020F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 literalLength,
                                 int32 formattedCount,
                                 valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                 [out] bool& shouldAppend) cil managed
    // SIG: 20 04 01 08 08 15 11 80 99 01 03 10 02
    {
      // Method begins at RVA 0x2642
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::.ctor

    .method /*06000210*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 literalLength,
                                 int32 formattedCount,
                                 valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                 class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                                 [out] bool& shouldAppend) cil managed
    // SIG: 20 05 01 08 08 15 11 80 99 01 03 12 80 C9 10 02
    {
      .param [4]/*0800046D*/ 
      .custom /*0C000449:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x2645
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::.ctor

    .method /*06000211*/ public hidebysig 
            instance bool  AppendLiteral(string 'value') cil managed
    // SIG: 20 01 02 0E
    {
      .custom /*0C0003DA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x2648
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendLiteral

    .method /*06000212*/ public hidebysig 
            instance bool  AppendFormatted(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value') cil managed
    // SIG: 20 01 02 15 11 80 95 01 03
    {
      .param [1]/*08000470*/ 
      .custom /*0C00044A:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x264b
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000213*/ public hidebysig 
            instance bool  AppendFormatted(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                                           [opt] int32 'alignment',
                                           [opt] string format) cil managed
    // SIG: 20 03 02 15 11 80 95 01 03 08 0E
    {
      .param [1]/*08000471*/ 
      .custom /*0C00044B:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
      .param [2]/*08000472*/  = int32(0x00000000)
      .param [3]/*08000473*/  = nullref
      .custom /*0C00044C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x264e
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000214*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value') cil managed
    // SIG: 30 01 01 02 1E 00
    {
      .custom /*0C0003DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
      .param type T /*2A0000C3*/ 
        .custom /*0C000280:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x2651
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000215*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value',
                                              string format) cil managed
    // SIG: 30 01 02 02 1E 00 0E
    {
      .custom /*0C0003E0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
      .param [1]/*08000475*/ 
      .custom /*0C00044D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x2654
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000216*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value',
                                              int32 'alignment') cil managed
    // SIG: 30 01 02 02 1E 00 08
    {
      .custom /*0C0003E2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
      .param type T /*2A0000C5*/ 
        .custom /*0C000285:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x2657
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000217*/ public hidebysig 
            instance bool  AppendFormatted<T>(!!T 'value',
                                              int32 'alignment',
                                              string format) cil managed
    // SIG: 30 01 03 02 1E 00 08 0E
    {
      .custom /*0C0003E4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
      .param [1]/*08000479*/ 
      .custom /*0C00044E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x265a
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000218*/ public hidebysig 
            instance bool  AppendFormatted(object 'value',
                                           [opt] int32 'alignment',
                                           [opt] string format) cil managed
    // SIG: 20 03 02 1C 08 0E
    {
      .custom /*0C0003E6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
      .param [2]/*0800047D*/  = int32(0x00000000)
      .param [3]/*0800047E*/  = nullref
      // Method begins at RVA 0x265d
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*06000219*/ public hidebysig 
            instance bool  AppendFormatted(string 'value') cil managed
    // SIG: 20 01 02 0E
    {
      .custom /*0C0003E8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
      // Method begins at RVA 0x2660
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

    .method /*0600021A*/ public hidebysig 
            instance bool  AppendFormatted(string 'value',
                                           [opt] int32 'alignment',
                                           [opt] string format) cil managed
    // SIG: 20 03 02 0E 08 0E
    {
      .custom /*0C0003EA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
      .param [2]/*08000481*/  = int32(0x00000000)
      .param [3]/*08000482*/  = nullref
      // Method begins at RVA 0x2663
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TryWriteInterpolatedStringHandler::AppendFormatted

  } // end of class TryWriteInterpolatedStringHandler

  .method /*06000007*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text) cil managed
  // SIG: 00 01 15 11 80 81 01 03 0E
  {
    .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000005*/ 
    .custom /*0C00002C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000008*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 00 02 15 11 80 81 01 03 0E 11 80 85
  {
    .custom /*0C000038:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000006*/ 
    .custom /*0C000031:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000009*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   int32 start) cil managed
  // SIG: 00 02 15 11 80 81 01 03 0E 08
  {
    .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000008*/ 
    .custom /*0C000039:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600000A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   int32 start,
                   int32 length) cil managed
  // SIG: 00 03 15 11 80 81 01 03 0E 08 08
  {
    .custom /*0C000040:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800000A*/ 
    .custom /*0C000042:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600000B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          AsMemory(string text,
                   valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 00 02 15 11 80 81 01 03 0E 11 80 89
  {
    .custom /*0C000045:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800000D*/ 
    .custom /*0C000052:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600000C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment) cil managed
  // SIG: 10 01 01 15 11 80 8D 01 1E 00 15 11 80 91 01 1E 00
  {
    .custom /*0C00004A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800000F*/ 
    .custom /*0C00005E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000010*/ 
    .custom /*0C000064:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600000D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                      int32 start) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 91 01 1E 00 08
  {
    .custom /*0C00004D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000011*/ 
    .custom /*0C000069:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000012*/ 
    .custom /*0C00006D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600000E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                      int32 start,
                      int32 length) cil managed
  // SIG: 10 01 03 15 11 80 8D 01 1E 00 15 11 80 91 01 1E 00 08 08
  {
    .custom /*0C000054:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000055:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000014*/ 
    .custom /*0C000076:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000015*/ 
    .custom /*0C00007E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*0600000F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array') cil managed
  // SIG: 10 01 01 15 11 80 8D 01 1E 00 1D 1E 00
  {
    .custom /*0C000059:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00005A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000018*/ 
    .custom /*0C000086:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000019*/ 
    .custom /*0C00008A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000010*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 1D 1E 00 11 80 85
  {
    .custom /*0C000061:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000062:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800001A*/ 
    .custom /*0C00008E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800001B*/ 
    .custom /*0C000092:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000011*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      int32 start) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 1D 1E 00 08
  {
    .custom /*0C000067:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000068:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800001D*/ 
    .custom /*0C00009A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800001E*/ 
    .custom /*0C00009E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000012*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      int32 start,
                      int32 length) cil managed
  // SIG: 10 01 03 15 11 80 8D 01 1E 00 1D 1E 00 08 08
  {
    .custom /*0C00006B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000020*/ 
    .custom /*0C0000A5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000021*/ 
    .custom /*0C0000AA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000013*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          AsMemory<T>(!!T[] 'array',
                      valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 1D 1E 00 11 80 89
  {
    .custom /*0C00006F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000070:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000024*/ 
    .custom /*0C0000B8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000025*/ 
    .custom /*0C0000BD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsMemory

  .method /*06000014*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text) cil managed
  // SIG: 00 01 15 11 80 95 01 03 0E
  {
    .custom /*0C000072:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000027*/ 
    .custom /*0C0000C4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2089
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000015*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text,
                 int32 start) cil managed
  // SIG: 00 02 15 11 80 95 01 03 0E 08
  {
    .custom /*0C000078:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000028*/ 
    .custom /*0C0000C8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000016*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text,
                 int32 start,
                 int32 length) cil managed
  // SIG: 00 03 15 11 80 95 01 03 0E 08 08
  {
    .custom /*0C000080:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800002A*/ 
    .custom /*0C0000CF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000017*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text,
                 valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 00 02 15 11 80 95 01 03 0E 11 80 85
  {
    .custom /*0C000083:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800002D*/ 
    .custom /*0C0000D8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000018*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          AsSpan(string text,
                 valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 00 02 15 11 80 95 01 03 0E 11 80 89
  {
    .custom /*0C000085:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800002F*/ 
    .custom /*0C0000DF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000019*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment) cil managed
  // SIG: 10 01 01 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00
  {
    .custom /*0C000088:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000089:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000031*/ 
    .custom /*0C0000E4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000032*/ 
    .custom /*0C0000E8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 11 80 85
  {
    .custom /*0C00008C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000033*/ 
    .custom /*0C0000EC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000034*/ 
    .custom /*0C0000EF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x209b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    int32 start) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 08
  {
    .custom /*0C000090:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000091:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000036*/ 
    .custom /*0C0000F6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000037*/ 
    .custom /*0C0000FA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x209e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    int32 start,
                    int32 length) cil managed
  // SIG: 10 01 03 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 08 08
  {
    .custom /*0C000095:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000096:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000039*/ 
    .custom /*0C000101:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800003A*/ 
    .custom /*0C000104:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T> segment,
                    valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 91 01 1E 00 11 80 89
  {
    .custom /*0C000098:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000099:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800003D*/ 
    .custom /*0C00010C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800003E*/ 
    .custom /*0C000111:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array') cil managed
  // SIG: 10 01 01 15 11 80 99 01 1E 00 1D 1E 00
  {
    .custom /*0C00009C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00009D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000040*/ 
    .custom /*0C000118:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000041*/ 
    .custom /*0C00011B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*0600001F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    valuetype [System.Runtime/*23000001*/]System.Index/*01000021*/ startIndex) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 1D 1E 00 11 80 85
  {
    .custom /*0C0000A0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000042*/ 
    .custom /*0C00011E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000043*/ 
    .custom /*0C000122:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000020*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    int32 start) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 1D 1E 00 08
  {
    .custom /*0C0000A3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000045*/ 
    .custom /*0C000129:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000046*/ 
    .custom /*0C00012D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000021*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    int32 start,
                    int32 length) cil managed
  // SIG: 10 01 03 15 11 80 99 01 1E 00 1D 1E 00 08 08
  {
    .custom /*0C0000A8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000048*/ 
    .custom /*0C000134:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000049*/ 
    .custom /*0C000138:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000022*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          AsSpan<T>(!!T[] 'array',
                    valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/ range) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 1D 1E 00 11 80 89
  {
    .custom /*0C0000AD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000AE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800004C*/ 
    .custom /*0C000140:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800004D*/ 
    .custom /*0C000144:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::AsSpan

  .method /*06000023*/ public hidebysig static 
          int32  BinarySearch<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                 class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T> comparable) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 12 80 9D 01 1E 00
  {
    .custom /*0C0000B1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001B*/ 
      .custom /*0C000093:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800004F*/ 
    .custom /*0C00014A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*06000024*/ public hidebysig static 
          int32  BinarySearch<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                 class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T> comparable) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 12 80 9D 01 1E 00
  {
    .custom /*0C0000B6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001C*/ 
      .custom /*0C000097:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000051*/ 
    .custom /*0C000150:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*06000025*/ public hidebysig static 
          int32  BinarySearch<T,byreflike (class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                                             !!T 'value',
                                                                                                                                                             !!TComparer comparer) cil managed
  // SIG: 10 02 03 08 15 11 80 95 01 1E 00 1E 00 1E 01
  {
    .custom /*0C0000BB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000BC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001D*/ 
      .custom /*0C00009B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TComparer /*2A00001E*/ 
      .custom /*0C00009F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000053*/ 
    .custom /*0C000156:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*06000026*/ public hidebysig static 
          int32  BinarySearch<T,byreflike (class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) TComparable>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                             !!TComparable comparable) cil managed
  // SIG: 10 02 02 08 15 11 80 95 01 1E 00 1E 01
  {
    .custom /*0C0000BF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00001F*/ 
      .custom /*0C0000A2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TComparable /*2A000020*/ 
      .custom /*0C0000A6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000056*/ 
    .custom /*0C00015D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*06000027*/ public hidebysig static 
          int32  BinarySearch<T,byreflike (class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                                             !!T 'value',
                                                                                                                                                             !!TComparer comparer) cil managed
  // SIG: 10 02 03 08 15 11 80 99 01 1E 00 1E 00 1E 01
  {
    .custom /*0C0000C2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000021*/ 
      .custom /*0C0000AB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TComparer /*2A000022*/ 
      .custom /*0C0000AF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000058*/ 
    .custom /*0C000162:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*06000028*/ public hidebysig static 
          int32  BinarySearch<T,byreflike (class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) TComparable>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                             !!TComparable comparable) cil managed
  // SIG: 10 02 02 08 15 11 80 99 01 1E 00 1E 01
  {
    .custom /*0C0000C6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000023*/ 
      .custom /*0C0000B4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TComparable /*2A000024*/ 
      .custom /*0C0000B9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800005B*/ 
    .custom /*0C000169:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::BinarySearch

  .method /*06000029*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0000CA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800005D*/ 
    .custom /*0C000170:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800005E*/ 
    .custom /*0C000175:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*0600002A*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000CD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800005F*/ 
    .custom /*0C000178:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000060*/ 
    .custom /*0C00017B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000061*/ 
    .custom /*0C00017F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*0600002B*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0000D1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000062*/ 
    .custom /*0C000183:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000063*/ 
    .custom /*0C000187:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*0600002C*/ public hidebysig static 
          int32  CommonPrefixLength<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000D4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000064*/ 
    .custom /*0C00018B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000065*/ 
    .custom /*0C00018F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000066*/ 
    .custom /*0C000192:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CommonPrefixLength

  .method /*0600002D*/ public hidebysig static 
          int32  CompareTo(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                           valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> other,
                           valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C0000D7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::CompareTo

  .method /*0600002E*/ public hidebysig static 
          bool  Contains(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                         valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C0000DB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Contains

  .method /*0600002F*/ public hidebysig static 
          bool  Contains<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0000DE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000029*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00003A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800006D*/ 
    .custom /*0C0001A7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800006E*/ 
    .custom /*0C0001AC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Contains

  .method /*06000030*/ public hidebysig static 
          bool  Contains<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C0000E1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00003F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800006F*/ 
    .custom /*0C0001B0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000070*/ 
    .custom /*0C0001B3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Contains

  .method /*06000031*/ public hidebysig static 
          bool  ContainsAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                            class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<string> values) cil managed
  // SIG: 00 02 02 15 11 80 95 01 03 15 12 80 AD 01 0E
  {
    .custom /*0C0000E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000072*/ 
    .custom /*0C0001B8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000032*/ public hidebysig static 
          bool  ContainsAny(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span,
                            class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<string> values) cil managed
  // SIG: 00 02 02 15 11 80 99 01 03 15 12 80 AD 01 0E
  {
    .custom /*0C0000E7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000074*/ 
    .custom /*0C0001C1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000033*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0000EB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000044:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000075*/ 
    .custom /*0C0001C4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000076*/ 
    .custom /*0C0001C7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000034*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0000EE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000049:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000077*/ 
    .custom /*0C0001CB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000078*/ 
    .custom /*0C0001CF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000035*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0000F1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000F2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002D*/ 
      .custom /*0C0000D9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00002D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00004C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000079*/ 
    .custom /*0C0001D3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000036*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1,
                                                                                                                    !!T value2) cil managed
  // SIG: 10 01 04 02 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C0000F4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000F5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002E*/ 
      .custom /*0C0000DC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00002E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000053:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800007C*/ 
    .custom /*0C0001DB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000037*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0000F9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00002F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000058:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000080*/ 
    .custom /*0C0001E9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000081*/ 
    .custom /*0C0001ED:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000038*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0000FD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000030*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000060:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000082*/ 
    .custom /*0C0001F0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000083*/ 
    .custom /*0C0001F3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*06000039*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0000FF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000100:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000031*/ 
      .custom /*0C0000E5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000031*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000066:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000084*/ 
    .custom /*0C0001F7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*0600003A*/ public hidebysig static 
          bool  ContainsAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1,
                                                                                                                    !!T value2) cil managed
  // SIG: 10 01 04 02 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000102:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000103:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000032*/ 
      .custom /*0C0000E9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000032*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00006A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000087*/ 
    .custom /*0C0001FD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAny

  .method /*0600003B*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C000105:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000033*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00006E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800008B*/ 
    .custom /*0C000204:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800008C*/ 
    .custom /*0C000206:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*0600003C*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000108:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000034*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000071:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800008D*/ 
    .custom /*0C000209:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800008E*/ 
    .custom /*0C00020B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*0600003D*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C00010B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000035*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000077:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800008F*/ 
    .custom /*0C00020E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000090*/ 
    .custom /*0C000211:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*0600003E*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00010F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000110:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000036*/ 
      .custom /*0C0000F7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000036*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00007F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000091*/ 
    .custom /*0C000214:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*0600003F*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1,
                                                                                                                          !!T value2) cil managed
  // SIG: 10 01 04 02 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000114:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000115:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000037*/ 
      .custom /*0C0000FB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000037*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000082:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000094*/ 
    .custom /*0C00021C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*06000040*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C000117:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000038*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000084:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000098*/ 
    .custom /*0C000226:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000099*/ 
    .custom /*0C000228:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*06000041*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00011A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000039*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000087:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800009A*/ 
    .custom /*0C00022B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800009B*/ 
    .custom /*0C00022D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*06000042*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C00011D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00003A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00008B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800009C*/ 
    .custom /*0C000230:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800009D*/ 
    .custom /*0C000233:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*06000043*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000120:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000121:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003B*/ 
      .custom /*0C000106:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00003B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00008F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800009E*/ 
    .custom /*0C000236:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*06000044*/ public hidebysig static 
          bool  ContainsAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1,
                                                                                                                          !!T value2) cil managed
  // SIG: 10 01 04 02 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000124:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000125:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003C*/ 
      .custom /*0C000109:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00003C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000094:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000A1*/ 
    .custom /*0C00023D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExcept

  .method /*06000045*/ public hidebysig static 
          bool  ContainsAnyExceptInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                  !!T lowInclusive,
                                                                                                                                  !!T highInclusive) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000127:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000128:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003D*/ 
      .custom /*0C00010D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000A5*/ 
    .custom /*0C000244:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExceptInRange

  .method /*06000046*/ public hidebysig static 
          bool  ContainsAnyExceptInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                  !!T lowInclusive,
                                                                                                                                  !!T highInclusive) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00012B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00012C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003E*/ 
      .custom /*0C000112:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000A8*/ 
    .custom /*0C000249:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyExceptInRange

  .method /*06000047*/ public hidebysig static 
          bool  ContainsAnyInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                            !!T lowInclusive,
                                                                                                                            !!T highInclusive) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00012F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000130:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003F*/ 
      .custom /*0C000116:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000AB*/ 
    .custom /*0C00024D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyInRange

  .method /*06000048*/ public hidebysig static 
          bool  ContainsAnyInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                            !!T lowInclusive,
                                                                                                                            !!T highInclusive) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000132:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000133:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000040*/ 
      .custom /*0C000119:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000AE*/ 
    .custom /*0C000251:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ContainsAnyInRange

  .method /*06000049*/ public hidebysig static 
          void  CopyTo<T>(!!T[] source,
                          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> destination) cil managed
  // SIG: 10 01 02 01 1D 1E 00 15 11 80 8D 01 1E 00
  {
    .custom /*0C000136:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000137:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000B1*/ 
    .custom /*0C000255:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*080000B2*/ 
    .custom /*0C000257:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::CopyTo

  .method /*0600004A*/ public hidebysig static 
          void  CopyTo<T>(!!T[] source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> destination) cil managed
  // SIG: 10 01 02 01 1D 1E 00 15 11 80 99 01 1E 00
  {
    .custom /*0C00013A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000B3*/ 
    .custom /*0C000259:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*080000B4*/ 
    .custom /*0C00025B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x212a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::CopyTo

  .method /*0600004B*/ public hidebysig static 
          int32  Count<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                               !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C00013D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000043*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000A7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000B5*/ 
    .custom /*0C00025D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000B6*/ 
    .custom /*0C000260:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Count

  .method /*0600004C*/ public hidebysig static 
          int32  Count<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                               !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C00013F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000044*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000AC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000B7*/ 
    .custom /*0C000263:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000B8*/ 
    .custom /*0C000266:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x212f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Count

  .method /*0600004D*/ public hidebysig static 
          int32  Count<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000143:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000045*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000B0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000B9*/ 
    .custom /*0C000269:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000BA*/ 
    .custom /*0C00026C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2132
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Count

  .method /*0600004E*/ public hidebysig static 
          int32  Count<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000147:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000046*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000B5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000BB*/ 
    .custom /*0C00026F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000BC*/ 
    .custom /*0C000271:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Count

  .method /*0600004F*/ public hidebysig static 
          bool  EndsWith(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                         valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C000149:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2138
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*06000050*/ public hidebysig static 
          bool  EndsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00014C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000047*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000BA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000C0*/ 
    .custom /*0C000277:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000C1*/ 
    .custom /*0C000279:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*06000051*/ public hidebysig static 
          bool  EndsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00014F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000048*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000BE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000C2*/ 
    .custom /*0C00027C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000C3*/ 
    .custom /*0C00027F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x213e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*06000052*/ public hidebysig static 
          bool  EndsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000153:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000049*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000C1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000C4*/ 
    .custom /*0C000282:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000C5*/ 
    .custom /*0C000284:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2141
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EndsWith

  .method /*06000053*/ public hidebysig static 
          valuetype System.Text.SpanLineEnumerator/*02000005*/ 
          EnumerateLines(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 11 14 15 11 80 95 01 03
  {
    .custom /*0C000155:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateLines

  .method /*06000054*/ public hidebysig static 
          valuetype System.Text.SpanLineEnumerator/*02000005*/ 
          EnumerateLines(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 11 14 15 11 80 99 01 03
  {
    .custom /*0C000158:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateLines

  .method /*06000055*/ public hidebysig static 
          valuetype System.Text.SpanRuneEnumerator/*02000006*/ 
          EnumerateRunes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 11 18 15 11 80 95 01 03
  {
    .custom /*0C00015A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateRunes

  .method /*06000056*/ public hidebysig static 
          valuetype System.Text.SpanRuneEnumerator/*02000006*/ 
          EnumerateRunes(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 11 18 15 11 80 99 01 03
  {
    .custom /*0C00015C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::EnumerateRunes

  .method /*06000057*/ public hidebysig static 
          bool  Equals(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> other,
                       valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C00015F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Equals

  .method /*06000058*/ public hidebysig static 
          int32  IndexOf(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                         valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C000161:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000059*/ public hidebysig static 
          int32  IndexOfAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                            class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<string> values) cil managed
  // SIG: 00 02 08 15 11 80 95 01 03 15 12 80 AD 01 0E
  {
    .custom /*0C000165:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*080000D1*/ 
    .custom /*0C000292:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*0600005A*/ public hidebysig static 
          int32  IndexOfAny(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span,
                            class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<string> values) cil managed
  // SIG: 00 02 08 15 11 80 99 01 03 15 12 80 AD 01 0E
  {
    .custom /*0C000167:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*080000D3*/ 
    .custom /*0C000295:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2159
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*0600005B*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C000168:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000C5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D4*/ 
    .custom /*0C000297:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000D5*/ 
    .custom /*0C000299:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*0600005C*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00016B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000C9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D6*/ 
    .custom /*0C00029B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000D7*/ 
    .custom /*0C00029D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*0600005D*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00016E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00016F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004C*/ 
      .custom /*0C000141:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00004C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000CC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000D8*/ 
    .custom /*0C00029F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*0600005E*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1,
                                                                                                                    !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000173:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000174:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004D*/ 
      .custom /*0C000145:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00004D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000D0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000DB*/ 
    .custom /*0C0002A3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2165
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*0600005F*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C000177:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000D3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000DF*/ 
    .custom /*0C0002A8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E0*/ 
    .custom /*0C0002AB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2168
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000060*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00017A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00004F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000D6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000E1*/ 
    .custom /*0C0002AE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000E2*/ 
    .custom /*0C0002B1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000061*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00017D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00017E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000050*/ 
      .custom /*0C00014D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000050*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000DA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000E3*/ 
    .custom /*0C0002B3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x216e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000062*/ public hidebysig static 
          int32  IndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                    !!T value0,
                                                                                                                    !!T value1,
                                                                                                                    !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C000181:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000182:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000051*/ 
      .custom /*0C000151:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000051*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000DD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000E6*/ 
    .custom /*0C0002B7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAny

  .method /*06000063*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C000186:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000052*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000E0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000EA*/ 
    .custom /*0C0002BC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000EB*/ 
    .custom /*0C0002BF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2174
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000064*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000189:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00018A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000053*/ 
      .custom /*0C000157:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000053*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000E2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000EC*/ 
    .custom /*0C0002C1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000065*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1,
                                                                                                                          !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C00018D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00018E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000054*/ 
      .custom /*0C000159:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000054*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000E6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000EF*/ 
    .custom /*0C0002C6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x217a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000066*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C000191:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000055*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000EA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F3*/ 
    .custom /*0C0002C8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F4*/ 
    .custom /*0C0002C9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000067*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000194:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000056*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000ED:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F5*/ 
    .custom /*0C0002CA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F6*/ 
    .custom /*0C0002CB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000068*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000197:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000057*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000F0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F7*/ 
    .custom /*0C0002CC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080000F8*/ 
    .custom /*0C0002CD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*06000069*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000199:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00019A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000058*/ 
      .custom /*0C000163:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000058*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000F3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000F9*/ 
    .custom /*0C0002CE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600006A*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          !!T value0,
                                                                                                                          !!T value1,
                                                                                                                          !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C00019C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00019D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000059*/ 
      .custom /*0C000166:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000059*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000F8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080000FC*/ 
    .custom /*0C0002D0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2189
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600006B*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001A0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00005A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000FC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000100*/ 
    .custom /*0C0002D1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000101*/ 
    .custom /*0C0002D2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600006C*/ public hidebysig static 
          int32  IndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001A3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00005B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0000FE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000102*/ 
    .custom /*0C0002D3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000103*/ 
    .custom /*0C0002D4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExcept

  .method /*0600006D*/ public hidebysig static 
          int32  IndexOfAnyExceptInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                  !!T lowInclusive,
                                                                                                                                  !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001A6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005C*/ 
      .custom /*0C00016C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000104*/ 
    .custom /*0C0002D5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2192
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExceptInRange

  .method /*0600006E*/ public hidebysig static 
          int32  IndexOfAnyExceptInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                  !!T lowInclusive,
                                                                                                                                  !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001AB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005D*/ 
      .custom /*0C000171:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000107*/ 
    .custom /*0C0002D8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyExceptInRange

  .method /*0600006F*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001AF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00005E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000107:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800010A*/ 
    .custom /*0C0002DE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800010B*/ 
    .custom /*0C0002DF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2198
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000070*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0001B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00005F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00010A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800010C*/ 
    .custom /*0C0002E0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800010D*/ 
    .custom /*0C0002E1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x219b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000071*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000060*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00010E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800010E*/ 
    .custom /*0C0002E2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800010F*/ 
    .custom /*0C0002E3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x219e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000072*/ public hidebysig static 
          int32  IndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                 !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C0001B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000061*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000113:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000110*/ 
    .custom /*0C0002E4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000111*/ 
    .custom /*0C0002E5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOf

  .method /*06000073*/ public hidebysig static 
          int32  IndexOfAnyInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                            !!T lowInclusive,
                                                                                                                            !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001BB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001BC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000062*/ 
      .custom /*0C000184:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000112*/ 
    .custom /*0C0002E6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyInRange

  .method /*06000074*/ public hidebysig static 
          int32  IndexOfAnyInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                            !!T lowInclusive,
                                                                                                                            !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001BF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000063*/ 
      .custom /*0C000188:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000115*/ 
    .custom /*0C0002E7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IndexOfAnyInRange

  .method /*06000075*/ public hidebysig static 
          bool  IsWhiteSpace(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 02 15 11 80 95 01 03
  {
    .custom /*0C0001C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::IsWhiteSpace

  .method /*06000076*/ public hidebysig static 
          int32  LastIndexOf(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                             valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                             valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C0001C6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*06000077*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001CA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000064*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00011C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800011C*/ 
    .custom /*0C0002E8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800011D*/ 
    .custom /*0C0002E9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*06000078*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001CE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000065*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00011F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800011E*/ 
    .custom /*0C0002EA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800011F*/ 
    .custom /*0C0002EB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*06000079*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001D1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000066*/ 
      .custom /*0C000193:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000066*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000123:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000120*/ 
    .custom /*0C0002EC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600007A*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1,
                                                                                                                        !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C0001D5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001D6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000067*/ 
      .custom /*0C000195:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000067*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000126:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000123*/ 
    .custom /*0C0002ED:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600007B*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001D8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000068*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00012A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000127*/ 
    .custom /*0C0002EE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000128*/ 
    .custom /*0C0002EF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600007C*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001DA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000069*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00012E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000129*/ 
    .custom /*0C0002F0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800012A*/ 
    .custom /*0C0002F1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600007D*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00006A*/ 
      .custom /*0C00019E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00006A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000131:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800012B*/ 
    .custom /*0C0002F2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600007E*/ public hidebysig static 
          int32  LastIndexOfAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                        !!T value0,
                                                                                                                        !!T value1,
                                                                                                                        !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C0001E2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00006B*/ 
      .custom /*0C0001A1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00006B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000135:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800012E*/ 
    .custom /*0C0002F3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAny

  .method /*0600007F*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C0001E5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000139:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000132*/ 
    .custom /*0C0002F4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000133*/ 
    .custom /*0C0002F5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000080*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001E7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001E8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00006D*/ 
      .custom /*0C0001A8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00006D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00013C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000134*/ 
    .custom /*0C0002F6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000081*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1,
                                                                                                                              !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 99 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C0001EB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001EC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00006E*/ 
      .custom /*0C0001AD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00006E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00013E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000137*/ 
    .custom /*0C0002FA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000082*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001EF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00006F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000142:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800013B*/ 
    .custom /*0C0002FE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800013C*/ 
    .custom /*0C000300:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000083*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001F2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000070*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000146:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800013D*/ 
    .custom /*0C000302:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800013E*/ 
    .custom /*0C000304:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000084*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0001F6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000071*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000148:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800013F*/ 
    .custom /*0C000306:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000140*/ 
    .custom /*0C000308:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000085*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001F8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001F9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000072*/ 
      .custom /*0C0001B9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000072*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00014B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000141*/ 
    .custom /*0C00030A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000086*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              !!T value0,
                                                                                                                              !!T value1,
                                                                                                                              !!T value2) cil managed
  // SIG: 10 01 04 08 15 11 80 95 01 1E 00 1E 00 1E 00 1E 00
  {
    .custom /*0C0001FA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001FB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000073*/ 
      .custom /*0C0001BD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000073*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00014E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000144*/ 
    .custom /*0C00030E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000087*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C0001FC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000074*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000152:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000148*/ 
    .custom /*0C000310:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000149*/ 
    .custom /*0C000311:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000088*/ public hidebysig static 
          int32  LastIndexOfAnyExcept<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> values) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0001FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000075*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000154:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800014A*/ 
    .custom /*0C000312:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800014B*/ 
    .custom /*0C000313:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExcept

  .method /*06000089*/ public hidebysig static 
          int32  LastIndexOfAnyExceptInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                      !!T lowInclusive,
                                                                                                                                      !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0001FF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000200:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000076*/ 
      .custom /*0C0001C8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800014C*/ 
    .custom /*0C000314:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExceptInRange

  .method /*0600008A*/ public hidebysig static 
          int32  LastIndexOfAnyExceptInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                      !!T lowInclusive,
                                                                                                                                      !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000201:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000202:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000077*/ 
      .custom /*0C0001CC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800014F*/ 
    .custom /*0C000315:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyExceptInRange

  .method /*0600008B*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000203:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000078*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00015B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000152*/ 
    .custom /*0C000317:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000153*/ 
    .custom /*0C000319:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*0600008C*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                     !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000205:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000079*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00015E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000154*/ 
    .custom /*0C00031B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000155*/ 
    .custom /*0C00031C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*0600008D*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000208:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000160:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000156*/ 
    .custom /*0C00031D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000157*/ 
    .custom /*0C00031E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*0600008E*/ public hidebysig static 
          int32  LastIndexOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                     !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C00020A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00007B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000164:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000158*/ 
    .custom /*0C00031F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000159*/ 
    .custom /*0C000320:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOf

  .method /*0600008F*/ public hidebysig static 
          int32  LastIndexOfAnyInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                                !!T lowInclusive,
                                                                                                                                !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 95 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00020C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00020D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00007C*/ 
      .custom /*0C0001DC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800015A*/ 
    .custom /*0C000321:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyInRange

  .method /*06000090*/ public hidebysig static 
          int32  LastIndexOfAnyInRange<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                !!T lowInclusive,
                                                                                                                                !!T highInclusive) cil managed
  // SIG: 10 01 03 08 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00020F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000210:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00007D*/ 
      .custom /*0C0001E0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800015D*/ 
    .custom /*0C000322:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::LastIndexOfAnyInRange

  .method /*06000091*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000212:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000213:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000160*/ 
    .custom /*0C000323:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000161*/ 
    .custom /*0C000324:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x21fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*06000092*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                            [out] int32& elementOffset) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 10 08
  {
    .custom /*0C000216:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000217:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000162*/ 
    .custom /*0C000325:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000163*/ 
    .custom /*0C000326:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2201
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*06000093*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000218:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000219:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000165*/ 
    .custom /*0C000328:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000166*/ 
    .custom /*0C000329:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2204
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*06000094*/ public hidebysig static 
          bool  Overlaps<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                            [out] int32& elementOffset) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00 10 08
  {
    .custom /*0C00021A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00021B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000167*/ 
    .custom /*0C00032A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000168*/ 
    .custom /*0C00032B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2207
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Overlaps

  .method /*06000095*/ public hidebysig static 
          void  Replace<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                !!T oldValue,
                                                                                                                !!T newValue) cil managed
  // SIG: 10 01 03 01 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C00021D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00021E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000082*/ 
      .custom /*0C0001F1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000082*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00016A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800016A*/ 
    .custom /*0C00032C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x220a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Replace

  .method /*06000096*/ public hidebysig static 
          void  Replace<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> source,
                                                                                                                valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> destination,
                                                                                                                !!T oldValue,
                                                                                                                !!T newValue) cil managed
  // SIG: 10 01 04 01 15 11 80 95 01 1E 00 15 11 80 99 01 1E 00 1E 00 1E 00
  {
    .custom /*0C000220:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000221:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000083*/ 
      .custom /*0C0001F4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000083*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00016D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800016D*/ 
    .custom /*0C00032F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800016E*/ 
    .custom /*0C000330:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x220c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Replace

  .method /*06000097*/ public hidebysig static 
          void  Reverse<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span) cil managed
  // SIG: 10 01 01 01 15 11 80 99 01 1E 00
  {
    .custom /*0C000223:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000224:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000171*/ 
    .custom /*0C000331:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x220e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Reverse

  .method /*06000098*/ public hidebysig static 
          int32  SequenceCompareTo<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000225:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000085*/ , class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/
      .custom /*0C000172:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000172*/ 
    .custom /*0C000332:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000173*/ 
    .custom /*0C000333:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2210
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceCompareTo

  .method /*06000099*/ public hidebysig static 
          int32  SequenceCompareTo<(class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 08 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000227:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000086*/ , class [System.Runtime/*23000001*/]System.IComparable`1/*01000027*/<!!T>/*1B000003*/
      .custom /*0C000176:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000174*/ 
    .custom /*0C000334:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000175*/ 
    .custom /*0C000335:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2213
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceCompareTo

  .method /*0600009A*/ public hidebysig static 
          bool  SequenceEqual<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00022A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000087*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000179:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000176*/ 
    .custom /*0C000336:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000177*/ 
    .custom /*0C000337:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2216
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*0600009B*/ public hidebysig static 
          bool  SequenceEqual<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other) cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00022C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000088*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00017C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000178*/ 
    .custom /*0C000339:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000179*/ 
    .custom /*0C00033A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2219
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*0600009C*/ public hidebysig static 
          bool  SequenceEqual<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                 [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C00022E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00022F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800017A*/ 
    .custom /*0C00033B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800017B*/ 
    .custom /*0C00033D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*0800017C*/  = nullref
    .custom /*0C00033F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x221c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*0600009D*/ public hidebysig static 
          bool  SequenceEqual<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> other,
                                 [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!T> comparer) cil managed
  // SIG: 10 01 03 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C000231:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000232:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800017D*/ 
    .custom /*0C000341:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800017E*/ 
    .custom /*0C000342:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*0800017F*/  = nullref
    .custom /*0C000343:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x221f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SequenceEqual

  .method /*0600009E*/ public hidebysig static 
          void  Sort<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span) cil managed
  // SIG: 10 01 01 01 15 11 80 99 01 1E 00
  {
    .custom /*0C000234:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000235:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000180*/ 
    .custom /*0C000344:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2222
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*0600009F*/ public hidebysig static 
          void  Sort<T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                        class [System.Runtime/*23000001*/]System.Comparison`1/*0100002C*/<!!T> comparison) cil managed
  // SIG: 10 01 02 01 15 11 80 99 01 1E 00 15 12 80 B1 01 1E 00
  {
    .custom /*0C000237:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000238:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00008C*/ 
      .custom /*0C000207:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000181*/ 
    .custom /*0C000345:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2224
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*060000A0*/ public hidebysig static 
          void  Sort<TKey,TValue>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TKey> keys,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TValue> items) cil managed
  // SIG: 10 02 02 01 15 11 80 99 01 1E 00 15 11 80 99 01 1E 01
  {
    .custom /*0C000239:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00023A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000183*/ 
    .custom /*0C000346:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000184*/ 
    .custom /*0C000347:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2226
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*060000A1*/ public hidebysig static 
          void  Sort<TKey,TValue>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TKey> keys,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TValue> items,
                                  class [System.Runtime/*23000001*/]System.Comparison`1/*0100002C*/<!!TKey> comparison) cil managed
  // SIG: 10 02 03 01 15 11 80 99 01 1E 00 15 11 80 99 01 1E 01 15 12 80 B1 01 1E 00
  {
    .custom /*0C00023B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00023C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000185*/ 
    .custom /*0C000348:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000186*/ 
    .custom /*0C000349:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000187*/ 
    .custom /*0C00034A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2228
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*060000A2*/ public hidebysig static 
          void  Sort<T,(class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                                          !!TComparer comparer) cil managed
  // SIG: 10 02 02 01 15 11 80 99 01 1E 00 1E 01
  {
    .custom /*0C00023E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000091*/ 
      .custom /*0C000215:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param constraint   TComparer/*2A000092*/ , class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!T>/*1B000002*/
      .custom /*0C000180:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000188*/ 
    .custom /*0C00034C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000189*/ 
    .custom /*0C00034D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*060000A3*/ public hidebysig static 
          void  Sort<TKey,TValue,(class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!TKey>/*1B000002*/) TComparer>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TKey> keys,
                                                                                                                                                       valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!TValue> items,
                                                                                                                                                       !!TComparer comparer) cil managed
  // SIG: 10 03 03 01 15 11 80 99 01 1E 00 15 11 80 99 01 1E 01 1E 02
  {
    .custom /*0C00023F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000240:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TComparer /*2A000095*/ 
      .custom /*0C00021F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param constraint   TComparer/*2A000095*/ , class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000028*/<!!TKey>/*1B000002*/
      .custom /*0C000185:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*0800018A*/ 
    .custom /*0C00034E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800018B*/ 
    .custom /*0C000350:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*0800018C*/ 
    .custom /*0C000352:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryExtensions::Sort

  .method /*060000A4*/ public hidebysig static 
          valuetype System.MemoryExtensions/*02000003*//SpanSplitEnumerator`1/*02000014*/<!!T> 
          Split<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> source,
                                                                                                        !!T separator) cil managed
  // SIG: 10 01 02 15 11 50 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000241:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000242:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000096*/ 
      .custom /*0C000222:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800018D*/ 
    .custom /*0C000354:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800018E*/ 
    .custom /*0C000355:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x222e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Split

  .method /*060000A5*/ public hidebysig static 
          valuetype System.MemoryExtensions/*02000003*//SpanSplitEnumerator`1/*02000014*/<!!T> 
          Split<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> source,
                                                                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> separator) cil managed
  // SIG: 10 01 02 15 11 50 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000243:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000097*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00018C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000190*/ 
    .custom /*0C000356:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000191*/ 
    .custom /*0C000357:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000192*/ 
    .custom /*0C000358:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Split

  .method /*060000A6*/ public hidebysig static 
          valuetype System.MemoryExtensions/*02000003*//SpanSplitEnumerator`1/*02000014*/<!!T> 
          SplitAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> source,
                                                                                                           valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> separators) cil managed
  // SIG: 10 01 02 15 11 50 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000245:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A000098*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000190:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000193*/ 
    .custom /*0C000359:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000194*/ 
    .custom /*0C00035A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000195*/ 
    .custom /*0C00035B:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*0100002D*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .custom /*0C00035C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .custom /*0C00035D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnscopedRefAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2234
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SplitAny

  .method /*060000A7*/ public hidebysig static 
          valuetype System.MemoryExtensions/*02000003*//SpanSplitEnumerator`1/*02000014*/<!!T> 
          SplitAny<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> source,
                                                                                                           class [System.Runtime/*23000001*/]System.Buffers.SearchValues`1/*0100002B*/<!!T> separators) cil managed
  // SIG: 10 01 02 15 11 50 01 1E 00 15 11 80 95 01 1E 00 15 12 80 AD 01 1E 00
  {
    .custom /*0C000246:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000247:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000099*/ 
      .custom /*0C000229:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000196*/ 
    .custom /*0C00035E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000197*/ 
    .custom /*0C00035F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2237
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SplitAny

  .method /*060000A8*/ public hidebysig static 
          int32  Split(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                       valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/> destination,
                       char separator,
                       [opt] valuetype [System.Runtime/*23000001*/]System.StringSplitOptions/*0100002F*/ options) cil managed
  // SIG: 00 04 08 15 11 80 95 01 03 15 11 80 99 01 11 80 89 03 11 80 BD
  {
    .custom /*0C000248:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*0800019C*/  = int32(0x00000000)
    // Method begins at RVA 0x223a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Split

  .method /*060000A9*/ public hidebysig static 
          int32  Split(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                       valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/> destination,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> separator,
                       [opt] valuetype [System.Runtime/*23000001*/]System.StringSplitOptions/*0100002F*/ options) cil managed
  // SIG: 00 04 08 15 11 80 95 01 03 15 11 80 99 01 11 80 89 15 11 80 95 01 03 11 80 BD
  {
    .custom /*0C00024A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*080001A0*/  = int32(0x00000000)
    // Method begins at RVA 0x223d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Split

  .method /*060000AA*/ public hidebysig static 
          int32  SplitAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/> destination,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> separators,
                          [opt] valuetype [System.Runtime/*23000001*/]System.StringSplitOptions/*0100002F*/ options) cil managed
  // SIG: 00 04 08 15 11 80 95 01 03 15 11 80 99 01 11 80 89 15 11 80 95 01 03 11 80 BD
  {
    .custom /*0C00024B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*080001A4*/  = int32(0x00000000)
    // Method begins at RVA 0x2240
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SplitAny

  .method /*060000AB*/ public hidebysig static 
          int32  SplitAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Range/*01000022*/> destination,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<string> separators,
                          [opt] valuetype [System.Runtime/*23000001*/]System.StringSplitOptions/*0100002F*/ options) cil managed
  // SIG: 00 04 08 15 11 80 95 01 03 15 11 80 99 01 11 80 89 15 11 80 95 01 0E 11 80 BD
  {
    .custom /*0C00024C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*080001A7*/ 
    .custom /*0C00036A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [4]/*080001A8*/  = int32(0x00000000)
    // Method begins at RVA 0x2243
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::SplitAny

  .method /*060000AC*/ public hidebysig static 
          bool  StartsWith(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                           valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 'value',
                           valuetype [System.Runtime/*23000001*/]System.StringComparison/*0100002A*/ comparisonType) cil managed
  // SIG: 00 03 02 15 11 80 95 01 03 15 11 80 95 01 03 11 80 A9
  {
    .custom /*0C00024E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2246
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*060000AD*/ public hidebysig static 
          bool  StartsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                   valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00024F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00009A*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000196:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080001AC*/ 
    .custom /*0C00036C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001AD*/ 
    .custom /*0C00036E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2249
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*060000AE*/ public hidebysig static 
          bool  StartsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                                   valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000250:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00009B*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C000198:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080001AE*/ 
    .custom /*0C000370:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001AF*/ 
    .custom /*0C000372:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x224c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*060000AF*/ public hidebysig static 
          bool  StartsWith<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                                   !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000252:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00009C*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00019B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*080001B0*/ 
    .custom /*0C000373:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001B1*/ 
    .custom /*0C000375:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x224f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::StartsWith

  .method /*060000B0*/ public hidebysig static 
          int32  ToLower(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                         valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000030*/ culture) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 99 01 03 12 80 C1
  {
    .custom /*0C000253:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*080001B4*/ 
    .custom /*0C000377:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2252
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToLower

  .method /*060000B1*/ public hidebysig static 
          int32  ToLowerInvariant(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination) cil managed
  // SIG: 00 02 08 15 11 80 95 01 03 15 11 80 99 01 03
  {
    .custom /*0C000254:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2255
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToLowerInvariant

  .method /*060000B2*/ public hidebysig static 
          int32  ToUpper(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                         valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000030*/ culture) cil managed
  // SIG: 00 03 08 15 11 80 95 01 03 15 11 80 99 01 03 12 80 C1
  {
    .custom /*0C000256:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*080001B9*/ 
    .custom /*0C000379:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2258
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToUpper

  .method /*060000B3*/ public hidebysig static 
          int32  ToUpperInvariant(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination) cil managed
  // SIG: 00 02 08 15 11 80 95 01 03 15 11 80 99 01 03
  {
    .custom /*0C000258:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::ToUpperInvariant

  .method /*060000B4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 8D 01 03 15 11 80 8D 01 03
  {
    .custom /*0C00025A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000B5*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 81 01 03 15 11 80 81 01 03
  {
    .custom /*0C00025C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2261
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000B6*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C00025F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2264
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000B7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
               char trimChar) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 03
  {
    .custom /*0C000262:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2267
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000B8*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> trimChars) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C000265:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x226a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000B9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> 
          Trim(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 15 11 80 99 01 03 15 11 80 99 01 03
  {
    .custom /*0C000268:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x226d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000BA*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 8D 01 03 15 11 80 8D 01 03
  {
    .custom /*0C00026B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2270
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000BB*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 81 01 03 15 11 80 81 01 03
  {
    .custom /*0C00026E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2273
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000BC*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C000270:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2276
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000BD*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                  char trimChar) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 03
  {
    .custom /*0C000272:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2279
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000BE*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> trimChars) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C000273:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x227c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000BF*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> 
          TrimEnd(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 15 11 80 99 01 03 15 11 80 99 01 03
  {
    .custom /*0C000275:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C0*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000276:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00009D*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C00019F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001CC*/ 
    .custom /*0C00037E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001CD*/ 
    .custom /*0C00037F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001CE*/ 
    .custom /*0C000381:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C1*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 1E 00
  {
    .custom /*0C000278:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00009E*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001A2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001CF*/ 
    .custom /*0C000383:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001D0*/ 
    .custom /*0C000385:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001D1*/ 
    .custom /*0C000387:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00027B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00009F*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001A4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001D2*/ 
    .custom /*0C000389:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001D3*/ 
    .custom /*0C00038B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001D4*/ 
    .custom /*0C00038D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 1E 00
  {
    .custom /*0C00027E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A0*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001A9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001D5*/ 
    .custom /*0C00038F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001D6*/ 
    .custom /*0C000391:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001D7*/ 
    .custom /*0C000393:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000281:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A1*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001AE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001D8*/ 
    .custom /*0C000394:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001D9*/ 
    .custom /*0C000395:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001DA*/ 
    .custom /*0C000396:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C5*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000283:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A2*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001B1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001DB*/ 
    .custom /*0C000397:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001DC*/ 
    .custom /*0C000398:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001DD*/ 
    .custom /*0C000399:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C6*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000286:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A3*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001B4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001DE*/ 
    .custom /*0C00039A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001DF*/ 
    .custom /*0C00039B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001E0*/ 
    .custom /*0C00039C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimEnd<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                          !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C000287:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A4*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001B6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001E1*/ 
    .custom /*0C00039D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001E2*/ 
    .custom /*0C00039E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001E3*/ 
    .custom /*0C00039F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2297
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimEnd

  .method /*060000C8*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 8D 01 03 15 11 80 8D 01 03
  {
    .custom /*0C000288:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000C9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<char> memory) cil managed
  // SIG: 00 01 15 11 80 81 01 03 15 11 80 81 01 03
  {
    .custom /*0C000289:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000CA*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span) cil managed
  // SIG: 00 01 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C00028A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000CB*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                    char trimChar) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 03
  {
    .custom /*0C00028B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000CC*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> span,
                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> trimChars) cil managed
  // SIG: 00 02 15 11 80 95 01 03 15 11 80 95 01 03 15 11 80 95 01 03
  {
    .custom /*0C00028C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000CD*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> 
          TrimStart(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> span) cil managed
  // SIG: 00 01 15 11 80 99 01 03 15 11 80 99 01 03
  {
    .custom /*0C00028D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000CE*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00028E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A5*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001BA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001EC*/ 
    .custom /*0C0003A5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001ED*/ 
    .custom /*0C0003A7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001EE*/ 
    .custom /*0C0003A9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000CF*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 1E 00
  {
    .custom /*0C00028F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A6*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001BE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001EF*/ 
    .custom /*0C0003AB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001F0*/ 
    .custom /*0C0003AD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001F1*/ 
    .custom /*0C0003AF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D0*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000290:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A7*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001C2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001F2*/ 
    .custom /*0C0003B0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001F3*/ 
    .custom /*0C0003B1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001F4*/ 
    .custom /*0C0003B2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D1*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 1E 00
  {
    .custom /*0C000291:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A8*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001C5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001F5*/ 
    .custom /*0C0003B3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001F6*/ 
    .custom /*0C0003B4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001F7*/ 
    .custom /*0C0003B5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000293:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000A9*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001C9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001F8*/ 
    .custom /*0C0003B6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001F9*/ 
    .custom /*0C0003B7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001FA*/ 
    .custom /*0C0003B8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C000294:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000AA*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001CD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001FB*/ 
    .custom /*0C0003B9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001FC*/ 
    .custom /*0C0003BA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001FD*/ 
    .custom /*0C0003BB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C000296:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000AB*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001D0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*080001FE*/ 
    .custom /*0C0003BC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001FF*/ 
    .custom /*0C0003BD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000200*/ 
    .custom /*0C0003BE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D5*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          TrimStart<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                            !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C000298:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000AC*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001D4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000201*/ 
    .custom /*0C0003BF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000202*/ 
    .custom /*0C0003C1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000203*/ 
    .custom /*0C0003C3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TrimStart

  .method /*060000D6*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00029A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000AD*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001D7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000204*/ 
    .custom /*0C0003C5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000205*/ 
    .custom /*0C0003C7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000206*/ 
    .custom /*0C0003C9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000D7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!!T> memory,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 8D 01 1E 00 15 11 80 8D 01 1E 00 1E 00
  {
    .custom /*0C00029C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000AE*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001D9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000207*/ 
    .custom /*0C0003CB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000208*/ 
    .custom /*0C0003CD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000209*/ 
    .custom /*0C0003CF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000D8*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C00029E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000AF*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001DD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800020A*/ 
    .custom /*0C0003D1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800020B*/ 
    .custom /*0C0003D3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800020C*/ 
    .custom /*0C0003D5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000D9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T> memory,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 81 01 1E 00 15 11 80 81 01 1E 00 1E 00
  {
    .custom /*0C0002A0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000B0*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001E1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*0800020D*/ 
    .custom /*0C0003D6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800020E*/ 
    .custom /*0C0003D7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800020F*/ 
    .custom /*0C0003D8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000DA*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0002A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000B1*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001E4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000210*/ 
    .custom /*0C0003D9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000211*/ 
    .custom /*0C0003DB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000212*/ 
    .custom /*0C0003DC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000DB*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> span,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 95 01 1E 00 15 11 80 95 01 1E 00 1E 00
  {
    .custom /*0C0002A2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000B2*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001E6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000213*/ 
    .custom /*0C0003DD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000214*/ 
    .custom /*0C0003DF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000215*/ 
    .custom /*0C0003E1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000DC*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> trimElements) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0002A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000B3*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001EA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000216*/ 
    .custom /*0C0003E3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000217*/ 
    .custom /*0C0003E5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000218*/ 
    .custom /*0C0003E7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000DD*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> 
          Trim<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> span,
                                                                                                       !!T trimElement) cil managed
  // SIG: 10 01 02 15 11 80 99 01 1E 00 15 11 80 99 01 1E 00 1E 00
  {
    .custom /*0C0002A5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000B4*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001EE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [0]/*08000219*/ 
    .custom /*0C0003E9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800021A*/ 
    .custom /*0C0003EB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800021B*/ 
    .custom /*0C0003EC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::Trim

  .method /*060000DE*/ public hidebysig static 
          bool  TryWrite(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         valuetype System.MemoryExtensions/*02000003*//TryWriteInterpolatedStringHandler/*02000015*/& 'handler',
                         [out] int32& charsWritten) cil managed
  // SIG: 00 03 02 15 11 80 99 01 03 10 11 54 10 08
  {
    .custom /*0C0002A6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800021D*/ 
    .custom /*0C0003ED:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute/*01000031*/::.ctor(string) /* 0A00001D */ = ( 01 00 0B 64 65 73 74 69 6E 61 74 69 6F 6E 00 00 ) // ...destination..
    // Method begins at RVA 0x22dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000DF*/ public hidebysig static 
          bool  TryWrite(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                         valuetype System.MemoryExtensions/*02000003*//TryWriteInterpolatedStringHandler/*02000015*/& 'handler',
                         [out] int32& charsWritten) cil managed
  // SIG: 00 04 02 15 11 80 99 01 03 12 80 C9 10 11 54 10 08
  {
    .custom /*0C0002A7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000220*/ 
    .custom /*0C0003EE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000221*/ 
    .custom /*0C0003EF:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute/*01000031*/::.ctor(string[]) /* 0A00001E */ = ( 01 00 02 00 00 00 0B 64 65 73 74 69 6E 61 74 69   // .......destinati
                                                                                                                                                                                                       6F 6E 08 70 72 6F 76 69 64 65 72 00 00 )          // on.provider..
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000E0*/ public hidebysig static 
          bool  TryWrite<TArg0>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                                class [System.Runtime/*23000001*/]System.Text.CompositeFormat/*01000033*/ format,
                                [out] int32& charsWritten,
                                !!TArg0 arg0) cil managed
  // SIG: 10 01 05 02 15 11 80 99 01 03 12 80 C9 12 80 CD 10 08 1E 00
  {
    .custom /*0C0002A9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002AA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TArg0 /*2A0000B5*/ 
      .custom /*0C00025E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000223*/ 
    .custom /*0C0003F0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000224*/ 
    .custom /*0C0003F1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000E1*/ public hidebysig static 
          bool  TryWrite<TArg0,TArg1>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                      class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                                      class [System.Runtime/*23000001*/]System.Text.CompositeFormat/*01000033*/ format,
                                      [out] int32& charsWritten,
                                      !!TArg0 arg0,
                                      !!TArg1 arg1) cil managed
  // SIG: 10 02 06 02 15 11 80 99 01 03 12 80 C9 12 80 CD 10 08 1E 00 1E 01
  {
    .custom /*0C0002AC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002AD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TArg0 /*2A0000B6*/ 
      .custom /*0C000261:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TArg1 /*2A0000B7*/ 
      .custom /*0C000264:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000228*/ 
    .custom /*0C0003F2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000229*/ 
    .custom /*0C0003F3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000E2*/ public hidebysig static 
          bool  TryWrite<TArg0,TArg1,TArg2>(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                                            class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                                            class [System.Runtime/*23000001*/]System.Text.CompositeFormat/*01000033*/ format,
                                            [out] int32& charsWritten,
                                            !!TArg0 arg0,
                                            !!TArg1 arg1,
                                            !!TArg2 arg2) cil managed
  // SIG: 10 03 07 02 15 11 80 99 01 03 12 80 C9 12 80 CD 10 08 1E 00 1E 01 1E 02
  {
    .custom /*0C0002AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TArg0 /*2A0000B8*/ 
      .custom /*0C000267:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TArg1 /*2A0000B9*/ 
      .custom /*0C00026A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param type TArg2 /*2A0000BA*/ 
      .custom /*0C00026D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800022E*/ 
    .custom /*0C0003F4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800022F*/ 
    .custom /*0C0003F5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000E3*/ public hidebysig static 
          bool  TryWrite(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                         class [System.Runtime/*23000001*/]System.Text.CompositeFormat/*01000033*/ format,
                         [out] int32& charsWritten,
                         object[] args) cil managed
  // SIG: 00 05 02 15 11 80 99 01 03 12 80 C9 12 80 CD 10 08 1D 1C
  {
    .custom /*0C0002B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000236*/ 
    .custom /*0C0003F6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000237*/ 
    .custom /*0C0003F7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000239*/ 
    .custom /*0C0003F8:0A00001F*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000034*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    .custom /*0C0003F9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x22eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

  .method /*060000E4*/ public hidebysig static 
          bool  TryWrite(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> destination,
                         class [System.Runtime/*23000001*/]System.IFormatProvider/*01000032*/ provider,
                         class [System.Runtime/*23000001*/]System.Text.CompositeFormat/*01000033*/ format,
                         [out] int32& charsWritten,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<object> args) cil managed
  // SIG: 00 05 02 15 11 80 99 01 03 12 80 C9 12 80 CD 10 08 15 11 80 95 01 1C
  {
    .custom /*0C0002B4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800023B*/ 
    .custom /*0C0003FA:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800023C*/ 
    .custom /*0C0003FB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800023E*/ 
    .custom /*0C0003FC:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*0100002D*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .custom /*0C0003FD:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .custom /*0C0003FE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    // Method begins at RVA 0x22ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryExtensions::TryWrite

} // end of class System.MemoryExtensions

.class /*02000004*/ public abstract auto ansi sealed beforefieldinit System.Text.EncodingExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000024:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000026:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000E5*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Decoder/*0100003A*/ decoder,
                        [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<uint8>& bytes,
                        class System.Buffers.IBufferWriter`1/*0200000A*/<char> writer,
                        bool flush,
                        [out] int64& charsUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 E9 10 15 11 34 01 05 15 12 28 01 03 02 10 0A 10 02
  {
    .custom /*0C0002B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000240*/ 
    .custom /*0C0003FF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000400:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000E6*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Decoder/*0100003A*/ decoder,
                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> bytes,
                        class System.Buffers.IBufferWriter`1/*0200000A*/<char> writer,
                        bool flush,
                        [out] int64& charsUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 E9 15 11 80 95 01 05 15 12 28 01 03 02 10 0A 10 02
  {
    .custom /*0C0002B6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000246*/ 
    .custom /*0C000401:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000E7*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Encoder/*0100003B*/ encoder,
                        [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<char>& chars,
                        class System.Buffers.IBufferWriter`1/*0200000A*/<uint8> writer,
                        bool flush,
                        [out] int64& bytesUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 ED 10 15 11 34 01 03 15 12 28 01 05 02 10 0A 10 02
  {
    .custom /*0C0002B8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800024C*/ 
    .custom /*0C000402:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000403:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000E8*/ public hidebysig static 
          void  Convert(class [System.Runtime/*23000001*/]System.Text.Encoder/*0100003B*/ encoder,
                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> chars,
                        class System.Buffers.IBufferWriter`1/*0200000A*/<uint8> writer,
                        bool flush,
                        [out] int64& bytesUsed,
                        [out] bool& completed) cil managed
  // SIG: 00 06 01 12 80 ED 15 11 80 95 01 03 15 12 28 01 05 02 10 0A 10 02
  {
    .custom /*0C0002B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000252*/ 
    .custom /*0C000404:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::Convert

  .method /*060000E9*/ public hidebysig static 
          uint8[]  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                            [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<char>& chars) cil managed
  // SIG: 00 02 1D 05 12 80 F1 10 15 11 34 01 03
  {
    .custom /*0C0002BA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000258*/ 
    .custom /*0C000405:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000406:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000EA*/ public hidebysig static 
          int64  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<char>& chars,
                          class System.Buffers.IBufferWriter`1/*0200000A*/<uint8> writer) cil managed
  // SIG: 00 03 0A 12 80 F1 10 15 11 34 01 03 15 12 28 01 05
  {
    .custom /*0C0002BB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800025A*/ 
    .custom /*0C000407:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000408:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2300
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000EB*/ public hidebysig static 
          int32  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<char>& chars,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> bytes) cil managed
  // SIG: 00 03 08 12 80 F1 10 15 11 34 01 03 15 11 80 99 01 05
  {
    .custom /*0C0002BD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0002BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800025C*/ 
    .custom /*0C000409:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800025D*/ 
    .custom /*0C00040A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2303
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000EC*/ public hidebysig static 
          int64  GetBytes(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> chars,
                          class System.Buffers.IBufferWriter`1/*0200000A*/<uint8> writer) cil managed
  // SIG: 00 03 0A 12 80 F1 15 11 80 95 01 03 15 12 28 01 05
  {
    .custom /*0C0002C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000260*/ 
    .custom /*0C00040B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2306
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetBytes

  .method /*060000ED*/ public hidebysig static 
          int64  GetChars(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<uint8>& bytes,
                          class System.Buffers.IBufferWriter`1/*0200000A*/<char> writer) cil managed
  // SIG: 00 03 0A 12 80 F1 10 15 11 34 01 05 15 12 28 01 03
  {
    .custom /*0C0002C2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000263*/ 
    .custom /*0C00040C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C00040D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2309
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetChars

  .method /*060000EE*/ public hidebysig static 
          int32  GetChars(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                          [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<uint8>& bytes,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<char> chars) cil managed
  // SIG: 00 03 08 12 80 F1 10 15 11 34 01 05 15 11 80 99 01 03
  {
    .custom /*0C0002C3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0002C4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000265*/ 
    .custom /*0C00040E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000266*/ 
    .custom /*0C00040F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetChars

  .method /*060000EF*/ public hidebysig static 
          int64  GetChars(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> bytes,
                          class System.Buffers.IBufferWriter`1/*0200000A*/<char> writer) cil managed
  // SIG: 00 03 0A 12 80 F1 15 11 80 95 01 05 15 12 28 01 03
  {
    .custom /*0C0002C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000269*/ 
    .custom /*0C000410:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x230f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetChars

  .method /*060000F0*/ public hidebysig static 
          string  GetString(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ encoding,
                            [in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<uint8>& bytes) cil managed
  // SIG: 00 02 0E 12 80 F1 10 15 11 34 01 05
  {
    .custom /*0C0002C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800026C*/ 
    .custom /*0C000411:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000412:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2312
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncodingExtensions::GetString

} // end of class System.Text.EncodingExtensions

.class /*02000005*/ public sequential ansi sealed beforefieldinit System.Text.SpanLineEnumerator
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C000029:0A000021*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*01000036*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:0A000022*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000037*/::.ctor(string,
                                                                                                                      bool) /* 0A000022 */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C00002B:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000038*/::.ctor(string) /* 0A000023 */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
  .field /*04000003*/ private object _dummy
  .field /*04000004*/ private int32 _dummyPrimitive
  .method /*060000F1*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> 
          get_Current() cil managed
  // SIG: 20 00 15 11 80 95 01 03
  {
    // Method begins at RVA 0x2315
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanLineEnumerator::get_Current

  .method /*060000F2*/ public hidebysig instance valuetype System.Text.SpanLineEnumerator/*02000005*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 14
  {
    // Method begins at RVA 0x2318
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanLineEnumerator::GetEnumerator

  .method /*060000F3*/ public hidebysig instance bool 
          MoveNext() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanLineEnumerator::MoveNext

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char>
          Current()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> System.Text.SpanLineEnumerator/*02000005*/::get_Current() /* 060000F1 */
  } // end of property SpanLineEnumerator::Current
} // end of class System.Text.SpanLineEnumerator

.class /*02000006*/ public sequential ansi sealed beforefieldinit System.Text.SpanRuneEnumerator
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C00002E:0A000021*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*01000036*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
  .custom /*0C00002F:0A000022*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000037*/::.ctor(string,
                                                                                                                      bool) /* 0A000022 */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C000030:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000038*/::.ctor(string) /* 0A000023 */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
  .field /*04000005*/ private object _dummy
  .field /*04000006*/ private int32 _dummyPrimitive
  .method /*060000F4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Text.Rune/*0100003D*/ 
          get_Current() cil managed
  // SIG: 20 00 11 80 F5
  {
    // Method begins at RVA 0x231e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanRuneEnumerator::get_Current

  .method /*060000F5*/ public hidebysig instance valuetype System.Text.SpanRuneEnumerator/*02000006*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanRuneEnumerator::GetEnumerator

  .method /*060000F6*/ public hidebysig instance bool 
          MoveNext() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2324
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpanRuneEnumerator::MoveNext

  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Text.Rune/*0100003D*/
          Current()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Text.Rune/*0100003D*/ System.Text.SpanRuneEnumerator/*02000006*/::get_Current() /* 060000F4 */
  } // end of property SpanRuneEnumerator::Current
} // end of class System.Text.SpanRuneEnumerator

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.SequenceMarshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000036:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .method /*060000F7*/ public hidebysig static 
          bool  TryGetArray<T>(valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!!T> sequence,
                               [out] valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000024*/<!!T>& segment) cil managed
  // SIG: 10 01 02 02 15 11 34 01 1E 00 10 15 11 80 91 01 1E 00
  {
    .param [1]/*0800026D*/ 
    .custom /*0C000413:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800026E*/ 
    .custom /*0C000414:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2327
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryGetArray

  .method /*060000F8*/ public hidebysig static 
          bool  TryGetReadOnlyMemory<T>(valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!!T> sequence,
                                        [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!!T>& memory) cil managed
  // SIG: 10 01 02 02 15 11 34 01 1E 00 10 15 11 80 81 01 1E 00
  {
    .param [1]/*0800026F*/ 
    .custom /*0C000415:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000270*/ 
    .custom /*0C000416:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x232a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryGetReadOnlyMemory

  .method /*060000F9*/ public hidebysig static 
          bool  TryGetReadOnlySequenceSegment<T>(valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!!T> sequence,
                                                 [out] class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!!T>& startSegment,
                                                 [out] int32& startIndex,
                                                 [out] class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!!T>& endSegment,
                                                 [out] int32& endIndex) cil managed
  // SIG: 10 01 05 02 15 11 34 01 1E 00 10 15 12 30 01 1E 00 10 08 10 15 12 30 01 1E 00 10 08
  {
    .param [1]/*08000271*/ 
    .custom /*0C000417:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000272*/ 
    .custom /*0C000418:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C000419:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000019 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000274*/ 
    .custom /*0C00041A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C00041B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000019 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x232d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryGetReadOnlySequenceSegment

  .method /*060000FA*/ public hidebysig static 
          bool  TryRead<valuetype .ctor (class [System.Runtime/*23000001*/]System.ValueType/*0100001A*/ modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.UnmanagedType/*0100003F*/)/*1B000006*/) T>(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                                                                                                                                                                                                     [out] !!T& 'value') cil managed
  // SIG: 10 01 02 02 10 15 11 3C 01 05 10 1E 00
  {
    .custom /*0C0002CF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A0000BE*/ 
      .custom /*0C000274:0A000025*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsUnmanagedAttribute/*0100003E*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2330
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceMarshal::TryRead

} // end of class System.Runtime.InteropServices.SequenceMarshal

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Buffers.ArrayBufferWriter`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
       implements class System.Buffers.IBufferWriter`1/*0200000A*/<!T>/*1B000007*/
{
  .interfaceimpl type class System.Buffers.IBufferWriter`1/*0200000A*/<!T>/*1B000007*/
  .custom /*0C00001C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000001*/ 
    .custom /*0C000017:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
  .method /*060000FB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2333
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::.ctor

  .method /*060000FC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCapacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x233b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::.ctor

  .method /*060000FD*/ public hidebysig specialname 
          instance int32  get_Capacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2343
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_Capacity

  .method /*060000FE*/ public hidebysig specialname 
          instance int32  get_FreeCapacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2346
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_FreeCapacity

  .method /*060000FF*/ public hidebysig specialname 
          instance int32  get_WrittenCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2349
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_WrittenCount

  .method /*06000100*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
          get_WrittenMemory() cil managed
  // SIG: 20 00 15 11 80 81 01 13 00
  {
    .param [0]/*08000279*/ 
    .custom /*0C00041C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x234c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_WrittenMemory

  .method /*06000101*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_WrittenSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .param [0]/*0800027A*/ 
    .custom /*0C00041D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x234f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::get_WrittenSpan

  .method /*06000102*/ public hidebysig newslot virtual final 
          instance void  Advance(int32 count) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2352
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::Advance

  .method /*06000103*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2354
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::Clear

  .method /*06000104*/ public hidebysig instance void 
          ResetWrittenCount() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2356
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ArrayBufferWriter`1::ResetWrittenCount

  .method /*06000105*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!T> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 8D 01 13 00 08
  {
    .param [0]/*0800027C*/ 
    .custom /*0C00041E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800027D*/  = int32(0x00000000)
    // Method begins at RVA 0x2358
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::GetMemory

  .method /*06000106*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!T> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 99 01 13 00 08
  {
    .param [0]/*0800027E*/ 
    .custom /*0C00041F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800027F*/  = int32(0x00000000)
    // Method begins at RVA 0x235b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBufferWriter`1::GetSpan

  .property /*17000003*/ instance callconv(8) int32
          Capacity()
  {
    .get instance int32 System.Buffers.ArrayBufferWriter`1/*02000008*/::get_Capacity() /* 060000FD */
  } // end of property ArrayBufferWriter`1::Capacity
  .property /*17000004*/ instance callconv(8) int32
          FreeCapacity()
  {
    .get instance int32 System.Buffers.ArrayBufferWriter`1/*02000008*/::get_FreeCapacity() /* 060000FE */
  } // end of property ArrayBufferWriter`1::FreeCapacity
  .property /*17000005*/ instance callconv(8) int32
          WrittenCount()
  {
    .get instance int32 System.Buffers.ArrayBufferWriter`1/*02000008*/::get_WrittenCount() /* 060000FF */
  } // end of property ArrayBufferWriter`1::WrittenCount
  .property /*17000006*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
          WrittenMemory()
  {
    .custom /*0C000032:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ArrayBufferWriter`1/*02000008*/::get_WrittenMemory() /* 06000100 */
  } // end of property ArrayBufferWriter`1::WrittenMemory
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          WrittenSpan()
  {
    .custom /*0C000037:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.ArrayBufferWriter`1/*02000008*/::get_WrittenSpan() /* 06000101 */
  } // end of property ArrayBufferWriter`1::WrittenSpan
} // end of class System.Buffers.ArrayBufferWriter`1

.class /*02000009*/ public abstract auto ansi sealed beforefieldinit System.Buffers.BuffersExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C00003D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000107*/ public hidebysig static 
          void  CopyTo<T>([in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!!T>& source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!!T> destination) cil managed
  // SIG: 10 01 02 01 10 15 11 34 01 1E 00 15 11 80 99 01 1E 00
  {
    .custom /*0C0002D6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0002D7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000280*/ 
    .custom /*0C000420:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000421:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000281*/ 
    .custom /*0C000422:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x235e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BuffersExtensions::CopyTo

  .method /*06000108*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000040*/<valuetype System.SequencePosition/*02000002*/> 
          PositionOf<(class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/) T>([in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!!T>& source,
                                                                                                             !!T 'value') cil managed
  // SIG: 10 01 02 15 11 81 01 01 11 08 10 15 11 34 01 1E 00 1E 00
  {
    .custom /*0C0002D9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A0000C0*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!!T>/*1B000004*/
      .custom /*0C0001F5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [1]/*08000282*/ 
    .custom /*0C000423:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000424:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000283*/ 
    .custom /*0C000425:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2360
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BuffersExtensions::PositionOf

  .method /*06000109*/ public hidebysig static 
          !!T[]  ToArray<T>([in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!!T>& sequence) cil managed
  // SIG: 10 01 01 1D 1E 00 10 15 11 34 01 1E 00
  {
    .custom /*0C0002DA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002DB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A0000C1*/ 
      .custom /*0C00027A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000284*/ 
    .custom /*0C000426:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000427:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2363
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BuffersExtensions::ToArray

  .method /*0600010A*/ public hidebysig static 
          void  Write<T>(class System.Buffers.IBufferWriter`1/*0200000A*/<!!T> writer,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!!T> 'value') cil managed
  // SIG: 10 01 02 01 15 12 28 01 1E 00 15 11 80 95 01 1E 00
  {
    .custom /*0C0002DC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002DD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A0000C2*/ 
      .custom /*0C00027D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000286*/ 
    .custom /*0C000428:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2366
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BuffersExtensions::Write

} // end of class System.Buffers.BuffersExtensions

.class /*0200000A*/ interface public abstract auto ansi beforefieldinit System.Buffers.IBufferWriter`1<T>
{
  .custom /*0C000041:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  .method /*0600010B*/ public hidebysig newslot abstract virtual 
          instance void  Advance(int32 count) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IBufferWriter`1::Advance

  .method /*0600010C*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000023*/<!T> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 8D 01 13 00 08
  {
    .param [0]/*08000288*/ 
    .custom /*0C000429:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000289*/  = int32(0x00000000)
    // Method begins at RVA 0x0
  } // end of method IBufferWriter`1::GetMemory

  .method /*0600010D*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!T> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 99 01 13 00 08
  {
    .param [0]/*0800028A*/ 
    .custom /*0C00042A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800028B*/  = int32(0x00000000)
    // Method begins at RVA 0x0
  } // end of method IBufferWriter`1::GetSpan

} // end of class System.Buffers.IBufferWriter`1

.class /*0200000B*/ public abstract auto ansi beforefieldinit System.Buffers.MemoryPool`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000041*/
{
  .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000047:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000041*/
  .custom /*0C000020:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000003*/ 
    .custom /*0C000021:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
  .method /*0600010E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2368
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MemoryPool`1::.ctor

  .method /*0600010F*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_MaxBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method MemoryPool`1::get_MaxBufferSize

  .method /*06000110*/ public hidebysig specialname static 
          class System.Buffers.MemoryPool`1/*0200000B*/<!T> 
          get_Shared() cil managed
  // SIG: 00 00 15 12 2C 01 13 00
  {
    // Method begins at RVA 0x2370
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPool`1::get_Shared

  .method /*06000111*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2373
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MemoryPool`1::Dispose

  .method /*06000112*/ family hidebysig newslot abstract virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method MemoryPool`1::Dispose

  .method /*06000113*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Buffers.IMemoryOwner`1/*01000042*/<!T> 
          Rent([opt] int32 minBufferSize) cil managed
  // SIG: 20 01 15 12 81 09 01 13 00 08
  {
    .param [1]/*0800028D*/  = int32(0xFFFFFFFF)
    // Method begins at RVA 0x0
  } // end of method MemoryPool`1::Rent

  .property /*17000008*/ instance callconv(8) int32
          MaxBufferSize()
  {
    .get instance int32 System.Buffers.MemoryPool`1/*0200000B*/::get_MaxBufferSize() /* 0600010F */
  } // end of property MemoryPool`1::MaxBufferSize
  .property /*17000009*/ callconv(8) class System.Buffers.MemoryPool`1/*0200000B*/<!T>
          Shared()
  {
    .get class System.Buffers.MemoryPool`1/*0200000B*/<!T> System.Buffers.MemoryPool`1/*0200000B*/::get_Shared() /* 06000110 */
  } // end of property MemoryPool`1::Shared
} // end of class System.Buffers.MemoryPool`1

.class /*0200000C*/ public abstract auto ansi beforefieldinit System.Buffers.ReadOnlySequenceSegment`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .param type T /*2A000004*/ 
    .custom /*0C000027:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
  .method /*06000114*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2375
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::.ctor

  .method /*06000115*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
          get_Memory() cil managed
  // SIG: 20 00 15 11 80 81 01 13 00
  {
    .param [0]/*0800028E*/ 
    .custom /*0C00042B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequenceSegment`1::get_Memory

  .method /*06000116*/ family hidebysig specialname 
          instance void  set_Memory(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 'value') cil managed
  // SIG: 20 01 01 15 11 80 81 01 13 00
  {
    .param [1]/*0800028F*/ 
    .custom /*0C00042C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2380
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::set_Memory

  .method /*06000117*/ public hidebysig specialname 
          instance class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T> 
          get_Next() cil managed
  // SIG: 20 00 15 12 30 01 13 00
  {
    .param [0]/*08000290*/ 
    .custom /*0C00042D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2382
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequenceSegment`1::get_Next

  .method /*06000118*/ family hidebysig specialname 
          instance void  set_Next(class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T> 'value') cil managed
  // SIG: 20 01 01 15 12 30 01 13 00
  {
    .param [1]/*08000291*/ 
    .custom /*0C00042E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2385
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::set_Next

  .method /*06000119*/ public hidebysig specialname 
          instance int64  get_RunningIndex() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2387
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequenceSegment`1::get_RunningIndex

  .method /*0600011A*/ family hidebysig specialname 
          instance void  set_RunningIndex(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x238a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlySequenceSegment`1::set_RunningIndex

  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
          Memory()
  {
    .custom /*0C000043:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/::get_Memory() /* 06000115 */
    .set instance void System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/::set_Memory(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>) /* 06000116 */
  } // end of property ReadOnlySequenceSegment`1::Memory
  .property /*1700000B*/ instance callconv(8) class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T>
          Next()
  {
    .custom /*0C000048:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T> System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/::get_Next() /* 06000117 */
    .set instance void System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/::set_Next(class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T>) /* 06000118 */
  } // end of property ReadOnlySequenceSegment`1::Next
  .property /*1700000C*/ instance callconv(8) int64
          RunningIndex()
  {
    .get instance int64 System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/::get_RunningIndex() /* 06000119 */
    .set instance void System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/::set_RunningIndex(int64) /* 0600011A */
  } // end of property ReadOnlySequenceSegment`1::RunningIndex
} // end of class System.Buffers.ReadOnlySequenceSegment`1

.class /*0200000D*/ public sequential ansi sealed beforefieldinit System.Buffers.ReadOnlySequence`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C00004F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000051:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .param type T /*2A000006*/ 
    .custom /*0C000033:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
  .class /*02000016*/ sequential ansi sealed nested public beforefieldinit Enumerator<T>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
  {
    .custom /*0C000081:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000010*/ 
      .custom /*0C000065:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .field /*04000013*/ private object _dummy
    .field /*04000014*/ private int32 _dummyPrimitive
    .method /*0600021B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor([in] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>& sequence) cil managed
    // SIG: 20 01 01 10 15 11 34 01 13 00
    {
      .param [1]/*08000483*/ 
      .custom /*0C00044F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
      .custom /*0C000450:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x2666
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::.ctor

    .method /*0600021C*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
            get_Current() cil managed
    // SIG: 20 00 15 11 80 81 01 13 00
    {
      .param [0]/*08000484*/ 
      .custom /*0C000451:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x2669
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*0600021D*/ public hidebysig 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x266c
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .property /*17000023*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
            Current()
    {
      .custom /*0C0000B3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ReadOnlySequence`1/*0200000D*//Enumerator/*02000016*/::get_Current() /* 0600021C */
    } // end of property Enumerator::Current
  } // end of class Enumerator

  .field /*04000007*/ private initonly object _dummy
  .field /*04000008*/ private initonly int32 _dummyPrimitive
  .field /*04000009*/ public static initonly valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> Empty
  .custom /*0C00003C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*0600011B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T> startSegment,
                               int32 startIndex,
                               class System.Buffers.ReadOnlySequenceSegment`1/*0200000C*/<!T> endSegment,
                               int32 endIndex) cil managed
  // SIG: 20 04 01 15 12 30 01 13 00 08 15 12 30 01 13 00 08
  {
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*0600011C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> memory) cil managed
  // SIG: 20 01 01 15 11 80 81 01 13 00
  {
    .param [1]/*08000297*/ 
    .custom /*0C00042F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*0600011D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T[] 'array') cil managed
  // SIG: 20 01 01 1D 13 00
  {
    // Method begins at RVA 0x2392
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*0600011E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T[] 'array',
                               int32 start,
                               int32 length) cil managed
  // SIG: 20 03 01 1D 13 00 08 08
  {
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::.ctor

  .method /*0600011F*/ public hidebysig specialname 
          instance valuetype System.SequencePosition/*02000002*/ 
          get_End() cil managed
  // SIG: 20 00 11 08
  {
    // Method begins at RVA 0x2398
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_End

  .method /*06000120*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> 
          get_First() cil managed
  // SIG: 20 00 15 11 80 81 01 13 00
  {
    .param [0]/*0800029C*/ 
    .custom /*0C000430:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x239b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_First

  .method /*06000121*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_FirstSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .param [0]/*0800029D*/ 
    .custom /*0C000431:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x239e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_FirstSpan

  .method /*06000122*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_IsEmpty

  .method /*06000123*/ public hidebysig specialname 
          instance bool  get_IsSingleSegment() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_IsSingleSegment

  .method /*06000124*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x23a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_Length

  .method /*06000125*/ public hidebysig specialname 
          instance valuetype System.SequencePosition/*02000002*/ 
          get_Start() cil managed
  // SIG: 20 00 11 08
  {
    // Method begins at RVA 0x23aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::get_Start

  .method /*06000126*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*//Enumerator/*02000016*/<!T> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 11 58 01 13 00
  {
    .param [0]/*0800029E*/ 
    .custom /*0C000432:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetEnumerator

  .method /*06000127*/ public hidebysig instance int64 
          GetOffset(valuetype System.SequencePosition/*02000002*/ position) cil managed
  // SIG: 20 01 0A 11 08
  {
    // Method begins at RVA 0x23b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetOffset

  .method /*06000128*/ public hidebysig instance valuetype System.SequencePosition/*02000002*/ 
          GetPosition(int64 offset) cil managed
  // SIG: 20 01 11 08 0A
  {
    // Method begins at RVA 0x23b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetPosition

  .method /*06000129*/ public hidebysig instance valuetype System.SequencePosition/*02000002*/ 
          GetPosition(int64 offset,
                      valuetype System.SequencePosition/*02000002*/ origin) cil managed
  // SIG: 20 02 11 08 0A 11 08
  {
    // Method begins at RVA 0x23b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::GetPosition

  .method /*0600012A*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(int32 start,
                int32 length) cil managed
  // SIG: 20 02 15 11 34 01 13 00 08 08
  {
    .param [0]/*080002A3*/ 
    .custom /*0C000433:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600012B*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(int32 start,
                valuetype System.SequencePosition/*02000002*/ end) cil managed
  // SIG: 20 02 15 11 34 01 13 00 08 11 08
  {
    .param [0]/*080002A6*/ 
    .custom /*0C000434:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600012C*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(int64 start) cil managed
  // SIG: 20 01 15 11 34 01 13 00 0A
  {
    .param [0]/*080002A9*/ 
    .custom /*0C000435:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600012D*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(int64 start,
                int64 length) cil managed
  // SIG: 20 02 15 11 34 01 13 00 0A 0A
  {
    .param [0]/*080002AB*/ 
    .custom /*0C000436:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600012E*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(int64 start,
                valuetype System.SequencePosition/*02000002*/ end) cil managed
  // SIG: 20 02 15 11 34 01 13 00 0A 11 08
  {
    .param [0]/*080002AE*/ 
    .custom /*0C000437:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*0600012F*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(valuetype System.SequencePosition/*02000002*/ start) cil managed
  // SIG: 20 01 15 11 34 01 13 00 11 08
  {
    .param [0]/*080002B1*/ 
    .custom /*0C000438:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000130*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(valuetype System.SequencePosition/*02000002*/ start,
                int32 length) cil managed
  // SIG: 20 02 15 11 34 01 13 00 11 08 08
  {
    .param [0]/*080002B3*/ 
    .custom /*0C000439:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000131*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(valuetype System.SequencePosition/*02000002*/ start,
                int64 length) cil managed
  // SIG: 20 02 15 11 34 01 13 00 11 08 0A
  {
    .param [0]/*080002B6*/ 
    .custom /*0C00043A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000132*/ public hidebysig instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          Slice(valuetype System.SequencePosition/*02000002*/ start,
                valuetype System.SequencePosition/*02000002*/ end) cil managed
  // SIG: 20 02 15 11 34 01 13 00 11 08 11 08
  {
    .param [0]/*080002B9*/ 
    .custom /*0C00043B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::Slice

  .method /*06000133*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::ToString

  .method /*06000134*/ public hidebysig instance bool 
          TryGet(valuetype System.SequencePosition/*02000002*/& position,
                 [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>& memory,
                 [opt] bool advance) cil managed
  // SIG: 20 03 02 10 11 08 10 15 11 80 81 01 13 00 02
  {
    .param [2]/*080002BD*/ 
    .custom /*0C00043C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080002BE*/  = bool(true)
    // Method begins at RVA 0x23d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlySequence`1::TryGet

  .property /*1700000D*/ instance callconv(8) valuetype System.SequencePosition/*02000002*/
          End()
  {
    .get instance valuetype System.SequencePosition/*02000002*/ System.Buffers.ReadOnlySequence`1/*0200000D*/::get_End() /* 0600011F */
  } // end of property ReadOnlySequence`1::End
  .property /*1700000E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T>
          First()
  {
    .custom /*0C000057:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000020*/<!T> System.Buffers.ReadOnlySequence`1/*0200000D*/::get_First() /* 06000120 */
  } // end of property ReadOnlySequence`1::First
  .property /*1700000F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          FirstSpan()
  {
    .custom /*0C00005F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.ReadOnlySequence`1/*0200000D*/::get_FirstSpan() /* 06000121 */
  } // end of property ReadOnlySequence`1::FirstSpan
  .property /*17000010*/ instance callconv(8) bool
          IsEmpty()
  {
    .get instance bool System.Buffers.ReadOnlySequence`1/*0200000D*/::get_IsEmpty() /* 06000122 */
  } // end of property ReadOnlySequence`1::IsEmpty
  .property /*17000011*/ instance callconv(8) bool
          IsSingleSegment()
  {
    .get instance bool System.Buffers.ReadOnlySequence`1/*0200000D*/::get_IsSingleSegment() /* 06000123 */
  } // end of property ReadOnlySequence`1::IsSingleSegment
  .property /*17000012*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Buffers.ReadOnlySequence`1/*0200000D*/::get_Length() /* 06000124 */
  } // end of property ReadOnlySequence`1::Length
  .property /*17000013*/ instance callconv(8) valuetype System.SequencePosition/*02000002*/
          Start()
  {
    .get instance valuetype System.SequencePosition/*02000002*/ System.Buffers.ReadOnlySequence`1/*0200000D*/::get_Start() /* 06000125 */
  } // end of property ReadOnlySequence`1::Start
} // end of class System.Buffers.ReadOnlySequence`1

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit System.Buffers.SequenceReaderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000056:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000135*/ public hidebysig static 
          bool  TryReadBigEndian(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                 [out] int16& 'value') cil managed
  // SIG: 00 02 02 10 15 11 3C 01 05 10 06
  {
    .custom /*0C0002F7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadBigEndian

  .method /*06000136*/ public hidebysig static 
          bool  TryReadBigEndian(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                 [out] int32& 'value') cil managed
  // SIG: 00 02 02 10 15 11 3C 01 05 10 08
  {
    .custom /*0C0002F8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadBigEndian

  .method /*06000137*/ public hidebysig static 
          bool  TryReadBigEndian(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                 [out] int64& 'value') cil managed
  // SIG: 00 02 02 10 15 11 3C 01 05 10 0A
  {
    .custom /*0C0002F9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadBigEndian

  .method /*06000138*/ public hidebysig static 
          bool  TryReadLittleEndian(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                    [out] int16& 'value') cil managed
  // SIG: 00 02 02 10 15 11 3C 01 05 10 06
  {
    .custom /*0C0002FB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadLittleEndian

  .method /*06000139*/ public hidebysig static 
          bool  TryReadLittleEndian(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                    [out] int32& 'value') cil managed
  // SIG: 00 02 02 10 15 11 3C 01 05 10 08
  {
    .custom /*0C0002FC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadLittleEndian

  .method /*0600013A*/ public hidebysig static 
          bool  TryReadLittleEndian(valuetype System.Buffers.SequenceReader`1/*0200000F*/<uint8>& reader,
                                    [out] int64& 'value') cil managed
  // SIG: 00 02 02 10 15 11 3C 01 05 10 0A
  {
    .custom /*0C0002FD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReaderExtensions::TryReadLittleEndian

} // end of class System.Buffers.SequenceReaderExtensions

.class /*0200000F*/ public sequential ansi sealed beforefieldinit System.Buffers.SequenceReader`1<valuetype .ctor (class [System.Runtime/*23000001*/]System.ValueType/*0100001A*/ modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.UnmanagedType/*0100003F*/)/*1B000006*/, class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!T>/*1B000005*/) T>
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C00005B:0A000021*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*01000036*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
  .custom /*0C00005C:0A000022*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000037*/::.ctor(string,
                                                                                                                      bool) /* 0A000022 */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C00005D:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute/*01000038*/::.ctor(string) /* 0A000023 */ = ( 01 00 0A 52 65 66 53 74 72 75 63 74 73 00 00 )    // ...RefStructs..
  .param type T /*2A000009*/ 
    .custom /*0C00003E:0A000025*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsUnmanagedAttribute/*0100003E*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
  .param constraint   T/*2A000009*/ , class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<!T>/*1B000005*/
    .custom /*0C00001D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A00001A */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
  .field /*0400000A*/ private object _dummy
  .field /*0400000B*/ private int32 _dummyPrimitive
  .method /*0600013B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> sequence) cil managed
  // SIG: 20 01 01 15 11 34 01 13 00
  {
    // Method begins at RVA 0x23ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::.ctor

  .method /*0600013C*/ public hidebysig specialname 
          instance int64  get_Consumed() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C0002FF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Consumed

  .method /*0600013D*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_CurrentSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .custom /*0C000301:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_CurrentSpan

  .method /*0600013E*/ public hidebysig specialname 
          instance int32  get_CurrentSpanIndex() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000303:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_CurrentSpanIndex

  .method /*0600013F*/ public hidebysig specialname 
          instance bool  get_End() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000305:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_End

  .method /*06000140*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000307:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Length

  .method /*06000141*/ public hidebysig specialname 
          instance valuetype System.SequencePosition/*02000002*/ 
          get_Position() cil managed
  // SIG: 20 00 11 08
  {
    .custom /*0C000309:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Position

  .method /*06000142*/ public hidebysig specialname 
          instance int64  get_Remaining() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00030B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2401
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Remaining

  .method /*06000143*/ public hidebysig specialname 
          instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          get_Sequence() cil managed
  // SIG: 20 00 15 11 34 01 13 00
  {
    .custom /*0C00030C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2404
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_Sequence

  .method /*06000144*/ public hidebysig specialname 
          instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> 
          get_UnreadSequence() cil managed
  // SIG: 20 00 15 11 34 01 13 00
  {
    .custom /*0C00030D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2407
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_UnreadSequence

  .method /*06000145*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> 
          get_UnreadSpan() cil managed
  // SIG: 20 00 15 11 80 95 01 13 00
  {
    .custom /*0C00030F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x240a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::get_UnreadSpan

  .method /*06000146*/ public hidebysig instance void 
          Advance(int64 count) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x240d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SequenceReader`1::Advance

  .method /*06000147*/ public hidebysig instance int64 
          AdvancePast(!T 'value') cil managed
  // SIG: 20 01 0A 13 00
  {
    // Method begins at RVA 0x240f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePast

  .method /*06000148*/ public hidebysig instance int64 
          AdvancePastAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> values) cil managed
  // SIG: 20 01 0A 15 11 80 95 01 13 00
  {
    .param [1]/*080002CE*/ 
    .custom /*0C00043D:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*06000149*/ public hidebysig instance int64 
          AdvancePastAny(!T value0,
                         !T value1) cil managed
  // SIG: 20 02 0A 13 00 13 00
  {
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*0600014A*/ public hidebysig instance int64 
          AdvancePastAny(!T value0,
                         !T value1,
                         !T value2) cil managed
  // SIG: 20 03 0A 13 00 13 00 13 00
  {
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*0600014B*/ public hidebysig instance int64 
          AdvancePastAny(!T value0,
                         !T value1,
                         !T value2,
                         !T value3) cil managed
  // SIG: 20 04 0A 13 00 13 00 13 00 13 00
  {
    // Method begins at RVA 0x241b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvancePastAny

  .method /*0600014C*/ public hidebysig instance void 
          AdvanceToEnd() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::AdvanceToEnd

  .method /*0600014D*/ public hidebysig instance bool 
          IsNext(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> next,
                 [opt] bool advancePast) cil managed
  // SIG: 20 02 02 15 11 80 95 01 13 00 02
  {
    .param [1]/*080002D8*/ 
    .custom /*0C00043E:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [2]/*080002D9*/  = bool(false)
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::IsNext

  .method /*0600014E*/ public hidebysig instance bool 
          IsNext(!T next,
                 [opt] bool advancePast) cil managed
  // SIG: 20 02 02 13 00 02
  {
    .param [2]/*080002DB*/  = bool(false)
    // Method begins at RVA 0x2424
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::IsNext

  .method /*0600014F*/ public hidebysig instance void 
          Rewind(int64 count) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2427
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SequenceReader`1::Rewind

  .method /*06000150*/ public hidebysig instance bool 
          TryAdvanceTo(!T delimiter,
                       [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 02 02 13 00 02
  {
    .param [2]/*080002DE*/  = bool(true)
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryAdvanceTo

  .method /*06000151*/ public hidebysig instance bool 
          TryAdvanceToAny(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiters,
                          [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 02 02 15 11 80 95 01 13 00 02
  {
    .param [1]/*080002DF*/ 
    .custom /*0C00043F:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [2]/*080002E0*/  = bool(true)
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryAdvanceToAny

  .method /*06000152*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<!T> destination) cil managed
  // SIG: 20 01 02 15 11 80 99 01 13 00
  {
    .custom /*0C000316:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryCopyTo

  .method /*06000153*/ public hidebysig instance bool 
          TryPeek([out] !T& 'value') cil managed
  // SIG: 20 01 02 10 13 00
  {
    .custom /*0C000318:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryPeek

  .method /*06000154*/ public hidebysig instance bool 
          TryPeek(int64 offset,
                  [out] !T& 'value') cil managed
  // SIG: 20 02 02 0A 10 13 00
  {
    .custom /*0C00031A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2435
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryPeek

  .method /*06000155*/ public hidebysig instance bool 
          TryRead([out] !T& 'value') cil managed
  // SIG: 20 01 02 10 13 00
  {
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryRead

  .method /*06000156*/ public hidebysig instance bool 
          TryReadTo([out] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>& sequence,
                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 34 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*080002E7*/ 
    .custom /*0C000440:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [3]/*080002E8*/  = bool(true)
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000157*/ public hidebysig instance bool 
          TryReadTo([out] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>& sequence,
                    !T delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 34 01 13 00 13 00 02
  {
    .param [3]/*080002EB*/  = bool(true)
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000158*/ public hidebysig instance bool 
          TryReadTo([out] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>& sequence,
                    !T delimiter,
                    !T delimiterEscape,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 04 02 10 15 11 34 01 13 00 13 00 13 00 02
  {
    .param [4]/*080002EF*/  = bool(true)
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*06000159*/ public hidebysig instance bool 
          TryReadTo([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 80 95 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*080002F1*/ 
    .custom /*0C000441:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [3]/*080002F2*/  = bool(true)
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*0600015A*/ public hidebysig instance bool 
          TryReadTo([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                    !T delimiter,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 80 95 01 13 00 13 00 02
  {
    .param [3]/*080002F5*/  = bool(true)
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*0600015B*/ public hidebysig instance bool 
          TryReadTo([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                    !T delimiter,
                    !T delimiterEscape,
                    [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 04 02 10 15 11 80 95 01 13 00 13 00 13 00 02
  {
    .param [4]/*080002F9*/  = bool(true)
    // Method begins at RVA 0x244a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadTo

  .method /*0600015C*/ public hidebysig instance bool 
          TryReadToAny([out] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>& sequence,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiters,
                       [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 34 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*080002FB*/ 
    .custom /*0C000442:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [3]/*080002FC*/  = bool(true)
    // Method begins at RVA 0x244d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadToAny

  .method /*0600015D*/ public hidebysig instance bool 
          TryReadToAny([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>& span,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> delimiters,
                       [opt] bool advancePastDelimiter) cil managed
  // SIG: 20 03 02 10 15 11 80 95 01 13 00 15 11 80 95 01 13 00 02
  {
    .param [2]/*080002FE*/ 
    .custom /*0C000443:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000035*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [3]/*080002FF*/  = bool(true)
    // Method begins at RVA 0x2450
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadToAny

  .method /*0600015E*/ public hidebysig instance bool 
          TryReadExact(int32 count,
                       [out] valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>& sequence) cil managed
  // SIG: 20 02 02 08 10 15 11 34 01 13 00
  {
    // Method begins at RVA 0x2453
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SequenceReader`1::TryReadExact

  .property /*17000014*/ instance callconv(8) int64
          Consumed()
  {
    .get instance int64 System.Buffers.SequenceReader`1/*0200000F*/::get_Consumed() /* 0600013C */
  } // end of property SequenceReader`1::Consumed
  .property /*17000015*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          CurrentSpan()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.SequenceReader`1/*0200000F*/::get_CurrentSpan() /* 0600013D */
  } // end of property SequenceReader`1::CurrentSpan
  .property /*17000016*/ instance callconv(8) int32
          CurrentSpanIndex()
  {
    .get instance int32 System.Buffers.SequenceReader`1/*0200000F*/::get_CurrentSpanIndex() /* 0600013E */
  } // end of property SequenceReader`1::CurrentSpanIndex
  .property /*17000017*/ instance callconv(8) bool
          End()
  {
    .get instance bool System.Buffers.SequenceReader`1/*0200000F*/::get_End() /* 0600013F */
  } // end of property SequenceReader`1::End
  .property /*17000018*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Buffers.SequenceReader`1/*0200000F*/::get_Length() /* 06000140 */
  } // end of property SequenceReader`1::Length
  .property /*17000019*/ instance callconv(8) valuetype System.SequencePosition/*02000002*/
          Position()
  {
    .get instance valuetype System.SequencePosition/*02000002*/ System.Buffers.SequenceReader`1/*0200000F*/::get_Position() /* 06000141 */
  } // end of property SequenceReader`1::Position
  .property /*1700001A*/ instance callconv(8) int64
          Remaining()
  {
    .get instance int64 System.Buffers.SequenceReader`1/*0200000F*/::get_Remaining() /* 06000142 */
  } // end of property SequenceReader`1::Remaining
  .property /*1700001B*/ instance callconv(8) valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>
          Sequence()
  {
    .get instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> System.Buffers.SequenceReader`1/*0200000F*/::get_Sequence() /* 06000143 */
  } // end of property SequenceReader`1::Sequence
  .property /*1700001C*/ instance callconv(8) valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T>
          UnreadSequence()
  {
    .get instance valuetype System.Buffers.ReadOnlySequence`1/*0200000D*/<!T> System.Buffers.SequenceReader`1/*0200000F*/::get_UnreadSequence() /* 06000144 */
  } // end of property SequenceReader`1::UnreadSequence
  .property /*1700001D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T>
          UnreadSpan()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<!T> System.Buffers.SequenceReader`1/*0200000F*/::get_UnreadSpan() /* 06000145 */
  } // end of property SequenceReader`1::UnreadSpan
} // end of class System.Buffers.SequenceReader`1

.class /*02000010*/ public sequential ansi sealed beforefieldinit System.Buffers.StandardFormat
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100001B*/<valuetype System.Buffers.StandardFormat/*02000010*/>/*1B000008*/
{
  .custom /*0C000063:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ private initonly int32 _dummyPrimitive
  .field /*0400000D*/ public static literal uint8 MaxPrecision = uint8(0x63)
  .field /*0400000E*/ public static literal uint8 NoPrecision = uint8(0xFF)
  .method /*0600015F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(char symbol,
                               [opt] uint8 precision) cil managed
  // SIG: 20 02 01 03 05
  {
    .param [2]/*08000303*/  = uint8(0xFF)
    // Method begins at RVA 0x2456
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::.ctor

  .method /*06000160*/ public hidebysig specialname 
          instance bool  get_HasPrecision() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2459
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_HasPrecision

  .method /*06000161*/ public hidebysig specialname 
          instance bool  get_IsDefault() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x245c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_IsDefault

  .method /*06000162*/ public hidebysig specialname 
          instance uint8  get_Precision() cil managed
  // SIG: 20 00 05
  {
    // Method begins at RVA 0x245f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_Precision

  .method /*06000163*/ public hidebysig specialname 
          instance char  get_Symbol() cil managed
  // SIG: 20 00 03
  {
    // Method begins at RVA 0x2462
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::get_Symbol

  .method /*06000164*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Buffers.StandardFormat/*02000010*/ other) cil managed
  // SIG: 20 01 02 11 40
  {
    // Method begins at RVA 0x2465
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Equals

  .method /*06000165*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000327:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000305*/ 
    .custom /*0C000444:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001F*/::.ctor(bool) /* 0A000019 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2468
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Equals

  .method /*06000166*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x246b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::GetHashCode

  .method /*06000167*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Buffers.StandardFormat/*02000010*/ left,
                            valuetype System.Buffers.StandardFormat/*02000010*/ right) cil managed
  // SIG: 00 02 02 11 40 11 40
  {
    // Method begins at RVA 0x246e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::op_Equality

  .method /*06000168*/ public hidebysig specialname static 
          valuetype System.Buffers.StandardFormat/*02000010*/ 
          op_Implicit(char symbol) cil managed
  // SIG: 00 01 11 40 03
  {
    // Method begins at RVA 0x2471
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::op_Implicit

  .method /*06000169*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Buffers.StandardFormat/*02000010*/ left,
                              valuetype System.Buffers.StandardFormat/*02000010*/ right) cil managed
  // SIG: 00 02 02 11 40 11 40
  {
    // Method begins at RVA 0x2474
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::op_Inequality

  .method /*0600016A*/ public hidebysig static 
          valuetype System.Buffers.StandardFormat/*02000010*/ 
          Parse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> format) cil managed
  // SIG: 00 01 11 40 15 11 80 95 01 03
  {
    .param [1]/*0800030B*/ 
    .custom /*0C000445:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000043*/::.ctor(string) /* 0A000026 */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2477
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Parse

  .method /*0600016B*/ public hidebysig static 
          valuetype System.Buffers.StandardFormat/*02000010*/ 
          Parse(string format) cil managed
  // SIG: 00 01 11 40 0E
  {
    .custom /*0C00032D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800030C*/ 
    .custom /*0C000446:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000043*/::.ctor(string) /* 0A000026 */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x247a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::Parse

  .method /*0600016C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00032E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x247d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::ToString

  .method /*0600016D*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<char> format,
                         [out] valuetype System.Buffers.StandardFormat/*02000010*/& result) cil managed
  // SIG: 00 02 02 15 11 80 95 01 03 10 11 40
  {
    .param [1]/*0800030D*/ 
    .custom /*0C000447:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000043*/::.ctor(string) /* 0A000026 */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2480
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StandardFormat::TryParse

  .property /*1700001E*/ instance callconv(8) bool
          HasPrecision()
  {
    .get instance bool System.Buffers.StandardFormat/*02000010*/::get_HasPrecision() /* 06000160 */
  } // end of property StandardFormat::HasPrecision
  .property /*1700001F*/ instance callconv(8) bool
          IsDefault()
  {
    .get instance bool System.Buffers.StandardFormat/*02000010*/::get_IsDefault() /* 06000161 */
  } // end of property StandardFormat::IsDefault
  .property /*17000020*/ instance callconv(8) uint8
          Precision()
  {
    .get instance uint8 System.Buffers.StandardFormat/*02000010*/::get_Precision() /* 06000162 */
  } // end of property StandardFormat::Precision
  .property /*17000021*/ instance callconv(8) char
          Symbol()
  {
    .get instance char System.Buffers.StandardFormat/*02000010*/::get_Symbol() /* 06000163 */
  } // end of property StandardFormat::Symbol
} // end of class System.Buffers.StandardFormat

.class /*02000011*/ public abstract auto ansi sealed beforefieldinit System.Buffers.Text.Utf8Formatter
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*0600016E*/ public hidebysig static 
          bool  TryFormat(bool 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 02 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000312*/  = nullref
    // Method begins at RVA 0x2483
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600016F*/ public hidebysig static 
          bool  TryFormat(uint8 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 05 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000316*/  = nullref
    // Method begins at RVA 0x2486
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000170*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 11 81 11 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*0800031A*/  = nullref
    // Method begins at RVA 0x2489
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000171*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000045*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 11 81 15 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*0800031E*/  = nullref
    // Method begins at RVA 0x248c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000172*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.Decimal/*01000046*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 11 81 19 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000322*/  = nullref
    // Method begins at RVA 0x248f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000173*/ public hidebysig static 
          bool  TryFormat(float64 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 0D 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000326*/  = nullref
    // Method begins at RVA 0x2492
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000174*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.Guid/*01000047*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 11 81 1D 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*0800032A*/  = nullref
    // Method begins at RVA 0x2495
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000175*/ public hidebysig static 
          bool  TryFormat(int16 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 06 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*0800032E*/  = nullref
    // Method begins at RVA 0x2498
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000176*/ public hidebysig static 
          bool  TryFormat(int32 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 08 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000332*/  = nullref
    // Method begins at RVA 0x249b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000177*/ public hidebysig static 
          bool  TryFormat(int64 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 0A 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000336*/  = nullref
    // Method begins at RVA 0x249e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000178*/ public hidebysig static 
          bool  TryFormat(int8 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 04 15 11 80 99 01 05 10 08 11 40
  {
    .custom /*0C000338:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800033A*/  = nullref
    // Method begins at RVA 0x24a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*06000179*/ public hidebysig static 
          bool  TryFormat(float32 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 0C 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*0800033E*/  = nullref
    // Method begins at RVA 0x24a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600017A*/ public hidebysig static 
          bool  TryFormat(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000048*/ 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 11 81 21 15 11 80 99 01 05 10 08 11 40
  {
    .param [4]/*08000342*/  = nullref
    // Method begins at RVA 0x24a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600017B*/ public hidebysig static 
          bool  TryFormat(uint16 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 07 15 11 80 99 01 05 10 08 11 40
  {
    .custom /*0C00033C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*08000346*/  = nullref
    // Method begins at RVA 0x24aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600017C*/ public hidebysig static 
          bool  TryFormat(uint32 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 09 15 11 80 99 01 05 10 08 11 40
  {
    .custom /*0C00033E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800034A*/  = nullref
    // Method begins at RVA 0x24ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

  .method /*0600017D*/ public hidebysig static 
          bool  TryFormat(uint64 'value',
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                          [out] int32& bytesWritten,
                          [opt] valuetype System.Buffers.StandardFormat/*02000010*/ format) cil managed
  // SIG: 00 04 02 0B 15 11 80 99 01 05 10 08 11 40
  {
    .custom /*0C000340:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800034E*/  = nullref
    // Method begins at RVA 0x24b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Formatter::TryFormat

} // end of class System.Buffers.Text.Utf8Formatter

.class /*02000012*/ public abstract auto ansi sealed beforefieldinit System.Buffers.Text.Utf8Parser
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*0600017E*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] bool& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 02 10 08 03
  {
    .param [4]/*08000352*/  = char(0x0000)
    // Method begins at RVA 0x24b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600017F*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint8& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 05 10 08 03
  {
    .param [4]/*08000356*/  = char(0x0000)
    // Method begins at RVA 0x24b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000180*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.DateTime/*01000044*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 11 10 08 03
  {
    .param [4]/*0800035A*/  = char(0x0000)
    // Method begins at RVA 0x24b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000181*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000045*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 15 10 08 03
  {
    .param [4]/*0800035E*/  = char(0x0000)
    // Method begins at RVA 0x24bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000182*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.Decimal/*01000046*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 19 10 08 03
  {
    .param [4]/*08000362*/  = char(0x0000)
    // Method begins at RVA 0x24bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000183*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] float64& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0D 10 08 03
  {
    .param [4]/*08000366*/  = char(0x0000)
    // Method begins at RVA 0x24c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000184*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.Guid/*01000047*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 1D 10 08 03
  {
    .param [4]/*0800036A*/  = char(0x0000)
    // Method begins at RVA 0x24c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000185*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int16& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 06 10 08 03
  {
    .param [4]/*0800036E*/  = char(0x0000)
    // Method begins at RVA 0x24c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000186*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int32& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 08 10 08 03
  {
    .param [4]/*08000372*/  = char(0x0000)
    // Method begins at RVA 0x24cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000187*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int64& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0A 10 08 03
  {
    .param [4]/*08000376*/  = char(0x0000)
    // Method begins at RVA 0x24ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000188*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] int8& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 04 10 08 03
  {
    .custom /*0C00034B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800037A*/  = char(0x0000)
    // Method begins at RVA 0x24d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*06000189*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] float32& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0C 10 08 03
  {
    .param [4]/*0800037E*/  = char(0x0000)
    // Method begins at RVA 0x24d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600018A*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000048*/& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 11 81 21 10 08 03
  {
    .param [4]/*08000382*/  = char(0x0000)
    // Method begins at RVA 0x24d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600018B*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint16& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 07 10 08 03
  {
    .custom /*0C00034F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*08000386*/  = char(0x0000)
    // Method begins at RVA 0x24da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600018C*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint32& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 09 10 08 03
  {
    .custom /*0C000351:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800038A*/  = char(0x0000)
    // Method begins at RVA 0x24dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

  .method /*0600018D*/ public hidebysig static 
          bool  TryParse(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                         [out] uint64& 'value',
                         [out] int32& bytesConsumed,
                         [opt] char standardFormat) cil managed
  // SIG: 00 04 02 15 11 80 95 01 05 10 0B 10 08 03
  {
    .custom /*0C000353:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800038E*/  = char(0x0000)
    // Method begins at RVA 0x24e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Utf8Parser::TryParse

} // end of class System.Buffers.Text.Utf8Parser

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit System.Buffers.Binary.BinaryPrimitives
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .method /*0600018E*/ public hidebysig static 
          float64  ReadDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0D 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadDoubleBigEndian

  .method /*0600018F*/ public hidebysig static 
          float64  ReadDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0D 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadDoubleLittleEndian

  .method /*06000190*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/ 
          ReadHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 25 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadHalfBigEndian

  .method /*06000191*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/ 
          ReadHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 25 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadHalfLittleEndian

  .method /*06000192*/ public hidebysig static 
          int16  ReadInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 06 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt16BigEndian

  .method /*06000193*/ public hidebysig static 
          int16  ReadInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 06 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt16LittleEndian

  .method /*06000194*/ public hidebysig static 
          int32  ReadInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 08 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt32BigEndian

  .method /*06000195*/ public hidebysig static 
          int32  ReadInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 08 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt32LittleEndian

  .method /*06000196*/ public hidebysig static 
          int64  ReadInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0A 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt64BigEndian

  .method /*06000197*/ public hidebysig static 
          int64  ReadInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0A 15 11 80 95 01 05
  {
    // Method begins at RVA 0x24fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt64LittleEndian

  .method /*06000198*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 
          ReadInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 29 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2501
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt128BigEndian

  .method /*06000199*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 
          ReadInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 29 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2504
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadInt128LittleEndian

  .method /*0600019A*/ public hidebysig static 
          native int  ReadIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 18 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2507
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadIntPtrBigEndian

  .method /*0600019B*/ public hidebysig static 
          native int  ReadIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 18 15 11 80 95 01 05
  {
    // Method begins at RVA 0x250a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadIntPtrLittleEndian

  .method /*0600019C*/ public hidebysig static 
          float32  ReadSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0C 15 11 80 95 01 05
  {
    // Method begins at RVA 0x250d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadSingleBigEndian

  .method /*0600019D*/ public hidebysig static 
          float32  ReadSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0C 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2510
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadSingleLittleEndian

  .method /*0600019E*/ public hidebysig static 
          uint16  ReadUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 07 15 11 80 95 01 05
  {
    .custom /*0C000360:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2513
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt16BigEndian

  .method /*0600019F*/ public hidebysig static 
          uint16  ReadUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 07 15 11 80 95 01 05
  {
    .custom /*0C000361:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2516
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt16LittleEndian

  .method /*060001A0*/ public hidebysig static 
          uint32  ReadUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 09 15 11 80 95 01 05
  {
    .custom /*0C000362:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2519
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt32BigEndian

  .method /*060001A1*/ public hidebysig static 
          uint32  ReadUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 09 15 11 80 95 01 05
  {
    .custom /*0C000363:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x251c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt32LittleEndian

  .method /*060001A2*/ public hidebysig static 
          uint64  ReadUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0B 15 11 80 95 01 05
  {
    .custom /*0C000364:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x251f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt64BigEndian

  .method /*060001A3*/ public hidebysig static 
          uint64  ReadUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 0B 15 11 80 95 01 05
  {
    .custom /*0C000365:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2522
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt64LittleEndian

  .method /*060001A4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 
          ReadUInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 2D 15 11 80 95 01 05
  {
    .custom /*0C000366:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2525
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt128BigEndian

  .method /*060001A5*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 
          ReadUInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 11 81 2D 15 11 80 95 01 05
  {
    .custom /*0C000367:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2528
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUInt128LittleEndian

  .method /*060001A6*/ public hidebysig static 
          native uint  ReadUIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 19 15 11 80 95 01 05
  {
    .custom /*0C000368:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x252b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUIntPtrBigEndian

  .method /*060001A7*/ public hidebysig static 
          native uint  ReadUIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source) cil managed
  // SIG: 00 01 19 15 11 80 95 01 05
  {
    .custom /*0C000369:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x252e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReadUIntPtrLittleEndian

  .method /*060001A8*/ public hidebysig static 
          uint8  ReverseEndianness(uint8 'value') cil managed
  // SIG: 00 01 05 05
  {
    // Method begins at RVA 0x2531
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001A9*/ public hidebysig static 
          int16  ReverseEndianness(int16 'value') cil managed
  // SIG: 00 01 06 06
  {
    // Method begins at RVA 0x2534
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001AA*/ public hidebysig static 
          int32  ReverseEndianness(int32 'value') cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x2537
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001AB*/ public hidebysig static 
          int64  ReverseEndianness(int64 'value') cil managed
  // SIG: 00 01 0A 0A
  {
    // Method begins at RVA 0x253a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001AC*/ public hidebysig static 
          int8  ReverseEndianness(int8 'value') cil managed
  // SIG: 00 01 04 04
  {
    .custom /*0C00036B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x253d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001AD*/ public hidebysig static 
          uint16  ReverseEndianness(uint16 'value') cil managed
  // SIG: 00 01 07 07
  {
    .custom /*0C00036D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2540
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001AE*/ public hidebysig static 
          uint32  ReverseEndianness(uint32 'value') cil managed
  // SIG: 00 01 09 09
  {
    .custom /*0C00036F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2543
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001AF*/ public hidebysig static 
          uint64  ReverseEndianness(uint64 'value') cil managed
  // SIG: 00 01 0B 0B
  {
    .custom /*0C000371:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2546
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B0*/ public hidebysig static 
          native int  ReverseEndianness(native int 'value') cil managed
  // SIG: 00 01 18 18
  {
    // Method begins at RVA 0x2549
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B1*/ public hidebysig static 
          native uint  ReverseEndianness(native uint 'value') cil managed
  // SIG: 00 01 19 19
  {
    .custom /*0C000374:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x254c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 
          ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 'value') cil managed
  // SIG: 00 01 11 81 29 11 81 29
  {
    // Method begins at RVA 0x254f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 
          ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 'value') cil managed
  // SIG: 00 01 11 81 2D 11 81 2D
  {
    .custom /*0C000376:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2552
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B4*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<int32> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<int32> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 08 15 11 80 99 01 08
  {
    // Method begins at RVA 0x2555
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B5*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 11 81 29 15 11 80 99 01 11 81 29
  {
    // Method begins at RVA 0x2557
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B6*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<int64> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<int64> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 0A 15 11 80 99 01 0A
  {
    // Method begins at RVA 0x2559
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B7*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<native int> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<native int> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 18 15 11 80 99 01 18
  {
    // Method begins at RVA 0x255b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B8*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<int16> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<int16> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 06 15 11 80 99 01 06
  {
    // Method begins at RVA 0x255d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001B9*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<native uint> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<native uint> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 19 15 11 80 99 01 19
  {
    .custom /*0C000378:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x255f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001BA*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint32> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint32> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 09 15 11 80 99 01 09
  {
    .custom /*0C00037A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2561
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001BB*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 11 81 2D 15 11 80 99 01 11 81 2D
  {
    .custom /*0C00037B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2563
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001BC*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint64> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint64> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 0B 15 11 80 99 01 0B
  {
    .custom /*0C00037C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2565
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001BD*/ public hidebysig static 
          void  ReverseEndianness(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint16> source,
                                  valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint16> destination) cil managed
  // SIG: 00 02 01 15 11 80 95 01 07 15 11 80 99 01 07
  {
    .custom /*0C00037D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2567
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::ReverseEndianness

  .method /*060001BE*/ public hidebysig static 
          bool  TryReadDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] float64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0D
  {
    // Method begins at RVA 0x2569
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadDoubleBigEndian

  .method /*060001BF*/ public hidebysig static 
          bool  TryReadDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] float64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0D
  {
    // Method begins at RVA 0x256c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadDoubleLittleEndian

  .method /*060001C0*/ public hidebysig static 
          bool  TryReadHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                     [out] valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 25
  {
    // Method begins at RVA 0x256f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadHalfBigEndian

  .method /*060001C1*/ public hidebysig static 
          bool  TryReadHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                        [out] valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 25
  {
    // Method begins at RVA 0x2572
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadHalfLittleEndian

  .method /*060001C2*/ public hidebysig static 
          bool  TryReadInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                      [out] int16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 06
  {
    // Method begins at RVA 0x2575
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt16BigEndian

  .method /*060001C3*/ public hidebysig static 
          bool  TryReadInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                         [out] int16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 06
  {
    // Method begins at RVA 0x2578
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt16LittleEndian

  .method /*060001C4*/ public hidebysig static 
          bool  TryReadInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                      [out] int32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 08
  {
    // Method begins at RVA 0x257b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt32BigEndian

  .method /*060001C5*/ public hidebysig static 
          bool  TryReadInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                         [out] int32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 08
  {
    // Method begins at RVA 0x257e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt32LittleEndian

  .method /*060001C6*/ public hidebysig static 
          bool  TryReadInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                      [out] int64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0A
  {
    // Method begins at RVA 0x2581
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt64BigEndian

  .method /*060001C7*/ public hidebysig static 
          bool  TryReadInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                         [out] int64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0A
  {
    // Method begins at RVA 0x2584
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt64LittleEndian

  .method /*060001C8*/ public hidebysig static 
          bool  TryReadInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 29
  {
    // Method begins at RVA 0x2587
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt128BigEndian

  .method /*060001C9*/ public hidebysig static 
          bool  TryReadInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 29
  {
    // Method begins at RVA 0x258a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadInt128LittleEndian

  .method /*060001CA*/ public hidebysig static 
          bool  TryReadIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] native int& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 18
  {
    // Method begins at RVA 0x258d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadIntPtrBigEndian

  .method /*060001CB*/ public hidebysig static 
          bool  TryReadIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] native int& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 18
  {
    // Method begins at RVA 0x2590
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadIntPtrLittleEndian

  .method /*060001CC*/ public hidebysig static 
          bool  TryReadSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] float32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0C
  {
    // Method begins at RVA 0x2593
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadSingleBigEndian

  .method /*060001CD*/ public hidebysig static 
          bool  TryReadSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] float32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0C
  {
    // Method begins at RVA 0x2596
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadSingleLittleEndian

  .method /*060001CE*/ public hidebysig static 
          bool  TryReadUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] uint16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 07
  {
    .custom /*0C000380:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2599
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt16BigEndian

  .method /*060001CF*/ public hidebysig static 
          bool  TryReadUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] uint16& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 07
  {
    .custom /*0C000382:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x259c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt16LittleEndian

  .method /*060001D0*/ public hidebysig static 
          bool  TryReadUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] uint32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 09
  {
    .custom /*0C000384:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x259f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt32BigEndian

  .method /*060001D1*/ public hidebysig static 
          bool  TryReadUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] uint32& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 09
  {
    .custom /*0C000386:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt32LittleEndian

  .method /*060001D2*/ public hidebysig static 
          bool  TryReadUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                       [out] uint64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0B
  {
    .custom /*0C000388:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt64BigEndian

  .method /*060001D3*/ public hidebysig static 
          bool  TryReadUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                          [out] uint64& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 0B
  {
    .custom /*0C00038A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt64LittleEndian

  .method /*060001D4*/ public hidebysig static 
          bool  TryReadUInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                        [out] valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 2D
  {
    .custom /*0C00038C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt128BigEndian

  .method /*060001D5*/ public hidebysig static 
          bool  TryReadUInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                           [out] valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 11 81 2D
  {
    .custom /*0C00038E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUInt128LittleEndian

  .method /*060001D6*/ public hidebysig static 
          bool  TryReadUIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                        [out] native uint& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 19
  {
    .custom /*0C000390:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUIntPtrBigEndian

  .method /*060001D7*/ public hidebysig static 
          bool  TryReadUIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> source,
                                           [out] native uint& 'value') cil managed
  // SIG: 00 02 02 15 11 80 95 01 05 10 19
  {
    .custom /*0C000392:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryReadUIntPtrLittleEndian

  .method /*060001D8*/ public hidebysig static 
          bool  TryWriteDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x25b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteDoubleBigEndian

  .method /*060001D9*/ public hidebysig static 
          bool  TryWriteDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           float64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x25ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteDoubleLittleEndian

  .method /*060001DA*/ public hidebysig static 
          bool  TryWriteHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                      valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 25
  {
    // Method begins at RVA 0x25bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteHalfBigEndian

  .method /*060001DB*/ public hidebysig static 
          bool  TryWriteHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                         valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 25
  {
    // Method begins at RVA 0x25c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteHalfLittleEndian

  .method /*060001DC*/ public hidebysig static 
          bool  TryWriteInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x25c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt16BigEndian

  .method /*060001DD*/ public hidebysig static 
          bool  TryWriteInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                          int16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x25c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt16LittleEndian

  .method /*060001DE*/ public hidebysig static 
          bool  TryWriteInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x25c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt32BigEndian

  .method /*060001DF*/ public hidebysig static 
          bool  TryWriteInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                          int32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x25cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt32LittleEndian

  .method /*060001E0*/ public hidebysig static 
          bool  TryWriteInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x25cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt64BigEndian

  .method /*060001E1*/ public hidebysig static 
          bool  TryWriteInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                          int64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x25d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt64LittleEndian

  .method /*060001E2*/ public hidebysig static 
          bool  TryWriteInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 29
  {
    // Method begins at RVA 0x25d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt128BigEndian

  .method /*060001E3*/ public hidebysig static 
          bool  TryWriteInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 29
  {
    // Method begins at RVA 0x25d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteInt128LittleEndian

  .method /*060001E4*/ public hidebysig static 
          bool  TryWriteIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        native int 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 18
  {
    // Method begins at RVA 0x25db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteIntPtrBigEndian

  .method /*060001E5*/ public hidebysig static 
          bool  TryWriteIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           native int 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 18
  {
    // Method begins at RVA 0x25de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteIntPtrLittleEndian

  .method /*060001E6*/ public hidebysig static 
          bool  TryWriteSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x25e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteSingleBigEndian

  .method /*060001E7*/ public hidebysig static 
          bool  TryWriteSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           float32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x25e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteSingleLittleEndian

  .method /*060001E8*/ public hidebysig static 
          bool  TryWriteUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 07
  {
    .custom /*0C0003A0:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt16BigEndian

  .method /*060001E9*/ public hidebysig static 
          bool  TryWriteUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           uint16 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 07
  {
    .custom /*0C0003A1:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt16LittleEndian

  .method /*060001EA*/ public hidebysig static 
          bool  TryWriteUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 09
  {
    .custom /*0C0003A2:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt32BigEndian

  .method /*060001EB*/ public hidebysig static 
          bool  TryWriteUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           uint32 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 09
  {
    .custom /*0C0003A3:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt32LittleEndian

  .method /*060001EC*/ public hidebysig static 
          bool  TryWriteUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0B
  {
    .custom /*0C0003A4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt64BigEndian

  .method /*060001ED*/ public hidebysig static 
          bool  TryWriteUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                           uint64 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 0B
  {
    .custom /*0C0003A6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt64LittleEndian

  .method /*060001EE*/ public hidebysig static 
          bool  TryWriteUInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                         valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 2D
  {
    .custom /*0C0003A8:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt128BigEndian

  .method /*060001EF*/ public hidebysig static 
          bool  TryWriteUInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                            valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 11 81 2D
  {
    .custom /*0C0003AA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUInt128LittleEndian

  .method /*060001F0*/ public hidebysig static 
          bool  TryWriteUIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                         native uint 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 19
  {
    .custom /*0C0003AC:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUIntPtrBigEndian

  .method /*060001F1*/ public hidebysig static 
          bool  TryWriteUIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                            native uint 'value') cil managed
  // SIG: 00 02 02 15 11 80 99 01 05 19
  {
    .custom /*0C0003AE:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2602
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryPrimitives::TryWriteUIntPtrLittleEndian

  .method /*060001F2*/ public hidebysig static 
          void  WriteDoubleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     float64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x2605
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteDoubleBigEndian

  .method /*060001F3*/ public hidebysig static 
          void  WriteDoubleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0D
  {
    // Method begins at RVA 0x2607
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteDoubleLittleEndian

  .method /*060001F4*/ public hidebysig static 
          void  WriteHalfBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                   valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 25
  {
    // Method begins at RVA 0x2609
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteHalfBigEndian

  .method /*060001F5*/ public hidebysig static 
          void  WriteHalfLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                      valuetype [System.Runtime/*23000001*/]System.Half/*01000049*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 25
  {
    // Method begins at RVA 0x260b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteHalfLittleEndian

  .method /*060001F6*/ public hidebysig static 
          void  WriteInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                    int16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x260d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt16BigEndian

  .method /*060001F7*/ public hidebysig static 
          void  WriteInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 06
  {
    // Method begins at RVA 0x260f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt16LittleEndian

  .method /*060001F8*/ public hidebysig static 
          void  WriteInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                    int32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x2611
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt32BigEndian

  .method /*060001F9*/ public hidebysig static 
          void  WriteInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 08
  {
    // Method begins at RVA 0x2613
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt32LittleEndian

  .method /*060001FA*/ public hidebysig static 
          void  WriteInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                    int64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x2615
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt64BigEndian

  .method /*060001FB*/ public hidebysig static 
          void  WriteInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                       int64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0A
  {
    // Method begins at RVA 0x2617
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt64LittleEndian

  .method /*060001FC*/ public hidebysig static 
          void  WriteInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 29
  {
    // Method begins at RVA 0x2619
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt128BigEndian

  .method /*060001FD*/ public hidebysig static 
          void  WriteInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        valuetype [System.Runtime/*23000001*/]System.Int128/*0100004A*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 29
  {
    // Method begins at RVA 0x261b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteInt128LittleEndian

  .method /*060001FE*/ public hidebysig static 
          void  WriteIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     native int 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 18
  {
    // Method begins at RVA 0x261d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteIntPtrBigEndian

  .method /*060001FF*/ public hidebysig static 
          void  WriteIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        native int 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 18
  {
    // Method begins at RVA 0x261f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteIntPtrLittleEndian

  .method /*06000200*/ public hidebysig static 
          void  WriteSingleBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     float32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x2621
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteSingleBigEndian

  .method /*06000201*/ public hidebysig static 
          void  WriteSingleLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        float32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0C
  {
    // Method begins at RVA 0x2623
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteSingleLittleEndian

  .method /*06000202*/ public hidebysig static 
          void  WriteUInt16BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     uint16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 07
  {
    .custom /*0C0003C0:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2625
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt16BigEndian

  .method /*06000203*/ public hidebysig static 
          void  WriteUInt16LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint16 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 07
  {
    .custom /*0C0003C2:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2627
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt16LittleEndian

  .method /*06000204*/ public hidebysig static 
          void  WriteUInt32BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     uint32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 09
  {
    .custom /*0C0003C4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2629
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt32BigEndian

  .method /*06000205*/ public hidebysig static 
          void  WriteUInt32LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint32 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 09
  {
    .custom /*0C0003C6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x262b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt32LittleEndian

  .method /*06000206*/ public hidebysig static 
          void  WriteUInt64BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                     uint64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0B
  {
    .custom /*0C0003C8:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x262d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt64BigEndian

  .method /*06000207*/ public hidebysig static 
          void  WriteUInt64LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                        uint64 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 0B
  {
    .custom /*0C0003CA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x262f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt64LittleEndian

  .method /*06000208*/ public hidebysig static 
          void  WriteUInt128BigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                      valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 2D
  {
    .custom /*0C0003CC:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2631
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt128BigEndian

  .method /*06000209*/ public hidebysig static 
          void  WriteUInt128LittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                         valuetype [System.Runtime/*23000001*/]System.UInt128/*0100004B*/ 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 11 81 2D
  {
    .custom /*0C0003CE:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2633
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUInt128LittleEndian

  .method /*0600020A*/ public hidebysig static 
          void  WriteUIntPtrBigEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                      native uint 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 19
  {
    .custom /*0C0003D0:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2635
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUIntPtrBigEndian

  .method /*0600020B*/ public hidebysig static 
          void  WriteUIntPtrLittleEndian(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000026*/<uint8> destination,
                                         native uint 'value') cil managed
  // SIG: 00 02 01 15 11 80 99 01 05 19
  {
    .custom /*0C0003D2:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2637
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BinaryPrimitives::WriteUIntPtrLittleEndian

} // end of class System.Buffers.Binary.BinaryPrimitives


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
